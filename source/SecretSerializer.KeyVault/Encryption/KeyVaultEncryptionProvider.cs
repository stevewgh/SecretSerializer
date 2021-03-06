﻿using System;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using SecretSerializer.Encryption;

namespace SecretSerializer.KeyVault.Encryption
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

        protected override Key GetKeyForNewSecret()
        {
            var bundle = keyVaultClient.GetSecretAsync(keyVaultUri, keySecretName).ConfigureAwait(false).GetAwaiter().GetResult();
            return GetKeyFromBundle(bundle);
        }

        protected override Key GetKeyForExistingSecret(Secret secret)
        {
            var bundle = keyVaultClient.GetSecretAsync(keyVaultUri, keySecretName, secret.KeyIdentifier).ConfigureAwait(false).GetAwaiter().GetResult();
            return GetKeyFromBundle(bundle);
        }

        private static Key GetKeyFromBundle(SecretBundle bundle)
        {
            try
            {
                return new Key(Convert.FromBase64String(bundle.Value), bundle.SecretIdentifier.Version);
            }
            catch
            {
                throw new Exception($"Unable to get key from the bundle found at {bundle.Id}");
            }
        }
    }
}