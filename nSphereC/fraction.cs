using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace nSphereC
{
    class Fraction
    {
        private BigIntPrime _num, _den;
        private Fraction _simplestForm;
        public static uint maxPrime = 0;
        public BigInteger Numerator
        {
            get
            {
                return _num.value;
            }
        }
        public BigInteger Denominator
        {
            get
            {
                return _den.value;
            }
        }
        public Double ExpValue
        {
            get
            {
                return BigInteger.Log(Numerator) - BigInteger.Log(Denominator);
            }
        }
        public Fraction SimplestForm
        {
            get
            {
                if(_simplestForm == null) findSimplestForm();
                return _simplestForm;
            }
        }
        private void findSimplestForm()
        {
            if (Denominator == 1 || Numerator == 1)
            {
                _simplestForm = this;
            }
            else
            {
                BigIntPrime n = _num, d = _den;
                Boolean NMin = Numerator <= Denominator;
                if ((NMin?d:n).value % (NMin?n:d).value == 0)
                {
                    if (NMin)
                    {
                        d /= n;
                        n = 1;
                    }
                    else
                    {
                        n /= d;
                        d = 1;
                    }
                }
                else
                {
                    int k = 0;
                    while (Primes.pCache[k] <= (n | d))
                    {
                        if(n.value % Primes.pCache[k] == 0 && d.value % Primes.pCache[k] == 0)
                        {
                            n /= Primes.pCache[k];
                            d /= Primes.pCache[k];
                            if (Primes.pCache[k] > maxPrime) maxPrime = Primes.pCache[k];
                            if ((NMin ? n : d).value == 1) break;
                        }
                        else
                        {
                            k++;
                            if (k == Primes.pCache.Count) break;
                        }
                    }
                }
                if(d.value == Denominator)
                {
                    _simplestForm = this;
                }
                else
                {
                    _simplestForm = new Fraction(n, d);
                }
            }

        }
        public Fraction(BigIntPrime n, BigIntPrime d)
        {
            _num = n;
            _den = d;
        }
        public override string ToString()
        {
            return ToString(false);
        }
        public string ToString(Boolean FullFraction)
        {
            if (!FullFraction && Denominator > uint.MaxValue) return "e^" + ExpValue.ToString();
            Boolean d1 = Denominator == 1;
            return (d1 ? "" : "(") + Numerator.ToString("n0") + (d1 ? "" : "/" + Denominator.ToString("n0") + ")");
        }
        public static Fraction operator *(Fraction left, Fraction right)
        {
            return (new Fraction(left._num * right._num, left._den * right._den)).SimplestForm;
        }
        //public override bool Equals(object obj)
        //{
        //    if (obj is Fraction) return this == (Fraction)obj;
        //    else if (obj is uint) return this == (uint)obj;
        //    else return base.Equals(obj);
        //}
        public static Boolean operator ==(Fraction left, Fraction right)
        {
            if ((object)left == null || (object)right == null) return (object)left == null && (object)right == null;
            return left.SimplestForm.Numerator == right.SimplestForm.Numerator && 
                left.SimplestForm.Denominator == right.SimplestForm.Denominator;
        }
        public static Boolean operator !=(Fraction left, Fraction right)
        {
            return !(left == right);
        }
        public static Boolean operator ==(Fraction left, int right)
        {
            if (left.Denominator > left.Numerator) return false;
            BigInteger iForm = left.Numerator % left.Denominator;
            if (iForm != 0) return false;
            return left.Numerator / left.Denominator == right;
        }
        public static Boolean operator !=(Fraction left, int right)
        {
            return !(left == right);
        }
        public static implicit operator Fraction(uint value)
        {
            return new Fraction(value, 1);
        }
    }
}
