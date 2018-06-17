using System.IO;
using System.Security.Cryptography;

namespace SecretSerializer.Encryption
{
    public abstract class EncryptionProvider : IEncryptionProvider
    {
        public Secret Encrypt(byte[] data)
        {
            var key = GetKeyForNewSecret(out var keyIdentifier);

            using (var aes = Aes.Create())
            {
                aes.Key = key;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var resultStream = new MemoryStream())
                {
                    using (var aesStream = new CryptoStream(resultStream, encryptor, CryptoStreamMode.Write))
                    using (var plainStream = new MemoryStream(data))
                    {
                        plainStream.CopyTo(aesStream);
                    }

                    return new Secret { Data = resultStream.ToArray(), Iv = aes.IV, KeyIdentifier = keyIdentifier };
                }
            }
        }

        public byte[] Decrypt(Secret secret)
        {
            var key = GetKeyForExistingSecret(secret);

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = secret.Iv;

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
        }

        protected abstract byte[] GetKeyForNewSecret(out string keyIdentifier);

        protected abstract byte[] GetKeyForExistingSecret(Secret secret);
    }
}