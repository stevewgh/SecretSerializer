# SecretSerializer
Implementation of Json.Net interfaces to allow secrets in objects to be serialized / deserialized safely.

Json.Net has many extension points where we can change how it serializes / deserializes the types in the object. Using a custom contact resolver we can encrypt / decrypt properties without forcing the consumer of Json.Net to use it any differently other than providing a customised JsonSerializationSettings instance at initialization.

#### Add the KeepSecret attribute to the class or properties that should be kept secret when serializing
```c#
public class Poco
{
  [KeepSecret]
  public string IContainASecret {get; set;}
  
  public string IWillBeSerializedInPlainText {get; set;}  
}
```

#### Serializing
When you serialize the class use JsonSerializationSettings to protect the properties marked with KeepSecret
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

#### Deserializing
When you deserialize the class use JsonSerializationSettings to deserialize the properties marked with KeepSecret
```c#
var key = new byte[16];
RandomNumberGenerator.Create().GetBytes(key);
var encryptionProvider = new FixedKeyAesEncryptionProvider(key);
var settings = new JsonSerializerSettings { 
   ContractResolver = new SecretContractResolver(encryptionProvider)
};

var instance = JsonConvert.Deserialize<Poco>(json, settings);
```
