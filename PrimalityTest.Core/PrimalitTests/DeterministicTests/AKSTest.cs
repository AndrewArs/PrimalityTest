using PrimalityTest.Core.Enums;
using System;
using System.Numerics;

namespace PrimalityTest.Core.PrimalitTests.DeterministicTests
{
    public class AKSTest
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

        private static int log;
        private static bool[] sieveArray;
        private const int SieveEratosSize = 100000000;

        /* aks constructor */
        static AKSTest()
        {
            SieveEratos();
        }

        /* function to check if a given number is prime or not */
        public static PrimeNumberState IsPrime(BigInteger n)
        {
            BigInteger lowR, powOf, x, leftH, rightH, fm, aBigNum;
            int totR, quot, tm, aCounter, aLimit, divisor;
            log = (int)LogBigNum(n);

            if (FindPower(n, log))
            {
                return PrimeNumberState.Composite;
            }

            x = lowR;
            totR = 2;

            for (lowR = 2; lowR.CompareTo(n) < 0; lowR = BigInteger.Add(lowR, BigInteger.One))
            {
                if (BigInteger.GreatestCommonDivisor(lowR, n).CompareTo(BigInteger.One) != 0)
                {
                    return PrimeNumberState.Composite;
                }

                totR = (int)lowR;

                if (IsSievePrime(totR))
                {
                    quot = LargestFactor(totR - 1);
                    divisor = (totR - 1) / quot;
                    tm = (int)(4 * (Math.Sqrt(totR)) * log);
                    powOf = MPower(n, new BigInteger(divisor), lowR);
                    if (quot >= tm && (powOf.CompareTo(BigInteger.One)) != 0)
                    {
                        break;
                    }
                }
            }

            fm = BigInteger.Subtract((MPower(x, lowR, n)), BigInteger.One);
            aLimit = (int)(2 * Math.Sqrt(totR) * log);

            for (aCounter = 1; aCounter < aLimit; aCounter++)
            {
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
            string str;
            int len;
            double num1, num2;
            str = "0," + bNum.ToString();
            len = str.Length - 1;
            num1 = Double.Parse(str);
            num2 = Math.Log10(num1) + len;

            return num2;
        }

        /*function that computes the log of a big number input in string format*/
        private static double LogBigNum(string str)
        {
            string s;
            int len;
            double num1, num2;
            len = str.Length;
            s = "0," + str;
            num1 = Double.Parse(s);
            num2 = Math.Log10(num1) + len;

            return num2;
        }

        /* function to compute the largest factor of a number */
        private static int LargestFactor(int num)
        {
            int i;
            i = num;
            if (i == 1) return i;
            while (i > 1)
            {
                while (sieveArray[i] == true)
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
            int l;
            double len;
            BigInteger low, high, mid, res;
            low = new BigInteger(10);
            high = new BigInteger(10);
            len = (bNum.ToString().Length) / val;
            l = (int)Math.Ceiling(len); 
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
            if (sieveArray[val] == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static long MPower(long x, long y, long n)
        {
            long m, p, z;
            m = y;
            p = 1;
            z = x;

            while (m > 0)
            {
                while (m % 2 == 0)
                {
                    m = m / 2;
                    z = (z * z) % n;
                }
                m = m - 1;
                p = (p * z) % n;
            }

            return p;
        }

        /* function, given a and b computes if a is a power of b */
        private static bool FindPower(BigInteger n, int l)
        {
            int i;

            for (i = 2; i < l; i++)
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
            BigInteger m, p, z, two;
            m = y;
            p = BigInteger.One;
            z = x;
            two = new BigInteger(2);

            while (m.CompareTo(BigInteger.Zero) > 0)
            {
                while ((m % two).CompareTo(BigInteger.Zero) == 0)
                {
                    m = BigInteger.Divide(m, two);
                    z = BigInteger.Multiply(z, z) % n;
                }
                m = BigInteger.Subtract(m, BigInteger.One);
                p = BigInteger.Multiply(p, z) % n;
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
            int i, j;
            sieveArray = new bool[SieveEratosSize + 1];
            sieveArray[1] = true;

            for (i = 2; i * i <= SieveEratosSize; i++)
            {
                if (sieveArray[i] != true)
                {
                    for (j = i * i; j <= SieveEratosSize; j += i)
                    {
                        sieveArray[j] = true;
                    }
                }
            }
        }
    }
}
