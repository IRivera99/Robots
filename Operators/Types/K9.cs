//Creado por Ignacio Rivera
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TP_Integrador.Territory;
using TP_Integrador.Territory.Locations;

namespace TP_Integrador.Operators.Types
{
    class K9 : Operator
    {
        public K9(int id, double maxSpeed, Quarter quarter) : base(id, maxSpeed, quarter, OperatorTypes.K9)
        {
            battery = new Battery(6500);
            maxLoad = 40;            
        }

        public K9(int id, double maxSpeed, bool standBy, Quarter quarter) : base(id, maxSpeed, standBy, quarter, OperatorTypes.K9)
        {
            battery = new Battery(6500);
            maxLoad = 40;
        }

        [JsonConstructor]
        public K9(int id, double maxSpeed, bool standBy) : base(id, maxSpeed, standBy, OperatorTypes.K9)
        {
            battery = new Battery(6500);
            maxLoad = 40;
        }

        public override string ToString()
        {
            return "Cuadrupedo K9 " + base.ToString();
        }

        public override string ToStringStateOnly()
        {
            return "Cuadrupedo K9 " + base.ToStringStateOnly();
        }                
    }
}
