using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace BlogClustering.Lib.Extenders
{
    /// <summary>
    /// Extends objects with method for string conversions and comparison.
    /// </summary>
    public static class ObjectExtender
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static string ToXml(this object obj)
        {
            using (var writer = new StringWriter())
            {
                var serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(writer, obj);
                return writer.ToString();
            }
        }

        public static byte[] ToBinary(this object obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        public static string ToBase64(this object obj)
        {
            return Convert.ToBase64String(obj.ToBinary());
        }

        public static bool IsSameAs(this object objectA, object objectB)
        {
            if (objectA == null)
            {
                throw new ArgumentNullException(nameof(objectB));
            }

            return ReferenceEquals(objectA, objectB);
        }

        public static bool IsEqualTo(this object objectA, object objectB, ObjectComparisionMethod method, bool allowSameObjectComparision = false)
        {
            if (objectA == null)
            {
                throw new ArgumentNullException(nameof(objectB));
            }

            if (!allowSameObjectComparision && objectA.IsSameAs(objectB))
            {
                throw new ArgumentException("Comparing an object with itself is not allowed", nameof(objectB));
            }

            string stringA, stringB;

            switch (method)
            {
                case ObjectComparisionMethod.Json:
                    stringA = objectA.ToJson();
                    stringB = objectB.ToJson();
                    break;

                case ObjectComparisionMethod.Xml:
                    stringA = objectA.ToXml();
                    stringB = objectB.ToXml();
                    break;

                case ObjectComparisionMethod.Binary:
                    stringA = objectA.ToBase64();
                    stringB = objectB.ToBase64();
                    break;

                default:
                    throw new ArgumentException($"Invalid CompareMethod: '{method}'", nameof(method));
            }

            return string.Compare(stringA, stringB) == 0;
        }
    }
}
