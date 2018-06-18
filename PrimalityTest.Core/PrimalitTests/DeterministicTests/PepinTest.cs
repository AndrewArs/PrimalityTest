using PrimalityTest.Core.Enums;
using PrimalityTest.Core.NumberAnalysers;
using System;
using System.Numerics;

namespace PrimalityTest.Core.PrimalitTests.DeterministicTests
{
    public static class PepinTest
    {
        public static PrimeNumberState IsPrime(BigInteger number)
        {
            if(number <= 3)
            {
                return PrimeNumberState.Prime;
            }

            if(number.IsEven)
            {
                return PrimeNumberState.Composite;
            }

            if(!NumbersAnalys.IsFermatNumber(number))
            {
                throw new Exception("It is not a fermat number!");
            }

            if (!int.TryParse(BigInteger.Divide(BigInteger.Subtract(number, 1), 2).ToString(), out var exp))
            {
                throw new Exception("Can't parse to int");
            }

            var sub = BigInteger.Subtract(BigInteger.Pow(number, exp), -1);

            var remainder = BigInteger.Remainder(sub, number);

            return remainder == 0 ? PrimeNumberState.Prime : PrimeNumberState.Composite;
        }
    }
}
