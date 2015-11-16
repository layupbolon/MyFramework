
using System.Text;

namespace Framework.Core.Security.Cryptography
{
    /// <summary>
	/// 哈希加密算法接口。
    /// </summary>
    public interface IHashCryptographer
    {
		CryptoAlgorithm CryptoAlgorithm { get; }

		string Encrypt(string plainText);
		string Encrypt(string plainText, DataMode encryptedType);
		string Encrypt(string plainText, Encoding encoding);
		string Encrypt(string plainText, Encoding encoding, DataMode encryptedType);

		string Encrypt(string plainText, string key);
		string Encrypt(string plainText, string key, DataMode encryptedType);
		string Encrypt(string plainText, string key, Encoding encoding);
		string Encrypt(string plainText, string key, Encoding encoding, DataMode encryptedType);

		bool IsMatch(string encryptedText, string plainText);
		bool IsMatch(string encryptedText, string plainText, DataMode encryptedType);
		bool IsMatch(string encryptedText, string plainText, Encoding encoding);
		bool IsMatch(string encryptedText, string plainText, Encoding encoding, DataMode encryptedType);

		bool IsMatch(string encryptedText, string plainText, string key);
		bool IsMatch(string encryptedText, string plainText, string key, DataMode encryptedType);
		bool IsMatch(string encryptedText, string plainText, string key, Encoding encoding);
		bool IsMatch(string encryptedText, string plainText, string key, Encoding encoding, DataMode encryptedType);
    }
}