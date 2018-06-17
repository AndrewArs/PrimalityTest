using PrimalityTest.Core.Enums;
using PrimalityTest.Core.NumberAnalysers;
using System;
using System.Numerics;

namespace PrimalityTest.Core.PrimalitTests.DeterministicTests
{
    public static class LucasLehmerTest
    {
        public static PrimeNumberState IsPrime(BigInteger number)
        {
            if (number.IsEven)
            {
                return number == 2 ? PrimeNumberState.Prime : PrimeNumberState.Composite;
            }

            if(!NumbersAnalys.IsMersenneNumber(number))
            {
                throw new Exception("Not a Mersenne number");
            }

            var num = BigInteger.Log(number + 1, 2);
            var s = new BigInteger(4);

            for (int i = 3; i <= num; i++)
            {
                s = (BigInteger.ModPow(s, 2, number) - 2) % number;
            }

            return s == BigInteger.Zero ? PrimeNumberState.Prime : PrimeNumberState.Composite;
        }
    }
}
