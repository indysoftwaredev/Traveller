using Microsoft.CodeAnalysis.CSharp.Syntax;
using Traveller.Models;

namespace Traveller.Calculators
{
    public class ShipArmorCalculator
    {
        public static Armor CalculateArmor(ArmorType armorType, int protectionLevel, Ship ship) {
            var armor = new Armor
            {
                ArmorType = armorType,
                TonsDisplacement = CalculateArmorTonnage(armorType, protectionLevel, ship.Hull.TonsDisplacement),
                Cost = CalculateArmorCost(armorType, protectionLevel, HullCalculator.CalculateCost(ship.Hull)),

                TechLevel = GetArmorTechLevel(armorType),
                ProtectionLevel = protectionLevel

            };

            return armor;
        }

        public static decimal CalculateArmorTonnage(ArmorType armorType, int protectionLevel, decimal hullTonnage)
        {
            decimal modifier = GetArmorTonnageModifier(armorType);
            return hullTonnage * modifier * protectionLevel / 100;
        }

        private static decimal GetArmorTonnageModifier(ArmorType armorType) 
        {
            switch (armorType)
            {
                case ArmorType.TitaniumSteel: return 2.5m;
                case ArmorType.CrystalIron: return 1.25m;
                case ArmorType.BondedSuperdense: return .8m;
                case ArmorType.MolecularBonded: return .5m;
            }
            return 1m;
        }

        public static decimal CalculateArmorCost(ArmorType armorType, int protectionLevel, decimal hullCost)
        {
            decimal modifier = GetArmorCostModifier(armorType);
            return hullCost * modifier * protectionLevel / 100;
        }

        private static decimal GetArmorCostModifier(ArmorType armorType)
        {
            switch (armorType)
            {
                case ArmorType.TitaniumSteel: return 2.5m;
                case ArmorType.CrystalIron: return 5m;
                case ArmorType.BondedSuperdense: return 8m;
                case ArmorType.MolecularBonded: return 15m;
            }
            return 1m;
        }

        public static int GetArmorTechLevel(ArmorType armorType)
        {
            switch(armorType)
            {
                case ArmorType.TitaniumSteel: return 7;
                case ArmorType.CrystalIron: return 10;
                case ArmorType.BondedSuperdense: return 14;
                case ArmorType.MolecularBonded: return 16;
            }
            return 1;
        }


    }
}
