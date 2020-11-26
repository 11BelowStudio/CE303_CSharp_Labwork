using System;
using System.Collections.Generic;
using System.Text;

namespace lab_6
{
    class Ex5And6
    {
        public static Dictionary<A, int> FrequencyMap<A>(IEnumerable<A> collection)
        {
            Dictionary<A, int> freqs = new Dictionary<A, int>();

            foreach(A item in collection)
            {
                
                if (freqs.ContainsKey(item))
                {
                    freqs[item]++;
                }
                else
                {
                    freqs.Add(item, 1);
                }
                //freqs[item] = count;
                //freqs.Add(item, count);
                //count += freqs[item];
            }

            return freqs;
        }


        public static void TestFrequency()
        {
            Console.WriteLine("Exercise 5\n");

            Console.WriteLine("Test 1");


            Dictionary<int, int> d1 = FrequencyMap(
                new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 1, 1, 1, 3, 3, 42, 67, 35 }
            );

            Console.WriteLine("Expected output: {1,4},{2,1},{3,3},{4,1},{5,1},{6,1},{7,1},{8,1},{9,1},{10,1},{42,1},{67,1},{35,1}");

            foreach(KeyValuePair<int,int> kv in d1)
            {
                Console.WriteLine($"{kv.Key},{kv.Value}");
            }
            Console.WriteLine();

            Console.WriteLine("Test 2");

            Dictionary<string, int> d2 = FrequencyMap(
                new string[] { "one", "one", "oatmeal", "kirbyisapinkguy", "kirby very cute", "deez nutz", "deez nutz", "deez nutz" }
            );

            Console.WriteLine("Expected output: {one,2},{oatmeal,1},{kirbyisapinkguy,1},{kirby very cute,1},{deez nutz,3}");

            foreach (KeyValuePair<string, int> kv in d2)
            {
                Console.WriteLine($"{kv.Key},{kv.Value}");
            }
            Console.WriteLine();

        }


        public static List<R> ZipWith<T1,T2,R,TL1,TL2>(
            Func<T1,T2,R> f, IEnumerable<TL1> l1,IEnumerable<TL2> l2
        ) where TL1: T1 where TL2: T2
        {
            List<R> result = new List<R>();

            IEnumerator<TL1> e1 = l1.GetEnumerator();

            IEnumerator<TL2> e2 = l2.GetEnumerator();

            while(e1.MoveNext() && e2.MoveNext())
            {
                result.Add(f.Invoke(e1.Current, e2.Current));
            }

            return result;
        }


        public static void TestZipWith()
        {
            Console.WriteLine("Exercise 6\n");

            Console.WriteLine("Test 1");

            List<int> l1 = ZipWith(
                (int a, int b) => a + b,
                new int[]{ 1,2,3,4,5},
                new int[] { 2, 3, 4, 5, 6 }
            );

            Console.WriteLine("Expected output: 3,5,7,9,11");

            foreach(int i in l1)
            {
                Console.WriteLine(i);
            }


            Console.WriteLine();

            Console.WriteLine("Test 2");

            List<string> l2 = ZipWith(
                (string s, float f) => $"{s}: {f.ToString("0.00")}",
                new HashSet<string>(new string[] { "a", "b", "c", "d", "banana" }),
                new HashSet<float>(new float[] { 13.1511f, 1524, 0.063f, 12.12f })
            );

            Console.WriteLine("Expected output: {a: 13.15},{b: 1524.00},{c: 0.06},{d: 12.12}");

            foreach (string s in l2)
            {
                Console.WriteLine(s);
            }
        }



        public static void Main(string[] args)
        {
            TestFrequency();
            Console.WriteLine();

            TestZipWith();
        }
    }
}
