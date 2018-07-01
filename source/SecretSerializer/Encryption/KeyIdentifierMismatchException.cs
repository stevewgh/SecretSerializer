using System;

namespace SecretSerializer.Encryption
{
    public class KeyIdentifierMismatchException : Exception
    {
        public KeyIdentifierMismatchException(string expected) : base($"Key identifier does not match the expected key identifier {expected}")
        {
        }
    }
}