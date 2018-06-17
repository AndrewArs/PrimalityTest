using PrimalityTest.Core.Enums;
using System;
using System.Numerics;

namespace PrimalityTest.Core.NumberAnalysers
{
    public static class NumbersAnalys
    {
        public static NumberState AnalysNumber(BigInteger number)
        {
            if(IsFermatNumber(number))
            {
                return NumberState.Fermat;
            }

            if(IsProthNumber(number))
            {
                return NumberState.Proth;
            }

            if(IsMersenneNumber(number))
            {
                return NumberState.Mersenne;
            }

            return NumberState.None;
        }

        internal static bool IsFermatNumber(BigInteger number)
        {
            return Math.Log(BigInteger.Log(number - 1, 2), 2) % 1 == 0;
        }

        internal static bool IsProthNumber(BigInteger number)
        {
            return number-- < (number & -number) * (number & -number);
        }

        internal static bool IsMersenneNumber(BigInteger number)
        {
            return BigInteger.Log(number + 1, 2) % 1 == 0;
        }
    }
}
