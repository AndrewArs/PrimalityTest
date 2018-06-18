using PrimalityTest.Core.Enums;
using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace PrimalityTest.Core.PrimalitTests.DeterministicTests
{
    public class AksTest
    {
        /***************************************************************************
        * The algorithm is -
        * 1. l <- log n
        * 2. for i<-2 to l
        *      a. if an is a power fo l
        *              return COMPOSITE
        * 3. r <- 2
        * 4. while r < n
        *      a. if gcd( r, n) != 1
        *              return COMPSITE
        *      b. if sieve marked n as PRIME
        *              q <- largest factor (r-1)
        *              o < - r-1 / q
        *              k <- 4*sqrt(r) * l
        *              if q > k and n <= r 
        *                      return PRIME
        *      c. x = 2
        *      d. for a <- 1 to k 
        *              if (x + a) ^n !=  x^n + mod (x^r - 1, n) 
        *                      return COMPOSITE
        *      e. return PRIME
        */

        private static int _log;
        private static bool[] _sieveArray;
        private const int SieveEratosSize = 100000000;

        /* aks constructor */
        static AksTest()
        {
            SieveEratos();
        }

        public static async Task<PrimeNumberState> IsPrime(BigInteger n, CancellationToken token)
        {
            return await Task.Run(() => PrimeTest(n, token), token);
        }

        /* function to check if a given number is prime or not */
        public static PrimeNumberState PrimeTest(BigInteger n, CancellationToken token)
        {
            BigInteger lowR, powOf, x, leftH, rightH, aBigNum;
            int quot, tm, aCounter, divisor;
            _log = (int)LogBigNum(n);

            if (FindPower(n, _log))
            {
                return PrimeNumberState.Composite;
            }

            x = lowR;
            var totR = 2;

            for (lowR = 2; lowR.CompareTo(n) < 0; lowR = BigInteger.Add(lowR, BigInteger.One))
            {
                token.ThrowIfCancellationRequested();

                if (BigInteger.GreatestCommonDivisor(lowR, n).CompareTo(BigInteger.One) != 0)
                {
                    return PrimeNumberState.Composite;
                }

                totR = (int)lowR;

                if (IsSievePrime(totR))
                {
                    quot = LargestFactor(totR - 1);
                    divisor = (totR - 1) / quot;
                    tm = (int)(4 * (Math.Sqrt(totR)) * _log);
                    powOf = MPower(n, new BigInteger(divisor), lowR);
                    if (quot >= tm && (powOf.CompareTo(BigInteger.One)) != 0)
                    {
                        break;
                    }
                }
            }

            var aLimit = (int)(2 * Math.Sqrt(totR) * _log);

            for (aCounter = 1; aCounter < aLimit; aCounter++)
            {
                token.ThrowIfCancellationRequested();

                aBigNum = new BigInteger(aCounter);
                leftH = (MPower(BigInteger.Subtract(x, aBigNum), n, n)) % n;
                rightH = (BigInteger.Subtract(MPower(x, n, n), aBigNum)) % n;

                if (leftH.CompareTo(rightH) != 0)
                {
                    return PrimeNumberState.Composite;
                }
            }

            return PrimeNumberState.Prime;
        }

        /* function that computes the log of a big number*/
        private static double LogBigNum(BigInteger bNum)
        {
            var str = "0," + bNum;
            var len = str.Length - 1;
            var num1 = double.Parse(str);
            var num2 = Math.Log10(num1) + len;

            return num2;
        }
        
        /* function to compute the largest factor of a number */
        private static int LargestFactor(int num)
        {
            var i = num;

            if (i == 1)
            {
                return i;
            }

            while (i > 1)
            {
                while (_sieveArray[i])
                {
                    i--;
                }
                if (num % i == 0)
                {
                    return i;
                }
                i--;
            }

            return num;
        }


        /*function given a and b, computes if a is power of b */
        private static bool FindPowerOf(BigInteger bNum, int val)
        {
            BigInteger low, high, mid, res;
            low = new BigInteger(10);
            high = new BigInteger(10);
            var len = (bNum.ToString().Length) / Convert.ToDouble(val);
            var l = (int)Math.Ceiling(len); 
            low = BigInteger.Pow(low, l - 1);
            high = BigInteger.Subtract(BigInteger.Pow(high, l), BigInteger.One);

            while (low.CompareTo(high) <= 0)
            {
                mid = BigInteger.Add(low, high);
                mid = BigInteger.Divide(mid, new BigInteger(2));
                res = BigInteger.Pow(mid, val);

                if (res.CompareTo(bNum) < 0)
                {
                    low = BigInteger.Add(mid, BigInteger.One);
                }
                else if (res.CompareTo(bNum) > 0)
                {
                    high = BigInteger.Subtract(mid, BigInteger.One);
                }
                else if (res.CompareTo(bNum) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        /* creates a sieve array that maintains a table for COMPOSITE-ness 
         * or possibly PRIME state for all values less than SIEVE_ERATOS_SIZE
         */
        private static bool IsSievePrime(int val)
        {
            return _sieveArray[val] == false;
        }

        /* function, given a and b computes if a is a power of b */
        private static bool FindPower(BigInteger n, int l)
        {
            for (var i = 2; i < l; i++)
            {
                if (FindPowerOf(n, i))
                {
                    return true;
                }
            }

            return false;
        }

        private static BigInteger MPower(BigInteger x, BigInteger y, BigInteger n)
        {
            var p = BigInteger.One;
            var two = new BigInteger(2);

            while (y.CompareTo(BigInteger.Zero) > 0)
            {
                while ((y % two).CompareTo(BigInteger.Zero) == 0)
                {
                    y = BigInteger.Divide(y, two);
                    x = BigInteger.Multiply(x, x) % n;
                }
                y = BigInteger.Subtract(y, BigInteger.One);
                p = BigInteger.Multiply(p, x) % n;
            }

            return p;
        }

        /* array to populate sieve array
         * the sieve array looks like this
         *  
         *  y index -> 0 1 2 3 4 5 6 ... n
         *  x index    1  
         *     |       2   T - T - T ...
         *     \/      3     T - - T ...
         *             4       T - - ...
         *             .         T - ...
         *             .           T ...
         *             n
         *             
         *
         *
         *
         */
        private static void SieveEratos()
        {
            _sieveArray = new bool[SieveEratosSize + 1];
            _sieveArray[1] = true;

            for (var i = 2; i * i <= SieveEratosSize; i++)
            {
                if (_sieveArray[i] != true)
                {
                    for (var j = i * i; j <= SieveEratosSize; j += i)
                    {
                        _sieveArray[j] = true;
                    }
                }
            }
        }
    }
}
