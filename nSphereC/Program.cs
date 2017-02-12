using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace nSphereC
{
    class Program
    {
        static void Main(string[] args)
        {
            while (doThings()) ;
        }
        static bool doThings()
        {
            string input;
            ushort dimensions;
            Console.WriteLine("Enter a number of dimensions(integer >= 0)");
            input = Console.ReadLine();
            if (!ushort.TryParse(input, out dimensions))
            {
                Console.WriteLine("Invalid input. Input must be an integer from 0 to " + ushort.MaxValue);
                return true;
            }
            var track = new Stopwatch();
            if (Primes.pCache == null || Primes.pCache.Last() < dimensions)
            {
                SieveOfAtkin.Generate(dimensions, track);
                Console.WriteLine(Primes.pCache.Count + " primes generated in " + track.Elapsed.ToString("g") +
                    " ending with " + Primes.pCache.Last().ToString());
            }
            SphereFormula s = null;
            Console.WriteLine("Time\t\t\tDim\tPrime\tFormula");
            var lastUpdate = new TimeSpan(0);
            ushort startDim = (SphereFormula.sCache == null ? (ushort)0 : SphereFormula.sCache.Dimensions);
            if (startDim > dimensions) startDim = 0;
            for (ushort i = startDim; i <= dimensions; i++)
            {
                try
                {
                    Fraction.maxPrime = 0;
                    track.Start();
                    s = SphereFormula.ForDimension(i);
                    track.Stop();
                    if (lastUpdate + new TimeSpan(0,0,6) <= track.Elapsed && i < dimensions)
                    {
                        WriteFormulaLine(track, i, s);
                        lastUpdate = track.Elapsed;
                    }
                }
                catch (Exception ex)
                {
                    track.Stop();
                    Console.WriteLine("Unable to calculate beyond" + s.Dimensions + " dimensions due to " + ex.Message);
                    break;
                }
            }
            WriteFormulaLine(track, s.Dimensions, s);
            Console.WriteLine("A unit (r=1) " + s.Dimensions + "-sphere has a " + s.Dimensions + "-volume of " + 
                s.nVolume(1) + " (e^" + s.nVolumeExp(1) + ")\n\nIf you would like to calculate the " + s.Dimensions + 
                "-volume for a different value of r, enter a floating-point value to use.\n" +
                "To exit, type Exit. To start over, type New. To display the full fractional form, type Fraction.");
            do
            {
                input = Console.ReadLine();
                double r;
                if(double.TryParse(input, out r))
                {
                    Console.WriteLine("With r=" + r + " the " + s.Dimensions + "-volume = " + s.nVolume(r) + " (e^" + s.nVolumeExp(r) +
                        ")\nCalculate with another value, Exit, or New?");
                }
                else
                {
                    switch (input.ToLower())
                    {
                        case "": break;
                        case "exit": return false;
                        case "new": return true;
                        case "fraction": Console.WriteLine(s.ToString(true));
                            break;
                        default:
                            Console.WriteLine("Unable to parse that value.");
                            break;
                    }
                }
            } while (true);
        }
        static void WriteFormulaLine(Stopwatch track, ushort i, SphereFormula s)
        {
            Console.WriteLine(track.Elapsed.ToString("g") + "\t\t" + i + "\t" + Fraction.maxPrime + "\t" + s);
        }
    }
}
