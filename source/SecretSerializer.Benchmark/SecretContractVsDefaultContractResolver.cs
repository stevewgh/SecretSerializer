using System;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;
using SecretSerializer.Encryption;

namespace SecretSerializer.Benchmark
{
    public class SecretContractVsDefaultContractResolver
    {
        private byte[] data;
        private JsonSerializerSettings defaultSerializer;
        private JsonSerializerSettings secretSerializer;
        private ClassWithSecrets instanceWithSecrets;

        [Params(100, 10000, 1000000)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            data = new byte[N];
            new Random().NextBytes(data);

            var key = new byte[32];
            RandomNumberGenerator.Create().GetBytes(key);
            secretSerializer = new JsonSerializerSettings
            {
                ContractResolver = new SecretContractResolver(new FixedKeyEncryptionProvider(key))
            };
            defaultSerializer = new JsonSerializerSettings();

            instanceWithSecrets = new ClassWithSecrets(data);
        }

        [Benchmark]
        public void Encrypted()
        {
            var json = JsonConvert.SerializeObject(instanceWithSecrets, secretSerializer);
            JsonConvert.DeserializeObject<ClassWithSecrets>(json, secretSerializer);
        }

        [Benchmark]
        public void Default()
        {
            var json = JsonConvert.SerializeObject(instanceWithSecrets, defaultSerializer);
            JsonConvert.DeserializeObject<ClassWithSecrets>(json, defaultSerializer);
        }
    }
}