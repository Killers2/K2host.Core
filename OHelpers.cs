/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2018-10-30                        | 
'| Use: General                                         |
' \====================================================/
*/
using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Reflection;
using System.Collections;
using System.Globalization;
using System.IO.Compression;
using System.Security.Claims;
using System.Linq.Expressions;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.ComponentModel;

using Microsoft.VisualBasic;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json;

using K2host.Core.Classes;
using K2host.Core.Enums;

namespace K2host.Core
{

    public static class OHelpers
    {

        #region Extentions

        public static long GetSize(this DirectoryInfo source)
        {
            return source.GetFiles().Sum(fi => fi.Length) + source.GetDirectories().Sum(di => di.GetSize());
        }

        public static void CopyTo(this DirectoryInfo source, DirectoryInfo target, Action<FileInfo> afterCopy, bool recursive = false, bool overwrite = false, string filter = "*.*")
        {

            Directory.CreateDirectory(target.FullName);

            source.GetFiles(filter).ForEach(fi => { 
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), overwrite);
                afterCopy.Invoke(fi);
            });

            if(recursive)
                source.GetDirectories().ForEach(di => { 
                    di.CopyTo(target.CreateSubdirectory(di.Name), afterCopy, recursive, overwrite, filter);
                });

        }

        public static IEnumerable<IEnumerable<T>> Split<T>(this ICollection<T> e, int length)
        {
            var output  = new List<List<T>>();
            var count   = (int)Math.Ceiling((double)e.Count / (double)length);

            for (int c = 0; c < count; c++)
            {
                var skip    = c * length;
                var take    = skip + length;
                var chunk   = new List<T>(length);

                for (int i = skip; i < take && i < e.Count; i++)
                    chunk.Add(e.ElementAt(i));

                output.Add(chunk);
            }

            return output;
        }

        public static void SetProtectedField(this object e, string fieldName, object value)
        {
            e.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance).SetValue(e, value);
        }

        public static T GetProtectedField<T>(this object e, string fieldName)
        {

            return (T)e.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(e);

        }

        public static I[] Filter<I>(this I[] e, Predicate<I> func)
        {
            List<I> filtered = new();
            e.ForEach(item => {
                bool include = func(item);
                if (include)
                    filtered.Add(item);
            });
            return filtered.ToArray();
        }

        public static bool Dispose<I>(this I[] e, out Exception error)
        {
            error = null;

            try
            {
                Array.Clear(e, 0, e.Length);
                e = Array.Empty<I>();
                e = null;
            }
            catch (Exception ex) {
                error = ex;
                return false;
            }

            return true;

        }

        public static bool Dispose<I>(this IEnumerable<I> e)
        {

            try
            {
                e.IsEnumerable();
                e = Array.Empty<I>();
                e = null;
            }
            catch (Exception)
            {
                return false;
            }

            return true;

        }

        public static bool IsAllowed(this long[] UsersPolicies, params long[] AllowedPolicies)
        {

            if (UsersPolicies.Length <= 0)
                return false;

            foreach (long Policy in AllowedPolicies)
                if (Array.BinarySearch(UsersPolicies, Policy) >= 0)
                    return true;

            return false;
        }

        public static string ArrayToString(this int[] e)
        {
            string result = string.Empty;
            try
            {
                for (int i = 0; i < e.Length; i++)
                    result += Convert.ToString(e[i]) + ',';

                result = result.Remove(result.Length - 1, 1);
            }
            catch { }
            return result;
        }

        public static string ArrayToString(this long[] e)
        {
            string result = string.Empty;
            try
            {
                for (int i = 0; i < e.Length; i++)
                    result += Convert.ToString(e[i]) + ',';

                result = result.Remove(result.Length - 1, 1);
            }
            catch { }
            return result;
        }

        public static string ArrayToString(this IEnumerable<long> e)
        {
            if (!e.Any())
                return string.Empty;

            string result = string.Empty;

            long[] work = e.ToArray();

            try
            {
                for (int i = 0; i < work.Length; i++)
                    result += Convert.ToString(work[i]) + ',';

                result = result.Remove(result.Length - 1, 1);
            }
            catch { }

            return result;
        }

        public static string ArrayToString(this string[] e, string det)
        {
            string result = string.Empty;
            try
            {
                for (int i = 0; i < e.Length; i++)
                    result += Convert.ToString(e[i]) + det;

                result = result.Remove(result.Length - 1, 1);
            }
            catch { }
            return result;
        }

        public static byte[] StrToByteArray(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static string ByteArrayToStr(this byte[] data)
        {
            return Encoding.UTF8.GetString(data, 0, data.Length);
        }
        
        public static I[] Sort<I>(this I[] Expression)
        {
            try
            {
                Array.Sort(Expression);
            }
            catch (Exception) { }

            return Expression;

        }

        public static int[] SplitInt(this string Expression, char Delimiter)
        {
            try
            {
                List<int> output = new();
                Expression
                    .Split(Delimiter, StringSplitOptions.RemoveEmptyEntries)
                    .ForEach(p => {
                        output.Add(Convert.ToInt32(p));
                    });

                return output.ToArray();
            }
            catch
            {

                return null;

            }
        }

        public static T FindObject<T>(this List<object> e)
        {

            T temp = default;

            try
            {
                IEnumerable<object> t = e.Where(x => x.GetType() == typeof(T));
                if (t.Any())
                    temp = (T)t.First();
            }
            catch { }

            return temp;
        }

        public static T FindObject<T>(this object[] e)
        {

            T temp = default;

            try
            {
                IEnumerable<object> t = e.Where(x => x.GetType() == typeof(T));
                if (t.Any())
                    temp = (T)t.First();
            }
            catch { }

            return temp;
        }

        public static Dictionary<string, string> ConvertQueryString(this string query)
        {

            if(string.IsNullOrEmpty(query))
                return new Dictionary<string, string>();

            if (query.StartsWith("?"))
                query = query.Remove(0, 1);

            string[] sections = query.Split(new char[] { '&' }, StringSplitOptions.None);

            return sections.ToDictionary(a => a.Substring(0, a.IndexOf("=")), a => a.Remove(0, a.IndexOf("=") + 1));
        }

        public static string JoinWith(this List<string> e, string delimiter)
        {
            string output = string.Empty;

            if (e.Count <= 0)
                return output;

            if (e.Count == 1)
                return e[0];

            foreach (string result in e)
                output += result + delimiter;

            output = output.Remove(output.Length - delimiter.Length, delimiter.Length);

            return output;
        }

        public static bool IsNumeric(this string text)
        {
            return text.All(c => Char.IsNumber(c));
        }

        public static void Each<T>(this IEnumerable<T> items, Predicate<T> predicate)
        {
            foreach (var item in items)
                predicate(item);
        }

        public static void Each<T>(this T[] items, Predicate<T> predicate)
        {
            foreach (var item in items)
                predicate(item);
        }

        public static void Each<T>(this List<T> items, Predicate<T> predicate)
        {
            foreach (var item in items)
                predicate(item);
        }

        public static void Each(this X509Certificate2Collection items, Predicate<X509Certificate2> predicate)
        {

            foreach (var item in items)
                predicate(item);
        }

        public static void ForEach(this X509Certificate2Collection items, Action<X509Certificate2> action)
        {

            foreach (var item in items)
                action(item);
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {

            foreach (var item in items)
                action(item);
        }

        public static void ForEach<T>(this T[] items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }

        public static void ForEach<T>(this List<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }

        public static int FindIndex<T>(this IEnumerable<T> items, Predicate<T> predicate)
        {
            int index = 0;
            foreach (var item in items)
            {
                if (predicate(item)) break;
                index++;
            }
            return index;
        }
       
        public static bool IsFilePath(this string e, out string[] path)
        {

            path = e.Split(new string[] { "/", "\\" }, StringSplitOptions.RemoveEmptyEntries);

            return path.Last().Contains(".");

        }

        public static bool IsDictionary(this object o)
        {
            if (o is PropertyInfo _info)
                return _info.PropertyType.Name.Contains("Dictionary");

            return o.GetType().Name.Contains("Dictionary");

        }

        public static bool IsList(this object o)
        {

            if (o is PropertyInfo _info)
                return _info.PropertyType.Name.Contains("List");

            return o.GetType().Name.Contains("List");

            //return o is IList &&
            //   o.GetType().IsGenericType &&
            //   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        public static bool IsEnumerable(this object o)
        {
            return o is IEnumerable &&
               o.GetType().IsGenericType &&
               o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(IEnumerable<>));
        }

        public static bool IsCollection(this object o)
        {
            return o is ICollection &&
               o.GetType().IsGenericType &&
               o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(ICollection<>));
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static byte[] SystemSubArray(this byte[] mydata, int index, int count)
        {
            if (count >= mydata.Length)
                return mydata;

            byte[] message = new byte[count];

            Buffer.BlockCopy(mydata, index, message, 0, count);

            return message;
        }

        public static byte[] SystemConcat(this byte[] data, byte[] val)
        {
            byte[] newdata = new byte[data.Length + val.Length];

            Buffer.BlockCopy(data, 0, newdata, 0, data.Length);

            Buffer.BlockCopy(val, 0, newdata, data.Length, val.Length);

            return newdata;
        }

        public static void Dispose(this byte[] e)
        {
            Array.Clear(e, 0, e.Length);
            Array.Resize(ref e, 0);
            e = null;
        }

        public static string FixedLength(this string e, int length)
        {
            try
            {
                return e + Strings.Space(length - e.Length);
            }
            catch { return e; }
        }

        public static string Push(this string e, int length)
        {
            return Microsoft.VisualBasic.Strings.Space(length) + e;
        }

        public static string Strech(this string e, int length)
        {
            return Microsoft.VisualBasic.Strings.StrDup(length, e);
        }

        public static string Displace(this string e, int length, string after)
        {

            string ret = string.Empty;

            if (e.Length > length)
                ret += Microsoft.VisualBasic.Strings.Left(e, (length - after.Length)) + after;
            else
                ret += e + Microsoft.VisualBasic.Strings.Space(length - e.Length);

            return ret;
        }
       
        public static string Ellipses(this string value, int maxChars)
        {
            const string ellipses = "...";
            return value.Length <= maxChars ? value : value.Substring(0, maxChars - ellipses.Length) + ellipses;
        }

        public static string AlignRight(this string e)
        {

            int c = (e.Length - e.Replace(" ", string.Empty).Length);

            string r = e.Replace(" ", string.Empty);

            return Microsoft.VisualBasic.Strings.Space(c) + r;

        }

        public static TKey FindKeyByValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary), "null dictionary");

            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
                if (value.Equals(pair.Value)) return pair.Key;

            throw new Exception("the value is not found in the dictionary");
        }

        public static TKey FindKeyByType<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Type value)
        {

            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary), "null dictionary");

            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {

                if (value == pair.Key.GetType())
                    return pair.Key;

            }

            throw new Exception("the value is not found in the dictionary");

        }

        public static TValue FindValueByType<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Type value)
        {

            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary), "null dictionary");

            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {

                if (value == pair.Value.GetType())
                    return pair.Value;

            }

            throw new Exception("the value is not found in the dictionary");

        }

        public static TKey FindKeyByType<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, string type)
        {

            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary), "null dictionary");

            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {

                if (type == pair.Key.GetType().Name)
                    return pair.Key;

            }

            throw new Exception("the value is not found in the dictionary");

        }

        public static TValue FindValueByType<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, string type)
        {

            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary), "null dictionary");

            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {

                if (type == pair.Value.GetType().Name)
                    return pair.Value;

            }

            throw new Exception("the value is not found in the dictionary");

        }

        public static void WriteToCsvFile(this DataTable dataTable, string filePath, bool escaped)
        {

            StringBuilder fileContent = new();

            foreach (DataColumn column in dataTable.Columns)
                fileContent.Append(column.ColumnName + ",");

            fileContent.Replace(",", System.Environment.NewLine, fileContent.Length - 1, 1);

            foreach (DataRow row in dataTable.Rows)
            {

                foreach (DataColumn column in dataTable.Columns)
                {
                    if (escaped)
                    {
                        fileContent.Append("\"" + row[column].ToString() + "\",");
                    }
                    else
                    {
                        fileContent.Append(row[column].ToString() + ",");
                    }
                }

                fileContent.Replace(",", System.Environment.NewLine, fileContent.Length - 1, 1);

            }

            File.WriteAllText(filePath, fileContent.ToString());

        }

        public static void WriteToXlsFile(this DataTable dataTable, string filePath)
        {

            StreamWriter wr = new(filePath);

            try
            {

                for (int i = 0; i < dataTable.Columns.Count; i++)
                    wr.Write(dataTable.Columns[i].ToString().ToUpper() + "\t");

                wr.WriteLine();

                for (int i = 0; i < (dataTable.Rows.Count); i++)
                {
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        if (dataTable.Rows[i][j] != null)
                        {
                            wr.Write(Convert.ToString(dataTable.Rows[i][j]) + "\t");
                        }
                        else
                        {
                            wr.Write("\t");
                        }
                    }

                    wr.WriteLine();

                }

                wr.Close();

            }
            catch (Exception)
            {
                throw;
            }

        }

        public static XmlElement XmlSerialize(this DataTable e)
        {

            XmlSerializer s = new(e.GetType());
            MemoryStream m = new();
            XmlDocument d = new();

            s.Serialize(m, e);
            m.Position = 0;
            d.Load(m);

            return d.DocumentElement;

        }

        public static XmlDocument XmlCustomSerialize(this DataTable e)
        {

            XmlDocument document = new();
            StringBuilder output = new();

            output.Append("<datatable>");

            foreach (DataRow r in e.Rows)
            {
                output.Append("<datarow>");

                foreach (DataColumn c in e.Columns)
                    output.Append("<" + c.ColumnName + ">" + r[c.ColumnName].ToString() + "</" + c.ColumnName + ">");

                output.Append("</datarow>");

            }

            output.Append("</datatable>");

            document.LoadXml(output.ToString());

            return document;

        }

        public static IEnumerable<int> WhereIs(this byte[] source, byte[] pattern)
        {
            for (int i = 0; i < source.Length; i++)
                if (source.Skip(i).Take(pattern.Length).SequenceEqual(pattern))
                    yield return i;
        }

        public static int WhereIsFirstIndex(this byte[] source, byte[] pattern)
        {

            for (int i = 0; i < source.Length; i++)
                if (source.Skip(i).Take(pattern.Length).SequenceEqual(pattern))
                    return i;

            return 0;

        }

        public static List<ArraySegment<byte>> SplitWith(this byte[] source, byte[] delimiter)
        {

            var result = new List<ArraySegment<byte>>();

            var segStart = 0;

            for (int i = 0, j = 0; i < source.Length; i++)
            {

                if (source[i] != delimiter[j])
                    continue;

                if (j++ != delimiter.Length - 1)
                    continue;

                var segLen = i - segStart - (delimiter.Length - 1);

                if (segLen > 0)
                    result.Add(new ArraySegment<byte>(source, segStart, segLen));

                segStart = i + 1;

                j = 0;
            }

            if (segStart < source.Length)
                result.Add(new ArraySegment<byte>(source, segStart, source.Length - segStart));

            return result;
        }

        public static byte[][] Split(this byte[] source, byte[] delimiter)
        {

            IEnumerable<byte[]> result = Array.Empty<byte[]>();

            int segStart = 0;

            for (int i = 0, j = 0; i < source.Length; i++)
            {

                if (source[i] != delimiter[j])
                    continue;

                byte[] check = source
                    .Skip(i)
                    .Take(delimiter.Length)
                    .ToArray();

                if (check.SequenceEqual(delimiter))
                {
                    int segLen = i - segStart; // - (delimiter.Length - 1);

                    if (segLen > 0)
                        result = result.Append(source.Skip(segStart).Take(segLen).ToArray());

                    segStart = i + 1;

                    j = 0;
                }

            }

            if (segStart < source.Length)
                result = result.Append(source.Skip(segStart).Take(source.Length - segStart).ToArray());

            return result.ToArray();

        }

        public static Delegate CreateDelegate(this MethodInfo methodInfo, object target)
        {
            Func<Type[], Type> getType;
            var isAction = methodInfo.ReturnType.Equals((typeof(void)));
            var types = methodInfo.GetParameters().Select(p => p.ParameterType);

            if (isAction)
            {
                getType = Expression.GetActionType;
            }
            else
            {
                getType = Expression.GetFuncType;
                types = types.Concat(new[] { methodInfo.ReturnType });
            }

            if (methodInfo.IsStatic)
            {
                return Delegate.CreateDelegate(getType(types.ToArray()), methodInfo);
            }

            return Delegate.CreateDelegate(getType(types.ToArray()), target, methodInfo.Name);
        }

        public static bool ChangeKey<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey oldKey, TKey newKey)
        {
            if (!dict.TryGetValue(oldKey, out TValue value))
                return false;

            dict.Remove(oldKey);
            dict[newKey] = value;  // or dict.Add(newKey, value) 
            return true;
        }

        public static Color Lighten(this Color color, float correctionfactory = 50f)
        {

            correctionfactory /= 100f;

            const float rgb255 = 255f;

            return Color.FromArgb(
                (int)((float)color.R + ((rgb255 - (float)color.R) * correctionfactory)),
                (int)((float)color.G + ((rgb255 - (float)color.G) * correctionfactory)),
                (int)((float)color.B + ((rgb255 - (float)color.B) * correctionfactory))
            );

        }

        public static Color Darken(this Color color, float correctionfactory = 50f)
        {

            const float hundredpercent = 100f;

            return Color.FromArgb(
                (int)(((float)color.R / hundredpercent) * correctionfactory),
                (int)(((float)color.G / hundredpercent) * correctionfactory),
                (int)(((float)color.B / hundredpercent) * correctionfactory)
            );

        }

        public static string[] Fracture(this string e, string delimiter)
        {
            return e.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
        }
        
        public static float Map(this float e, float in_min, float in_max, float out_min, float out_max)
        {
            return (e - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        public static int Map(this int e, int in_min, int in_max, int out_min, int out_max)
        {
            return ((int)e - (int)in_min) * (out_max - out_min) / ((int)in_max - (int)in_min) + out_min;
        }

        public static float Map(this int value, int in_min, int in_max, float out_min, float out_max)
        {
            return (value - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        public static int Map(this float value, float in_min, float in_max, int out_min, int out_max)
        {
            return ((int)value - (int)in_min) * (out_max - out_min) / ((int)in_max - (int)in_min) + out_min;
        }
        
        public static string AsString(this XmlDocument xmlDoc)
        {
            using StringWriter sw = new();
            using XmlTextWriter tx = new(sw);
            xmlDoc.WriteTo(tx);
            string strXmlText = sw.ToString();
            return strXmlText;
        }
        
        public static MemoryStream ToExpandableMemoryStream(this byte[] e)
        {
            var m = new MemoryStream();

            m.Write(e, 0, e.Length);

            m.Position = 0;

            m.Seek(0, SeekOrigin.Begin);

            return m;
        }

        public static MemoryStream ToMemoryStream(this byte[] e) 
        {
            return new MemoryStream(e);
        }

        private static void CheckIsEnum<T>(bool withFlags)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException(string.Format("Type '{0}' is not an enum", typeof(T).FullName));
            if (withFlags && !Attribute.IsDefined(typeof(T), typeof(FlagsAttribute)))
                throw new ArgumentException(string.Format("Type '{0}' doesn't have the 'Flags' attribute", typeof(T).FullName));
        }

        public static bool IsFlagSet<T>(this T value, T flag) where T : struct
        {
            CheckIsEnum<T>(true);
            long lValue = Convert.ToInt64(value);
            long lFlag = Convert.ToInt64(flag);
            return (lValue & lFlag) != 0;
        }

        public static IEnumerable<T> GetFlags<T>(this T value) where T : struct
        {
            CheckIsEnum<T>(true);
            foreach (T flag in Enum.GetValues(typeof(T)).Cast<T>())
            {
                if (value.IsFlagSet(flag))
                    yield return flag;
            }
        }

        public static T SetFlags<T>(this T value, T flags, bool on) where T : struct
        {
            CheckIsEnum<T>(true);
            long lValue = Convert.ToInt64(value);
            long lFlag = Convert.ToInt64(flags);
            if (on)
            {
                lValue |= lFlag;
            }
            else
            {
                lValue &= (~lFlag);
            }
            return (T)Enum.ToObject(typeof(T), lValue);
        }

        public static T SetFlags<T>(this T value, T flags) where T : struct
        {
            return value.SetFlags(flags, true);
        }

        public static T ClearFlags<T>(this T value, T flags) where T : struct
        {
            return value.SetFlags(flags, false);
        }

        public static T CombineFlags<T>(this IEnumerable<T> flags) where T : struct
        {
            CheckIsEnum<T>(true);
            long lValue = 0;
            foreach (T flag in flags)
            {
                long lFlag = Convert.ToInt64(flag);
                lValue |= lFlag;
            }
            return (T)Enum.ToObject(typeof(T), lValue);
        }

        public static string GetDescription<T>(this T value) where T : struct
        {
            CheckIsEnum<T>(false);
            string name = Enum.GetName(typeof(T), value);
            if (name != null) {
                FieldInfo field = typeof(T).GetField(name);
                if (field != null)
                    if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr)
                        return attr.Description;
            }
            return null;
        }

        #endregion

        #region Compression Methods and Functions

        public static void CompressFile(string sourceFile, string destinationFile)
        {
            if (!File.Exists(sourceFile)) { throw new FileNotFoundException(); }
            byte[] buffer;
            FileStream sourceStream = null;
            FileStream destinationStream = null;
            GZipStream compressedStream = null;
            try
            {
                sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                buffer = new byte[sourceStream.Length + 1];
                int checkCounter = sourceStream.Read(buffer, 0, buffer.Length);
                destinationStream = new FileStream(destinationFile, FileMode.OpenOrCreate, FileAccess.Write);
                compressedStream = new GZipStream(destinationStream, CompressionMode.Compress, true);
                compressedStream.Write(buffer, 0, buffer.Length);
            }
            catch
            { }
            if (sourceStream != null)
            {
                sourceStream.Close();
                sourceStream.Dispose();
            }
            if (compressedStream != null)
            {
                compressedStream.Close();
                compressedStream.Dispose();
            }
            if (destinationStream != null)
            {
                destinationStream.Close();
                destinationStream.Dispose();
            }
        }

        public static void DecompressFile(string sourceFile, string destinationFile)
        {
            if (!File.Exists(sourceFile)) { throw new FileNotFoundException(); }
            FileStream sourceStream = null;
            FileStream destinationStream = null;
            GZipStream decompressedStream = null;
            byte[] quartetBuffer;
            try
            {
                sourceStream = new FileStream(sourceFile, FileMode.Open);
                decompressedStream = new GZipStream(sourceStream, CompressionMode.Decompress, true);
                quartetBuffer = new byte[5];
                int position = Convert.ToInt32(sourceStream.Length) - 4;
                sourceStream.Position = position;
                sourceStream.Read(quartetBuffer, 0, 4);
                sourceStream.Position = 0;
                int checkLength = BitConverter.ToInt32(quartetBuffer, 0);
                byte[] buffer = new byte[checkLength + 101];
                int offset = 0;
                int total = 0;
                while (true)
                {
                    int bytesRead = decompressedStream.Read(buffer, offset, 100);
                    if (bytesRead == 0)
                        break;
                    offset += bytesRead;
                    total += bytesRead;
                }
                if (System.IO.File.Exists(destinationFile))
                    System.IO.File.Delete(destinationFile);
                destinationStream = new FileStream(destinationFile, FileMode.CreateNew, FileAccess.Write, FileShare.Write);
                destinationStream.Write(buffer, 0, total);
                destinationStream.Flush();
            }
            catch
            { }
            if (sourceStream != null)
            {
                sourceStream.Close();
                sourceStream.Dispose();
            }
            if (decompressedStream != null)
            {
                decompressedStream.Close();
                decompressedStream.Dispose();
            }
            if (destinationStream != null)
            {
                destinationStream.Close();
                destinationStream.Dispose();
            }
        }

        public static MemoryStream CompressData(MemoryStream SourceMemoryStream)
        {
            byte[] buffer;
            MemoryStream ReturnStream = new();
            GZipStream CompressedStream;
            buffer = SourceMemoryStream.ToArray();
            CompressedStream = new GZipStream(ReturnStream, CompressionMode.Compress, true);
            CompressedStream.Write(buffer, 0, buffer.Length);
            CompressedStream.Close();
            CompressedStream.Dispose();
            SourceMemoryStream.Close();
            SourceMemoryStream.Dispose();
            ReturnStream.Position = 0;
            return ReturnStream;
        }

        public static MemoryStream DecompressData(MemoryStream SourceMemoryStream)
        {
            MemoryStream ReturnStream = new();
            GZipStream DecompressedStream = null;
            byte[] quartetBuffer;
            SourceMemoryStream.Position = 0;
            try
            {
                DecompressedStream = new GZipStream(SourceMemoryStream, CompressionMode.Decompress, true);
                quartetBuffer = new byte[5];
                int position = Convert.ToInt32(SourceMemoryStream.Length) - 4;
                SourceMemoryStream.Position = position;
                SourceMemoryStream.Read(quartetBuffer, 0, 4);
                SourceMemoryStream.Position = 0;
                int checkLength = BitConverter.ToInt32(quartetBuffer, 0);
                byte[] buffer = new byte[checkLength + 101];
                int offset = 0;
                int total = 0;
                while (true)
                {
                    int bytesRead = DecompressedStream.Read(buffer, offset, 100);
                    if (bytesRead == 0)
                        break;
                    offset += bytesRead;
                    total += bytesRead;
                }
                ReturnStream.Write(buffer, 0, total);
                ReturnStream.Flush();
            }
            catch
            {
            }
            finally
            {
                if (SourceMemoryStream != null)
                    SourceMemoryStream.Close();
                if (DecompressedStream != null)
                    DecompressedStream.Close();
            }
            ReturnStream.Position = 0;
            return ReturnStream;
        }

        public static byte[] CompressData(byte[] SourceData)
        {
            MemoryStream rs = CompressData(new MemoryStream(SourceData));
            return rs.ToArray();
        }

        public static byte[] DecompressData(byte[] SourceData)
        {
            MemoryStream rs = DecompressData(new MemoryStream(SourceData));
            return rs.ToArray();
        }

        public static string MakeDocumentBase64(string filename)
        {
            FileStream fs = new(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader br = new(fs);
            byte[] b = br.ReadBytes((int)fs.Length);
            fs.Close();
            fs.Dispose();
            br.Close();
            return Convert.ToBase64String(b);
        }
        
        #endregion

        #region Arrays Methods and Functions

        public static List<int> SplitIntToList(string Expression, char Delimiter)
        {
            try
            {
                List<int> output = new();
                Expression
                    .Split(Delimiter, StringSplitOptions.RemoveEmptyEntries)
                    .ForEach(p => {
                        output.Add(Convert.ToInt32(p));
                    });

                return output;
            }
            catch
            {

                return new List<int>();

            }
        }

        public static List<long> SplitLongToList(string Expression, char Delimiter)
        {
            try
            {
                List<long> output = new();
                Expression
                    .Split(Delimiter, StringSplitOptions.RemoveEmptyEntries)
                    .ForEach(p => {
                        output.Add(Convert.ToInt64(p));
                    });

                return output;
            }
            catch
            {

                return new List<long>();

            }
        }

        public static long[] SplitLong(string Expression, char Delimiter)
        {

            List<long> output = new();

            Expression
                .Split(Delimiter, StringSplitOptions.RemoveEmptyEntries)
                .ForEach(p => { 
                    output.Add(Convert.ToInt64(p)); 
                });

            return output.ToArray();

        }

        public static string[] SplitAdvanced(string expression, string delimiter, string qualifier, bool ignoreCase)
        {
            bool _QualifierState = false;
            int _StartIndex = 0;
            System.Collections.ArrayList _Values = new();
            for (int _CharIndex = 0; _CharIndex <= expression.Length - 1; _CharIndex++)
            {
                if ((qualifier != null) && string.Compare(expression.Substring(_CharIndex, qualifier.Length), qualifier, ignoreCase) == 0)
                {
                    _QualifierState = !_QualifierState;
                }
                else if (!_QualifierState && (delimiter != null) && string.Compare(expression.Substring(_CharIndex, delimiter.Length), delimiter, ignoreCase) == 0)
                {
                    _Values.Add(expression[_StartIndex.._CharIndex]);
                    _StartIndex = _CharIndex + 1;
                }
            }
            if (_StartIndex < expression.Length)
                _Values.Add(expression[_StartIndex..]);
            string[] _returnValues = new string[_Values.Count];
            _Values.CopyTo(_returnValues);
            return _returnValues;
        }

        public static bool FindPolicy(int[] Policies, int IntPolicy)
        {
            bool boolresult = false;
            try
            {
                if (Array.BinarySearch(Policies, IntPolicy) >= 0)
                    boolresult = true;
            }
            catch { }
            return boolresult;
        }

        public static bool FindPolicy(long[] Policies, long IntPolicy)
        {
            bool boolresult = false;
            try
            {
                if (Array.BinarySearch(Policies, IntPolicy) >= 0)
                    boolresult = true;
            }
            catch { }
            return boolresult;
        }

        public static bool ArraySearch(ref int[] a, int index)
        {
            bool boolresult = false;
            try
            {
                for (int i = 0; i < a.Length; i++)
                {
                    if (a[i] == index)
                        return true;
                    Thread.Sleep(1);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return boolresult;
        }

        public static string ArrayToString(int start, int end)
        {

            string result = string.Empty;
            try
            {
                for (int i = start; i <= end; i++)
                    result += i.ToString() + ',';

                result = result.Remove(result.Length - 1, 1);

            }
            catch { }
            return result;

        }
       
        public static int SeekByte(byte[] Data, byte Value, int StartPointer, ref int Length)
        {
            int Pointer;
            //if (Length == null) { Length = 0; }
            for (Pointer = StartPointer; Pointer <= Data.Length - 1; Pointer++)
            {
                if (Data[Pointer] == Value)
                {
                    Length = Pointer - StartPointer;
                    return Pointer;
                }
            }

            return -1;
        }

        public static byte[] SeekByteChunk(byte[] Data, int StartPointer, int Length)
        {

            List<byte> output = new();

            int EndPointer = (StartPointer + Length);

            for (int Pointer = StartPointer; Pointer <= EndPointer - 1; Pointer++)
                output.Add(Data[Pointer]);

            return output.ToArray();
        }

        public static byte[] SeekByteChunk(MemoryStream e, int Length)
        {

            byte[] output = new byte[Length];

            e.Read(output, 0, Length);

            return output;

        }

        public static string GetDataDump(byte[] Data, long StartPointer = 0, long EndPointer = -1)
        {
            string DumpLine = string.Empty;
            string DataLine = string.Empty;
            string RetVal = string.Empty;
            int Address = 0;
            byte Pos = 0;

            string Ch;
            long i;

            if (EndPointer == -1)
                EndPointer = Data.Length - 1;

            for (i = Clamp(StartPointer, 0, Data.Length - 1); i <= Clamp(EndPointer, 0, Data.Length - 1); i++)
            {
                Pos += 1;
                if (Pos > 16)
                {
                    Pos = 1;
                    RetVal += Microsoft.VisualBasic.Strings.Format(Address, "0000") + "   " + DumpLine.Trim() + "   " + DataLine + ControlChars.CrLf;
                    DumpLine = "";
                    DataLine = "";
                    Address += 1;
                }

                Ch = Conversion.Hex(Data[i]);
                if (Microsoft.VisualBasic.Strings.Len(Ch) < 2)
                    Ch = "0" + Ch;
                DumpLine += Ch + (Pos == 8 ? "  " : " ");

                if ((Data[i] > 31 & Data[i] < 127) | (Data[i] > 127))
                {
                    DataLine += Microsoft.VisualBasic.Strings.Chr(Data[i]);
                }
                else
                {
                    DataLine += ".";
                }
            }
            DumpLine += Microsoft.VisualBasic.Strings.Space(((16 - Pos) * 3) + (Pos < 8 ? 1 : 0));
            RetVal += Microsoft.VisualBasic.Strings.Format(Address + 1, "0000") + "   " + DumpLine + "  " + DataLine;

            return RetVal;
        }

        public static byte[] CombineByteArrays(byte[] a, byte[] b)
        {
            byte[] ret = new byte[a.Length + b.Length];
            Buffer.BlockCopy(a, 0, ret, 0, a.Length);
            Buffer.BlockCopy(b, 0, ret, a.Length, b.Length);
            return ret;
        }

        public static T[] Combine<T>(T[] first, T[] second)
        {
            T[] ret = new T[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }

        public static T[] Combine<T>(params T[][] arrays)
        {
            T[] ret = new T[arrays.Sum(x => x.Length)];
            int offset = 0;
            foreach (T[] data in arrays)
            {
                Buffer.BlockCopy(data, 0, ret, offset, data.Length);
                offset += data.Length;
            }
            return ret;
        }

        public static IEnumerable<int> IndexOf(byte[] source, byte[] pattern)
        {
            for (int i = 0; i < source.Length; i++)
                if (source.Skip(i).Take(pattern.Length).SequenceEqual(pattern))
                    yield return i;
        }

        #endregion

        #region NullSafe Methods and Functions

        public static object NullSafeString(object arg, object returnIfnull = null)
        {
            object returnValue;
            if ((object.ReferenceEquals(arg, System.DBNull.Value)) || (arg == null) || (object.ReferenceEquals(arg, string.Empty)))
            {
                returnValue = returnIfnull;
            }
            else
            {
                try
                {
                    returnValue = arg;
                }
                catch
                {
                    returnValue = returnIfnull;
                }
            }
            return returnValue;
        }
       
        public static string NullSafeString(object arg)
        {
            string returnValue;
            if ((object.ReferenceEquals(arg, System.DBNull.Value)) || (arg == null) || (object.ReferenceEquals(arg, string.Empty)))
            {
                returnValue = string.Empty;
            }
            else
            {
                try
                {
                    returnValue = (string)arg;
                }
                catch
                {
                    returnValue = string.Empty;
                }
            }
            return returnValue;
        }

        public static DateTime NullSafeDateTime(object arg)
        {
            DateTime returnValue;
            if ((object.ReferenceEquals(arg, System.DBNull.Value)) || (arg == null) || (object.ReferenceEquals(arg, string.Empty)))
            {
                returnValue = DateTime.MinValue;
            }
            else
            {
                try
                {
                    returnValue = (DateTime)arg;
                }
                catch
                {
                    returnValue = DateTime.MinValue;
                }
            }
            return returnValue;
        }

        public static int NullSafeInt(object arg)
        {
            int returnValue;
            if ((object.ReferenceEquals(arg, System.DBNull.Value)) || (arg == null) || (object.ReferenceEquals(arg, string.Empty)))
            {
                returnValue = 0;
            }
            else
            {
                try
                {
                    returnValue = (int)arg;
                }
                catch
                {
                    returnValue = 0;
                }
            }
            return returnValue;
        }

        public static long NullSafeLong(object arg)
        {
            long returnValue;
            if ((object.ReferenceEquals(arg, System.DBNull.Value)) || (arg == null) || (object.ReferenceEquals(arg, string.Empty)))
            {
                returnValue = 0;
            }
            else
            {
                try
                {
                    returnValue = (long)arg;
                }
                catch
                {
                    returnValue = 0;
                }
            }
            return returnValue;
        }

        public static decimal NullSafeDecimal(object arg)
        {
            decimal returnValue;
            if ((object.ReferenceEquals(arg, System.DBNull.Value)) || (arg == null) || (object.ReferenceEquals(arg, string.Empty)))
            {
                returnValue = (decimal)0.00;
            }
            else
            {
                try
                {
                    returnValue = (decimal)arg;
                }
                catch
                {
                    returnValue = (decimal)0.00;
                }
            }
            return returnValue;
        }

        public static double NullSafeDouble(object arg)
        {
            double returnValue;
            if ((object.ReferenceEquals(arg, System.DBNull.Value)) || (arg == null) || (object.ReferenceEquals(arg, string.Empty)))
            {
                returnValue = 0.00D;
            }
            else
            {
                try
                {
                    returnValue = (double)arg;
                }
                catch
                {
                    returnValue = 0.00D;
                }
            }
            return returnValue;
        }

        public static short NullSafeShort(object arg)
        {
            short returnValue;
            if ((object.ReferenceEquals(arg, System.DBNull.Value)) || (arg == null) || (object.ReferenceEquals(arg, string.Empty)))
            {
                returnValue = 0;
            }
            else
            {
                try
                {
                    returnValue = (short)arg;
                }
                catch
                {
                    returnValue = 0;
                }
            }
            return returnValue;
        }

        #endregion

        #region Encryption Methods and Functions

        public static string EncryptB64(string strText)
        {
            return Convert.ToBase64String(StrToByteArray(strText));
        }

        public static string DecryptB64(string e)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(e));
        }

        public static string EncryptMd5(Stream e, bool dispose)
        {
            e.Position = 0;
            MD5CryptoServiceProvider c = new();
            byte[] b = c.ComputeHash(e);
            if (dispose) { e.Close(); e.Dispose(); }
            return BitConverter.ToString(b).Replace("-", string.Empty);
        }

        public static string EncryptMd5(string e)
        {
            MD5CryptoServiceProvider c = new();
            byte[] b = c.ComputeHash(Encoding.UTF8.GetBytes(e));
            c.Clear();
            return BitConverter.ToString(b).Replace("-", string.Empty);
        }

        public static string EncryptSha1(Stream e, bool dispose)
        {
            e.Position = 0;
            SHA1CryptoServiceProvider c = new();
            byte[] b = c.ComputeHash(e);
            if (dispose) { e.Close(); e.Dispose(); }
            return BitConverter.ToString(b).Replace("-", string.Empty);
        }

        public static string EncryptSha1(string e)
        {
            SHA1CryptoServiceProvider c = new();
            byte[] b = c.ComputeHash(Encoding.UTF8.GetBytes(e));
            c.Clear();
            return BitConverter.ToString(b).Replace("-", string.Empty);
        }

        public static string UniqueIdent(string suffix)
        {
            string begining = Guid.NewGuid().ToString();
            SHA1CryptoServiceProvider c = new();
            byte[] b = c.ComputeHash(System.Text.Encoding.UTF8.GetBytes(begining + suffix));
            c.Clear();
            return BitConverter.ToString(b).Replace("-", string.Empty);
        }

        public static string UniqueIdent()
        {
            string begining = Guid.NewGuid().ToString();
            string ending = DateTime.Now.Ticks.ToString();
            SHA1CryptoServiceProvider c = new();
            byte[] b = c.ComputeHash(Encoding.UTF8.GetBytes(begining + ending));
            c.Clear();
            return BitConverter.ToString(b).Replace("-", string.Empty);
        }

        public static string EncryptQ128(string val, OEncryptQ128Strength strength)
        {
            string a = Strings.StrReverse(EncryptB64(val));
            byte[] b = System.Text.UTF8Encoding.ASCII.GetBytes(a);
            string c = string.Empty;
            for (int x = 0; x < b.Length; x++) { c += "0x" + Conversion.Hex(b[x]); }
            string d = EncryptB64(c);
            byte[] e = System.Text.UTF8Encoding.ASCII.GetBytes(d);
            byte[] f = new byte[e.Length];
            bool g = false;
            for (int x = 0; x < f.Length; x++)
            {
                byte h;
                if (g) { h = e[x]; } else { h = (e[x] <= (int)strength) ? e[x] : (byte)(e[x] - (int)strength); }
                f[x] = h;
                if (g) { g = false; } else { g = true; }
            }
            string i = System.Text.UTF8Encoding.ASCII.GetString(f);
            i = Strings.StrReverse(i);
            return i;
        }

        public static string DecryptQ128(string val, OEncryptQ128Strength strength)
        {
            string a = Strings.StrReverse(val);
            byte[] b = System.Text.UTF8Encoding.ASCII.GetBytes(a);
            byte[] c = new byte[b.Length];
            bool d = false;
            for (int x = 0; x < b.Length; x++)
            {
                byte z;
                if (d) { z = b[x]; } else { z = (b[x] >= (255 - (int)strength)) ? b[x] : (byte)(b[x] + (int)strength); }
                c[x] = z;
                if (d) { d = false; } else { d = true; }
            }
            string e = System.Text.UTF8Encoding.ASCII.GetString(c);
            string f = DecryptB64(e);
            f = f.Remove(0, 2);
            string[] g = Strings.Split(f, "0x");
            byte[] h = new byte[g.Length];
            for (int x = 0; x < g.Length; x++) { h[x] = (byte)int.Parse(g[x], NumberStyles.HexNumber); }
            string i = System.Text.UTF8Encoding.ASCII.GetString(h);
            return DecryptB64(Strings.StrReverse(i));
        }
        
        public static string EncryptAes(string val, string key, byte[] salt)
        {

            byte[] a = Encoding.Unicode.GetBytes(val);
            string o = string.Empty;

            using (Aes e = Aes.Create())
            {

                Rfc2898DeriveBytes c = new(key, salt);
                e.Key   = c.GetBytes(32);
                e.IV    = c.GetBytes(16);

                using MemoryStream m = new();
                using (CryptoStream s = new(m, e.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    s.Write(a, 0, a.Length);
                    s.Close();
                }
                o = Convert.ToBase64String(m.ToArray());

            }

            return o;

        }

        public static string DecryptAes(string val, string key, byte[] salt)
        {
                      
            val = val.Replace(" ", "+"); //Base64 and unicode
            byte[] a;

            //Sometimes the format is missing the equals operator
            try { a = Convert.FromBase64String(val); } catch  {
                try { a = Convert.FromBase64String(val + "="); } catch {
                    try 
                    { 
                        a = Convert.FromBase64String(val + "=="); 
                    }  
                    catch(Exception) 
                    { 
                        throw; 
                    }
                }
            }

            using (Aes e = Aes.Create())
            {

                Rfc2898DeriveBytes c = new(key, salt);
                e.Key   = c.GetBytes(32);
                e.IV    = c.GetBytes(16);

                using MemoryStream m = new();
                using (CryptoStream s = new(m, e.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    s.Write(a, 0, a.Length);
                    s.Close();
                }
                val = Encoding.Unicode.GetString(m.ToArray());

            }

            return val;
        }
       
        public static byte[] EncryptOaepSha1(X509Certificate2 cert, byte[] data)
        {
            // GetRSAPublicKey returns an object with an independent lifetime, so it should be
            // handled via a using statement.
            // OAEP allows for multiple hashing algorithms, what was formermly just "OAEP" is now OAEP-SHA1.
            using RSA rsa = cert.GetRSAPublicKey();
            return rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA1);
        }

        public static byte[] DecryptOaepSha1(X509Certificate2 cert, byte[] data)
        {
            // GetRSAPrivateKey returns an object with an independent lifetime, so it should be
            // handled via a using statement.
            using RSA rsa = cert.GetRSAPrivateKey();
            return rsa.Decrypt(data, RSAEncryptionPadding.OaepSHA1);
        }
       
        public static string EncryptRSASha1(X509Certificate2 cert, string data)
        {
            byte[] lp = Encoding.UTF8.GetBytes(data);
            using RSA rsa = cert.GetRSAPublicKey();
            return Convert.ToBase64String(rsa.Encrypt(lp, RSAEncryptionPadding.OaepSHA1));
        }

        public static string DecryptRSASha1(X509Certificate2 cert, string data)
        {

            data = data.Replace(" ", "+"); //Base64 and unicode
            byte[] a;

            //Sometimes the format is missing the equals operator
            try { a = Convert.FromBase64String(data); }
            catch
            {
                try { a = Convert.FromBase64String(data + "="); }
                catch
                {
                    try
                    {
                        a = Convert.FromBase64String(data + "==");
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            using RSA rsa = cert.GetRSAPrivateKey();
            return Encoding.UTF8.GetString(rsa.Decrypt(a, RSAEncryptionPadding.OaepSHA1));
        }

        public static int GetRsaMaxDataLength(int keySize, bool optimalAsymmetricEncryptionPadding)
        {

            if (optimalAsymmetricEncryptionPadding)
                return ((keySize - 384) / 8) + 7;
            else
                return ((keySize - 384) / 8) + 37;
        }

        #endregion

        #region Json JWT Tokens and tools

        public static string JWTGenerateToken(string _secret, string _algorithm, DateTime _utcExpiryDateTime, ClaimsIdentity _claims)
        {

            byte[] key = Convert.FromBase64String(_secret);

            SymmetricSecurityKey secKey = new(key);            // add the bytes to the key obj and get the semetric sec obj.

            SecurityTokenDescriptor descriptor = new()        // this object create the the items in the second part of the json. 
            {
                Subject             = _claims,                                      // All claims on this token
                Expires             = _utcExpiryDateTime,                           // expiry date. :)
                SigningCredentials  = new SigningCredentials(secKey, _algorithm),   // Add the signature type / encyption method.
                IssuedAt            = DateTime.Now,                                 // The datetime created
            };

            JwtSecurityTokenHandler handler = new();        // create the handler
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);    // use the handler to create the token obj from the sec descriptor.

            return handler.WriteToken(token);

        }

        public static ClaimsPrincipal JWTGetPrincipal(string token, string _secret)
        {
            try
            {
                
                JwtSecurityTokenHandler tHandler = new();
                JwtSecurityToken        jwtToken = (JwtSecurityToken)tHandler.ReadToken(token);

                if (jwtToken == null)
                    return null;

                byte[] key = Convert.FromBase64String(_secret);

                TokenValidationParameters parameters = new()
                {
                    RequireExpirationTime   = true,
                    ValidateIssuer          = false,        // validate issuer can be used for server side stuff.
                    ValidateAudience        = false,        // validate the user with the token.
                    IssuerSigningKey        = new SymmetricSecurityKey(key)
                };

                ClaimsPrincipal principal = tHandler.ValidateToken(token, parameters, out SecurityToken securityToken);

                return principal;

            }
            catch(Exception)
            {
                return null;
            }

        }

        public static string JWTValidateTokenClaimValue(string token, string _secret, string _claimType)
        {

            ClaimsPrincipal principal = JWTGetPrincipal(token, _secret);     // Decrypt the token.

            if (principal == null)
                return null;

            ClaimsIdentity identity;

            try
            {
                identity = (ClaimsIdentity)principal.Identity;                  // get the ident of the token to access the elements. (payload)
            }
            catch (NullReferenceException)
            {
                return null;
            }

            Claim _claim = identity.FindFirst(_claimType);                      // read the element passed from the function. (_claimType)

            return _claim.Value;                                                // return the value.

        }

        public static List<Claim> JWTValidateTokenGetClaims(string token, string _secret)
        {

            ClaimsPrincipal principal = JWTGetPrincipal(token, _secret); // Decrypt the token.

            if (principal == null)
                return null;

            ClaimsIdentity identity;

            try
            {
                identity = (ClaimsIdentity)principal.Identity;  // get the ident of the token to access the elements. (payload)
            }
            catch (NullReferenceException)
            {
                return null;
            }

            return identity.Claims.ToList(); // return the list.

        }

        public static ClaimsIdentity JWTValidateTokenGetIdentity(string token, string _secret)
        {

            ClaimsPrincipal principal = JWTGetPrincipal(token, _secret);     // Decrypt the token.

            if (principal == null)
                return null;

            ClaimsIdentity identity;

            try
            {
                identity = (ClaimsIdentity)principal.Identity;                  // get the ident of the token to access the elements. (payload)
            }
            catch (NullReferenceException)
            {
                return null;
            }

            return identity;

        }

        public static string JWTDateTimeToJSON(DateTime givenDateTime)
        {
            string jsonDateTime = JsonConvert.SerializeObject(givenDateTime, new JsonSerializerSettings { DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ" });
            jsonDateTime = jsonDateTime.Replace("\"", "");
            //JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat };
            //jsonDateTime = JsonConvert.SerializeObject(givenDateTime, microsoftDateFormatSettings);
            //jsonDateTime = jsonDateTime.Replace("\"\\/Date(", "").Replace(")\\/\"", "");
            return jsonDateTime;
        }

        public static dynamic JWTJSONToDateTime(string jsonDateTime)
        {
            dynamic userDateTime = null;

            if (!string.IsNullOrEmpty(jsonDateTime))
            {
                jsonDateTime = "\"" + jsonDateTime + "\"";
                userDateTime = JsonConvert.DeserializeObject(jsonDateTime, new JsonSerializerSettings { DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ" });
                //JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat };
                //userDateTime = JsonConvert.DeserializeObject("\"\\/Date(" + jsonDateTime + ")\\/\"", microsoftDateFormatSettings);
            }

            return userDateTime;
        }

        #endregion

        #region DateTime Methods and Functions

        public static string GetDayName()
        {
            return System.DateTime.Today.DayOfWeek.ToString();
        }

        public static string GetDayInt(int intDay)
        {
            string Day = "";
            switch (intDay)
            {
                case 1:
                    Day = "Monday";
                    break;
                case 2:
                    Day = "Tuesday";
                    break;
                case 3:
                    Day = "Wednesday";
                    break;
                case 4:
                    Day = "Thursday";
                    break;
                case 5:
                    Day = "Friday";
                    break;
                case 6:
                    Day = "Saturday";
                    break;
                case 0:
                    Day = "Sunday";
                    break;
            }
            return Day;
        }

        public static string GetMonthName(int intMonth, bool longName)
        {

            switch (intMonth)
            {
                case 1: return (longName ? "January" : "Jan");
                case 2: return (longName ? "February" : "Feb");
                case 3: return (longName ? "March" : "Ma");
                case 4: return (longName ? "April" : "Apr");
                case 5: return (longName ? "May" : "May");
                case 6: return (longName ? "June" : "Jun");
                case 7: return (longName ? "July" : "Jul");
                case 8: return (longName ? "August" : "Aug");
                case 9: return (longName ? "September" : "Sep");
                case 10: return (longName ? "October" : "Oct");
                case 11: return (longName ? "November" : "Nov");
                case 12: return (longName ? "December" : "Dec");
                default:
                    break;
            }

            return string.Empty;

        }

        public static string GetHumanReadableDate(DateTime e)
        {
            try
            {

                string ret = string.Empty;

                string DayName = DayOfWeek.Monday.ToString();
                string DayNumber = "1";
                string MonthNumber = "1";
                string YearNumber = "1970";

                DayName = e.DayOfWeek.ToString();
                DayNumber = (e.Day.ToString().Length == 1 ? "0" + e.Day.ToString() : e.Day.ToString());
                MonthNumber = e.Month.ToString();
                YearNumber = e.Year.ToString();

                return DayName + " " + DayNumber + " " + GetMonthName(int.Parse(MonthNumber), false) + " " + YearNumber;

            }
            catch
            {

                return e.ToLongDateString() + " at " + e.ToShortTimeString();

            }

        }

        public static long UnixTime()
        {
            return DateAndTime.DateDiff("S", "01/01/1970", DateAndTime.Now);
        }

        public static string UnixTimeMilliSecond()
        {
            return DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss:ffff");
        }

        public static long DateTime2UnixTime(DateTime e)
        {
            return DateAndTime.DateDiff(DateInterval.Second, Convert.ToDateTime("01/01/1970"), e);
        }

        public static long DateTime2UnixTime(DateTime from, DateTime to)
        {
            return DateAndTime.DateDiff(DateInterval.Second, from, to);
        }

        public static DateTime UnixTime2DateTime(long UTCTimeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(UTCTimeStamp);
        }

        public static DateTime UnixTime2DateTime(long UTCTimeStamp, int UTCOffset)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(UTCTimeStamp + UTCOffset);
        }

        public static string Seconds2Time(int Seconds)
        {
            string r;
            int Hrs;
            int Min;
            int Sec;
            Hrs = (int)Math.Floor((double)(Seconds / 60 / 60));
            Min = (int)Math.Floor((double)((Seconds - Hrs * 60 * 60) / 60));
            Sec = Seconds - (Hrs * 60 * 60) - (Min * 60);
            r = Microsoft.VisualBasic.Strings.Format(Hrs, "00") + ":" + Microsoft.VisualBasic.Strings.Format(Min, "00") + ":" + Microsoft.VisualBasic.Strings.Format(Sec, "00");
            return r;
        }

        public static string ConvertTime(ulong TotalSeconds, int UpFormat)
        {
            // Type 0: 1d 4h 15m 47s
            // Type 1: 1 day, 4:15:47
            // Type 2: 1 day 4hrs 15mins 47secs
            long Seconds;
            long Minutes;
            long Hours;
            long Days;
           
            string DayString;
            string HourString;
            string MinuteString;
            string SecondString;
            
            Seconds = (long)(TotalSeconds % 60);
            Minutes = (long)(TotalSeconds / 60 % 60);
            Hours = (long)(TotalSeconds / 3600 % 24);
            Days = (long)(TotalSeconds / 3600 / 24);
           
            switch (UpFormat)
            {
                case 0:
                    DayString = "d ";
                    HourString = "h ";
                    MinuteString = "m ";
                    SecondString = "s";
                    break;
                case 1:
                    if (Days == 1) { DayString = " day, "; } else { DayString = " days, "; }
                    HourString = ":";
                    MinuteString = ":";
                    SecondString = "";
                    break;
                case 2:
                    if (Days == 1) { DayString = " day, "; } else { DayString = " days, "; }
                    if (Hours == 1) { HourString = " hr, "; } else { HourString = " hrs, "; }
                    if (Minutes == 1) { MinuteString = " min, "; } else { MinuteString = " mins, "; }
                    if (Seconds == 1) { SecondString = " sec, "; } else { SecondString = " secs, "; }
                    break;
                default:
                    DayString = "d ";
                    HourString = "h ";
                    MinuteString = "m ";
                    SecondString = "s";
                    break;
            }

            return Days switch
            {
                0 => Microsoft.VisualBasic.Strings.Format(Hours, "0") + HourString + Microsoft.VisualBasic.Strings.Format(Minutes, "00") + MinuteString + Microsoft.VisualBasic.Strings.Format(Seconds, "00") + SecondString,
                _ => Days + DayString + Microsoft.VisualBasic.Strings.Format(Hours, "0") + HourString + Microsoft.VisualBasic.Strings.Format(Minutes, "00") + MinuteString + Microsoft.VisualBasic.Strings.Format(Seconds, "00") + SecondString,
            };

        }
      
        public static long GetMilliSeconds(int minuites)
        {
            long m;
            m = (minuites * 60);
            return (m * 1000);
        }

        public static long GetSeconds(int milliseconds)
        {
            return (milliseconds / 1000);
        }

        #endregion

        #region Math Methods and Functions

        public static long Clamp(long Value, long Minimum, long Maximum)
        {
            if (Value < Minimum)
                Value = Minimum;
            if (Value > Maximum)
                Value = Maximum;
            return Value;
        }

        public static int Clamp(int Value, int Minimum, int Maximum)
        {
            if (Value < Minimum)
                Value = Minimum;
            if (Value > Maximum)
                Value = Maximum;
            return Value;
        }

        public static short Clamp(short Value, short Minimum, short Maximum)
        {
            if (Value < Minimum)
                Value = Minimum;
            if (Value > Maximum)
                Value = Maximum;
            return Value;
        }

        public static double Clamp(double Value, double Minimum, double Maximum)
        {
            if (Value < Minimum)
                Value = Minimum;
            if (Value > Maximum)
                Value = Maximum;
            return Value;
        }

        public static float Clamp(float Value, float Minimum, float Maximum)
        {
            if (Value < Minimum)
                Value = Minimum;
            if (Value > Maximum)
                Value = Maximum;
            return Value;
        }

        public static byte Clamp(byte Value, byte Minimum, byte Maximum)
        {
            if (Value < Minimum)
                Value = Minimum;
            if (Value > Maximum)
                Value = Maximum;
            return Value;
        }

        public static double Lerp(double ValueA, double ValueB, double Amount)
        {
            return ValueA + (ValueB - ValueA) * Amount;
        }

        public static float Lerp(float ValueA, float ValueB, float Amount)
        {
            return ValueA + (ValueB - ValueA) * Amount;
        }

        public static float Radians(float x)
        {
            return x * (float)Math.PI / 180f;
        }

        public static float Mod(float a, float b)
        {
            return a % b;
        }

        public static int GetRandomNumber(int MaxNumber, int MinNumber)
        {
            try
            {

                Random r = new(DateTime.Now.Millisecond);

                if (MinNumber > MaxNumber)
                {
                    int t = MinNumber;
                    MinNumber = MaxNumber;
                    MaxNumber = t;
                }

                return r.Next(MinNumber, MaxNumber);

            }
            catch
            {
                return 0;
            }
        }

        public static string GetPercent(int Value, int Maximum, out int Result)
        {
            int a = ((Value / Maximum) * 100);
            Result = a;
            return a + " %";
        }

        public static int GetPercent(int Value, int Maximum)
        {
            return ((Value / Maximum) * 100);
        }
      
        public static long GetPercent(long Value, long Maximum)
        {
            return ((Value / Maximum) * 100);
        }
        
        public static float GetPercent(float Value, float Maximum)
        {
            return ((Value / Maximum) * 100.00F);
        }

        #endregion

        #region Other Methods and Functions

        public static Type[] GetTypesInNamespace(string nameSpace)
        {
            return AppDomain.CurrentDomain
                     .GetAssemblies()
                     .SelectMany(t => t.GetTypes())
                     .Where(t => t.IsClass && t.IsPublic && t.Namespace == nameSpace)
                     .ToArray();
        }

        public static Type[] GetTypeFromDomain(string typename)
        {
            return AppDomain.CurrentDomain
                     .GetAssemblies()
                     .SelectMany(t => t.GetTypes())
                     .Where(t => t.IsClass && t.IsPublic && t.Name == typename)
                     .ToArray();
        }

        public static Type[] GetTypesInDomain()
        {
            return AppDomain.CurrentDomain
                     .GetAssemblies()
                     .SelectMany(t => t.GetTypes())
                     .Where(t => t.IsClass && t.IsPublic)
                     .ToArray();
        }

        public static byte[] ImageToByteArray(Image imageIn)
        {
            MemoryStream ms = new();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        public static bool ValidPhoneNumber(string phoneNumber, string countryCode, int digitLength)
        {
            //will match +61 or +61- or 0 or nothing followed by a nine digit number
            return Regex.Match(phoneNumber, @"^([\+]?" + countryCode + "[-]?|[0])?[1-9][0-9]{" + digitLength.ToString() + "}$").Success;
            //to vary this, replace 61 with an international code of your choice 
            //or remove [\+]?61[-]? if international code isn't needed
            //{8} is the number of digits in the actual phone number less one
        }

        public static bool ValidateEmailAddress(string EmailAddress)
        {

            string p = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(p, RegexOptions.IgnoreCase).IsMatch(EmailAddress);

        }

        public static XmlNode FindXmlNode(XmlNodeList list, string nodeName)
        {
            if (list.Count > 0)
            {
                foreach (XmlNode node in list)
                {
                    if (node.Name.Equals(nodeName)) return node;
                    if (node.HasChildNodes)
                    {
                        XmlNode nodeFound = FindXmlNode(node.ChildNodes, nodeName);
                        if (nodeFound != null)
                            return nodeFound;
                    }
                }
            }
            return null;
        }

        public static string FilenameFromLocation(string filename)
        {
            string SP = "";
            string SR = Microsoft.VisualBasic.Strings.StrReverse(filename);
            string SQ = filename[^(SR.IndexOf("\\"))..];
            int SO = Microsoft.VisualBasic.Strings.InStr(1, SP, ".");
            if (SO != 0)
            {
                SQ = filename.Substring(filename.Length - SR.IndexOf("\\"), SP.Length - 4);
            }
            return SQ;
        }

        public static string ConvertBytes(ulong Bytes)
        {
            if (Bytes >= 1099511627776L)
            {
                return Microsoft.VisualBasic.Strings.Format(Bytes / 1024 / 1024 / 1024 / 1024, "#0.00") + " TB";
            }
            else if (Bytes >= 1073741824)
            {
                return Microsoft.VisualBasic.Strings.Format(Bytes / 1024 / 1024 / 1024, "#0.00") + " GB";
            }
            else if (Bytes >= 1048576)
            {
                return Microsoft.VisualBasic.Strings.Format(Bytes / 1024 / 1024, "#0.00") + " MB";
            }
            else if (Bytes >= 1024)
            {
                return Microsoft.VisualBasic.Strings.Format(Bytes / 1024, "#0.00") + " KB";
            }
            else if (Bytes > 0 & Bytes < 1024)
            {
                return Conversion.Fix((double)Bytes) + " Bytes";
            }
            else
            {
                return "0 Bytes";
            }
        }

        public static string ConvertBytes(ulong Bytes, string type)
        {
            if (type == "t")
            {
                return Microsoft.VisualBasic.Strings.Format(Bytes / 1024 / 1024 / 1024 / 1024, "#0.00");
            }
            else if (type == "g")
            {
                return Microsoft.VisualBasic.Strings.Format(Bytes / 1024 / 1024 / 1024, "#0.00");
            }
            else if (type == "m")
            {
                return Microsoft.VisualBasic.Strings.Format(Bytes / 1024 / 1024, "#0.00");
            }
            else if (type == "k")
            {
                return Microsoft.VisualBasic.Strings.Format(Bytes / 1024, "#0.00");
            }
            else if (type == "b")
            {
                return Conversion.Fix((double)Bytes).ToString();
            }
            else
            {
                return "0";
            }
        }

        public static string GetOsVersion()
        {
            OperatingSystem osInfo;
            string sAns = "";
            osInfo = System.Environment.OSVersion;
            switch (osInfo.Platform)
            {
                case PlatformID.Win32Windows:
                    switch ((osInfo.Version.Minor))
                    {
                        case 0:
                            sAns = "Windows 95";
                            break;
                        case 10:
                            if (osInfo.Version.Revision.ToString() == "2222A")
                            {
                                sAns = "Windows 98 Second Edition";
                            }
                            else
                            {
                                sAns = "Windows 98";
                            }
                            break;
                        case 90:
                            sAns = "Windows Me";
                            break;
                    }
                    break;
                case PlatformID.Win32NT:
                    switch ((osInfo.Version.Major))
                    {
                        case 3:
                            sAns = "Windows NT 3.51";
                            break;
                        case 4:
                            sAns = "Windows NT 4.0";
                            break;
                        case 5:
                            if (osInfo.Version.Minor == 0)
                            {
                                sAns = "Windows 2000";
                            }
                            else if (osInfo.Version.Minor == 1)
                            {
                                sAns = "Windows XP";
                            }
                            else if (osInfo.Version.Minor == 2)
                            {
                                sAns = "Windows Server 2003";
                            }
                            break;
                        case 6:
                            if (osInfo.Version.Minor == 0)
                            {
                                sAns = "Windows Vista Ultimate";
                            }
                            else
                            {
                                sAns = "Unknown Windows Version";
                            }
                            break;
                    }
                    break;
            }
            return sAns;
        }

        public static string NormalizeString(string sString)
        {
            string[] P = sString.Split('-');
            string r = "";
            foreach (string ch in P)
            {
                r += (char)Conversion.Val("&H" + ch);
            }
            return r;
        }

        public static string Hex2Bin(string HexCode)
        {
            return (HexCode.ToUpper()) switch
            {
                "0" => "0000",
                "1" => "0001",
                "2" => "0010",
                "3" => "0011",
                "4" => "0100",
                "5" => "0101",
                "6" => "0110",
                "7" => "0111",
                "8" => "1000",
                "9" => "1001",
                "A" => "1010",
                "B" => "1011",
                "C" => "1100",
                "D" => "1101",
                "E" => "1110",
                "F" => "1111",
                _ => "0000",
            };
        }

        public static int Hex2Dec(string HexCode)
        {
            return (HexCode.ToUpper()) switch
            {
                "0" => 0,
                "1" => 1,
                "2" => 2,
                "3" => 3,
                "4" => 4,
                "5" => 5,
                "6" => 6,
                "7" => 7,
                "8" => 8,
                "9" => 9,
                "A" => 10,
                "B" => 11,
                "C" => 12,
                "D" => 13,
                "E" => 14,
                "F" => 15,
                _ => 0,
            };
        }

        public static OAge CalculateAge(DateTime startDate, DateTime endDate)
        {

            if (startDate.Date > endDate.Date)
                throw new ArgumentException("startDate cannot be higher then endDate", nameof(startDate));

            int years = endDate.Year - startDate.Year;
            int months;
            int days;

            // Check if the last year, was a full year.
            if (endDate < startDate.AddYears(years) && years != 0)
                years--;

            // Calculate the number of months.
            startDate = startDate.AddYears(years);

            if (startDate.Year == endDate.Year)
                months = endDate.Month - startDate.Month;
            else
                months = (12 - startDate.Month) + endDate.Month;

            // Check if last month was a complete month.
            if (endDate < startDate.AddMonths(months) && months != 0)
                months--;

            // Calculate the number of days.
            startDate = startDate.AddMonths(months);
            days = (endDate - startDate).Days;
            OAge ret = new()
            {
                Days = days,
                Months = months,
                Years = years
            };

            return ret;

        }

        public static T StringToEnum<T>(string name)
        {

            string[] names = Enum.GetNames(typeof(T));

            if (((IList)names).Contains(name))
            {

                return (T)Enum.Parse(typeof(T), name);

            }

            else
                return default;

        }

        public static string EmailCleaner(string emailAddreess)
        {

            try
            {
                return Regex.Replace(emailAddreess, @"[^a-zA-Z0-9_/-@.]", string.Empty, RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            catch (RegexMatchTimeoutException)
            {
                return emailAddreess;
            }

        }

        public static bool PostalCodeValidiator(string postalcode)
        {
            try
            {

                if (postalcode.Length > 10)
                    postalcode = postalcode.Remove(10);

                return Regex.IsMatch(postalcode, @"^((([A-PR-UWYZ][0-9])|([A-PR-UWYZ][0-9][0-9])|([A-PR-UWYZ][A-HK-Y][0-9])|([A-PR-UWYZ][A-HK-Y][0-9][0-9])|([A-PR-UWYZ][0-9][A-HJKSTUW])|([A-PR-UWYZ][A-HK-Y][0-9][ABEHMNPRVWXY]))\s?([0-9][ABD-HJLNP-UW-Z]{2})|(GIR)\s?(0AA))$", RegexOptions.None, TimeSpan.FromSeconds(1.5));

            }
            catch (Exception)
            {
                return false;
            }

        }

        public static byte[] GetFile(string file)
        {
            if (!File.Exists(file))
                return null;

            FileStream fs = new(file, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fs.Length];

            fs.Read(buffer, 0, buffer.Length);

            fs.Close();
            fs.Dispose();

            return buffer;
        }
       
        public static string GetXmlBuilderTemplate()
        {

            StringBuilder result = new();

            result.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            result.Append("<builder version=\"1.8\">");
            result.Append("<message>{0}</message>");
            result.Append("{1}");
            result.Append("</builder>");

            return result.ToString();

        }

        public static string DecimalPositionToDegreesPosition(double Position, OLongLat Type, ONmeaFormat Outputformat, int SecondResolution = 2) 
        {
            int     Degress;
            double  Minutes;
            double  Seconds;
            string  Direction   = string.Empty;
            double  tmpPos      = Position;

            if (tmpPos < 0)
                tmpPos = Position * -1;

            Degress = Convert.ToInt32(Math.Floor(tmpPos));
            Minutes = (tmpPos - Degress) * 60;
            Seconds = (Minutes - Math.Floor(Minutes)) * 60;
            Minutes = Math.Floor(Minutes);
            Seconds = Math.Round(Seconds, SecondResolution);

            switch (Type) {
                case OLongLat.Latitude:
                    if (Position < 0)
                        Direction = "S";
                    else
                        Direction = "N";
                    break;
                case OLongLat.Longitude:
                    if (Position < 0)
                        Direction = "W";
                    else
                        Direction = "E";
                    break;
            }

            return Outputformat switch
            {
                ONmeaFormat.NMEA => AddZeros(Degress, 3) + AddZeros(Minutes, 2) + AddZeros(Seconds, 2),
                ONmeaFormat.WithSigns => Degress + "°" + Minutes + "\"\"" + Seconds + "'" + Direction,
                _ => string.Empty,
            };

        }

        public static string AddZeros(double Value, int Zeros)
        {
            if (Math.Floor(Value).ToString().Length < Zeros)
                return Value.ToString().PadLeft(Zeros, System.Convert.ToChar("0"));
            else
                return Value.ToString();
        }

        public static double DistanceCalculator(double Lat1, double Long1, double Lat2, double Long2)
        {
            // Returns distance in Kilometers
            const double EarthRadiusKms = 6376.5;

            double dLat1InRad   = Lat1 * (Math.PI / 180);
            double dLong1InRad  = Long1 * (Math.PI / 180);
            double dLat2InRad   = Lat2 * (Math.PI / 180);
            double dLong2InRad  = Long2 * (Math.PI / 180);
            double dLongitude   = dLong2InRad - dLong1InRad;
            double dLatitude    = dLat2InRad - dLat1InRad;

            double a = Math.Pow(Math.Sin(dLatitude / 2), 2) + Math.Cos(dLat1InRad) * Math.Cos(dLat2InRad) * Math.Pow(Math.Sin(dLongitude / 2), 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return (EarthRadiusKms * c);

        }

        #endregion

    }

}
