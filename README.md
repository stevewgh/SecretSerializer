# SecretSerializer
Implementation of Json.Net interfaces to allow secrets in objects to be serialized / deserialized safely.

#### Add the KeepSecret attribute to the class or properties that should be kept secret when serializing
```c#
public class Poco
{
  [KeepSecret]
  public string IContainASecret {get; set;}
}
```

#### When you serialize the class use JsonSerializationSettings to protect the properties marked with KeepSecret
```c#
var key = new byte[16];
RandomNumberGenerator.Create().GetBytes(key);
var encryptionProvider = new FixedKeyAesEncryptionProvider(key);
var settings = new JsonSerializerSettings { 
   ContractResolver = new SecretContractResolver(encryptionProvider)
};

var instance = new Poco();  
var json = JsonConvert.Serialize(instance, settings);
```

#### When you deserialize the class use JsonSerializationSettings to deserialize the properties marked with KeepSecret
```c#
var key = new byte[16];
RandomNumberGenerator.Create().GetBytes(key);
var encryptionProvider = new FixedKeyAesEncryptionProvider(key);
var settings = new JsonSerializerSettings { 
   ContractResolver = new SecretContractResolver(encryptionProvider)
};

var instance = JsonConvert.Deserialize<Poco>(json, settings);
```
