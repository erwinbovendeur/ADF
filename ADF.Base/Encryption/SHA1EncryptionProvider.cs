using System.Security.Cryptography;
using System.Text;
using Adf.Core.Encryption;

namespace Adf.Base.Encryption
{
    public class SHA1EncryptionProvider : IEncryptionProvider
    {
        public string Encrypt(string value)
        {
            SHA1 sha1 = new SHA1Managed();

            byte[] sha1Bytes = Encoding.Default.GetBytes(value);

            byte[] cryString = sha1.ComputeHash(sha1Bytes);

            string sha1Str = string.Empty;

            for (int i = 0; i < cryString.Length; i++)
            {
                sha1Str += cryString[i].ToString("X2");
            }

            return sha1Str;
        }
    }
}
