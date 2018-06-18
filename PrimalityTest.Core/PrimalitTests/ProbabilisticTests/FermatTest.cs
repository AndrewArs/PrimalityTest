using PrimalityTest.Core.Enums;
using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace PrimalityTest.Core.PrimalitTests.ProbabilisticTests
{
    public static class FermatTest
    {
        public static async Task<PrimeNumberState> IsPrime(BigInteger number, int iterations, CancellationToken token)
        {
            return await Task.Run(() => PrimeTest(number, iterations, token), token);
        }

        /*Returns true if a BigInteger is probably prime, that is,
           it passes iterations tests based on Fermat's Little Theorem.
           if P is Prime [and a is not divisble by p] then a^(p-1)-1 mod p =1*/
        private static PrimeNumberState PrimeTest(BigInteger number, int iterations, CancellationToken token)
        {
            var rand = new Random();

            for (var i = 0; i < iterations; i++)
            {
                token.ThrowIfCancellationRequested();

                var a = new BigInteger(rand.Next());  // random big-int
                                                      // calculate a^(p-1) mod p
                                                      // return true iff it passes the Fermat Test
                                                      
                var ans = BigInteger.ModPow(a, BigInteger.Subtract(number, BigInteger.One), number);
                if (!ans.IsOne)
                {
                    return PrimeNumberState.Composite;  // not prime  / it is definitely composite
                }
            }

            // the calculation == 1 testCount times, and so is probably correct!
            return PrimeNumberState.ProbablyPrime;
        }
    }
}
