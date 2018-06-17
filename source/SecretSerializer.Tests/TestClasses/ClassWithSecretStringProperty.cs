using System;

namespace SecretSerializer.Tests.TestClasses
{
    public class ClassWithSecretStringProperty
    {
        public ClassWithSecretStringProperty()
        {
            ShouldBeKeptSecret = Guid.NewGuid().ToString();
        }

        [KeepSecret]
        public string ShouldBeKeptSecret { get; set; }
    }
}
