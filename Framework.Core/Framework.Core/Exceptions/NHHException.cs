using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Exceptions
{
    /// <summary>
    /// The exception type for the NHH system.
    /// A base class for all exceptions that will be throw when an error has occurred in NHH system.  
    /// </summary>
    [Serializable]
    public class NHHException : Exception
    {
        public NHHException()
            : base("An error has occurred in NHH!")
        {
        }

        public NHHException(string message)
            : base(message)
        {
        }

        public NHHException(Exception innerException)
            : base("An error has occurred in NHH!", innerException)
        {
        }

        public NHHException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public NHHException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
