using System;

namespace SecretSerializer.Tests.TestClasses
{
    public class ClassWithComplexTypeProperty
    {
        public ClassWithComplexTypeProperty()
        {
            ComplexProperty = new Complex();
            ShouldBeKeptSecret = Guid.NewGuid().ToString();
        }

        public Complex ComplexProperty { get; set; }

        [KeepSecret]
        public string ShouldBeKeptSecret { get; set; }

        public class Complex
        {
            public Complex()
            {
                NotSecret = Guid.NewGuid().ToString();
            }

            public string NotSecret { get; set; }
        }
    }
}