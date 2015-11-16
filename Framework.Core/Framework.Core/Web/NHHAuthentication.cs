using Framework.Core.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Web
{
    public sealed class NHHAuthentication
    {
        public static string NHHAuthHeaderName = "X-NHH-Header";

        public static string Encrypt(string ticket)
        {
            return Cryptographer.DES.Encrypt(ticket);
        }

        public static string Decrypt(string encryptedTicket)
        {
            return Cryptographer.DES.Decrypt(encryptedTicket);
        }


    }
}
