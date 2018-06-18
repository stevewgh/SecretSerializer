# SecretSerializer
Implementation of Json.Net interfaces to allow secrets in objects to be serialized / deserialized safely.

Json.Net has many extension points where we can change how it serializes / deserializes the types in the object. Using a custom contact resolver we can encrypt / decrypt properties without forcing the consumer of Json.Net to use it any differently other than providing a customised JsonSerializationSettings instance.

#### 1. Decide what should be protected
Add the KeepSecret attribute to the class or properties that should be kept secret when serializing
```c#
public class Poco
{
  [KeepSecret]
  public string IContainASecret {get; set;}
  
  public string IWillBeSerializedInPlainText {get; set;}  
}
```

#### 2. Create a custom JsonSerializerSettings instance
```c#
var key = new byte[32];
var encryptionProvider = new FixedKeyEncryptionProvider(key);

var settings = new JsonSerializerSettings { 
   ContractResolver = new SecretContractResolver(encryptionProvider)
};
```

#### 3. When Serializing pass the JsonSerializerSettings instance to the Serialize method
```c#
var instance = new Poco();  
// any properties marked with [KeepSecret] will be encrypted before being serialized
var json = JsonConvert.Serialize(instance, settings); 
```

#### 4. When Deserializing pass the JsonSerializerSettings instance to the Deserialize method
```c#
var instance = JsonConvert.Deserialize<Poco>(json, settings);
```
