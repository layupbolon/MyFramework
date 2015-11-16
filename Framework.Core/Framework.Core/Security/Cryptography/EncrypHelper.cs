
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Framework.Core.Security.Cryptography
{
	/// <summary>
	/// 
	/// </summary>
	public static class EncrypHelper
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sourse"></param>
		/// <returns></returns>
		public static string EncrypString(string sourse)
		{
			return EncrypString(CryptoAlgorithm.DES, sourse);			
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="algorithm"></param>
		/// <param name="sourse"></param>
		/// <returns></returns>
		public static string EncrypString(CryptoAlgorithm algorithm, string sourse)
		{
			if (sourse == null)
			{
				return string.Empty;
			}

			return Cryptographer.Encrypt(algorithm, sourse);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sourse"></param>
		/// <returns></returns>
		public static string DecryptString(string sourse)
		{
			if (sourse == null)
			{
				return string.Empty;
			}
			sourse = sourse.Replace(" ", "+");
			return DecryptString(CryptoAlgorithm.DES, sourse);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="algorithm"></param>
		/// <param name="sourse"></param>
		/// <returns></returns>
		public static string DecryptString(CryptoAlgorithm algorithm, string sourse)
		{
			if (sourse == null)
			{
				return string.Empty;
			}

			return Cryptographer.Decrypt(algorithm, sourse);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string GetMD5(string s)
		{
			return GetMD5(s, "utf-8");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <param name="inputCharset"></param>
		/// <returns></returns>
		public static string GetMD5(string s, string inputCharset)
		{
			byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.GetEncoding(inputCharset).GetBytes(s));
			StringBuilder builder = new StringBuilder(0x20);
			for (int i = 0; i < buffer.Length; i++)
			{
				builder.Append(buffer[i].ToString("x").PadLeft(2, '0'));
			}
			return builder.ToString();
		}
	}
}
