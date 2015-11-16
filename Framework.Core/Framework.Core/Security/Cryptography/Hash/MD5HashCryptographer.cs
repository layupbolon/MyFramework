using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;


namespace Framework.Core.Security.Cryptography
{
	/// <summary>
	/// http://buchananweb.co.uk/security03.aspx
	/// </summary>
	public class MD5HashCryptographer : AbstractHashCryptographer
	{
		/// <summary>
		/// 
		/// </summary>
		public override CryptoAlgorithm CryptoAlgorithm
		{
			get { return CryptoAlgorithm.MD5; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="plainText"></param>
		/// <param name="encoding"></param>
		/// <param name="encryptedType"></param>
		/// <returns></returns>
		public override string DoEncrypt(string plainText, Encoding encoding, DataMode encryptedType)
		{
			byte[] data = encoding.GetBytes(plainText);
			byte[] result = new MD5CryptoServiceProvider().ComputeHash(data);
			return BytesToString(result,  encoding, encryptedType);
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
			throw new NotSupportedException();
		}
	}
}