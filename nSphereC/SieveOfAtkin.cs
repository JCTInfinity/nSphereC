using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace nSphereC
{
    class SieveOfAtkin
    {
        // Sieve Of Atkin, based on the wikipedia article: https://en.wikipedia.org/wiki/Sieve_of_Atkin
        // And further developed based on Axel Magnuson's firstpass function found in the code he posted
        //   here: http://mathoverflow.net/questions/13116/c-sieve-of-atkin-overlooks-a-few-prime-numbers
        private uint _limit;
        private Dictionary<uint, Boolean> is_prime = new Dictionary<uint, bool>();
        private static uint[] s = { 1, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 49, 53, 59 };
        private SieveOfAtkin(uint limit)
        {
            _limit = limit;
            for (uint i = 1; i <= limit; i++) if (s.Contains(i % 60)) is_prime[i] = false;
        }
        private void flip(uint n)
        {
            if (is_prime.ContainsKey(n)) is_prime[n] = !is_prime[n];
        }
        private static uint sqrt(double value)
        {
            return (uint)Math.Sqrt(Math.Abs(value));
        }
        private void firstpass()
        {
            uint xroof, x, yroof, y, n, nmod;
            // n = 4x^2 + y^2
            xroof = sqrt((Double)(_limit - 1) / 4);
            for(x= 1; x <= xroof; x++)
            {
                yroof = sqrt((Double)_limit - 4 * x * x);
                for(y = 1; y <= yroof; y++)
                {
                    n = 4 * x * x + y * y;
                    nmod = n % 12;
                    if (nmod == 1 || nmod == 5) flip(n);
                }
            }

            // n = 3x^2 + y^2
            xroof = sqrt((Double)(_limit - 1) / 3);
            for(x=1; x<= xroof; x++)
            {
                yroof = sqrt((Double)_limit - 3 * x * x);
                for (y=1; y <= yroof; y++)
                {
                    n = 3 * x * x + y * y;
                    nmod = n % 12;
                    if (nmod == 7) flip(n);
                }
            }

            // n = 3x^2 - y^2
            xroof = sqrt((Double)(_limit + 1) / 3);
            for(x = 1; x<= xroof; x++)
            {
                yroof = sqrt((double)3 * x * x - 1);
                if (yroof >= x) yroof = x - 1;
                for (y=1; y<= yroof; y++)
                {
                    n = 3 * x * x - y * y;
                    nmod = n % 12;
                    if (nmod == 11) flip(n);
                }
            }
        }
        private void sieve()
        {
            for(uint n = 7; n <= _limit; n++)
            {
                if (!s.Contains(n % 60)) continue;
                uint nsqr = n * n;
                if(is_prime.ContainsKey(n) && is_prime[n])
                {
                    for(uint k = 1; nsqr * k <= _limit; k++)
                    {
                        if (!s.Contains(k % 60)) continue;
                        if (is_prime.ContainsKey(nsqr * k)) is_prime[nsqr * k] = false;
                    }
                }

            }
        }
        public static void Generate(uint limit, Stopwatch timer)
        {
            timer.Start();
            var soa = new SieveOfAtkin(limit);
            soa.firstpass();
            soa.sieve();
            Primes.pCache = new List<uint>() { 2, 3, 5, 7 };
            Primes.pCache.AddRange(soa.is_prime.Where(p => p.Value && p.Key > 7).Select(p => p.Key));
            timer.Stop();
        }
    }
}
