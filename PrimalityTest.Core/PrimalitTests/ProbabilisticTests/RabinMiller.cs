using PrimalityTest.Core.Enums;
using System;
using System.Numerics;

namespace PrimalityTest.Core.PrimalitTests.ProbabilisticTests
{
    public class RabinMiller
    {
        private static BigInteger ZERO = BigInteger.Zero;
        private static BigInteger ONE = BigInteger.One;
        private static BigInteger TWO = new BigInteger(2);
        private static BigInteger THREE = new BigInteger(3);

        public static PrimeNumberState IsPrime(BigInteger n, int k)
        {
            if (n.CompareTo(ONE) == 0)
            {
                return PrimeNumberState.Composite;
            }

            if (n.CompareTo(THREE) < 0)
            {
                return PrimeNumberState.Prime;
            }

            var s = 0;
            var d = BigInteger.Subtract(n, ONE);

            while ((d % TWO).Equals(ZERO))
            {
                s++;
                d = BigInteger.Divide(d, TWO);
            }

            for (int i = 0; i < k; i++)
            {
                var a = UniformRandom(TWO, BigInteger.Subtract(n, ONE));
                var x = BigInteger.ModPow(a, d, n);

                if (x.Equals(ONE) || x.Equals(BigInteger.Subtract(n, ONE)))
                {
                    continue;
                }

                var r = 0;
                for (; r < s; r++)
                {
                    x = BigInteger.ModPow(x, TWO, n);

                    if (x.Equals(ONE))
                    {
                        return PrimeNumberState.Composite;
                    }

                    if (x.Equals(BigInteger.Subtract(n, ONE)))
                    {
                        break;
                    }
                }
                if (r == s) // None of the steps made x equal n-1.
                {
                    return PrimeNumberState.Composite;
                }
            }

            return PrimeNumberState.ProbablyPrime;
        }

        private static BigInteger UniformRandom(BigInteger bottom, BigInteger top)
        {
            var rnd = new Random();
            BigInteger res;

            do
            {
                res = RandomIntegerBelow(top);
            } while (res.CompareTo(bottom) < 0 || res.CompareTo(top) > 0);

            return res;
        }

        private static BigInteger RandomIntegerBelow(BigInteger N)
        {
            var random = new Random();
            byte[] bytes = N.ToByteArray();
            BigInteger R;

            do
            {
                random.NextBytes(bytes);
                bytes[bytes.Length - 1] &= 0x7F; //force sign bit to positive
                R = new BigInteger(bytes);
            } while (R >= N);

            return R;
        }
    }
}
