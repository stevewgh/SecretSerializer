﻿using System;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SecretSerializer.Encryption;
using SecretSerializer.Tests.TestClasses;
using Xunit;

namespace SecretSerializer.Tests
{
    public class SecretContractResolverTests
    {
        private readonly IEncryptionProvider encryptionProvider = new StubEncryptionProvider();

        [Fact]
        public void Given_A_Property_With_KeepSecret_Attribute_Then_The_Converter_Is_A_SecretConverter()
        {
            var resolver = new SecretContractResolver(encryptionProvider);

            var contract = (JsonObjectContract) resolver.ResolveContract(typeof(ClassWithSecretStringProperty));

            contract.Properties
                .GetProperty(nameof(ClassWithSecretStringProperty.ShouldBeKeptSecret),
                    StringComparison.InvariantCultureIgnoreCase).Converter.Should().BeOfType<SecretConverter>();
        }

        [Fact]
        public void Given_A_Class_With_KeepSecret_Attribute_Then_The_Converter_Is_A_SecretConverter()
        {
            var resolver = new SecretContractResolver(encryptionProvider);

            var contract = (JsonObjectContract) resolver.ResolveContract(typeof(ClassWithSecretComplexTypeProperty));

            contract.Properties
                .GetProperty(nameof(ClassWithSecretComplexTypeProperty.ComplexProperty),
                    StringComparison.InvariantCultureIgnoreCase).Converter.Should().BeOfType<SecretConverter>();
        }

        [Theory]
        [InlineData(typeof(ClassWithSecretStringProperty))]
        [InlineData(typeof(ClassWithSecretIntegerProperty))]
        [InlineData(typeof(ClassWithSecretComplexTypeProperty))]
        [InlineData(typeof(ClassWithSecretNestedComplexTypeProperty))]
        public void Given_An_Instance_Of_A_ClassWithSecrets_The_Instance_Can_Be_Serialized_And_Deserialized(Type typeContainingSecret)
        {
            var serializerSettings = new JsonSerializerSettings { ContractResolver = new SecretContractResolver(encryptionProvider)};
            var objectContainingSecret = Activator.CreateInstance(typeContainingSecret);
            var deserializeMethod = typeof(JsonConvert).GetMethods().First(m => m.GetParameters().Any(info => info.ParameterType == typeof(JsonSerializerSettings)) && m.Name == nameof(JsonConvert.DeserializeObject) & m.IsGenericMethod).MakeGenericMethod(typeContainingSecret);

            var json = JsonConvert.SerializeObject(objectContainingSecret, serializerSettings);

            deserializeMethod.Invoke(null, new object[]{json, serializerSettings})
                .Should().BeEquivalentTo(objectContainingSecret);
        }
    }
}