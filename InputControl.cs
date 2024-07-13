//Creado por Ignacio Rivera
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Integrador
{
    abstract class InputControl
    {
        //Probando sumarios y clases accesibles sin new (Conceptos)
        /// <summary>
        /// Creo que no hace falta aclarar que hace esto no? xD
        /// </summary>
        public static int ReadIntOnly()
        {
            int intReturn = 0;            
            bool converted = false;

            while (!converted)
            {
                string input = Console.ReadLine();
                try
                {
                    intReturn = Convert.ToInt32(input);
                    converted = true;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Error! Por favor ingrese solo números.");
                    Console.ReadLine();
                }                
            }
            return intReturn;
        }
        /// <summary>
        /// No se aceptan numeros menores que el mínimo, si se aceptan numeros iguales que el mínimo
        /// </summary>
        public static int ReadIntMinLimit(int min) 
        {
            int intReturn = 0;
            bool converted = false;
            bool inRange = false;

            while (!converted || !inRange)
            {
                string input = Console.ReadLine();
                try
                {
                    intReturn = Convert.ToInt32(input);
                    converted = true;
                    if (intReturn >= min) 
                    { 
                        inRange = true;
                    }
                    else
                    {
                        Console.WriteLine($"Error! Por favor ingrese número mayores a {min}");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Error! Por favor ingrese solo números.");
                    Console.ReadLine();
                }
            }
            return intReturn;

        }

        /// <summary>
        /// Se aceptan los numeros marcados como limites min y max
        /// </summary>
        public static int ReadIntRangeOnly(int min, int max)
        {
            int intReturn = 0;
            bool converted = false;
            bool inRange = false;

            while (!converted || !inRange)
            {
                string input = Console.ReadLine();
                try
                {
                    intReturn = Convert.ToInt32(input);
                    converted = true;
                    if (intReturn >= min && intReturn <= max)
                    {
                        inRange = true;
                    }
                    else
                    {
                        Console.WriteLine($"Error! Por favor ingrese sólo número entre {min} y {max}");
                    }
                        
                }
                catch (FormatException)
                {
                    Console.WriteLine("Error! Por favor ingrese solo números.");
                }
            }
            return intReturn;

        }
    }
}
