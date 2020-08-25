using System;
using Abismus;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Abismus.CodeGeneration
{
    using Abismus.Signature;

    class Program
    {
        static void Main(string[] args)
        {
            var dels = typeof(Node.Dels);

            var members = dels.GetMembers(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public);

            var types = new List<Type>() {
                typeof(bool),
                typeof(int),
                typeof(float),
                typeof(double),
                typeof(decimal),
                typeof(DateTime),
                typeof(string),
                typeof(bool?),
                typeof(int?),
                typeof(float?),
                typeof(double?),
                typeof(decimal?),
                typeof(DateTime?)
            };

            var prop1 = new Property<Type>("InputTypeProperty", (Type t) => t.FullName ?? "(Unnamed type)", types);

            var par1 = new SpacePartition<Type>(50, prop1, PropIdMaps.PositionPropIdMap);

            var prop2 = new Property<Type>("OutputTypeProperty", (Type t) => t.FullName ?? "(Unnamed type)", types);

            var par2 = new SpacePartition<Type>(50, prop2, PropIdMaps.PositionPropIdMap);

            var floats = new List<float>() {
                3.2f,
                4.45f,
                6.192f,
                2.1f,
                3.32f
            };

            var prop3 = new Property<float>("SomeNumber", (float t) => t.ToString(), floats);

            var par3 = new SpacePartition<float>(30, prop3, PropIdMaps.PositionPropIdMap);

            var space = new Space()
                .WithPartition(par1)
                .WithPartition(par2)
                .WithPartition(par3);

            var en = space.Enumerate();

            string appDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Abismus");

            string filePath = Path.Combine(appDir, "Space_1.txt");

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                space.WriteTo(fileStream);
            }

            //if (Types.Count > propIdsHalfLength) throw new InvalidOperationException("PropIds is short. Should be double the " + 
            //    "length of Types");
            // Map values of the property under consideration, Type, to the expressed values in Types.
            //InputTypesValues = new Dictionary<Type, ulong>();
            //OutputTypesValues = new Dictionary<Type, ulong>();

            //static IEnumerable<(ulong, TProp)> 
            //    GetSpace<TProp>(int spaceSize, int numberProps, IEnumerable<TProp> propValues, Func<int, ulong> propIdMap)
            //{
            //    int spaceSizeFloorHalf = spaceSize.FloorHalf();

            //    //using var writer = new StreamWriter(stream, System.Text.Encoding.UTF8);
            //    for (int i = 1; i <= numberProps; ++i)
            //    {
            //        int propSpacePos = spaceSizeFloorHalf * (i - 1);
            //        for (int j = 0; j < propValues.Count(); ++j)
            //        {
            //            //if (propValues.ElementAt(j) == default) continue;
            //            ulong propId = propIdMap(j + propSpacePos);
            //            yield return (propId, propValues.ElementAt(j));
            //        }
            //    }

            //    //// Index input types with values in the first PropIds subrange.
            //    //// Input type values range from PropIds[0] to PropIds[Types.Count].
            //    //for (int i = 0; i < types.Count; ++i)
            //    //{
            //    //    if (types[i] == default) continue;
            //    //    ulong propId = propIdMap(i);
            //    //    string valueString = types[i].FullName;
            //    //    writer.WriteLine(valueString, propId);
            //    //    //InputTypesValues.Add(types[i], propId);
            //    //}
            //    //// Index output types with values in the second and final PropIds subrange.
            //    //// Output types values range from PropIds[propIdsHalfLength] to PropIds[proIdsHalfLength + Types.Count].
            //    //for (int i = 0; i < types.Count; ++i)
            //    //{
            //    //    if (types[i] == default) continsue;
            //    //    ulong propId = propIdMap(i + spaceSizeFloorHalf);
            //    //    string valueString = types[i].FullName;
            //    //    writer.WriteLine(valueString, propId);
            //    //    //OutputTypesValues.Add(types[i], propId);
            //    //}
            //}
        }
    }
}
