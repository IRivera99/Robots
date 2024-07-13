//Creado por Ignacio Rivera
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Integrador.Operators
{
    class Battery
    {        
        int maxMilliAmps;
        int actualMaxMilliAmps;
        int actualMilliAmps;
        bool perforated;
        bool damaged;
        bool conectedPort;

        public Battery(int maxMilliAmps)
        {            
            this.maxMilliAmps = maxMilliAmps;
            actualMaxMilliAmps = maxMilliAmps;
            actualMilliAmps = maxMilliAmps; 
            damaged = false;
            perforated = false;
            conectedPort = true;
        }

        public bool EnoughBattery(int requestedMilliAmps)
        {
            bool enough = false;

            if (perforated)
            {
                requestedMilliAmps *= 5;
            }

            if (requestedMilliAmps <= actualMilliAmps)
            {
                enough = true;
            }               

            return enough;
        }

        public int GetCharge()
        {
            return actualMilliAmps;
        }

        public int GetBatteryPercentage()
        {
            return Convert.ToInt32((actualMilliAmps * 100) / actualMaxMilliAmps);
        }

        public int GetBatteryNeeded()
        {
            return actualMaxMilliAmps - actualMilliAmps;
        }

        public bool ConsumeBattery(int mAh)
        {
            bool consumed = false;
            if (perforated)
            {
                mAh *= 5;
            }

            if (EnoughBattery(mAh))
            {
                actualMilliAmps -= mAh;
                consumed = true;
            }                

            return consumed;
        }

        public int ChargeBattery(int mAh)
        {
            int excedent = 0;

            if((mAh + actualMilliAmps) > actualMaxMilliAmps)
            {
                actualMilliAmps = actualMaxMilliAmps;
                excedent = (mAh + actualMaxMilliAmps) - actualMaxMilliAmps;
            }
            else
            {
                actualMilliAmps += mAh;
            }

            return excedent;
        }
        
        public void ChargeFullBattery()
        {
            actualMilliAmps = actualMaxMilliAmps;
        }

        public bool DamageBattery()
        {
            if (actualMaxMilliAmps - Convert.ToInt32(actualMaxMilliAmps * 0.2) > 0 && !damaged)
            {
                actualMaxMilliAmps -= Convert.ToInt32(actualMaxMilliAmps * 0.2);
                if(actualMilliAmps > actualMaxMilliAmps)
                {
                    actualMilliAmps = actualMaxMilliAmps;
                }
                damaged = true;
            }                        
            return damaged;
        }

        public bool PerforateBattery()
        {
            perforated = true;
            return perforated;
        }

        public bool DisconnectPort()
        {
            conectedPort = false;
            return conectedPort;
        }

        public bool IsPerorated()
        {
            return perforated;
        }

        public bool IsPortConnected()
        {
            return conectedPort;
        }

        public void ChangeBattery()
        {
            actualMaxMilliAmps = maxMilliAmps;
            perforated = false;
            conectedPort = true;
        }
    }
}
