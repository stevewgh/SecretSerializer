using System;
using System.Text;
using Newtonsoft.Json;

namespace SecretSerializer
{
    public class SecretConverter : JsonConverter
    {
        private readonly IEncryptionProvider provider;

        public SecretConverter(IEncryptionProvider provider)
        {
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var bodyJsonBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
            var secret = provider.Encrypt(bodyJsonBytes);
            serializer.Serialize(writer, secret);            
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var secret = serializer.Deserialize<Secret>(reader);
            var bodyJsonBytes = provider.Decrypt(secret);
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(bodyJsonBytes), objectType);
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

    }
}