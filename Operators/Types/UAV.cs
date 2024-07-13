//Creado por Ignacio Rivera
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TP_Integrador.Territory.Locations;

namespace TP_Integrador.Operators.Types
{
    class UAV : Operator
    { 
        public UAV(int id, double maxSpeed, Quarter quarter) : base(id, maxSpeed, quarter, OperatorTypes.UAV)
        {
            battery = new Battery(4000);
            maxLoad = 5;
        }

        public UAV(int id, double maxSpeed, bool standBy, Quarter quarter) : base(id, maxSpeed, standBy, quarter, OperatorTypes.UAV)
        {
            battery = new Battery(4000);
            maxLoad = 5;
        }

        [JsonConstructor]
        public UAV(int id, double maxSpeed, bool standBy) : base (id, maxSpeed, standBy, OperatorTypes.UAV)
        {
            battery = new Battery(4000);
            maxLoad = 5;
        }

        public override string ToString()
        {
            return "Dron UAV " + base.ToString();
        }

        public override string ToStringStateOnly()
        {
            return "Dron UAV " + base.ToStringStateOnly();
        }        
    }
}
