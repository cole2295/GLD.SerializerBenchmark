﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GLD.SerializerBenchmark
{
    internal struct Measurements
    {
        public int Size;
        public long Time;
    }

    internal class Tester
    {
        public static void Tests(int repetitions, Dictionary<string, ISerDeser> serializers)
        {
            var measurements = new Dictionary<string, Measurements[]>();
            foreach (var serializer in serializers)
                measurements[serializer.Key] = new Measurements[repetitions];
            var original = Person.Generate(); // the same data for all serializers
            for (var i = 0; i < repetitions; i++)
            {
                foreach (var serializer in serializers)
                {
                    var sw = Stopwatch.StartNew();
                    var serialized = serializer.Value.Serialize<Person>(original);

                    measurements[serializer.Key][i].Size = serialized.Length;

                    var processed = serializer.Value.Deserialize<Person>(serialized);

                    sw.Stop();
                    measurements[serializer.Key][i].Time = sw.ElapsedTicks;
                    Report.TimeAndDocument(serializer.Key, sw.ElapsedTicks, serialized);
                    var errors = original.Compare(processed);
                    errors[0] = serializer.Key + errors[0];
                    Report.Errors(errors);
                }
                GC.Collect(); // it has very little impact for repetitions < 100
                GC.WaitForFullGCComplete();
                GC.Collect();
            }
            Report.AllResults(measurements);
        }

      
    }
}