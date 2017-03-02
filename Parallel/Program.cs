using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Parallel
{
    class Program
    {
        static void Main(string[] args)
        {
            Counter firstElement;

            //List
            Stopwatch sw = new Stopwatch();
            var counters = BenchmarkList(sw);

            Thread.Sleep(3000);

            //Enumerator
            BenchmarkEnumerator(sw, counters);

            //Array
            Counter[] countersArray;
            BenchmarkArraySort(counters, sw);  
            BenchmarkArrayPlinq(counters, sw);

            Console.WriteLine("Benchmark finished, press a key to exit");
            Console.Read();                                                       
        }

        private static void BenchmarkArrayPlinq(List<Counter> counters, Stopwatch sw)
        {
            Counter[] countersArray;
            Counter firstElement;
            countersArray = counters.ToArray();
            sw.Restart();
            firstElement =
                (from counter in countersArray.AsParallel().WithDegreeOfParallelism(8) orderby counter.Value select counter)
                .First();
            Console.WriteLine("Parallel array: " + sw.ElapsedMilliseconds + "ms");
        }

        private static void BenchmarkArraySort(List<Counter> counters, Stopwatch sw)
        {
            var countersArray = counters.ToArray();
            sw.Restart();
            Array.Sort(countersArray);
            var vv = countersArray[0];
            sw.Stop();
            Console.WriteLine("No parallel array: " + sw.ElapsedMilliseconds + "ms");
        }

        private static void BenchmarkEnumerator(Stopwatch sw, List<Counter> counters)
        {
            Counter firstElement;
            sw.Restart();
            firstElement = (from counter in counters.AsParallel().AsOrdered() orderby counter.Value select counter).First();
            sw.Stop();
            Console.WriteLine("Parallel enumerator: " + sw.ElapsedMilliseconds + "ms");
            firstElement = null;
        }

        private static List<Counter> BenchmarkList(Stopwatch sw)
        {
            Counter firstElement;
            List<Counter> counters = new List<Counter>();
            for (int i = 0; i < 10000000; i++)
            {
                counters.Add(new Counter());
            }
            sw.Restart();
            firstElement = counters.OrderBy(t => t.Value).ToList().First();
            sw.Stop();
            Console.WriteLine("No parallel list: " + sw.ElapsedMilliseconds + "ms");
            firstElement = null;

            sw.Restart();
            firstElement =
                (from counter in counters.AsParallel().AsOrdered() orderby counter.Value select counter).ToList().First();
            sw.Stop();
            Console.WriteLine("Parallel list: " + sw.ElapsedMilliseconds + "ms");
            return counters;
        }
    }
}
