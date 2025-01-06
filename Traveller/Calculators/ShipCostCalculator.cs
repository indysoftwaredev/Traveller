using System.CodeDom;
using Traveller.Models;

namespace Traveller.Calculators
{
    public class ShipCostCalculator
    {
        public static decimal Calculate(Ship ship)
        {
            decimal cost = 0;
            cost += ship.Hull.Cost;

            //when there are components in the component list, 
            //we will compile those costs as well

            return cost;
        }
    }
}
