//Creado por Ignacio Rivera
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TP_Integrador.Operators;
using TP_Integrador.Operators.Types;
using TP_Integrador.Territory;
using TP_Integrador.Territory.Locations;

namespace TP_Integrador
{
    internal class SaveSystem
    {
        private static SaveSystem _instance;
        private static string homePath = Path.GetFullPath("../../");
        private static string dataPath = homePath + "Data/";
        private static string mapDataPath = dataPath + "Map/";
        private static string operatorsDataPath = dataPath + "Operators/";
        private static string mapFileName = "map.map";

        private bool CheckLoadedMap(List<Location> locations)
        {
            bool ok = false;
            int quarters = 0;
            int recycling = 0;

            foreach (Location location in locations)
            {
                if (location.Type == LocationTypes.Quarter)
                {
                    quarters++;
                }
                if (location.Type == LocationTypes.Recycling)
                {
                    recycling++;
                }
            }

            if (quarters > 0 && quarters <= Map.GetMaxQuarters() && recycling > 0 && recycling <= Map.GetMaxRecyclingPoints())
            {
                ok = true;
            }

            return ok;
        }

        public static SaveSystem GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SaveSystem();
            }
            return _instance;
        }

        #region Map handlers
        public void SaveMap(Map map)
        {
            if (!Directory.Exists(mapDataPath))
            {
                Directory.CreateDirectory(mapDataPath);
                Console.WriteLine($"Directorio de mapas creado en {mapDataPath}");
            }

            try
            {
                if (map == null)
                {
                    throw new NoNullAllowedException();
                }

                if (File.Exists(mapDataPath + mapFileName))
                {
                    Console.WriteLine("El mapa ya existe, desea sobreescribir? (Ingrese 1 para sobreescribir)");
                    int opción = InputControl.ReadIntOnly();
                    if (opción != 1)
                    {
                        throw new TaskCanceledException();
                    }
                    File.Delete(mapDataPath + mapFileName);
                }

                StreamWriter streamWriter = File.CreateText(mapDataPath + mapFileName);

                foreach (Location location in map.GetMap().Values)
                {
                    string locationJson = string.Empty;

                    if (location is Quarter quarter)
                    {
                        locationJson = JsonSerializer.Serialize(quarter);
                    }
                    else
                    {
                        locationJson = JsonSerializer.Serialize(location);
                    }
                    streamWriter.WriteLine(locationJson);
                }

                streamWriter.Close();

                Console.WriteLine("Mapa guardado con éxito!");
            }
            catch (NoNullAllowedException)
            {
                Console.WriteLine("Error, el mapa es nulo...");
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("No se sobreescribió el mapa...");
            }
        }

        public Map LoadMap()
        {
            Map map;

            try
            {
                if (!File.Exists(mapDataPath + mapFileName))
                {
                    throw new FileNotFoundException();
                }

                StreamReader streamReader = File.OpenText(mapDataPath + mapFileName);

                List<Location> locations = new List<Location>();
                string locationString = string.Empty;

                while (!streamReader.EndOfStream)
                {
                    locationString = streamReader.ReadLine();
                    if (locationString.Contains("Name"))
                    {
                        Quarter quarter = JsonSerializer.Deserialize<Quarter>(locationString);
                        locations.Add(quarter);
                    }
                    else
                    {
                        Location location = JsonSerializer.Deserialize<Location>(locationString);
                        locations.Add(location);
                    }
                }

                streamReader.Close();

                if (!CheckLoadedMap(locations))
                {
                    throw new FormatException();
                }

                map = Map.GetInstance();
                map.AddLocationsToMap(locations);

                Console.WriteLine("Mapa cargado con éxito!");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error! No el archivo de mapa...");
                Console.WriteLine("Generando nuevo mapa...");
                map = Map.GetInstance(20, 20);
            }
            catch (FormatException)
            {
                Console.WriteLine("Error! El mapa no cumple con el formato preestablecido...");
                Console.WriteLine("Generando nuevo mapa...");
                map = Map.GetInstance(20, 20);
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("Error al intentar cargar el mapa, archivo corrupto...");
                Console.WriteLine("Generando nuevo mapa...");
                map = Map.GetInstance(20, 20);
            }

            return map;
        }

        public bool CheckMapExistence()
        {
            return File.Exists(mapDataPath + mapFileName);
        }

        #endregion

        #region Operators handlers
        public void SaveOperators(Quarter quarter)
        {
            if (!Directory.Exists(operatorsDataPath))
            {
                Directory.CreateDirectory(operatorsDataPath);
                Console.WriteLine($"Directorio de operadores creado en {operatorsDataPath}");
            }

            try
            {
                if (quarter == null)
                {
                    throw new NoNullAllowedException("El cuartel es nulo...");//No te hagas el vivo pa... dea xD
                }

                List<Operator> operators = quarter.GetOperators();                                

                if(operators.Count < 1) 
                {
                    throw new NoNullAllowedException("No existen operadores...");//Otra vez???
                }
                                
                bool overwriteAll = true;
                string quarterName = quarter.Name.Replace(' ', '_');

                for (int i = 0; i < operators.Count; i++)
                {                    
                    string operatorFileName = $"Operator_{operators[i].Id}.{quarterName}";
                    if (File.Exists(operatorsDataPath + operatorFileName))
                    {
                        Console.WriteLine($"Uno o mas operadores existe, desdea sobreescribir todo? (Ingrese 1 para sobreescribir)");
                        int opción = InputControl.ReadIntOnly();
                        if (opción != 1)
                        {
                            overwriteAll = false;
                        }
                        i = operators.Count;
                    }
                }

                foreach (Operator op in operators) 
                {
                    string operatorFileName = $"Operator_{op.Id}.{quarterName}";
                    bool save = true;                    

                    #region Verify existance one by one (now commented)
                    //if (File.Exists(operatorsDataPath + operatorFileName))
                    //{
                    //    Console.WriteLine($"El operador {op.Id} en {op.GetOrigin()} ya existe, desea sobreescribir? (Ingrese 1 para sobreescribir)");
                    //    int opción = InputControl.ReadIntOnly();
                    //    if (opción == 1)
                    //    {
                    //        File.Delete(operatorsDataPath + operatorFileName);                            
                    //    }
                    //    else
                    //    {
                    //        save = false;
                    //    }                        
                    //}
                    #endregion                    

                    if (File.Exists(operatorsDataPath + operatorFileName))
                    {
                        if (overwriteAll)
                        {
                            File.Delete(operatorsDataPath + operatorFileName);
                        }
                        else
                        {
                            save = false;
                        }                        
                    }

                    if (save)
                    {
                        StreamWriter streamWriter = File.CreateText(operatorsDataPath + operatorFileName);
                        string operatorJson = JsonSerializer.Serialize(op);
                        streamWriter.WriteLine(operatorJson);
                        streamWriter.Close();
                    }                    
                }                

                Console.WriteLine("Operadores guardados con éxito!");
            }
            catch (NoNullAllowedException ex)
            {
                Console.WriteLine(ex.Message);
            }            

        }

        public bool LoadOperators(Quarter quarter)
        {
            List<Operator> operators = new List<Operator>();
            string quarterName = quarter.Name.Replace(" ", "_");

            try
            {
                string[] operatorsFilesNames = Directory.GetFiles(operatorsDataPath, $"*.{quarterName}");

                if (operatorsFilesNames.Length < 1)
                {
                    throw new FileNotFoundException();
                }

                for (int i = 0; i < operatorsFilesNames.Length; i++)
                {
                    StreamReader streamReader = File.OpenText(operatorsFilesNames[i]);
                    string operatorString = streamReader.ReadLine();

                    if(operatorString.Contains("\"Type\":0"))
                    {
                        UAV op = JsonSerializer.Deserialize<UAV>(operatorString);
                        operators.Add(new UAV(op.Id, op.MaxSpeed, op.StandBy, quarter));
                    }
                    else if (operatorString.Contains("\"Type\":1"))
                    {
                        M8 op = JsonSerializer.Deserialize<M8>(operatorString);
                        operators.Add(new M8(op.Id, op.MaxSpeed, op.StandBy, quarter));
                    }
                    else if(operatorString.Contains("\"Type\":2"))
                    {
                        K9 op = JsonSerializer.Deserialize<K9>(operatorString);
                        operators.Add(new K9(op.Id, op.MaxSpeed, op.StandBy, quarter));
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }

                    streamReader.Close();
                }               

                quarter.AddOperatorsToQuarter(operators);

                Console.WriteLine($"Operadores del cuartel '{quarter.Name}' cargados con éxito!");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"No se econtraron operadores guardados para el cuartel {quarter.Name}");
                return false;
                //Console.WriteLine($"Desea agregar operadores al {quarter.Name}? (Ingrese la cantidad, si es menor a 1 no se agregará ninguno)");
                //int opción = InputControl.ReadIntOnly();
                //if (opción > 0)
                //{
                //    quarter.AddRandomOperatorsToQuarter(opción);
                //    SaveOperators(quarter);
                //}
            }
            catch (NotSupportedException)
            {
                Console.WriteLine($"Error al intentar cargar un operador del cuartel '{quarter.Name}', porfavor verifique los archivos...");
                return false;
            }
            catch (JsonException)
            {
                Console.WriteLine($"Error al intentar cargar un operador del cuartel '{quarter.Name}', porfavor verifique los archivos...");
                return false;
            }
            return true;
        }

        public bool CheckOperatorsExistence(Quarter quarter)
        {
            string quarterName = quarter.Name.Replace(" ", "_");
            string[] operatorsFilesNames = Directory.GetFiles(operatorsDataPath, $"*.{quarterName}");
            return operatorsFilesNames.Length > 0 ? true : false;
        }

        #endregion
    }
}
