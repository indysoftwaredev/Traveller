using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Traveller.Models
{
    public class Armor : ShipComponent
    {
        public override string Name { get; set; } = "";

        public override decimal TonsDisplacement { get => base.TonsDisplacement; set => base.TonsDisplacement = value; }

        public override decimal Cost { get => base.Cost; set => base.Cost = value; }

        private ArmorType armorType;
        public ArmorType ArmorType
        {
            get => armorType;
            set
            {
                armorType = value;
                Name = $"Armor: {GetDisplayName(value)}";
            }
        }

        private string GetDisplayName(ArmorType type)
        {
            var memberInfo = type.GetType().GetMember(type.ToString()).FirstOrDefault();
            var displayAttribute = memberInfo?.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.Name ?? type.ToString();
        }
    }

    public enum ArmorType
    {
        [Display(Name = "Titanium Steel")]
        TitaniumSteel,
        CrystalIron,
        [Display(Name = "Bonded Superdense")]
        BondedSuperdense,
        [Display(Name = "Molecular Bonded")]
        MolecularBonded
    }
}
