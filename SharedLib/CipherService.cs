using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;

namespace SharedLib
{
    /// <summary>
    /// 
    /// </summary>
    public static class CipherService
    {
        static readonly Encoding encoding = Encoding.Unicode;
        static readonly int secret_key_size = 32;
        static readonly int iv_key_size = 16;

        /// <summary>
        /// 
        /// </summary>
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("android")]
        public static async Task<string> EncryptAsStringAsync(string clearText, string EncryptionKey, byte[] salt) => Convert.ToBase64String(await EncryptAsync(encoding.GetBytes(clearText), EncryptionKey, salt));

        /// <summary>
        /// 
        /// </summary>
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("android")]
        public static async Task<byte[]> EncryptAsync(byte[] clearBytes, string EncryptionKey, byte[] salt)
        {
            using Aes encryption = Aes.Create();
            using Rfc2898DeriveBytes pdb = new(EncryptionKey, salt);
            encryption.Key = pdb.GetBytes(secret_key_size);
            encryption.IV = pdb.GetBytes(iv_key_size);
            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, encryption.CreateEncryptor(), CryptoStreamMode.Write);
            await cs.WriteAsync(clearBytes, 0, clearBytes.Length);
            cs.Close();
            return ms.ToArray();
        }


        /// <summary>
        /// 
        /// </summary>
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("android")]
        public static async Task<string> DecryptAsStringAsync(string cipherText, string EncryptionKey, byte[] salt) => Convert.ToBase64String(await DecryptAsync(Convert.FromBase64String(cipherText.Replace(" ", "+")), EncryptionKey, salt));

        /// <summary>
        /// 
        /// </summary>
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("android")]
        public static async Task<byte[]> DecryptAsync(byte[] cipherBytes, string EncryptionKey, byte[] salt)
        {
            using Aes encryption = Aes.Create();
            using Rfc2898DeriveBytes pdb = new(EncryptionKey, salt);
            encryption.Key = pdb.GetBytes(secret_key_size);
            encryption.IV = pdb.GetBytes(iv_key_size);
            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, encryption.CreateDecryptor(), CryptoStreamMode.Write);
            await cs.WriteAsync(cipherBytes, 0, cipherBytes.Length);
            cs.Close();
            return ms.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("android")]
        public static async Task<MemoryStream> DecryptAsync(MemoryStream ms, string EncryptionKey, byte[] salt) => new MemoryStream(await DecryptAsync(ms.ToArray(), EncryptionKey, salt));

        /// <summary>
        /// 
        /// </summary>
        public static byte[] GenerateRandomEntropy(int sise)
        {
            byte[] randomBytes = new byte[sise];
            using RandomNumberGenerator rngCsp = RandomNumberGenerator.Create();
            rngCsp.GetBytes(randomBytes);
            return randomBytes;
        }
    }
}