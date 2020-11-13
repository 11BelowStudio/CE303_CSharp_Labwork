using System;
using SieveLibrary;

/*
dotnet run Ex1Console/Ex1Console.csproj
*/

/*
dotnet run --project "c:\Users\Richard Lowe\Documents\University OneDrive\OneDrive - University of Essex\Year 3\CE303 Advanced Programming\C# labs\Lab2\Ex1Console\Ex1Console.csproj"
*/

namespace Ex1Console
{
    class Program
    {
        public static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            const int n = 100;
            const int o = 51;
            Sieve sieve = new Sieve(n);

            try
            {
                Console.WriteLine($"{o} is {(sieve[o] ? "prime" : "not prime")}");
            }
            catch (SieveException ex)
            {
                Console.WriteLine(ex.Message);
            }

            var sieve2 = new Sieve(20);
            foreach(int i in sieve2){
                Console.Write($"{i} ");
            }
            Console.WriteLine();
        }
    }
}
