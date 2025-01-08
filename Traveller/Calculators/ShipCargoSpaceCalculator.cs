using Traveller.Models;

namespace Traveller.Calculators
{
    public class ShipCargoSpaceCalculator
    {
        public static decimal Calculate(Ship ship)
        {
            decimal cargoSpace = ship.Hull.UsableTonnage;

            foreach (ShipComponent shipComponent in ship.Components)
            {
                cargoSpace -= shipComponent.TonsDisplacement;
            }

            return cargoSpace;
        }
    }
}
