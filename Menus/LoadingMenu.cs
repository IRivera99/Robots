using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TP_Integrador.Territory;
using TP_Integrador.Territory.Locations;

namespace TP_Integrador.Menus
{
    internal class LoadingMenu
    {
        private static LoadingMenu _instance;
        private string _welcome = "Bienvenidos al simulador de robots!";
        private string _mapLoadOption = "Existe un mapa guardado, ¿desea cargarlo? (1 = Si | 2 = No)";
        private string _mapInexistant = "No existe un mapa guardado...";
        private string _mapGenerating = "Generando mapa...";
        private string _mapLoading = "Cargando mapa...";
        private string _mapSaveOption = "¿Desea guardar el mapa creado? (1 = Si | 2 = No)";
        private string _operatorLoadOption1 = "Existen operadores para el cuartel ";
        private string _operatorLoadOption2 = ", ¿desea cargarlo/s? (1 = Si | 2 = No)";
        private string _operatorLoading = "Cargando operadores...";
        private string _operatorGeneratingCant1 = "El cuartel '";
        private string _operatorGeneratingCant2 = "' no puede quedar vacío, indique cuántos operadores desea agregar (Ingresar número mayores a 0)";
        private string _operatorsSaveOption1 = "Desea guardar los operadores generados para el cuartel '";
        private string _operatorsSaveOption2 = "'? (1 = Si | 2 = No)";
        private string _error = "ERROR! EL PROGRAMA FINALIZARÁ";
        private int _option;
        SaveSystem saveSystem = SaveSystem.GetInstance();
        Map map;
        
        public static LoadingMenu GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LoadingMenu();
            }
            return _instance;
        }

        public Map ShowLoadingMenu()
        {            
            Console.WriteLine(_welcome);
            #region Manejador de mapa
            if (saveSystem.CheckMapExistence()) //Se verifica si existe un mapa guardado
            {
                Console.WriteLine(_mapLoadOption);
                _option = InputControl.ReadIntRangeOnly(1,2);
                if (_option == 1) //Si quiere lo carga
                {
                    Console.WriteLine(_mapLoading);
                    map = saveSystem.LoadMap();
                    map.PrintMap();
                }                    
                if (_option == 2) //Si no quiere genera uno nuevo
                {
                    GenerateMap();
                }
            }
            else
            {
                Console.WriteLine(_mapInexistant);
                GenerateMap();
            }
            #endregion

            #region Manejador de Operadores (Robots)    
            List<Quarter> quarters = map.GetQuarterLocations(); //Obtenemos los cuarteles del mapa
            if (quarters.Count > 0) //Si el mapa esta roto por algun motivo rompemos todo
            {
                foreach (Quarter quarter in quarters)
                {
                    if (saveSystem.CheckOperatorsExistence(quarter)) //Para cada cuartel verificamos si tiene operadores
                    {
                        Console.WriteLine(_operatorLoadOption1 + quarter.Name + _operatorLoadOption2);
                        _option = InputControl.ReadIntRangeOnly(1, 2);
                        if(_option == 1) //Si selecciona cargarlos...
                        {
                            Console.WriteLine(_operatorLoading);
                            if (!saveSystem.LoadOperators(quarter))//Si hay algún error, añadir operadores al cuarteles NOTA:Se puede hacer que funcione solo con un operador pero sería un quilombo xD
                            {
                                GenerateOperators(quarter);
                            }
                        }
                        if(_option == 2)//Aca hay que generar nuevos
                        {
                            GenerateOperators(quarter);//Facilaso no? xD
                        }
                    }
                    
                }
            }
            else
            {
                Console.WriteLine(_error);
                return null;
            }
            
            
            #endregion

            Console.ReadLine();
            return map;
        }

        private void GenerateOperators(Quarter quarter)
        {
            Console.WriteLine(_operatorGeneratingCant1 + quarter.Name + _operatorGeneratingCant2);
            _option = InputControl.ReadIntMinLimit(1);
            quarter.AddRandomOperatorsToQuarter(_option);
            Console.WriteLine(_operatorsSaveOption1 + quarter.Name + _operatorsSaveOption2);
            _option = InputControl.ReadIntRangeOnly(1, 2);
            if (_option == 1)
            {
                saveSystem.SaveOperators(quarter);
            }
        }

        private void GenerateMap()
        {
            Console.WriteLine(_mapGenerating);
            map = Map.GetInstance(20, 20);
            map.PrintMap();
            Console.WriteLine(_mapSaveOption);
            _option = InputControl.ReadIntRangeOnly(1, 2);
            if (_option == 1) // Si pone 1 guarda el nuevo mapa generado (si desea sobreescribir el archivo)
                saveSystem.SaveMap(map);
        }
    }
}
