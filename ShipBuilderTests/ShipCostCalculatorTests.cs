using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traveller.Calculators;
using Traveller.Models;

namespace ShipBuilderCalculatorTests
{
    public class ShipCostCalculatorTests
    {

        [Fact]
        public void TestSuiteIsWorking()
        {
            Assert.True(true);
        }

        [Fact]
        public void NewShipCostIs_EqualTo_ItsHullCost()
        {
            //because every ship has a hull when created
            Hull hull = new Hull();
            Ship ship = new Ship();

            Assert.Equal(hull.Cost, ShipCostCalculator.Calculate(ship));
        }

        [Fact]
        public void ShipCost_IsCalculatedViaProperty()
        {
            Hull hull = new Hull();
            Ship ship = new Ship();

            Assert.Equal(hull.Cost, ship.Cost);
        }

    }
}
