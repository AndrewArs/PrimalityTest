using PrimalityTest.Core.Enums;
using PrimalityTest.Core.NumberAnalysers;
using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace PrimalityTest.Core.PrimalitTests.DeterministicTests
{
    public static class LucasLehmerTest
    {
        public static async Task<PrimeNumberState> IsPrime(BigInteger number, CancellationToken token)
        {
            return await Task.Run(() => PrimeTest(number, token), token);
        }

        private static PrimeNumberState PrimeTest(BigInteger number, CancellationToken token)
        {
            if (number.IsEven)
            {
                return number == 2 ? PrimeNumberState.Prime : PrimeNumberState.Composite;
            }

            if (!NumbersAnalys.IsMersenneNumber(number))
            {
                throw new Exception("Not a Mersenne number");
            }

            var num = BigInteger.Log(number + 1, 2);
            var s = new BigInteger(4);

            for (var i = 3; i <= num; i++)
            {
                token.ThrowIfCancellationRequested();

                s = (BigInteger.ModPow(s, 2, number) - 2) % number;
            }

            return s == BigInteger.Zero ? PrimeNumberState.Prime : PrimeNumberState.Composite;
        }
    }
}
