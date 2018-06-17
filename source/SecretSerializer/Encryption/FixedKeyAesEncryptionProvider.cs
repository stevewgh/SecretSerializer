using System;

namespace SecretSerializer.Encryption
{
    public class FixedKeyAesEncryptionProvider : EncryptionProvider
    {
        private readonly byte[] key;

        public FixedKeyAesEncryptionProvider(byte[] key)
        {
            if (key.Length != 32)
            {
                throw new ArgumentException("Key must be 32 bytes", nameof(key));
            }

            this.key = key;
        }

        protected override byte[] GetKeyForExistingSecret(Secret secret)
        {
            return key;
        }

        protected override byte[] GetKeyForNewSecret(out string keyIdentifier)
        {
            keyIdentifier = "fixed";
            return key;
        }
    }
}
