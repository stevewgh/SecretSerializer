namespace SecretSerializer
{
    public interface IEncryptionProvider
    {
        Secret Encrypt(byte[] data);
        byte[] Decrypt(Secret secret);
    }
}