using System;

namespace SecretSerializer
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class KeepSecretAttribute : Attribute
    {
    }
}
