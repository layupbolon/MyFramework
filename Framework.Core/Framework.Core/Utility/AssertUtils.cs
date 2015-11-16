using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Utility
{
    /// <summary>
    /// Assertion utility methods that simplify things such as argument checks.
    /// </summary>
    /// <remarks>
    /// <p>
    /// Not intended to be used directly by applications.
    /// </p>
    /// </remarks>
    /// <author>Aleksandar Seovic</author>
    public static class AssertUtils
    {
        /// <summary>
        /// Checks the value of the supplied <paramref name="argument"/> and throws an
        /// <see cref="System.ArgumentNullException"/> if it is <see langword="null"/>.
        /// </summary>
        /// <param name="argument">The object to check.</param>
        /// <param name="name">The argument name.</param>
        /// <exception cref="System.ArgumentNullException">
        /// If the supplied <paramref name="argument"/> is <see langword="null"/>.
        /// </exception>
        public static void ArgumentNotNull(string name,object argument)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(name,string.Format("{0} can not be null!", name));
            }
        }

        /// <summary>
        /// Checks the value of the supplied <paramref name="argument"/> and throws an
        /// <see cref="System.ArgumentNullException"/> if it is <see langword="null"/>.
        /// </summary>
        /// <param name="argument">The object to check.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="message">
        /// An arbitrary message that will be passed to any thrown
        /// <see cref="System.ArgumentNullException"/>.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// If the supplied <paramref name="argument"/> is <see langword="null"/>.
        /// </exception>
        public static void ArgumentNotNull(string name, object argument, string message)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(name, message);
            }
        }

        /// <summary>
        /// Checks the value of the supplied string <paramref name="argument"/> and throws an
        /// <see cref="System.ArgumentNullException"/> if it is <see langword="null"/> or
        /// contains only whitespace character(s).
        /// </summary>
        /// <param name="argument">The string to check.</param>
        /// <param name="name">The argument name.</param>
        /// <exception cref="System.ArgumentNullException">
        /// If the supplied <paramref name="argument"/> is <see langword="null"/> or
        /// contains only whitespace character(s).
        /// </exception>
        public static void ArgumentHasText(string argument, string name)
        {
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentNullException(
                    name,
                    string.Format("{0:1} can not be null or empty!", name, argument));
            }
        }

        /// <summary>
        /// Checks the value of the supplied string <paramref name="argument"/> and throws an
        /// <see cref="System.ArgumentNullException"/> if it is <see langword="null"/> or
        /// contains only whitespace character(s).
        /// </summary>
        /// <param name="argument">The string to check.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="message">
        /// An arbitrary message that will be passed to any thrown
        /// <see cref="System.ArgumentNullException"/>.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// If the supplied <paramref name="argument"/> is <see langword="null"/> or
        /// contains only whitespace character(s).
        /// </exception>
        public static void ArgumentHasText(string name, string argument, string message)
        {
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentNullException(name, message);
            }
        }


        /// <summary>
        /// Assert a bool expression, throwing <code>InvalidOperationException</code>
        /// if the expression is <code>false</code>.
        /// </summary>
        /// <param name="expression">a boolean expression.</param>
        /// <param name="message">The exception message to use if the assertion fails</param>
        /// <exception cref="InvalidOperationException">if expression is <code>false</code></exception>
        public static void State(bool expression, string message)
        {
            if (!expression)
            {
                throw new InvalidOperationException(message);
            }
        }
    }
}
