using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace nSphereC
{
    public static class Primes
    {
        public static List<uint> pCache;
        public static uint LargestPrime(uint composite)
        {
            if (composite == 1 || pCache.Contains(composite)) return composite;
            int start = pCache.IndexOf(pCache.LastOrDefault(p => p <= composite / 2));
            if (start < 0) return composite;
            for (int i = start; i >= 0; i--)
            {
                if (composite % pCache[i] == 0) return pCache[i];
            }
            return composite;
        }
        public static uint LargestPrime(BigInteger composite, uint OldLargestPrime)
        {
            if (composite == 1) return 1;
            for(int i = pCache.IndexOf(OldLargestPrime); i >= 0; i--)
            {
                if (composite % pCache[i] == 0) return pCache[i];
            }
            return 1;
        }
    }
}
