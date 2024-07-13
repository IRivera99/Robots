//Creado por Ignacio Rivera
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP_Integrador.Territory.Locations;
using TP_Integrador.Operators;
using TP_Integrador.Operators.Types;

namespace TP_Integrador.Territory.Locations
{
    class Quarter : Location
    {
        List<Operator> operators;
        Map map = null;

        public string Name { get;}

        public Quarter(string name, int coordX, int coordY) : base(coordX, coordY, type: LocationTypes.Quarter)
        {            
            operators = new List<Operator>();
            Name = name;
        }

        private OperatorTypes GetRandomOperatorType(Random random)
        {
            Array typesArray = Enum.GetValues(typeof(OperatorTypes));
            int index = random.Next(0, typesArray.Length);
            return (OperatorTypes)typesArray.GetValue(index);
        }

        public void AsignMap(Map map)
        {
            if(this.map == null)
            {
                this.map = map;
            }
        }

        public List<Operator> GetOperators()
        {
            return operators;
        }

        public List<Operator> GetOperators(Location location)
        {
            return operators.FindAll(o => o.GetLocation().Equals(location));
        }
        
        public Operator GetOperator(int id)
        {
            return operators.Find(o => o.GetId() == id);
        }

        public Operator GetOperator(Location location)
        {
            return operators.Find(o => o.GetLocation() == location);
        }        

        public bool RemoveOperator(int id)
        {
            Operator op = operators.Find(o => o.GetId() == id);
            return operators.Remove(op);
        }

        public bool TotalRecall(bool safeReturn)
        {
            bool done = false;

            if(map != null)
            {
                foreach (Operator op in operators)
                {
                    op.Travel(this, map, safeReturn);
                }

                Operator notReturned = operators.Find(o => !o.GetLocation().Equals(LocationTypes.Quarter));

                if (notReturned == null)
                {
                    done = true;
                }
            }           

            return done;
        }

        public bool MakeOperatorTravel(int id, Location destiny, bool safeTravel)
        {
            bool done = false;
            
            if(map != null)
            {
                int index = operators.FindIndex(o => o.GetId() == id);

                if (index > -1)
                {
                    done = operators[index].Travel(destiny, map, safeTravel);
                }
            }            

            return done;
        }

        public bool ChangeOperatorState(int id, bool standBy)
        {
            bool done = false;
            int index = operators.FindIndex(o => o.GetId() == id);

            if (index > -1)
            {
                operators[index].ChangeState(standBy);
                done = true;
            }

            return done;
        }

        public int GetMaxId()
        {
            int id = 0;
            foreach (Operator op in operators) 
            { 
                if (op.GetId() > id)
                {
                    id = op.GetId();
                }
            }
            return id;
        }

        public bool AddOperator(Operator op)
        {
            bool done = false;

            Operator sameId = operators.Find(o => o.GetId() == op.GetId());

            if (sameId == null)
            {
                operators.Add(op);
                done = true;
            }

            return done;
        }

        public void AddRandomOperatorsToQuarter(int amount)
        {
            Random random = new Random();

            for (int i = 0; i < amount; i++)
            {
                bool added = false;
                Operator op = null;                
                OperatorTypes type = GetRandomOperatorType(random);                

                while (!added)
                {
                    int id = GetMaxId() + 1;

                    if (type == OperatorTypes.UAV)
                        op = new UAV(id, random.Next(30, 60), this);

                    if (type == OperatorTypes.K9)
                        op = new K9(id, random.Next(20, 50), this);

                    if (type == OperatorTypes.M8)
                        op = new M8(id, random.Next(15, 40), this);

                    added = AddOperator(op);
                }
            }

            Console.WriteLine($"{GetOperators().Count} operadores añadidos");
        }

        public void AddOperatorsToQuarter(List<Operator> operators)
        {
            foreach (Operator op in operators)
            {
                AddOperator(op);
            }
        }
    }
}
