using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using System;

namespace SecretSerializer.Encryption
{
    public class KeyVaultEncryptionProvider : EncryptionProvider
    {
        private readonly IKeyVaultClient keyVaultClient;
        private readonly string keyVaultUri;
        private readonly string keySecretName;

        public KeyVaultEncryptionProvider(IKeyVaultClient keyVaultClient, string keyVaultUri, string keySecretName)
        {
            this.keyVaultClient = keyVaultClient ?? throw new ArgumentNullException();

            if (string.IsNullOrEmpty(keyVaultUri))
            {
                throw new ArgumentException("message", nameof(keyVaultUri));
            }

            this.keyVaultUri = keyVaultUri;

            if (string.IsNullOrEmpty(keySecretName))
            {
                throw new ArgumentException(nameof(keySecretName));
            }

            this.keySecretName = keySecretName;
        }

        protected override byte[] GetKeyForNewSecret(out string keyIdentifier)
        {
            var bundle = keyVaultClient.GetSecretAsync(keyVaultUri, keySecretName).ConfigureAwait(false).GetAwaiter().GetResult();
            keyIdentifier = bundle.SecretIdentifier.Version;
            return GetKeyFromBundle(bundle);
        }

        protected override byte[] GetKeyForExistingSecret(Secret secret)
        {
            var bundle = keyVaultClient.GetSecretAsync(keyVaultUri, keySecretName, secret.KeyIdentifier).ConfigureAwait(false).GetAwaiter().GetResult();
            return GetKeyFromBundle(bundle);
        }

        private static byte[] GetKeyFromBundle(SecretBundle bundle)
        {
            try
            {
                return Convert.FromBase64String(bundle.Value);
            }
            catch
            {
                throw new Exception($"Unable to get key from the bundle found at {bundle.Id}");
            }
        }
    }
}