using System;
using System.Collections;
using System.Collections.Generic;

namespace SieveLibrary
{
    public class Sieve: IEnumerable<int>
    {
        
        public Sieve(int n){
            if (n < 1){
                throw new SieveException(
                    "Please use a number greater than 0 to construct the sieve."
                );
            }
            ResetTheSieve(n);
        }

        //whether or not numbers are prime from 1 -> N
        bool[] isPrime;
        //the upper bound held in the sieve
        int sieveUpperBound;
        
        public int N
        {
            get => sieveUpperBound;
            set{
                if (value < 1){
                    throw new SieveException(
                        "Please only use a value that's greater than 0"
                    );
                }else{
                    ResetTheSieve(value);
                }
            }
        }

        //read-only indexer
        public bool this[int index]
        {
            get{
                if (index < 0){
                    throw new SieveException(
                        "Please enter an index greater than 0"
                    );
                } else if (index > sieveUpperBound){
                    throw new SieveException(
                        $"{index} is out of bounds! Please reset the N property first."
                    );
                } else{
                    return isPrime[index];
                }
            }

        }

        //method to construct the  isPrime array
        private void ResetTheSieve(int numToSieve)
        {
            //does nothing if a negative value was given
            if (numToSieve < 0){
                return;
            }
            sieveUpperBound = numToSieve;
            //constructs a primeSieve array, size is numToSieve+1
            isPrime = new bool[numToSieve +1];
            //element at index 0 is always false
            isPrime[0] = false;
            //if numToSieve is greater than 0 (2 or more elements in list)
            if (numToSieve > 0){
                //1 isn't prime (technically), that's set to false
                isPrime[0] = false;

                //List to hold sieved primes
                List<int> sievedPrimes = new List<int>();

                //for everything in range 2->numToSieve, works out if it's prime or not
                for(int i = 2; i <= numToSieve; i++){
                    //we assume that i is prime until proven otherwise
                    bool currentIsPrime = true;
                    //we then test it against all of the sieved primes
                    foreach(int j in sievedPrimes){
                        if (i % j == 0){
                            //if it is divisible by one of these primes,
                            //we know it isn't prime.
                            currentIsPrime = false;
                            break; //so we skip to the end
                        }
                    }
                    //if it's prime
                    if (currentIsPrime){
                        //we add it to the sieved primes
                        sievedPrimes.Add(i);
                    }
                    //we add it's prime status to index i+1 in primeSieve
                    isPrime[i] = currentIsPrime;
                }
            }

        }

        /// <summary>
        /// implementation of GetEnumerator method
        /// </summary>

        public IEnumerator<int> GetEnumerator()
        {
            return new SieveEnum(isPrime);
        }

        private IEnumerator GetEnumerator1(){
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator) GetEnumerator1();
        }

        /*
        public SieveEnum GetEnumerator()
        {
            return new SieveEnum(isPrime);
        }
        */


        public class SieveEnum: IEnumerator<int>
        {
            private List<int> allPrimes;
            private int currentIndex;
            private int current;

            public SieveEnum(bool[] isPrimeArray){
                currentIndex = -1;
                allPrimes = new List<int>();
                for (int i = 0; i < isPrimeArray.Length; i++){
                    if (isPrimeArray[i]){
                        allPrimes.Add(i);
                    }
                }
                //primes = allPrimes.ToArray();
            }

            public bool MoveNext()
            {
                if (++currentIndex >= allPrimes.Count)
                {
                    return false;
                }
                else
                {
                    current = allPrimes[currentIndex];
                }
                return true;
            }

            public void Reset(){ currentIndex = -1; }

            void IDisposable.Dispose(){}

            public int Current
            {
                get { return current; }
            }

            object IEnumerator.Current
            {
                get {return current;}
            }

        }

    }
}