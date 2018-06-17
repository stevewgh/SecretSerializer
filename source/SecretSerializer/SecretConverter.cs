using System;
using System.Text;
using Newtonsoft.Json;

namespace SecretSerializer
{
    public class SecretConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var bodyJsonBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
            var secret = new Secret {Body = bodyJsonBytes, Iv = new byte[16]};
            serializer.Serialize(writer, secret);            
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var secret = serializer.Deserialize<Secret>(reader);
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(secret.Body), objectType);
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public class Secret
        {
            public byte[] Body { get; set; }
            public byte[] Iv { get; set; }
        }
    }
}