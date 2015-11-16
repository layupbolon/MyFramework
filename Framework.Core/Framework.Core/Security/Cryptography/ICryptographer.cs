
using System.Text;

namespace Framework.Core.Security.Cryptography
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICryptographer : IHashCryptographer
    {
		string Decrypt(string encryptedText);
		string Decrypt(string encryptedText, DataMode encryptedType);
		string Decrypt(string encryptedText, Encoding encoding);
		string Decrypt(string encryptedText, Encoding encoding, DataMode encryptedType);

		string Decrypt(string encryptedText, string key);
		string Decrypt(string encryptedText, string key, DataMode encryptedType);
		string Decrypt(string encryptedText, string key, Encoding encoding);
		string Decrypt(string encryptedText, string key, Encoding encoding, DataMode encryptedType);
    }
}