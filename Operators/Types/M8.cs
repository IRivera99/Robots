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
    class M8: Operator
    {
        public M8(int id, double maxSpeed, Quarter quarter) : base(id, maxSpeed, quarter, OperatorTypes.M8)
        {
            battery = new Battery(12250);
            maxLoad = 250;            
        }

        public M8(int id, double maxSpeed, bool standBy, Quarter quarter) : base(id, maxSpeed, standBy, quarter, OperatorTypes.M8)
        {
            battery = new Battery(12250);
            maxLoad = 250;
        }

        [JsonConstructor]
        public M8(int id, double maxSpeed, bool standBy) : base(id, maxSpeed, standBy, OperatorTypes.M8)
        {
            battery = new Battery(12250);
            maxLoad = 250;
        }

        public override string ToString()
        {
            return "Semi-Humanoide M8 " + base.ToString();
        }

        public override string ToStringStateOnly()
        {
            return "Semi-Humanoide M8 " + base.ToStringStateOnly();
        }        
    }
}
