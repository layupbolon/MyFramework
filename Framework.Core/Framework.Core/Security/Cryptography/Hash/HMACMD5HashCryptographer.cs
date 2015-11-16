using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

using Framework.Core.Utility;

namespace Framework.Core.Security.Cryptography
{
	/// <summary>
	/// http://buchananweb.co.uk/security01.aspx
	/// </summary>
	public class HMACMD5HashCryptographer : AbstractHashCryptographer
	{
		/// <summary>
		/// 
		/// </summary>
		public override CryptoAlgorithm CryptoAlgorithm
		{
			get { return CryptoAlgorithm.HMACMD5; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="plainText"></param>
		/// <param name="key"></param>
		/// <param name="encoding"></param>
		/// <param name="encryptedType"></param>
		/// <returns></returns>
		public override string DoEncrypt(string plainText, string key, Encoding encoding, DataMode encryptedType)
		{
			byte[] keyByte = encoding.GetBytes(key);
			HMACMD5 hmacMD5 = new HMACMD5(keyByte);

			byte[] messageBytes = encoding.GetBytes(plainText);
			byte[] hashMessage = hmacMD5.ComputeHash(messageBytes);

			return BytesToString(hashMessage, encoding, encryptedType);
		}
	}
}