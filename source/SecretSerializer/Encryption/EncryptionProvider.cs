using System;
using System.IO;
using System.Security.Cryptography;

namespace SecretSerializer.Encryption
{
    public abstract class EncryptionProvider : IEncryptionProvider
    {
        public Secret Encrypt(byte[] data)
        {
            var key = GetKeyForNewSecret();

            using (var aes = CreateAesOrThrowIfNull())
            {
                aes.Key = key.Value;

                return EncryptStream(data, aes, key.Identifier);
            }
        }

        public byte[] Decrypt(Secret secret)
        {
            var key = GetKeyForExistingSecret(secret);

            AssertKeyIsValid(key, secret);

            using (var aes = CreateAesOrThrowIfNull())
            {
                aes.Key = key.Value;
                aes.IV = secret.Iv;

                try
                {
                    return DecryptStream(secret, aes);
                }
                catch (CryptographicException)
                {
                    return new byte[0];
                }
            }
        }

        protected abstract Key GetKeyForNewSecret();

        protected abstract Key GetKeyForExistingSecret(Secret secret);

        private static Secret EncryptStream(byte[] data, SymmetricAlgorithm aes, string keyIdentifier)
        {
            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (var resultStream = new MemoryStream())
            {
                using (var aesStream = new CryptoStream(resultStream, encryptor, CryptoStreamMode.Write))
                using (var plainStream = new MemoryStream(data))
                {
                    plainStream.CopyTo(aesStream);
                }

                return new Secret {Data = resultStream.ToArray(), Iv = aes.IV, KeyIdentifier = keyIdentifier};
            }
        }

        private static byte[] DecryptStream(Secret secret, SymmetricAlgorithm aes)
        {
            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            using (var sourceStream = new MemoryStream(secret.Data))
            {
                using (var csDecrypt = new CryptoStream(sourceStream, decryptor, CryptoStreamMode.Read))
                {
                    using (var destStream = new MemoryStream())
                    {
                        csDecrypt.CopyTo(destStream);
                        return destStream.ToArray();
                    }
                }
            }
        }

        private static Aes CreateAesOrThrowIfNull()
        {
            var aes = Aes.Create();
            if (aes == null)
            {
                throw new Exception("Unable to create Aes instance.");
            }

            return aes;
        }

        private static void AssertKeyIsValid(Key key, Secret secret)
        {
            if (key.Identifier != secret.KeyIdentifier)
            {
                throw new KeyIdentifierMismatchException(key.Identifier, secret.KeyIdentifier);
            }
        }
    }
}