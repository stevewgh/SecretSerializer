using System;

namespace SecretSerializer.Encryption
{
    public class FixedKeyEncryptionProvider : EncryptionProvider
    {
        private readonly Key fixedKey;

        public FixedKeyEncryptionProvider(Key fixedKey)
        {
            this.fixedKey = fixedKey;
        }

        protected override Key GetKeyForExistingSecret(Secret secret)
        {
            return fixedKey;
        }

        protected override bool VerifyKeyIdentity(Key key, Secret secret)
        {
            return GetKeyIdentifier(key) == secret.KeyIdentifier;
        }

        protected override Key GetKeyForNewSecret(out string keyIdentifier)
        {
            keyIdentifier = GetKeyIdentifier(fixedKey);
            return fixedKey;
        }

        private static string GetKeyIdentifier(Key key)
        {
            return $"fixed;{Convert.ToBase64String(key.Identifier)}";
        }
    }
}