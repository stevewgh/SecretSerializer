using System;
using System.IO;
using System.Security.Cryptography;

namespace SecretSerializer
{
    public class FixedKeyAesEncryptionProvider : IEncryptionProvider
    {
        private readonly byte[] key;

        public FixedKeyAesEncryptionProvider(byte[] key)
        {
            if (key.Length != 16)
            {
                throw new ArgumentException("Key must be 16 bytes", nameof(key));
            }

            this.key = key;
        }

        public Secret Encrypt(byte[] data)
        {
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

                    return new Secret{Data = resultStream.ToArray(), Iv = aes.IV, KeyIdentifier = "Fixed"};
                }
            }
        }

        public byte[] Decrypt(Secret secret)
        {
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
    }
}
