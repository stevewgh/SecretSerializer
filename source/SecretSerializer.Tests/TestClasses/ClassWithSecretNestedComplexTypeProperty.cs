using System;

namespace SecretSerializer.Tests.TestClasses
{
    public class ClassWithSecretNestedComplexTypeProperty
    {
        public ClassWithSecretNestedComplexTypeProperty()
        {
            ComplexProperty = new Complex();
        }

        public Complex ComplexProperty { get; set; }

        public class Complex
        {
            public Complex()
            {
                NotSecret = Guid.NewGuid().ToString();
                ShouldBeKeptSecret = Guid.NewGuid().ToString();
            }

            public string NotSecret { get; set; }
            
            [KeepSecret]
            public string ShouldBeKeptSecret { get; set; }
        }
    }
}