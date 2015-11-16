
using System;
using System.Text;

namespace Framework.Core.Security.Cryptography
{
    /// <summary>
	/// 哈希加密算法实现基类。
    /// </summary>
	public abstract class AbstractHashCryptographer : AbstractCryptographer
	{
		/// <summary>
		/// 
		/// </summary>
		public override abstract CryptoAlgorithm CryptoAlgorithm { get; }

		/// <summary>
		/// 默认的加密类型。
		/// </summary>
		public override DataMode DefaultDataMode
		{
			get { return DataMode.Hex; }
		}

		#region [ Decrypt ]

		public override string DoEncrypt(string plainText, string key, Encoding encoding, DataMode encryptedType)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override string DoDecrypt(string encryptedText, string key, Encoding encoding, DataMode encryptedType)
		{
			throw new NotSupportedException();
		}

		#endregion
    }
}