# SecretSerializer
Implementation of Json.Net interfaces to allow secrets in objects to be serialized / deserialized safely.

Json.Net has many extension points where we can change how it serializes / deserializes the types in the object. Using a custom contact resolver we can encrypt / decrypt properties without forcing the consumer of Json.Net to use it any differently other than providing a customised JsonSerializationSettings instance.

#### 1. Decide what should be protected
Add the KeepSecret attribute to the class or properties that should be kept secret when serializing
```c#
public class Poco
{
  public Poco()
  {
      IContainASecret = Guid.NewGuid().ToString();
      IWillBeSerializedInPlainText = Guid.NewGuid().ToString();
  }
  
  [KeepSecret]
  public string IContainASecret {get; set;}
  
  public string IWillBeSerializedInPlainText {get; set;}  
}
```

#### 2. Create a custom JsonSerializerSettings instance
```c#

// we are using a fixed key encryption provider here
// there's also a KeyVaultEncryptionProvider which supports key rotation
// you can provide your own by implementing the IEncryptionProvider interface and providing it 
// to the SecretContractResolver CTOR
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

The Json will look something like this:

```json
{
"IContainASecret":{
  "KeyIdentifier":"fixed;2fSE8uyF/lUIiqUGKRKaGHXoJzJI1lZ3rIF+BlIKH8o=",
  "Data":"x0F/1s2enXeBgKolEGnbTLG3nDZjZHbPO7N0g0WyA5iWW/hitohAbCs509jhrdqj",
  "Iv":"nyWfg1ez0qF6GayP3APnuA=="
},
"IWillBeSerializedInPlainText":"8c094773-e471-4bdb-8674-b8b3d2fd5e3d"
}
```
The property marked with [KeepSecret] has been encrypted, whereas the other property has been serialized as plain text.

#### 4. When Deserializing pass the JsonSerializerSettings instance to the Deserialize method
```c#
var instance = JsonConvert.Deserialize<Poco>(json, settings);
```
