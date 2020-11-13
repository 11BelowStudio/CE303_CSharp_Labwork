using Microsoft.VisualStudio.TestTools.UnitTesting;
using SieveLibrary;

namespace SieveTest
{
    [TestClass]
    public class SieveTester
    {

        /*
        dotnet test SieveTest/SieveTest.csproj
        */

        //https://docs.microsoft.com/en-us/dotnet/core/tutorials/testing-library-with-visual-studio-code

        [TestMethod]
        public void ExampleTest()
        {
            Sieve s = new Sieve(100);
            Assert.IsTrue(s[19]);
            Assert.IsFalse(s[91]);
        }

        [TestMethod]
        public void TestPrimes(){
            Sieve s = new Sieve(100);
            int[] primes = {2, 3, 5, 7, 11, 13};
            foreach(var i in primes)
            {
                bool result = s[i];
                Assert.IsTrue(
                    result,
                    $"Expected for {i}: true; Actual: {result}"
                );
            }
        }

        [TestMethod]
        public void TestNotPrimes(){
            Sieve s = new Sieve(100);
            int[] notPrimes = {1,4,6,8,9,10,12};
            foreach(var i in notPrimes)
            {
                bool result = s[i];
                Assert.IsFalse(
                    result,
                    $"Expected for {i}: false; Actual: {result}"
                );
            }
        }

        [TestMethod]
        public void TestOutOfBounds(){
            Sieve s = new Sieve(100);
            int[] invalidInputs = {-1, -3, 101, 1000};
            foreach(var i in invalidInputs){
                //try{
                Assert.ThrowsException<SieveException>(
                    () => s[i],
                    $"s[{i}] did not throw SieveException!"
                );
                //} catch (AssertFailedException e){
                //    Debug.Write(e.Message + "\n");
                //}
            }
        }

        [TestMethod]
        public void TestInvalidInitialisations(){
            Sieve s;
            int[] invalidInputs = {0, -1, -100};
            foreach(var i in invalidInputs){
                //try{
                    Assert.ThrowsException<SieveException>(
                        () => s = new Sieve(i),
                        $"s = new Sieve({i}) did not throw exception!"
                    );
                //} catch (AssertFailedException e){
                    //Debug.Write(e.Message + "\n");
                //}
            }
        }

        [TestMethod]
        public void TestInvalidNSet(){
            Sieve s = new Sieve(100);
            int[] invalidInputs = {0, -1, -100};
            foreach(var i in invalidInputs){
                //try{
                    Assert.ThrowsException<SieveException>(
                        () => s.N = i,
                        $"s.N = {i} did not throw exception!"
                    );
                //} catch (AssertFailedException e){
                    //Debug.WriteLine(e.Message + "\n");
                //}
            }
        }

        [TestMethod]
        public void TestValidNSet(){
            Sieve s = new Sieve(1);
            int[] validInputs = {2,3,100,5,20,1};
            foreach(var i in validInputs){
                try{
                    s.N = i;
                } catch (SieveException e){
                    Assert.Fail($"s.N = {i} threw an exception!\n{e.StackTrace}");
                }
            }
        }

        //TODO: more tests
    }
}
