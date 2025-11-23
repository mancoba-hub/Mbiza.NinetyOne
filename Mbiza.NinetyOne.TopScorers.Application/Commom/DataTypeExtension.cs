using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbiza.NinetyOne.TopScorers.Application.Commom
{
    public static class DataTypeExtension
    {
        /// <summary>
        /// Converts the specified string representation of a number to its 32-bit signed integer equivalent. Returns 0
        /// if the conversion fails.
        /// </summary>
        /// <remarks>This method does not throw an exception if the conversion fails. If <paramref
        /// name="value"/> is null, empty, or not a valid integer, the method returns 0.</remarks>
        /// <param name="value">The string containing the number to convert. Can be in any format accepted by <see cref="int.TryParse"/>.</param>
        /// <returns>A 32-bit signed integer equivalent to the number contained in <paramref name="value"/>; or 0 if <paramref
        /// name="value"/> is not a valid integer representation.</returns>
        public static int ToInt32(string value)
        {
            if (int.TryParse(value, out int result))
            {
                return result;
            }
            return 0;
        }
    }
}
