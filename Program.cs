using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPCalc
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Input IP:");
                var ip = Console.ReadLine();
                Console.WriteLine("Input Mask:");
                var mask = Console.ReadLine();

                Calc calc = new Calc(ip, mask);
                calc.Devider();

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message); 
            }
        }
    }
}
