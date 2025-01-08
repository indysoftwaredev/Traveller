using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traveller.Calculators;
using Traveller.Models;

namespace ShipBuilderCalculatorTests
{
    public class ShipCargoSpaceCalculatorTests
    {

        [Fact]
        public void TestFixtureIsWorking()
        {
            Assert.True(true);
        }

        [Fact]
        public void ShipCargoSpaceIs_EqualToHullSpace_WhenShipIsNew()
        {
            Ship ship = new Ship();

            Assert.Equal(ship.Hull.UsableTonnage, ShipCargoSpaceCalculator.Calculate(ship));
        }

        [Fact]
        public void ShipSpaceIs_EqualToHullSpace_MinusComponentSpace()
        {
            Ship ship = new Ship();
            Armor armor = new Armor();
            armor.TonsDisplacement = 1;
            ship.Components.Add( armor );

            decimal usableTonnage = ship.Hull.UsableTonnage;
            Assert.Equal(ship.CargoSpace, usableTonnage - armor.TonsDisplacement );
        }
    }
}
