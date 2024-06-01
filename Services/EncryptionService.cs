using System.IO.Compression;
using System.Security.Cryptography;

namespace Hackathon2024API.Services
{
    public class EncryptionCompressionService
    {
        public EncryptionCompressionService() { }

        public readonly byte[] salt = new byte[] { 0xA0, 0x1A, 0xBB, 0xEA, 0x0B, 0x02, 0x12, 0x10 };
        public const int iterations = 1001;
        public void DecompressFile(Stream source, Stream destination, string password)
        {

            using (GZipStream decompressionStream = new GZipStream(source, CompressionMode.Decompress))
            {
                decompressionStream.CopyTo(destination);
            }
        }
        public void DecryptDecompressFile(Stream source, Stream destination, string password)
        {
            AesManaged aes = new AesManaged();
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, salt, iterations);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Mode = CipherMode.CBC;
            ICryptoTransform transform = aes.CreateDecryptor(aes.Key, aes.IV);

            using (CryptoStream cryptoStream = new CryptoStream(source, transform, CryptoStreamMode.Read))
            {
                using (GZipStream decompressionStream = new GZipStream(cryptoStream, CompressionMode.Decompress))
                {
                    decompressionStream.CopyTo(destination);
                }
            }
        }
        public void CompressFile(Stream source, Stream destination, string password)
        {
            using (GZipStream compressionStream = new GZipStream(destination, CompressionLevel.SmallestSize))
            {
                source.CopyTo(compressionStream);
                //compressionStream.CopyTo(cryptoStream);
            }
        }
        public void EncryptCompressFile(Stream source, Stream destination, string password)
        {
        AesManaged aes = new AesManaged();
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, salt, iterations);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Mode = CipherMode.CBC;
            ICryptoTransform transform = aes.CreateEncryptor(aes.Key, aes.IV);

            using (CryptoStream cryptoStream = new CryptoStream(destination, transform, CryptoStreamMode.Write))
            {
                using (GZipStream compressionStream = new GZipStream(cryptoStream, CompressionLevel.SmallestSize))
                {
                    source.CopyTo(compressionStream);
                    //compressionStream.CopyTo(cryptoStream);
                }
            }
        }
    }
}
