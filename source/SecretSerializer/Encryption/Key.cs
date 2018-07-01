using System;

namespace SecretSerializer.Encryption
{
    public class Key
    {
        public const int KeySize = 32;

        public Key(byte[] keyBytes, string identifier)
        {
            if (keyBytes.Length != KeySize)
            {
                throw new ArgumentException($"Key must be {KeySize} keyBytes", nameof(keyBytes));
            }

            Value = keyBytes;
            Identifier = identifier;
        }

        public byte[] Value { get; }
        public string Identifier { get; }
    }
}