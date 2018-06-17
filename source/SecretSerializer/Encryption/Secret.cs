namespace SecretSerializer.Encryption
{
    public class Secret
    {
        public string KeyIdentifier { get; set; }
        public byte[] Data { get; set; }
        public byte[] Iv { get; set; }
    }
}