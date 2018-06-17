namespace SecretSerializer.Tests
{
    public class StubEncryptionProvider : IEncryptionProvider
    {
        public Secret Encrypt(byte[] data)
        {
            return new Secret{ Data = data, Iv = new byte[16], KeyIdentifier = "NotARealKey"};
        }

        public byte[] Decrypt(Secret secret)
        {
            return secret.Data;
        }
    }
}
