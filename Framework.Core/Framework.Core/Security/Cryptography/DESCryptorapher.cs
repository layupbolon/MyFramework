
using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Framework.Core.Security.Cryptography
{
    /// <summary>
    /// 
    /// </summary>
	public class DESCryptographer : AbstractCryptographer
    {
        // Fields
        private static byte[] desIV = new byte[] { 0x1d, 0x87, 0x34, 9, 0x41, 3, 0x61, 0x62 };
        private static byte[] desKey = new byte[] { 1, 0x4d, 0x54, 0x22, 0x45, 90, 0x17, 0x2c };

		/// <summary>
		/// 
		/// </summary>
		public override CryptoAlgorithm CryptoAlgorithm
		{
			get { return CryptoAlgorithm.DES; }
		}

		public override string DoEncrypt(string plainText, string key, Encoding encoding, DataMode encryptedType)
		{
			MemoryStream stream = new MemoryStream(200);
			stream.SetLength(0L);
			byte[] bytes = Encoding.Unicode.GetBytes(plainText);
			DES des = new DESCryptoServiceProvider();
			CryptoStream stream2 = new CryptoStream(stream, des.CreateEncryptor(desKey, desIV), CryptoStreamMode.Write);
			stream2.Write(bytes, 0, bytes.Length);
			stream2.FlushFinalBlock();
			stream.Flush();
			stream.Seek(0L, SeekOrigin.Begin);
			byte[] buffer = new byte[stream.Length];
			stream.Read(buffer, 0, buffer.Length);
			stream2.Close();
			stream.Close();
			return Convert.ToBase64String(buffer, 0, buffer.Length);
		}

		public override string DoDecrypt(string encryptedText, string key, Encoding encoding, DataMode encryptedType)
		{
			MemoryStream stream = new MemoryStream(200);
			stream.SetLength(0L);
			byte[] buffer = Convert.FromBase64String(encryptedText);
			DES des = new DESCryptoServiceProvider();
			des.KeySize = 0x40;
			CryptoStream stream2 = new CryptoStream(stream, des.CreateDecryptor(desKey, desIV), CryptoStreamMode.Write);
			stream2.Write(buffer, 0, buffer.Length);
			stream2.FlushFinalBlock();
			stream.Flush();
			stream.Seek(0L, SeekOrigin.Begin);
			byte[] buffer2 = new byte[stream.Length];
			stream.Read(buffer2, 0, buffer2.Length);
			stream2.Close();
			stream.Close();
			return Encoding.Unicode.GetString(buffer2);
		}
    }
}