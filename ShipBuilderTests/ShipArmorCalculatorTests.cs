using Traveller.Calculators;
using Traveller.Models;

namespace ShipBuilderCalculatorTests
{
    public class ShipArmorCalculatorTests
    {
        [Fact]
        public void TestSuiteIsWorking()
        {
            Assert.True(true);
        }

        [Fact]
        public void TitaniumSteel_IsTechLevel_7()
        {
            Assert.Equal(7, ShipArmorCalculator.GetArmorTechLevel(ArmorType.TitaniumSteel));
        }

        [Fact]
        public void CrystalIron_IsTechLevel_10()
        {
            Assert.Equal(10, ShipArmorCalculator.GetArmorTechLevel(ArmorType.CrystalIron));
        }

        [Fact]
        public void BondedSuperdense_IsTechLevel_14()
        {
            Assert.Equal(14, ShipArmorCalculator.GetArmorTechLevel(ArmorType.BondedSuperdense));
        }

        [Fact]
        public void MolecularBonded_IsTechLevel_16()
        {

            Assert.Equal(16, ShipArmorCalculator.GetArmorTechLevel(ArmorType.MolecularBonded));
        }

        [Theory]
        [InlineData(1, 100, 2.5)]
        [InlineData(2, 100, 5)]
        public void TitaniumSteel_Tonnage_Is2Point5Percent_PerProtectionPoint(int protectionPoints, decimal hullTonnage, decimal expectedValue)
        {
            Assert.Equal(expectedValue, ShipArmorCalculator.CalculateArmorTonnage(ArmorType.TitaniumSteel, protectionPoints, hullTonnage));
        }

        [Theory]
        [InlineData(1, 100, 1.25)]
        [InlineData(2, 100, 2.5)]
        [InlineData(2, 200, 5)]
        public void CrystalIron_Tonnage_Is1Point25_PerProtectionPoint(int protectionPoints, decimal hullTonnage, decimal expectedValue)
        {
            Assert.Equal(expectedValue, ShipArmorCalculator.CalculateArmorTonnage(ArmorType.CrystalIron, protectionPoints, hullTonnage));
        }

        [Theory]
        [InlineData(1, 200, 1.6)]
        public void BondedSuperdense_Tonnage_IsPoint8_PerProtectionPoint(int protectionPoints, decimal hullTonnage, decimal expectedValue)
        {
            Assert.Equal(expectedValue, ShipArmorCalculator.CalculateArmorTonnage(ArmorType.BondedSuperdense, protectionPoints, hullTonnage));
        }

        [Theory]
        [InlineData(1, 200, 1)]
        public void MolecularBonded_Tonnage_IsPoint5_PerProtectionPoint(int protectionPoints, decimal hullTonnage, decimal expectedValue)
        {
            Assert.Equal(expectedValue, ShipArmorCalculator.CalculateArmorTonnage(ArmorType.MolecularBonded, protectionPoints, hullTonnage));
        }

        [Fact]
        public void TitaniumSteel_Cost_Is2Point5PercentOfHullCost()
        {
            decimal actual = ShipArmorCalculator.CalculateArmorCost(ArmorType.TitaniumSteel, 1, 100);

            Assert.Equal(2.5m, actual);
        }

        [Fact]
        public void CrystalIron_Cost_Is5PercentOfHullCost()
        {

            decimal actual = ShipArmorCalculator.CalculateArmorCost(ArmorType.CrystalIron, 1, 100);

            Assert.Equal(5, actual);
        }

        [Fact]
        public void BondedSuperdense_Cost_Is8PercentOfHullCost()
        {

            decimal actual = ShipArmorCalculator.CalculateArmorCost(ArmorType.BondedSuperdense, 1, 100);

            Assert.Equal(8, actual);
        }

        [Fact]
        public void MolecularBonded_Cost_Is15PercentOfHullCost()
        {

            decimal actual = ShipArmorCalculator.CalculateArmorCost(ArmorType.MolecularBonded, 1, 100);

            Assert.Equal(15, actual);
        }

    }
}
