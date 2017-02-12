using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nSphereC
{
    class SphereFormula
    {
        public static SphereFormula sCache;
        public ushort Dimensions, πs = 0, Rs;
        public Fraction fraction = 1;
        public string ToString(Boolean FullFraction)
        {
            var factors = new List<string>();
            if (fraction != 1) factors.Add(fraction.SimplestForm.ToString(FullFraction));
            if (πs > 0) factors.Add(powerString("π", πs));
            factors.Add(powerString("r", Rs));
            return string.Join(" * ", factors);
        }
        public override string ToString()
        {
            return ToString(false);
        }
        private string powerString(string value, ushort power)
        {
            if (power == 1) return value;
            return value + "^" + power;
        }
        private SphereFormula(ushort d)
        {
            Dimensions = d;
            Rs = d;
            if (d > 0)
            {
                fraction = sCache.fraction;
                πs = (ushort)(d / 2);
                if (d % 2 == 0) fraction *= 2;
                f(d);
            }
        }
        public static SphereFormula ForDimension(ushort dimensions)
        {
            if (sCache == null || sCache.Dimensions > dimensions) sCache = new SphereFormula(0);
            if (sCache.Dimensions < dimensions)
            {
                for (ushort i = (ushort)(sCache.Dimensions + 1); i <= dimensions; i++)
                {
                    sCache = new SphereFormula(i);
                }
            }
            return sCache;
        }
        public double nVolume(double r)
        {
            return Math.Exp(nVolumeExp(r));
        }
        public double nVolumeExp(double r)
        {
            return fraction.ExpValue + Math.Log(Math.PI) * πs + Math.Log(r) * Rs;
        }
        private static Dictionary<ushort, Fraction> fCache = new Dictionary<ushort, Fraction>();
        private void f(ushort n)
        {
            for(ushort i = n; i > 1; i -= 2)
            {
                if (!fCache.ContainsKey(i)) fCache.Add(i, (new Fraction((ushort)(i - 1), i)).SimplestForm);
                fraction *= fCache[i];
            }
        }
    }
}
