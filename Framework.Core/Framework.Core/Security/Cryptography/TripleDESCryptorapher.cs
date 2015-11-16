
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Framework.Core.Security.Cryptography
{
    /// <summary>
	/// http://buchananweb.co.uk/security07.aspx
	/// http://www.yaosansi.com/post/1106.html
    /// </summary>
	public class TripleDESCryptographer : AbstractCryptographer
    {
        // Fields
		private const string publicKey = "RGVz25uIIYZDdxYCEexY1WQx";

		/// <summary>
		/// 
		/// </summary>
		public override CryptoAlgorithm CryptoAlgorithm
		{
			get { return CryptoAlgorithm.TripleDES; }
		}

		/// <summary>
		/// 
		/// </summary>
		protected override List<DataMode> AvailableDataModes
		{
			get
			{
				if (_availableDataModes == null)
				{
					_availableDataModes = new List<DataMode>();
				}

				if (_availableDataModes.Count <= 0)
				{
					_availableDataModes.Add(DataMode.Hex);
					_availableDataModes.Add(DataMode.Base64);
				}

				return _availableDataModes;
			}
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
			TripleDES tripleDes = new TripleDESCryptoServiceProvider();
			tripleDes.Key = StringToByte(publicKey, 24);
			tripleDes.IV = StringToByte(key, 8);
			byte[] desKey = tripleDes.Key;
			byte[] desIV = tripleDes.IV;

			ICryptoTransform encryptor = tripleDes.CreateEncryptor(desKey, desIV);

			string encryptedString = string.Empty;
			using (MemoryStream mStream = new MemoryStream())
			{
				CryptoStream cStream = new CryptoStream(mStream, encryptor, CryptoStreamMode.Write);

				// Write all data to the crypto stream and flush it.
				byte[] toEncryptedBytes = encoding.GetBytes(plainText);
				cStream.Write(toEncryptedBytes, 0, toEncryptedBytes.Length);
				cStream.FlushFinalBlock();

				// Get the encrypted array of bytes.
				byte[] encryptedBytes = mStream.ToArray();

				encryptedString = BytesToString(encryptedBytes, encoding, encryptedType);
			}

			return encryptedString;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="encryptedText"></param>
		/// <param name="key"></param>
		/// <param name="encoding"></param>
		/// <param name="encryptedType"></param>
		/// <returns></returns>
		public override string DoDecrypt(string encryptedText, string key, Encoding encoding, DataMode encryptedType)
		{
			TripleDES tripleDes = new TripleDESCryptoServiceProvider();
			tripleDes.Key = StringToByte(publicKey, 24);
			tripleDes.IV = StringToByte(key, 8);
			byte[] desKey = tripleDes.Key;
			byte[] desIV = tripleDes.IV;

			ICryptoTransform decryptor = tripleDes.CreateDecryptor(desKey, desIV);

			string decryptedString = string.Empty;
			byte[] encryptedBytes = StringToBytes(encryptedText, encoding, encryptedType);
			using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes))
			{
				using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
				{
					decryptedString = BytesToString(csDecrypt, encoding);
				}
			}

			return decryptedString;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="StringToConvert"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static byte[] StringToByte(string stringToConvert, int length)
		{
			char[] charArray = stringToConvert.ToCharArray();
			byte[] byteArray = new byte[length];
			int byteArrayLength = charArray.Length >= length ? length : charArray.Length;
			for (int i = 0; i < byteArrayLength; i++)
			{
				byteArray[i] = Convert.ToByte(charArray[i]);
			}

			return byteArray;
		}
    }
}