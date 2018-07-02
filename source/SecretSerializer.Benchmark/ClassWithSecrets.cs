namespace SecretSerializer.Benchmark
{
    public class ClassWithSecrets
    {
        [KeepSecret]
        public byte[] Data { get; }

        public ClassWithSecrets(byte[] data)
        {
            Data = data;
        }
    }
}
