using System;
using System.Security.Cryptography;

namespace SecretSerializer.Encryption
{
    public class FixedKeyEncryptionProvider : EncryptionProvider
    {
        private readonly Key fixedKey;

        public FixedKeyEncryptionProvider(byte[] keyBytes)
        {
            this.fixedKey = ConvertBytesToKey(keyBytes);
        }

        public FixedKeyEncryptionProvider(Key key)
        {
            this.fixedKey = key;
        }

        protected override Key GetKeyForExistingSecret(Secret secret)
        {
            return fixedKey;
        }

        protected override Key GetKeyForNewSecret()
        {
            return fixedKey;
        }

        private static Key ConvertBytesToKey(byte[] keyBytes)
        {
            return new Key(keyBytes, GetKeyIdentifier(keyBytes));
        }

        private static string GetKeyIdentifier(byte[] keyBytes)
        {
            using (var hmac = new HMACSHA256(keyBytes))
            {
                return $"fixed;{Convert.ToBase64String(hmac.ComputeHash(keyBytes))}";
            }
        }
    }
}