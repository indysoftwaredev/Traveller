using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Traveller.Calculators;

namespace Traveller.Models
{
    [Serializable]
    public class Ship
    {
        public Ship()
        {
            Components = new List<ShipComponent>();
        }

        public int Id { get; set; }

        [DisplayName("Hull Type")]
        public Hull Hull { get; set; } = new Hull();

        public List<ShipComponent> Components { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = String.Empty;

        public int HullPoints => HullCalculator.CalculateHullPoints(Hull);

        public decimal Cost => ShipCostCalculator.Calculate(this);
        
        public decimal TotalCostMCr => Cost / 1000000;
    }

    public enum TechLevel
    {
        TL7 = 7,
        TL8 = 8,
        TL9 = 9,
        TL10 = 10,
        TL11 = 11,
        TL12 = 12,
        TL13 = 13,
        TL14 = 14,
        TL15 = 15
    }
}