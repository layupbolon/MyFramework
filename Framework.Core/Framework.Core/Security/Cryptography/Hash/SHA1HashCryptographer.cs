using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

using Framework.Core.Utility;

namespace Framework.Core.Security.Cryptography
{
	/// <summary>
	/// http://buchananweb.co.uk/security03.aspx
	/// </summary>
	public class SHA1HashCryptographer : AbstractHashCryptographer
	{
		/// <summary>
		/// 
		/// </summary>
		public override CryptoAlgorithm CryptoAlgorithm
		{
			get { return CryptoAlgorithm.SHA1; }
		}

		public override string DoEncrypt(string plainText, Encoding encoding, DataMode encryptedType)
		{
            AssertUtils.ArgumentNotNull("encoding", encoding);

			byte[] data = encoding.GetBytes(plainText);
			byte[] result = new SHA1CryptoServiceProvider().ComputeHash(data);
			return BytesToString(result, encoding, encryptedType);
		}

		public override string DoEncrypt(string plainText, string key, Encoding encoding, DataMode encryptedType)
		{
			throw new NotSupportedException();
		}
	}
}