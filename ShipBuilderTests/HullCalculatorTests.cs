using Traveller.Calculators;
using Traveller.Models;

namespace ShipBuilderTests
{
    public class HullCalculatorTests
    {
        [Fact]
        public void TestFixtureIsWorking()
        {
            Assert.True(true);
        }

        [Fact]
        public void CalculateCost_ItCosts50KPerTon()
        {
            Hull hull = new Hull();
            hull.TonsDisplacement = 100;

            Assert.Equal(100 * 50000, HullCalculator.CalculateCost(hull));
        }

        [Theory]
        [InlineData(100, 40)]
        [InlineData(101, 40)]
        [InlineData(102, 40)]
        [InlineData(103, 41)]
        [InlineData(104, 41)]
        [InlineData(105, 42)]
        public void CalculateHullPoints_1Point_Per2AndAHalfTons(int tonsDisplacement, int expectedHullPoints)
        {
            Hull hull = new Hull();
            hull.TonsDisplacement = tonsDisplacement;
            Assert.Equal(expectedHullPoints, HullCalculator.CalculateHullPoints(hull));
        }

        [Theory]
        [InlineData(25000, 12500)]
        [InlineData(25001, 12500)]
        [InlineData(25002, 12501)]
        public void CalculateHullPoints_2Points_Per2Tons_WhenTonsAreGreaterOrEqualTo_25K(int tonsDisplacement, int expectedHullPoints)
        {
            Hull hull = new Hull();
            hull.TonsDisplacement = tonsDisplacement;
            Assert.Equal(expectedHullPoints, HullCalculator.CalculateHullPoints(hull));
        }

        [Theory]
        [InlineData(100000, 66666)]
        [InlineData(100001, 66667)]
        [InlineData(100002, 66668)]
        public void CalculateHullPoints_1HullPoint_Per1AndAHalfTons_WhenTonsAreGreaterOrEqualTo_100K(int tonsDisplacement, int expectedHullPoints)
        {
            Hull hull = new Hull();
            hull.TonsDisplacement = tonsDisplacement;
            Assert.Equal(expectedHullPoints, HullCalculator.CalculateHullPoints(hull));
        }

        [Fact]
        public void CalculateCost_25KPerTon_WhenGravityHullIsFalse()
        {
            Hull hull = new Hull
            {
                TonsDisplacement = 100,
                GravityHull = false
            };

            Assert.Equal(2500000, HullCalculator.CalculateCost(hull));
        }

        [Fact]
        public void CalculateCost_ReinforcedHullCostsFiftyPercentMore()
        {
            Hull hull = new Hull
            {
                TonsDisplacement = 100,
                Construction = Construction.Reinforced
            };

            Assert.Equal(75000 * 100, HullCalculator.CalculateCost(hull));
        }

        [Fact]
        public void CalculateHullPoints_ReinforcedHullHas10PercentMoreHullPoints()
        {
            Hull hull = new Hull
            {
                TonsDisplacement = 100,
                Construction = Construction.Reinforced
            };

            Assert.Equal(44, HullCalculator.CalculateHullPoints(hull));
        }

        [Fact]
        public void CalculateHullPoints_LightHullCosts25PercentLess()
        {

            Hull hull = new Hull
            {
                TonsDisplacement = 100,
                Construction = Construction.Light
            };

            Assert.Equal(50000 * hull.TonsDisplacement * .75m, HullCalculator.CalculateCost(hull));
        }

        [Fact]
        public void CalculateHullPoints_LightHullHas10PercentFewerHullPoints()
        {
            Hull hull = new Hull
            {
                TonsDisplacement = 100,
                Construction = Construction.Light
            };

            Assert.Equal(36, HullCalculator.CalculateHullPoints(hull));
        }

        [Fact]
        public void CalculateHullCost_StreamlinedCosts20PercentMore()
        {
            Hull hull = new Hull
            {
                TonsDisplacement = 100,
                HullConfiguration = HullConfiguration.Streamlined
            };

            Assert.Equal(50000 * hull.TonsDisplacement * 1.2m, HullCalculator.CalculateCost(hull));
        }

        [Fact]
        public void CalculateHullCost_SphereCosts20PercentLess()
        {
            Hull hull = new Hull
            {
                TonsDisplacement = 100,
                HullConfiguration = HullConfiguration.Sphere
            };

            Assert.Equal(50000 * hull.TonsDisplacement * .8m, HullCalculator.CalculateCost(hull));
        }

        [Fact]
        public void CalculateHullCost_CloseStructureCosts10PercentLess()
        {
            Hull hull = new Hull
            {
                TonsDisplacement = 100,
                HullConfiguration = HullConfiguration.CloseStructure
            };

            Assert.Equal(50000 * hull.TonsDisplacement * .9m, HullCalculator.CalculateCost(hull));
        }

        [Fact]
        public void CalculateHullPoints_CloseStructureHas10PercentMore()
        {
            Hull hull = new Hull
            {
                TonsDisplacement = 100,
                HullConfiguration = HullConfiguration.CloseStructure
            };

            Assert.Equal(44, HullCalculator.CalculateHullPoints(hull));
        }

        [Fact]
        public void CalculateHullCost_DispersedStructureCosts50PercentLess()
        {
            Hull hull = new Hull
            {
                TonsDisplacement = 100,
                HullConfiguration = HullConfiguration.DispersedStructure
            };

            Assert.Equal(50000 * hull.TonsDisplacement * .5m, HullCalculator.CalculateCost(hull));
        }

        [Fact]
        public void CalculateHullPoints_DispersedStructureHas10PercentLess()
        {
            Hull hull = new Hull
            {
                TonsDisplacement = 100,
                HullConfiguration = HullConfiguration.DispersedStructure
            };

            Assert.Equal(36, HullCalculator.CalculateHullPoints(hull));
        }

        [Fact]
        public void CalculateHullCost_PlanetoidStructureCosts4000PerTon()
        {
            Hull hull = new Hull
            {
                TonsDisplacement = 100,
                HullConfiguration = HullConfiguration.Planetoid
            };

            Assert.Equal(4000 * hull.TonsDisplacement, HullCalculator.CalculateCost(hull));
        }

        [Fact]
        public void CalculateHullPoints_PlanetoidStructureHas25PercentMore()
        {
            Hull hull = new Hull
            {
                TonsDisplacement = 100,
                HullConfiguration = HullConfiguration.Planetoid
            };

            Assert.Equal(50, HullCalculator.CalculateHullPoints(hull));
        }

        [Fact]
        public void CalculateHullCost_BufferedPlanetoidStructureCosts4000PerTon()
        {
            Hull hull = new Hull
            {
                TonsDisplacement = 100,
                HullConfiguration = HullConfiguration.BufferedPlanetoid
            };

            Assert.Equal(4000 * hull.TonsDisplacement, HullCalculator.CalculateCost(hull));
        }

        [Fact]
        public void CalculateHullPoints_PlanetoidStructureHas50PercentMore()
        {
            Hull hull = new Hull
            {
                TonsDisplacement = 100,
                HullConfiguration = HullConfiguration.BufferedPlanetoid
            };

            Assert.Equal(60, HullCalculator.CalculateHullPoints(hull));
        }

        [Fact]
        public void CalculateUsableTonnage_DefaultIsHullTonnage()
        {
            Hull hull = new Hull();
            hull.TonsDisplacement = 100;
            Assert.Equal(100, hull.UsableTonnage);
        }

        [Fact]
        public void CalculateUsableTonnage_PlanetoidIs80Percent()
        {
            Hull hull = new Hull();
            hull.TonsDisplacement = 100;
            hull.HullConfiguration = HullConfiguration.Planetoid;
            Assert.Equal(80, hull.UsableTonnage);
        }

        [Fact]
        public void CalculateUsableTonnage_BufferedPlanetoidIs80Percent()
        {
            Hull hull = new Hull();
            hull.TonsDisplacement = 100;
            hull.HullConfiguration = HullConfiguration.BufferedPlanetoid;
            Assert.Equal(65, hull.UsableTonnage);
        }
    }
}