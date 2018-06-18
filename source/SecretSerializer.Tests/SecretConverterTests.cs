using System.IO;
using FluentAssertions;
using Newtonsoft.Json;
using SecretSerializer.Encryption;
using Xunit;

namespace SecretSerializer.Tests
{
    public class SecretConverterTests
    {
        [Fact]
        public void Given_An_Object_The_Object_Is_Converted_To_A_Secret_When_It_Is_Serialized()
        {
            var converter = new SecretConverter(new StubEncryptionProvider());
            var memoryStream = new MemoryStream();
            var jsonTextWriter = new JsonTextWriter(new StreamWriter(memoryStream));

            var objectWithSecrets = new TestClasses.ClassWithComplexTypeProperty();

            converter.WriteJson(jsonTextWriter, objectWithSecrets, JsonSerializer.CreateDefault());
            jsonTextWriter.Flush();
            memoryStream.Position = 0;

            var secret = JsonConvert.DeserializeObject<Secret>(new StreamReader(memoryStream).ReadToEnd());
            secret.Data.Should().NotBeEmpty();
            secret.Iv.Should().NotBeEmpty();
            secret.KeyIdentifier.Should().NotBeEmpty();
        }
    }
}