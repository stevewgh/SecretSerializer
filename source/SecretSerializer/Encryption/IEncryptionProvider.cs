namespace SecretSerializer.Encryption
{
    public interface IEncryptionProvider
    {
        Secret Encrypt(byte[] data);
        byte[] Decrypt(Secret secret);
    }
}