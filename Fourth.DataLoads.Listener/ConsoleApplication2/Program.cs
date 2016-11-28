﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
namespace SerializationTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var count = 100000;
            var rnd = new Random(DateTime.UtcNow.GetHashCode());
            Console.WriteLine("Generating {0} arrays of data...", count);
            var arrays = new List<int[]>();
            for (int i = 0; i < count; i++)
            {
                var elements = rnd.Next(1, 100);
                var array = new int[elements];
                for (int j = 0; j < elements; j++)
                {
                    array[j] = rnd.Next();
                }
                arrays.Add(array);
            }
            Console.WriteLine("Test data generated.");
            var stopWatch = new Stopwatch();

            Console.WriteLine("Testing BinarySerializer...");
            var binarySerializer = new BinarySerializer();
            var binarySerialized = new List<byte[]>();
            var binaryDeserialized = new List<int[]>();

            stopWatch.Reset();
            stopWatch.Start();
            foreach (var array in arrays)
            {
                binarySerialized.Add(binarySerializer.Serialize(array));
            }
            stopWatch.Stop();
            Console.WriteLine("BinaryFormatter: Serializing took {0}ms.", stopWatch.Elapsed.TotalMilliseconds);

            stopWatch.Reset();
            stopWatch.Start();
            foreach (var serialized in binarySerialized)
            {
                binaryDeserialized.Add(binarySerializer.Deserialize<int[]>(serialized));
            }
            stopWatch.Stop();
            Console.WriteLine("BinaryFormatter: Deserializing took {0}ms.", stopWatch.Elapsed.TotalMilliseconds);


            Console.WriteLine();
            Console.WriteLine("Testing ProtoBuf serializer...");
            var protobufSerializer = new ProtoBufSerializer();
            var protobufSerialized = new List<byte[]>();
            var protobufDeserialized = new List<int[]>();

            stopWatch.Reset();
            stopWatch.Start();
            foreach (var array in arrays)
            {
                protobufSerialized.Add(protobufSerializer.Serialize(array));
            }
            stopWatch.Stop();
            Console.WriteLine("ProtoBuf: Serializing took {0}ms.", stopWatch.Elapsed.TotalMilliseconds);

            stopWatch.Reset();
            stopWatch.Start();
            foreach (var serialized in protobufSerialized)
            {
                protobufDeserialized.Add(protobufSerializer.Deserialize<int[]>(serialized));
            }
            stopWatch.Stop();
            Console.WriteLine("ProtoBuf: Deserializing took {0}ms.", stopWatch.Elapsed.TotalMilliseconds);

            Console.WriteLine();
            Console.WriteLine("Testing NetSerializer serializer...");
            var netSerializerSerializer = new ProtoBufSerializer();
            var netSerializerSerialized = new List<byte[]>();
            var netSerializerDeserialized = new List<int[]>();

            stopWatch.Reset();
            stopWatch.Start();
            foreach (var array in arrays)
            {
                netSerializerSerialized.Add(netSerializerSerializer.Serialize(array));
            }
            stopWatch.Stop();
            Console.WriteLine("NetSerializer: Serializing took {0}ms.", stopWatch.Elapsed.TotalMilliseconds);

            stopWatch.Reset();
            stopWatch.Start();
            foreach (var serialized in netSerializerSerialized)
            {
                netSerializerDeserialized.Add(netSerializerSerializer.Deserialize<int[]>(serialized));
            }
            stopWatch.Stop();
            Console.WriteLine("NetSerializer: Deserializing took {0}ms.", stopWatch.Elapsed.TotalMilliseconds);

            var xmlSerializerSerializer = new XmlkaSerializer();
            var xmlSerializerSerialized = new List<byte[]>();
            stopWatch.Reset();
            stopWatch.Start();
            foreach (var array in arrays)
            {
                xmlSerializerSerialized.Add(xmlSerializerSerializer.Serialize(array));
            }
            stopWatch.Stop();
            Console.WriteLine("XmlSerializer: Serializing took {0}ms.", stopWatch.Elapsed.TotalMilliseconds);

            Console.WriteLine("Press any key to end.");
            Console.ReadKey();
        }

        public class BinarySerializer
        {
            private static readonly BinaryFormatter Formatter = new BinaryFormatter();

            public byte[] Serialize(object toSerialize)
            {
                using (var stream = new MemoryStream())
                {
                    Formatter.Serialize(stream, toSerialize);
                    return stream.ToArray();
                }
            }

            public T Deserialize<T>(byte[] serialized)
            {
                using (var stream = new MemoryStream(serialized))
                {
                    var result = (T)Formatter.Deserialize(stream);
                    return result;
                }
            }
        }

        public class ProtoBufSerializer
        {
            public byte[] Serialize(object toSerialize)
            {
                using (var stream = new MemoryStream())
                {
                    ProtoBuf.Serializer.Serialize(stream, toSerialize);
                    return stream.ToArray();
                }
            }

            public T Deserialize<T>(byte[] serialized)
            {
                using (var stream = new MemoryStream(serialized))
                {
                    var result = ProtoBuf.Serializer.Deserialize<T>(stream);
                    return result;
                }
            }
        }

        public class NetSerializer
        {
            private static readonly NetSerializer Serializer = new NetSerializer();
            public byte[] Serialize(object toSerialize)
            {
                return Serializer.Serialize(toSerialize);
            }

            public T Deserialize<T>(byte[] serialized)
            {
                return Serializer.Deserialize<T>(serialized);
            }
        }
        public class XmlkaSerializer
        {
            public byte[] Serialize(object toSerialize)
            {
                XmlSerializer xsSubmit = new XmlSerializer(toSerialize.GetType());
                using (var stream = new MemoryStream())
                {
                    xsSubmit.Serialize(stream, toSerialize);
                    return stream.ToArray();
                }
            }
        }
    }
}