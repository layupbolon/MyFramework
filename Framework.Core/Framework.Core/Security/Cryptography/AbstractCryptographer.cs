
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Framework.Core.Text;


namespace Framework.Core.Security.Cryptography
{
    /// <summary>
	/// 哈希加密算法实现基类。
    /// </summary>
    public abstract class AbstractCryptographer : ICryptographer
	{
		protected List<DataMode> _availableDataModes = new List<DataMode>();

		/// <summary>
		/// 
		/// </summary>
		public AbstractCryptographer()
		{
		}

		/// <summary>
		/// 加密算法类型。
		/// </summary>
		public abstract CryptoAlgorithm CryptoAlgorithm { get; }

		/// <summary>
		/// 默认的加密类型。
		/// </summary>
		public virtual DataMode DefaultDataMode
		{
			get { return DataMode.Base64; }
		}

		/// <summary>
		/// 可用的DataModes.
		/// </summary>
		protected virtual List<DataMode> AvailableDataModes
		{
			get
			{
				if (_availableDataModes == null)
				{
					_availableDataModes = new List<DataMode>();
				}

				if (_availableDataModes.Count <= 0)
				{					
					_availableDataModes.Add(DataMode.Plain);
					_availableDataModes.Add(DataMode.Hex);
					_availableDataModes.Add(DataMode.Base64);
				}

				return _availableDataModes;
			}
		}

		#region [ Encrypt ]

		/// <summary>
		/// 
		/// </summary>
		/// <param name="plainText"></param>
		/// <returns></returns>
		public string Encrypt(string plainText)
		{
			return Encrypt(plainText, Encoding.UTF8, DefaultDataMode);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="plainText"></param>
		/// <param name="encryptedType"></param>
		/// <returns></returns>
		public string Encrypt(string plainText, DataMode encryptedType)
		{
			return Encrypt(plainText, Encoding.UTF8, encryptedType);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="plainText"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		public string Encrypt(string plainText, Encoding encoding)
		{
			return Encrypt(plainText, encoding, DefaultDataMode);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="plainText"></param>
		/// <param name="encoding"></param>
		/// <param name="dataMode"></param>
		/// <returns></returns>
		public string Encrypt(string plainText, Encoding encoding, DataMode dataMode)
		{
			ValidateDataMode(dataMode);
			
			if (encoding == null)
			{
				encoding = Encoding.UTF8;
			}

			return DoEncrypt(plainText, encoding, dataMode);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="plainText"></param>
		/// <param name="encoding"></param>
		/// <param name="dataMode"></param>
		/// <returns></returns>
		public virtual string DoEncrypt(string plainText, Encoding encoding, DataMode dataMode)
		{
			return Encrypt(plainText, string.Empty, encoding, dataMode);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="plainText"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public string Encrypt(string plainText, string key)
		{
			return Encrypt(plainText, key, Encoding.UTF8);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="plainText"></param>
		/// <param name="key"></param>
		/// <param name="dataMode"></param>
		/// <returns></returns>
		public string Encrypt(string plainText, string key, DataMode dataMode)
		{
			return Encrypt(plainText, key, Encoding.UTF8, dataMode);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="plainText"></param>
		/// <param name="key"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		public string Encrypt(string plainText, string key, Encoding encoding)
		{
			return Encrypt(plainText, key, encoding, DefaultDataMode);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="plainText"></param>
		/// <param name="key"></param>
		/// <param name="encoding"></param>
		/// <param name="dataMode"></param>
		/// <returns></returns>
		public string Encrypt(string plainText, string key, Encoding encoding, DataMode dataMode)
		{
			ValidateDataMode(dataMode);
			return DoEncrypt(plainText, key, encoding, dataMode);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="plainText"></param>
		/// <param name="key"></param>
		/// <param name="encoding"></param>
		/// <param name="dataMode"></param>
		/// <returns></returns>
		public abstract string DoEncrypt(string plainText, string key, Encoding encoding, DataMode dataMode);	

		#endregion

		#region [ Decrypt ]

		/// <summary>
		/// 
		/// </summary>
		/// <param name="encryptedText"></param>
		/// <returns></returns>
		public string Decrypt(string encryptedText)
		{
			return Decrypt(encryptedText, Encoding.UTF8, DataMode.Base64);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="encryptedText"></param>
		/// <param name="dataMode"></param>
		/// <returns></returns>
		public string Decrypt(string encryptedText, DataMode dataMode)
		{
			return Decrypt(encryptedText, Encoding.UTF8, dataMode);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="encryptedText"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		public string Decrypt(string encryptedText, Encoding encoding)
		{
			return Decrypt(encryptedText, encoding, DataMode.Base64);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="encryptedText"></param>
		/// <param name="encoding"></param>
		/// <param name="dataMode"></param>
		/// <returns></returns>
		public virtual string Decrypt(string encryptedText, Encoding encoding, DataMode dataMode)
		{
			if (encoding == null)
			{
				encoding = Encoding.UTF8;
			}

			ValidateDataMode(dataMode);
			return DoDecrypt(encryptedText, string.Empty, encoding, dataMode);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="encryptedText"></param>
		/// <param name="encoding"></param>
		/// <param name="dataMode"></param>
		/// <returns></returns>
		public virtual string DoDecrypt(string encryptedText, Encoding encoding, DataMode dataMode)
		{
			return Decrypt(encryptedText, string.Empty, encoding, dataMode);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="encryptedText"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public string Decrypt(string encryptedText, string key)
		{
			return Decrypt(encryptedText, key, Encoding.UTF8, DataMode.Base64);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="encryptedText"></param>
		/// <param name="key"></param>
		/// <param name="encryptedType"></param>
		/// <returns></returns>
		public string Decrypt(string encryptedText, string key, DataMode encryptedType)
		{
			return Decrypt(encryptedText, key, Encoding.UTF8, encryptedType);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="encryptedText"></param>
		/// <param name="key"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		public string Decrypt(string encryptedText, string key, Encoding encoding)
		{
			return Decrypt(encryptedText, key, encoding, DataMode.Base64);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="encryptedText"></param>
		/// <param name="key"></param>
		/// <param name="encoding"></param>
		/// <param name="dataMode"></param>
		/// <returns></returns>
		public string Decrypt(string encryptedText, string key, Encoding encoding, DataMode dataMode)
		{
			if (encoding == null)
			{
				encoding = Encoding.UTF8;
			}

			ValidateDataMode(dataMode);
			return DoDecrypt(encryptedText, key, encoding, dataMode);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="encryptedText"></param>
		/// <param name="key"></param>
		/// <param name="encoding"></param>
		/// <param name="dataMode"></param>
		/// <returns></returns>
		public abstract string DoDecrypt(string encryptedText, string key, Encoding encoding, DataMode dataMode);

		#endregion

		#region [ IsMatch ]

		/// <summary>
		/// 
		/// </summary>
		/// <param name="encryptedText"></param>
		/// <param name="plainText"></param>
		/// <returns></returns>
		public bool IsMatch(string encryptedText, string plainText)
		{
			string encryptedTextToCompare = Decrypt(plainText);
			return (string.Compare(plainText, encryptedTextToCompare, false) == 0);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="encryptedText"></param>
		/// <param name="plainText"></param>
		/// <param name="encryptedType"></param>
		/// <returns></returns>
		public bool IsMatch(string encryptedText, string plainText, DataMode encryptedType)
		{
			return IsMatch(encryptedText, plainText, Encoding.UTF8, encryptedType);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="encryptedText"></param>
		/// <param name="plainText"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		public bool IsMatch(string encryptedText, string plainText, Encoding encoding)
		{
			return IsMatch(encryptedText, plainText, encoding, DefaultDataMode);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="encryptedText"></param>
		/// <param name="plainText"></param>
		/// <param name="encoding"></param>
		/// <param name="encryptedType"></param>
		/// <returns></returns>
		public virtual bool IsMatch(string encryptedText, string plainText, Encoding encoding, DataMode encryptedType)
		{
			string plainTextToCompare = Decrypt(encryptedText, encoding, encryptedType);
			return (string.Compare(plainText, plainTextToCompare, false) == 0);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="encryptedText"></param>
		/// <param name="plainText"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool IsMatch(string encryptedText, string plainText, string key)
		{
			return IsMatch(encryptedText, plainText, key, Encoding.UTF8, DefaultDataMode);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="encryptedText"></param>
		/// <param name="plainText"></param>
		/// <param name="key"></param>
		/// <param name="encryptedType"></param>
		/// <returns></returns>
		public bool IsMatch(string encryptedText, string plainText, string key, DataMode encryptedType)
		{
			return IsMatch(encryptedText, plainText, key, Encoding.UTF8, encryptedType);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="encryptedText"></param>
		/// <param name="plainText"></param>
		/// <param name="key"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		public bool IsMatch(string encryptedText, string plainText, string key, Encoding encoding)
		{
			return IsMatch(encryptedText, plainText, key, encoding, DefaultDataMode);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="encryptedText"></param>
		/// <param name="plainText"></param>
		/// <param name="key"></param>
		/// <param name="encoding"></param>
		/// <param name="encryptedType"></param>
		/// <returns></returns>
		public virtual bool IsMatch(string encryptedText, string plainText, string key, Encoding encoding, DataMode encryptedType)
		{
			string plainTextToCompare = Decrypt(encryptedText, key, encoding, encryptedType);
			return (string.Compare(plainText, plainTextToCompare, false) == 0);
		}

		#endregion

		#region [ Helper ]

		/// <summary>
		/// 校验加密算法数据模式。
		/// </summary>
		/// <param name="dataMode"></param>
		protected void ValidateDataMode(DataMode dataMode)
		{
			if (!AvailableDataModes.Contains(dataMode))
			{
                throw new NotSupportedException(string.Format("DataMode {0} & CrytoAlgorithm {1} not support!", dataMode.ToString(),CryptoAlgorithm.ToString()));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="str"></param>
		/// <param name="encoding"></param>
		/// <param name="dataMode"></param>
		/// <returns></returns>
		protected byte[] StringToBytes(string str, Encoding encoding, DataMode dataMode)
		{
			if (encoding == null)
			{
				encoding = Encoding.UTF8;
			}

			byte[] byteArray = new byte[0];
			switch(dataMode)
			{
				case DataMode.Plain:
					byteArray = encoding.GetBytes(str);
					break;
				case DataMode.Hex:
					byteArray = HexEncoding.GetBytes(str);
					break;
				case DataMode.Base64:
					byteArray = Convert.FromBase64String(str);
					break;
			}

			return byteArray;
		}

		/// <summary>
		/// 把字节数组转换成字符串。
		/// </summary>
		/// <param name="buff">要转换的字节数组。</param>
		/// <param name="encoding">编码类型。</param>
		/// <param name="dataMode"></param>
		protected string BytesToString(byte[] buff,  Encoding encoding, DataMode dataMode)
		{
			string bytesString = string.Empty;
			switch (dataMode)
			{
				case DataMode.Plain:
					bytesString = encoding.GetString(buff);
					break;
				case DataMode.Hex:
					bytesString = HexEncoding.ToString(buff);
					break;
				case DataMode.Base64:
					bytesString = Convert.ToBase64String(buff);
					break;
			}

			return bytesString;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		protected string BytesToString(CryptoStream buff, Encoding encoding)
		{
			int b = 0;
			List<byte> byteList = new List<byte>();
			do
			{
				b = buff.ReadByte();
				if (b != -1) byteList.Add((byte)b);

			} while (b != -1);		

			return encoding.GetString(byteList.ToArray());
		}

		#endregion
	}
}