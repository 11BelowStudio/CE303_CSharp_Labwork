using System;
using System.Collections.Generic;

namespace exercise_2
{
    class Program
    {

        static bool IsPrime(int testThis){
            if (testThis < 1){
                //anything below 0 isn't prime
                return false;
            } else if (testThis == 1){
                 //1 is prime because it's only divisble by itself
                return true;
            }
            //we test testThis against every number between 0 and itself
            for(int i = 2; i < testThis; i++){
                if ((testThis % i) == 0){
                    //if it neatly divides into that number, we know it's not prime
                    //so we return false
                    return false;
                }
            }
            //if it survived all that without returning false,
            //it's prime, so we return true.
            return true;
        }

        static int SievePrimeCount(int testTo){
            if (testTo < 1){
                return 0; //negative numbers and 0 aren't prime
            }


            //here's a List to hold all the primes
            //we find between 1 and testTo (inclusive)
            List<int> sievedPrimes = new List<int>();


            //First, we test everything in the range 2-testTo (inclusive)
            for(int i = 2; i <= testTo; i++){

                //we assume that i is prime until proven otherwise
                bool probablyPrime = true;

                //we then test it against all of the sieved primes
                foreach(int j in sievedPrimes){
                    if (i % j == 0){
                        //if it is divisible by one of these primes,
                        //we know it isn't prime.
                        probablyPrime = false;
                        break; //so we skip testing it against the others
                    }
                }
                //if, after that, we still think it's prime
                if(probablyPrime){
                    //we add it to the sievedPrimes
                    sievedPrimes.Add(i);
                }

            }
            //We finally add 1 to the list of sievedPrimes
            //we do this at the end, because it would break the prime test otherwise
            sievedPrimes.Add(1);

            //we return the count of sieved primes.
            return sievedPrimes.Count;
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            const int n = 50;
            Console.WriteLine($"All primes up to {n}:");
            for(int i = 1; i <= n; i++){
                if (IsPrime(i)){
                    Console.WriteLine(i);
                }
            }

            Console.WriteLine("");
            Console.WriteLine($"There are {SievePrimeCount(n)} primes between 0 and {n}.");

            

        }
    }
}
