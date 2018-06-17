namespace SecretSerializer.Tests.TestClasses
{
    public class ClassWithSecretIntegerProperty
    {
        [KeepSecret]
        public int ShouldBeKeptSecret { get; set; }
    }
}