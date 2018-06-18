using PrimalityTest.Core.Enums;
using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace PrimalityTest.Core.PrimalitTests.ProbabilisticTests
{
    public class RabinMiller
    {
        private static readonly BigInteger Zero = BigInteger.Zero;
        private static readonly BigInteger One = BigInteger.One;
        private static readonly BigInteger Two = new BigInteger(2);
        private static readonly BigInteger Three = new BigInteger(3);

        public static async Task<PrimeNumberState> IsPrime(BigInteger number, int iterations, CancellationToken token)
        {
            return await Task.Run(() => PrimeTest(number, iterations, token), token);
        }

        public static PrimeNumberState PrimeTest(BigInteger number, int iterations, CancellationToken token)
        {
            if (number.CompareTo(One) == 0)
            {
                return PrimeNumberState.Composite;
            }

            if (number.CompareTo(Three) < 0)
            {
                return PrimeNumberState.Prime;
            }

            var s = 0;
            var d = BigInteger.Subtract(number, One);

            while ((d % Two).Equals(Zero))
            {
                s++;
                d = BigInteger.Divide(d, Two);
            }

            for (var i = 0; i < iterations; i++)
            {
                token.ThrowIfCancellationRequested();

                var a = UniformRandom(Two, BigInteger.Subtract(number, One));
                var x = BigInteger.ModPow(a, d, number);

                if (x.Equals(One) || x.Equals(BigInteger.Subtract(number, One)))
                {
                    continue;
                }

                var r = 0;
                for (; r < s; r++)
                {
                    token.ThrowIfCancellationRequested();

                    x = BigInteger.ModPow(x, Two, number);

                    if (x.Equals(One))
                    {
                        return PrimeNumberState.Composite;
                    }

                    if (x.Equals(BigInteger.Subtract(number, One)))
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
            BigInteger res;

            do
            {
                res = RandomIntegerBelow(top);
            } while (res.CompareTo(bottom) < 0 || res.CompareTo(top) > 0);

            return res;
        }

        private static BigInteger RandomIntegerBelow(BigInteger n)
        {
            var random = new Random();
            var bytes = n.ToByteArray();
            BigInteger r;

            do
            {
                random.NextBytes(bytes);
                bytes[bytes.Length - 1] &= 0x7F; //force sign bit to positive
                r = new BigInteger(bytes);
            } while (r >= n);

            return r;
        }
    }
}
