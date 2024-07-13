//Creado por Ignacio Rivera
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Integrador.Territory.Locations
{
    class Location
    {
        protected LocationTypes type;
        protected int coordX;
        protected int coordY;
        protected int cost = 0; //G
        protected int heuristic = 0; //H
        protected int totalCost = 0; //F = G + H
        protected Location parent = null;

        public int CoordX { get; }
        public int CoordY { get; }
        public LocationTypes Type { get; }

        protected Location(int coordX, int coordY)
        {
            this.coordX = coordX;
            this.coordY = coordY;
            CoordX = coordX;
            CoordY = coordY;
        }        

        public Location(int coordX, int coordY, LocationTypes type)
        {
            this.coordX = coordX;
            this.coordY = coordY;
            this.type = type;
            CoordX = coordX;
            CoordY = coordY;
            Type = type;
        }

        public (int, int) GetCoords()
        {
            return (coordX, coordY);
        }        

        public int GetCoordX()
        {
            return coordX;
        }

        public int GetCoordY()
        {
            return coordY;
        }

        public LocationTypes GetLocationType()
        {
            return type;
        }

        protected void SetParent(Location parent)
        {
            this.parent = parent;
        }

        protected int CalculateCost(Location fromLocation)
        {
            int distance = Math.Abs(coordX - fromLocation.coordX) + Math.Abs(coordY - fromLocation.coordY);
            if (distance == 1)
            {
                cost = fromLocation.cost + 10;
            }            
            return cost;
        }

        protected int CalculateHeuristic(Location destiny)
        {
            int xDistance = Math.Abs(coordX - destiny.GetCoordX());
            int yDistance = Math.Abs(coordY - destiny.GetCoordY());
            heuristic = (xDistance + yDistance) * 10;                                  
            return heuristic;
        }        

        public int CalculateTotalCost(Location destiny, Location fromLocation)
        {
            if(fromLocation != null)
            {
                CalculateCost(fromLocation);
                SetParent(fromLocation);
            }            
            if( destiny != null)
                CalculateHeuristic(destiny);            
            totalCost = heuristic + cost;
            return totalCost;
        }
        
        public int CalculateNewMovementCost(Location fromLocation)
        {
            int cost = -1;
            int distance = Math.Abs(coordX - fromLocation.coordX) + Math.Abs(coordY - fromLocation.coordY);
            if (distance == 1)
            {
                cost = fromLocation.cost + 10;
            }
            return cost;
        }

        public int GetTotalCost()
        {
            return totalCost;
        }

        public int GetCost()
        {
            return cost;
        }        

        public Location GetParent()
        {
            return parent;
        }

        public void ResetParameters()
        {
            cost = 0;
            heuristic = 0;
            totalCost = 0;
            parent = null;
        }        

        public override string ToString()
        {
            return $"({coordX}, {coordY}) {totalCost}";
        }        
    }
}
