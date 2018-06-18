using PrimalityTest.Core.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrimalityTest.Core.PrimalitTests.ProbabilisticTests
{
    public static class SolovayStrassen
    {
        public static async Task<PrimeNumberState> IsPrime(long number, int iterations, CancellationToken token)
        {
            return await Task.Run(() => PrimeTest(number, iterations, token), token);
        }

        /** Function to check if prime or not **/
        private static PrimeNumberState PrimeTest(long number, int iterations, CancellationToken token)
        {
            if (number % 2 == 0)
            {
                return number == 2 ? PrimeNumberState.Prime : PrimeNumberState.Composite;
            }

            var rand = new Random();

            for (var i = 0; i < iterations; i++)
            {
                token.ThrowIfCancellationRequested();

                long r = Math.Abs(rand.Next());
                var a = r % (number - 1) + 1;
                var jacobian = (number + Jacobi(a, number)) % number;
                var mod = ModPow(a, (number - 1) / 2, number);

                if (jacobian == 0 || mod != jacobian)
                {
                    return PrimeNumberState.Composite;
                }
            }

            return PrimeNumberState.ProbablyPrime;
        }

        /** Function to calculate jacobi (a/b) **/
        private static long Jacobi(long a, long b)
        {
            if (b <= 0 || b % 2 == 0)
            {
                return 0;
            }

            var j = 1L;

            if (a < 0)
            {
                a = -a;
                if (b % 4 == 3)
                {
                    j = -j;
                }
            }

            while (a != 0)
            {
                while (a % 2 == 0)
                {
                    a /= 2;
                    if (b % 8 == 3 || b % 8 == 5)
                    {
                        j = -j;
                    }
                }

                var temp = a;
                a = b;
                b = temp;

                if (a % 4 == 3 && b % 4 == 3)
                {
                    j = -j;
                }

                a %= b;
            }

            return b == 1 ? j : 0;
        }

        /** Function to calculate (a ^ b) % c **/
        private static long ModPow(long a, long b, long c)
        {
            var res = 1L;

            for (var i = 0; i < b; i++)
            {
                res *= a;
                res %= c;
            }

            return res % c;
        }
    }
}
