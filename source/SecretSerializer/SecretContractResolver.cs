using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SecretSerializer.Encryption;

namespace SecretSerializer
{
    public class SecretContractResolver : DefaultContractResolver
    {
        private readonly IEncryptionProvider encryptionProvider;

        public SecretContractResolver(IEncryptionProvider encryptionProvider)
        {
            this.encryptionProvider = encryptionProvider ?? throw new ArgumentNullException(nameof(encryptionProvider));
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            if (member.CustomAttributes.Any(data => data.AttributeType == typeof(KeepSecretAttribute)))
            {
                var property = base.CreateProperty(member, memberSerialization);
                property.Converter = new SecretConverter(encryptionProvider);
                return property;
            }

            if (member is PropertyInfo propertyInfo && propertyInfo.PropertyType.CustomAttributes.Any(data => data.AttributeType == typeof(KeepSecretAttribute)))
            {
                var property = base.CreateProperty(member, memberSerialization);
                property.Converter = new SecretConverter(encryptionProvider);
                return property;
            }

            return base.CreateProperty(member, memberSerialization);
        }
    }
}
