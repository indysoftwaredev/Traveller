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

            foreach (ShipComponent shipComponent in ship.Components)
            {
                cost += shipComponent.Cost;
            }

            return cost;
        }
    }
}
