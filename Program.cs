//Creado por Ignacio Rivera
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP_Integrador.Territory;
using TP_Integrador.Territory.Locations;
using TP_Integrador.Operators.Types;
using TP_Integrador.Menus;

namespace TP_Integrador
{
    class Program
    {
        static void Main(string[] args)
        {
            LoadingMenu mainMenu = LoadingMenu.GetInstance();
            Map map = mainMenu.ShowLoadingMenu();
            map.PrintOperators();
            Console.ReadLine();
            //Falta todo lo operativo xD

        }
    }
}
