using PrimalityTest.Core.Enums;
using System;

namespace PrimalityTest.Core.PrimalitTests.ProbabilisticTests
{
    public static class SolovayStrassen
    {
        /** Function to check if prime or not **/
        public static PrimeNumberState IsPrime(long n, int iteration)
        {
            if (n % 2 == 0)
            {
                return n == 2 ? PrimeNumberState.Prime : PrimeNumberState.Composite;
            }

            var rand = new Random();

            for (var i = 0; i < iteration; i++)
            {
                long r = Math.Abs(rand.Next());
                long a = r % (n - 1) + 1;
                long jacobian = (n + Jacobi(a, n)) % n;
                long mod = ModPow(a, (n - 1) / 2, n);

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

            if (b == 1)
            {
                return j;
            }

            return 0;
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
