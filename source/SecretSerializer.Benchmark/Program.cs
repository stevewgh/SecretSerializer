using BenchmarkDotNet.Running;

namespace SecretSerializer.Benchmark
{
    public class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<SecretContractVsDefaultContractResolver>();
        }
    }
}