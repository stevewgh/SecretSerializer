using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using SecretSerializer.Encryption;
using Xunit;

namespace SecretSerializer.Tests
{
    public class FixedKeyEncryptionproviderTests
    {
        private readonly FixedKeyEncryptionProvider sut;

        public FixedKeyEncryptionproviderTests()
        {
            sut = new FixedKeyEncryptionProvider(GenerateKeyBytes());
        }

        [Fact]
        public void Given_A_Fixed_Key_When_Encrypting_Then_Encryption_Has_Take_Place()
        {
            var data = Encoding.UTF8.GetBytes(nameof(FixedKeyEncryptionproviderTests));
            
            var secret = sut.Encrypt(data);

            secret.Data.Should().NotBeEquivalentTo(data);
        }

        [Fact]
        public void Given_A_Fixed_Key_When_Data_Is_Encrypted_Then_Decryption_Return_The_Unencrypted_Data()
        {
            var data = Encoding.UTF8.GetBytes(nameof(FixedKeyEncryptionproviderTests));
            
            var secret = sut.Encrypt(data);

            sut.Decrypt(secret).Should().BeEquivalentTo(data);
        }

        [Fact]
        public void Given_An_Incorrect_Key_Identifier_When_Decryption_Is_Attempted_Then_An_Exception_Is_Thrown()
        {
            var data = Encoding.UTF8.GetBytes(nameof(FixedKeyEncryptionproviderTests));
            var key = GenerateKeyBytes();
            var encryptionProviderWithDifferentKey = new FixedKeyEncryptionProvider(key);
            
            var secret = sut.Encrypt(data);

            Assert.Throws<KeyIdentifierMismatchException>(() => encryptionProviderWithDifferentKey.Decrypt(secret));
        }

        [Fact]
        public void Given_An_Incorrect_Key_When_Decryption_Is_Attempted_Then_Decryption_Does_Not_Return_The_Unencrypted_Data()
        {
            var data = Encoding.UTF8.GetBytes(nameof(FixedKeyEncryptionproviderTests));
            
            var secret = sut.Encrypt(data);

            //  make a new key appear to be the same by using the same identifier
            var incorrectKey = new Key(GenerateKeyBytes(), secret.KeyIdentifier);
            var encryptionProviderWithDifferentKey = new FixedKeyEncryptionProvider(incorrectKey);

            encryptionProviderWithDifferentKey.Decrypt(secret).Should().NotBeEquivalentTo(data);
        }

        [Fact]
        public void Given_A_Single_Key_When_Encrypting_Two_Secrets_Then_The_KeyIdentifier_Is_The_Same_For_Each_Secret()
        {
            var data = Encoding.UTF8.GetBytes(nameof(FixedKeyEncryptionproviderTests));
            
            var secret1 = sut.Encrypt(data);
            var secret2 = sut.Encrypt(data.Reverse().ToArray());

            secret1.KeyIdentifier.Should().Be(secret2.KeyIdentifier);
        }

        [Fact]
        public void Given_Two_Keys_When_Encrypted_Then_The_KeyIdentifier_Is_Unique_For_Each_Secret()
        {
            var data = Encoding.UTF8.GetBytes(nameof(FixedKeyEncryptionproviderTests));
            var provider1 = new FixedKeyEncryptionProvider(GenerateKeyBytes());
            var provider2 = new FixedKeyEncryptionProvider(GenerateKeyBytes());
            
            var secret1 = provider1.Encrypt(data);
            var secret2 = provider2.Encrypt(data);

            secret1.KeyIdentifier.Should().NotBe(secret2.KeyIdentifier);
        }

        private static byte[] GenerateKeyBytes()
        {
            var keyBytes = new byte[Key.KeySize];
            using (var rnd = RandomNumberGenerator.Create())
            {
                rnd.GetBytes(keyBytes);
            }
            return keyBytes;
        }
    }
}