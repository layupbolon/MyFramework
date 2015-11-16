using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Utility
{
    /// <summary>
    /// Helper class for generating random values
    /// http://www.jonasjohn.de/snippets/csharp/random-helper-class.htm
    /// </summary>
    public static class RandomHelper
    {
        private static Random randomSeed = new Random();

        /// <summary>
        /// Generates a random string with the given length
        /// </summary>
        /// <param name="size">Size of the string</param>
        /// <param name="lowerCase">If true, generate lowercase string</param>
        /// <returns>Random string</returns>
        public static string RandomString(int size, bool lowerCase)
        {
            // StringBuilder is faster than using strings (+=)
            StringBuilder randStr = new StringBuilder(size);

            // Ascii start position (65 = A / 97 = a)
            int start = (lowerCase) ? 97 : 65;

            // Add random chars
            for (int i = 0; i < size; i++)
                randStr.Append((char)(26 * randomSeed.NextDouble() + start));

            return randStr.ToString();
        }

        /// <summary>
        /// Returns a random number.
        /// </summary>
        /// <param name="min">Minimal result</param>
        /// <param name="max">Maximal result</param>
        /// <returns>Random number</returns>
        public static int RandomNumber(int minimal, int maximal)
        {
            return randomSeed.Next(minimal, maximal);
        }

        /// <summary>
        /// Returns a random number.
        /// </summary>
        /// <param name="length">length</param>
        /// <returns>Random number</returns>
        public static string RandomNumberString(int length)
        {
            string randomString = string.Empty;
            if (length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    randomString = string.Format("{0}{1}", randomString, RandomNumber(0, 9));
                }
            }
            return randomString;
        }

        /// <summary>
        /// Returns a random boolean value
        /// </summary>
        /// <returns>Random boolean value</returns>
        public static bool RandomBool()
        {
            return (randomSeed.NextDouble() > 0.5);
        }

        /// <summary>
        /// Returns a random color
        /// </summary>
        /// <returns></returns>
        public static Color RandomColor()
        {
            return System.Drawing.Color.FromArgb(
                randomSeed.Next(256),
                randomSeed.Next(256),
                randomSeed.Next(256)
            );
        }
    }

}
