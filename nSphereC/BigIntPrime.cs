using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace nSphereC
{
    class BigIntPrime
    {
        public BigInteger value;
        public UInt32 prime;
        public BigIntPrime(BigInteger value, UInt32 prime)
        {
            this.value = value;
            this.prime = prime;
        }
        public BigIntPrime(UInt32 value)
        {
            this.value = value;
            prime = Primes.LargestPrime(value);
        }
        public override string ToString()
        {
            return value.ToString() + " (" + prime.ToString() + ")";
        }
        public static implicit operator BigIntPrime(uint value)
        {
            return new BigIntPrime(value);
        }
        public static BigIntPrime operator *(BigIntPrime left, BigIntPrime right)
        {
            return new BigIntPrime(left.value * right.value, left | right);
        }
        public static BigIntPrime operator /(BigIntPrime left, BigIntPrime right)
        {
            BigInteger value = left.value / right.value;
            uint prime = Primes.LargestPrime(value, left | right);
            return new BigIntPrime(value, prime);
        }
        public static uint operator |(BigIntPrime left, BigIntPrime right)
        {
            return left.prime < right.prime ? right.prime : left.prime;
        }
    }
}
