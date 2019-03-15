using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDP
{
    public static class HelperMethods
    {
        public static Byte[] Prepend(this Byte[] bytes1, Byte[] bytes2)
        {
            Byte[] temp = new Byte[bytes1.Length + bytes2.Length];
            Array.Copy(bytes2, temp, bytes2.Length);
            Array.Copy(bytes1, 0, temp, bytes2.Length, bytes1.Length);
            return temp;
        }

        public static Byte[] Append(this Byte[] bytes1, Byte[] bytes2)
        {
            Byte[] temp = new Byte[bytes1.Length + bytes2.Length];
            Array.Copy(bytes1, temp, bytes1.Length);
            Array.Copy(bytes2, 0, temp, bytes1.Length, bytes2.Length);
            return temp;
        }

        public static String ToHexString(this Byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString().ToUpper();
        }

        public static T ByteArrayToType<T>(params byte[] input)
        {
            object retval = null;

            if (typeof(T) == typeof(UInt16))
            {
                retval = BitConverter.ToUInt16(input, 0);
            }
            if (typeof(T) == typeof(UInt32))
            {
                retval = BitConverter.ToUInt32(input, 0);
            }
            if (typeof(T) == typeof(float))
            {
                retval = BitConverter.ToSingle(input, 0);
            }
            if (typeof(T) == typeof(Int32))
            {
                retval = BitConverter.ToInt32(input, 0);
            }
            if (typeof(T) == typeof(Int16) || typeof(T) == typeof(short))
            {
                if (input.Length > 1)
                {
                    retval = BitConverter.ToInt16(input, 0);
                }
                else if (input.Length == 1)
                {
                    retval = BitConverter.ToInt16(new Byte[] { input[0], 0 }, 0);
                }
            }

            return (T)Convert.ChangeType(retval, typeof(T));
        }

        public static T[] CopySlice<T>(this T[] source, int index, int length, bool padToLength = false)
        {
            int n = length;
            T[] slice = null;

            if (source.Length < index + length)
            {
                n = source.Length - index;
                if (padToLength)
                {
                    slice = new T[length];
                }
            }

            if (slice == null) slice = new T[n];
            Array.Copy(source, index, slice, 0, n);
            return slice;
        }

        public static IEnumerable<T[]> Slices<T>(this T[] source, int count, bool padToLength = false)
        {
            for (var i = 0; i < source.Length; i += count)
                yield return source.CopySlice(i, count, padToLength);
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            ObservableCollection<T> retval = new ObservableCollection<T>();
            foreach (T item in source)
            {
                retval.Add(item);
            }
            return retval;
        }

        public static Byte[] GetByteArrayFromIndexes(int offset, Byte[] Source, params int[] indexes)
        {
            Byte[] retval = new Byte[indexes.Length];

            for (int i = 0; i < indexes.Length; i++)
            {
                retval[i] = Source[indexes[i] + offset];
            }
            return retval;
        }
    }
}
