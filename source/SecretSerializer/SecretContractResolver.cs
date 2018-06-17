using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SecretSerializer
{
    public class SecretContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            if (member.CustomAttributes.Any(data => data.AttributeType == typeof(KeepSecretAttribute)))
            {
                var property = base.CreateProperty(member, memberSerialization);
                property.Converter = new SecretConverter();
                return property;
            }

            if (member is PropertyInfo propertyInfo && propertyInfo.PropertyType.CustomAttributes.Any(data => data.AttributeType == typeof(KeepSecretAttribute)))
            {
                var property = base.CreateProperty(member, memberSerialization);
                property.Converter = new SecretConverter();
                return property;
            }

            return base.CreateProperty(member, memberSerialization);
        }
    }
}
