using System;

namespace SecretSerializer.Encryption
{
    public class KeyIdentifierMismatchException : Exception
    {
        public KeyIdentifierMismatchException(string given, string expected) : base($"Key identifier {given} does not match the expected key identifier {expected}")
        {
        }
    }
}