
using System.Diagnostics;

namespace Framework.Core.Security.Cryptography
{
    /// <summary>
    /// 
    /// </summary>
    public static class Cryptographer
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        public static ICryptographer GetCryptographer(CryptoAlgorithm algorithm)
        {
            ICryptographer crypto = null;
            switch (algorithm)
            {
                case CryptoAlgorithm.DES:
                    crypto = new DESCryptographer();
                    break;

                case CryptoAlgorithm.RC2:
                    crypto = new RC2Cryptographer();
                    break;

                case CryptoAlgorithm.Rijndael:
                    crypto = new RijndaelCryptographer();
                    break;

                case CryptoAlgorithm.TripleDES:
                    crypto = new TripleDESCryptographer();
                    break;

                case CryptoAlgorithm.RSA:
                    crypto = new RSACryptorapher();
                    break;

				case CryptoAlgorithm.MD5:
					crypto = new MD5HashCryptographer();
					break;
				case CryptoAlgorithm.HMACMD5:
					crypto = new HMACMD5HashCryptographer();
					break;
				case CryptoAlgorithm.SHA1:
					crypto = new SHA1HashCryptographer();
					break;
                default:
                    Debug.Assert(false);
                    break;
            }
            Debug.Assert(crypto != null);
            return crypto;
        }

		/// <summary>
		/// 
		/// </summary>
		public static IHashCryptographer MD5
		{
			get
			{
				return GetCryptographer(CryptoAlgorithm.MD5);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static IHashCryptographer SHA1
		{
			get
			{
				return GetCryptographer(CryptoAlgorithm.SHA1);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static IHashCryptographer HMACMD5
		{
			get
			{
				return GetCryptographer(CryptoAlgorithm.HMACMD5);
			}
		}

        /// <summary>
        /// 
        /// </summary>
        public static ICryptographer DES
        {
            get
            {
                return GetCryptographer(CryptoAlgorithm.DES);
            }
        }

		/// <summary>
		/// 
		/// </summary>
		public static ICryptographer TripleDES
		{
			get
			{
				return GetCryptographer(CryptoAlgorithm.TripleDES);
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="algorithm"></param>
        /// <param name="encryptedBase64ConnectString"></param>
        /// <returns></returns>
        public static string Decrypt(CryptoAlgorithm algorithm, string encryptedBase64ConnectString)
        {
            ICryptographer cryptographer = GetCryptographer(algorithm);
            return cryptographer.Decrypt(encryptedBase64ConnectString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="algorithm"></param>
        /// <param name="plainConnectString"></param>
        /// <returns></returns>
        public static string Encrypt(CryptoAlgorithm algorithm, string plainConnectString)
        {
            ICryptographer cryptographer = GetCryptographer(algorithm);
            return cryptographer.Encrypt(plainConnectString);
        }
    }
}
