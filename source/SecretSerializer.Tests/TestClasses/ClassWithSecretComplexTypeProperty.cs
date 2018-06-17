using System;

namespace SecretSerializer.Tests.TestClasses
{
    public class ClassWithSecretComplexTypeProperty
    {
        public ClassWithSecretComplexTypeProperty()
        {
            ComplexProperty = new Complex();
        }

        public Complex ComplexProperty { get; set; }

        [KeepSecret]
        public class Complex
        {
            public Complex()
            {
                ShouldBeKeptSecret = Guid.NewGuid().ToString();
                ShouldAlsoBeKeptSecret = Guid.NewGuid().ToString();
            }

            public string ShouldBeKeptSecret { get; set; }

            public string ShouldAlsoBeKeptSecret { get; set; }
        }
    }
}