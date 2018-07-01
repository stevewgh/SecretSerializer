using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SecretSerializer.Encryption
{
    public class Key
    {
        public const int KeySize = 32;

        public Key()
        {
            this.Value = new byte[KeySize];
            using (var rnd = RandomNumberGenerator.Create())
            {
                rnd.GetBytes(Value);
            }
            SetIdentifier();
        }

        public Key(byte[] keyBytes)
        {
            if (keyBytes.Length != KeySize)
            {
                throw new ArgumentException($"Key must be {KeySize} keyBytes", nameof(keyBytes));
            }

            this.Value = keyBytes;            
            SetIdentifier();
        }

        private void SetIdentifier()
        {
            using (var sha = SHA256.Create())
            {
                // a fast repeatable process of uniquely identifying the key without compromising its value
                const string salt = "a94NZ8Nom9";
                const string pepper = "ws7vrPpExW";
                
                var pseudoKey =
                    Encoding.UTF8.GetBytes(salt)
                        .Union(Value.Take(8))
                        .Union(Encoding.UTF8.GetBytes(pepper))
                        .ToArray();

                Identifier = sha.ComputeHash(pseudoKey);
            }
        }

        public byte[] Value { get; }
        public byte[] Identifier { get; private set; }
    }
}