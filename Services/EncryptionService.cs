using System.Security.Cryptography;

namespace Hackathon2024API.Services
{
    public class EncryptionService
    {
        public EncryptionService() { }

        public readonly byte[] salt = new byte[] { 0xA0, 0x1A, 0xBB, 0xEA, 0x0B, 0x02, 0x12, 0x10 };
        public const int iterations = 1001;
        public void DecryptFile(Stream source, Stream destination, string password)
        {
            AesManaged aes = new AesManaged();
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, salt, iterations);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Mode = CipherMode.CBC;
            ICryptoTransform transform = aes.CreateDecryptor(aes.Key, aes.IV);

            using (CryptoStream cryptoStream = new CryptoStream(destination, transform, CryptoStreamMode.Write, true))//видит бог, я не хотел оставлять true для хранения стрима открытым
            {
                source.CopyTo(cryptoStream);
                //try
                //{
                //}
                //catch (CryptographicException exception)
                //{
                //    if (exception.Message == "Padding is invalid and cannot be removed.")
                //        throw new ApplicationException("Cryptographic Exception (!)", exception);
                //    else throw;
                //}
            }
        }
        public void EncryptFile(Stream source, Stream destination, string password)
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
                    source.CopyTo(cryptoStream);
                }
        }
    }
}
