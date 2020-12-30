using System.Data.Common;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data
{
    /// <summary>
    /// Provides extension methods for System.Data.Common.DbDataReader.
    /// </summary>
    public static class DbDataReaderExtensions
    {
        #region Get Value Safetly - Advantage of these method over the .NET provided is, If any column value is DBNull then that throws error, It will not throw error and give default value.
        /// <summary>
        /// Gets the value of the specified column as a Boolean.
        /// </summary>
        /// <param name="reader">The data reader to get the column value from.</param>
        /// <param name="name">The name of the column.</param>
        /// <returns>If value is DBNull then <see cref="default"/> value; otherwise, The value of the specified column.</returns>
        public static bool Boolean(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return reader.GetBoolean(name);
        }
        public static byte Byte(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return reader.GetByte(name);
        }
        public static long Bytes(this DbDataReader reader, string name, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return reader.GetBytes(name, dataOffset, buffer, bufferOffset, length);
        }
        public static char Char(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return reader.GetChar(name);
        }
        public static long Chars(this DbDataReader reader, string name, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return reader.GetChars(name, dataOffset, buffer, bufferOffset, length);
        }
        public static DateTime DateTime(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return reader.GetDateTime(name);
        }
        public static decimal Decimal(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return reader.GetDecimal(name);
        }
        public static double Double(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return reader.GetDouble(name);
        }
        public static T FieldValue<T>(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return reader.GetFieldValue<T>(name);
        }
        public static Task<T> FieldValueAsync<T>(this DbDataReader reader, string name, CancellationToken cancellationToken = default)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return reader.GetFieldValueAsync<T>(name, cancellationToken);
        }
        public static float Float(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return reader.GetFloat(name);
        }
        public static Guid Guid(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return reader.GetGuid(name);
        }
        public static short Int16(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return reader.GetInt16(name);
        }
        public static int Int32(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return reader.GetInt32(name);
        }
        public static long Int64(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return reader.GetInt64(name);
        }
        public static Stream Stream(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return reader.GetStream(name);
        }
        public static string String(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return reader.GetString(name);
        }
        public static TextReader TextReader(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return reader.GetTextReader(name);
        }
        public static object Value(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return reader.GetValue(name);
        }

        public static bool Boolean(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return reader.GetBoolean(ordinal);
        }
        public static byte Byte(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return reader.GetByte(ordinal);
        }
        public static long Bytes(this DbDataReader reader, int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return reader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
        }
        public static char Char(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return reader.GetChar(ordinal);
        }
        public static long Chars(this DbDataReader reader, int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return reader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);
        }
        public static DateTime DateTime(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return reader.GetDateTime(ordinal);
        }
        public static decimal Decimal(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return reader.GetDecimal(ordinal);
        }
        public static double Double(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return reader.GetDouble(ordinal);
        }
        public static T FieldValue<T>(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return reader.GetFieldValue<T>(ordinal);
        }
        public static Task<T> FieldValueAsync<T>(this DbDataReader reader, int ordinal, CancellationToken cancellationToken = default)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return reader.GetFieldValueAsync<T>(ordinal, cancellationToken);
        }
        public static float Float(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return reader.GetFloat(ordinal);
        }
        public static Guid Guid(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return reader.GetGuid(ordinal);
        }
        public static short Int16(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return reader.GetInt16(ordinal);
        }
        public static int Int32(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return reader.GetInt32(ordinal);
        }
        public static long Int64(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return reader.GetInt64(ordinal);
        }
        public static Stream Stream(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return reader.GetStream(ordinal);
        }
        public static string String(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return reader.GetString(ordinal);
        }
        public static TextReader TextReader(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return reader.GetTextReader(ordinal);
        }
        public static object Value(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return reader.GetValue(ordinal);
        }
        #endregion

        #region Value after conversion - Use this when want to Convert one DataType to Another while reading from DbDataReader
        public static bool GetAsBoolean(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return Convert.ToBoolean(reader.GetValue(name));
        }
        public static byte GetAsByte(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return Convert.ToByte(reader.GetValue(name));
        }
        public static char GetAsChar(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return Convert.ToChar(reader.GetValue(name));
        }
        public static DateTime GetAsDateTime(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return Convert.ToDateTime(reader.GetValue(name));
        }
        public static decimal GetAsDecimal(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return Convert.ToDecimal(reader.GetValue(name));
        }
        public static double GetAsDouble(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return Convert.ToDouble(reader.GetValue(name));
        }
        public static float GetAsFloat(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return Convert.ToSingle(reader.GetValue(name));
        }
        public static short GetAsInt16(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return Convert.ToInt16(reader.GetValue(name));
        }
        public static int GetAsInt32(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return Convert.ToInt32(reader.GetValue(name));
        }
        public static long GetAsInt64(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return Convert.ToInt64(reader.GetValue(name));
        }
        public static string GetAsString(this DbDataReader reader, string name)
        {
            if (reader.IsDBNull(name))
            {
                return default;
            }

            return Convert.ToString(reader.GetValue(name));
        }
        public static bool GetAsBoolean(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return Convert.ToBoolean(reader.GetBoolean(ordinal));
        }
        public static byte GetAsByte(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return Convert.ToByte(reader.GetBoolean(ordinal));
        }
        public static char GetAsChar(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return Convert.ToChar(reader.GetBoolean(ordinal));
        }
        public static DateTime GetAsDateTime(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return Convert.ToDateTime(reader.GetBoolean(ordinal));
        }
        public static decimal GetAsDecimal(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return Convert.ToDecimal(reader.GetBoolean(ordinal));
        }
        public static double GetAsDouble(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return Convert.ToDouble(reader.GetBoolean(ordinal));
        }
        public static float GetAsFloat(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return Convert.ToSingle(reader.GetBoolean(ordinal));
        }
        public static short GetAsInt16(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return Convert.ToInt16(reader.GetBoolean(ordinal));
        }
        public static int GetAsInt32(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return Convert.ToInt32(reader.GetBoolean(ordinal));
        }
        public static long GetAsInt64(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return Convert.ToInt64(reader.GetBoolean(ordinal));
        }
        public static string GetAsString(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default;
            }

            return Convert.ToString(reader.GetBoolean(ordinal));
        }
        #endregion
    }
}
