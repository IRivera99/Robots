//Creado por Ignacio Rivera
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP_Integrador.Territory;
using TP_Integrador.Operators.Types;
using TP_Integrador.Territory.Locations;
using System.Text.Json.Serialization;

namespace TP_Integrador.Operators
{
    abstract class Operator
    {
        protected int id;
        protected Battery battery;
        protected bool standBy; //true= STANDBY, false = OPERATIVE
        protected int maxLoad;
        protected int actualLoad;
        protected double maxSpeed;
        protected Location location;
        protected Quarter origin;
        protected OperatorTypes type;

        public int Id { get; }
        public double MaxSpeed {get; }
        public bool StandBy { get; } 
        public OperatorTypes Type { get; }              

        protected Operator(int id, double maxSpeed, Quarter quarter, OperatorTypes type)
        {
            this.id = id;
            Id = id;
            standBy = false;
            StandBy = false;
            actualLoad = 0;
            this.maxSpeed = maxSpeed;
            origin = quarter;
            location = origin;
            this.type = type;         
        }

        protected Operator(int id, double maxSpeed, bool standBy, Quarter quarter, OperatorTypes type)
        {
            this.id = id;
            Id = id;
            this.standBy = standBy;
            StandBy = standBy;
            actualLoad = 0;
            this.maxSpeed = maxSpeed;
            origin = quarter;
            location = origin;
            this.type = type;
        }

        protected Operator(int id, double maxSpeed, bool standBy, OperatorTypes type)
        {
            Id = id;
            this.id = id;
            StandBy = standBy;
            this.standBy = standBy;
            actualLoad = 0;
            MaxSpeed = maxSpeed;
            this.maxSpeed = maxSpeed;
            Type = type;
        }

        protected double CalculateActualSpeed()
        {
            double speed = maxSpeed;
            int loop = (actualLoad * 10) / maxLoad;

            for (int i = 0; i < loop; i++)
            {
                speed -= maxSpeed * 0.05;
            }

            return speed;
        }

        protected bool AddLoad(int kg)
        {
            bool added = false;

            if (kg <= maxLoad && (kg + actualLoad) <= maxLoad)
            {
                actualLoad += kg;
                added = true;
            }

            return added;
        }

        protected void SubstractLoad(int kg)
        {
            if (kg > actualLoad)
                actualLoad = 0;
            else
                actualLoad -= kg;
        }

        protected int CalculateDistanceToTravel(List<Location> route)
        {
            int distance = 0;

            for (int i = 0; i < route.Count; i++)
            {
                if (i == 0)
                {
                    distance += Math.Abs(this.location.GetCoordX() - route[i].GetCoordX());
                    distance += Math.Abs(this.location.GetCoordY() - route[i].GetCoordY());
                }
                else
                {
                    distance += Math.Abs(route[i].GetCoordX() - route[i - 1].GetCoordX());
                    distance += Math.Abs(route[i].GetCoordY() - route[i - 1].GetCoordY());
                }
            }

            return distance; 
        }

        protected void Damage(Location location)
        {
            Random rand = new Random();
            switch (location.GetLocationType())
            {
                case LocationTypes.Verter:
                    {
                        int chance = rand.Next(0, 100);
                        if(chance == 1)
                        {
                            battery.PerforateBattery();
                        }
                    }
                    break;
                case LocationTypes.ElectricVerter:
                    {
                        battery.DamageBattery();
                    }
                    break;
                default:
                    break;
            }
        }

        protected bool CalculateRoute(Location destiny, Map map, bool safeRoute, out List<Location> route, out int distance)
        {
            bool canMakeRoute = true;
            route = new List<Location>();
            distance = 0;

            #region Set prohibited location types
            List<LocationTypes> prohibitedLocationTypes = new List<LocationTypes>();

            if(type == OperatorTypes.M8 || type == OperatorTypes.K9)
            {
                prohibitedLocationTypes.Add(LocationTypes.Lake);
            }

            if (safeRoute)
            {
                prohibitedLocationTypes.Add(LocationTypes.Verter);
                prohibitedLocationTypes.Add(LocationTypes.ElectricVerter);
            }
            #endregion

            #region Searching routes (A* algorithm)
            List<Location> openList = new List<Location>();
            List<Location> closeList = new List<Location>();

            Location currentLocation = location;
            closeList.Add(currentLocation);

            while (!closeList.Contains(destiny) && canMakeRoute)
            {
                List<Location> neighbors = map.GetNeighbors(currentLocation);

                neighbors.RemoveAll(
                    neighbor => neighbor == null ||
                                closeList.Contains(neighbor) ||
                                prohibitedLocationTypes.Contains(neighbor.GetLocationType())
                                );

                foreach (Location neighbor in neighbors)
                {
                    if (!openList.Contains(neighbor))
                    {
                        neighbor.CalculateTotalCost(destiny, currentLocation);
                        openList.Add(neighbor);
                    }
                    else if (neighbor.CalculateNewMovementCost(currentLocation) > 0 &&
                        neighbor.CalculateNewMovementCost(currentLocation) < neighbor.GetCost())
                    {
                        openList.Remove(neighbor);
                        neighbor.ResetParameters();
                        neighbor.CalculateTotalCost(destiny, currentLocation);
                        openList.Add(neighbor);
                    }
                }

                Location minCost = null;
                foreach (Location location in openList)
                {
                    if (minCost == null || location.GetTotalCost() < minCost.GetTotalCost())
                    {
                        minCost = location;
                    }
                }

                if (minCost != null)
                {
                    closeList.Add(minCost);
                    currentLocation = closeList.Last();
                }
                else
                {
                    canMakeRoute = false;
                }

                foreach (Location location in closeList)
                {
                    openList.Remove(location);
                }
            }
            #endregion

            if (canMakeRoute)
            {
                #region Search best route
                bool initialLocationFound = false;
                int index = closeList.Count - 1;                

                while (!initialLocationFound)
                {
                    if (!initialLocationFound && index == closeList.Count - 1)
                    {
                        route.Add(closeList[index]);
                    }
                    else if (!initialLocationFound && closeList[index].GetCoords() == route.Last().GetParent().GetCoords())
                    {
                        if (closeList[index].GetParent() == location)
                        {
                            initialLocationFound = true;
                        }
                        route.Add(closeList[index]);
                    }
                    index--;
                }

                route.Reverse();                
                distance = CalculateDistanceToTravel(route);
                #endregion
            }

            return canMakeRoute;
        }

        public Location GetLocation()
        {
            return location;
        }

        public Quarter GetOrigin()
        {
            return origin;
        }

        public int GetId()
        {
            return id;
        }

        public bool IsStandBy()
        {
            return standBy;
        }

        public void ChangeState(bool standBy)
        {
            this.standBy = standBy;
        }

        public bool Travel(Location destiny, Map map, bool safeTravel)
        {
            bool done = false;
            bool canMake = CalculateRoute(destiny, map, safeTravel, out List<Location> route, out int distance);            
            double speed = CalculateActualSpeed();
            int milliAmpsToConsume = Convert.ToInt32((distance * 1000) / speed);
            bool checkBattery = battery.EnoughBattery(milliAmpsToConsume);

            if (!standBy && canMake && checkBattery)
            {               
                foreach (Location location in route)
                {                    
                    Damage(location);
                }

                map.PrintRoute(route, location);
                location = destiny;
                battery.ConsumeBattery(milliAmpsToConsume);
                done = true;                               
            }

            map.ResetLocationsParameters();

            return done;
        }        

        public bool TransferBattery(Operator opReciever)
        {
            bool done = false;
            int batteryNeeded = opReciever.battery.GetBatteryNeeded();
            bool checkLocation = location.Equals(opReciever.location);
            bool portsConnected = battery.IsPortConnected() && opReciever.battery.IsPortConnected();

            if (!standBy && checkLocation && portsConnected)
            {
                opReciever.battery.ChargeBattery(battery.GetCharge());
                battery.ConsumeBattery(batteryNeeded);
                done = true;
            }

            return done;
        }

        public bool TransferLoad(Operator opReciever)
        {
            bool done = false;

            if (!standBy && location.Equals(opReciever.location) && opReciever.AddLoad(actualLoad))
            {
                SubstractLoad(actualLoad);
                done = true;
            }

            return done;
        }

        #region toStrings
        public override string ToString()
        {
            string state = "Operativo";
            if (standBy)
            {
                state = "Stand By";
            }
            return "ID: " + id + 
                   "\nCarga restante: " + battery.GetBatteryPercentage() + "% (" + battery.GetCharge() + " mAh)" +
                   "\nEstado: " + state +
                   "\nPeso: " + actualLoad + " / " + maxLoad + " kg" +
                   "\nVelocidad: " + CalculateActualSpeed() + " km/h" +
                   "\nLocalización: " + location.ToString();
        }

        public virtual string ToStringStateOnly()
        {
            string state = "Operativo";
            if (standBy)
            {
                state = "Stand By";
            }
            return "ID: " + id +
                   "\nEstado: " + state;
        }

        public string ToStringIdOnly()
        {
            return "ID: " + id;
        }
        #endregion
    }
}
