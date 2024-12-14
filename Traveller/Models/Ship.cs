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
            Components.Add(new Hull());
        }

        public int Id { get; set; }

        public List<ShipComponent> Components { get; private set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [DisplayName("Hull Type")]
        public Hull Hull
        {
            get => Components.OfType<Hull>().FirstOrDefault();
            set
            {
                var existingHull = Hull;
                if (existingHull != null)
                    Components.Remove(existingHull);
                if (value != null)
                    Components.Add(value);
            }
        }

        // Core systems
        public JumpDrive JumpDrive { get; set; }
        public ManeuverDrive ManeuverDrive { get; set; }
        public PowerPlant PowerPlant { get; set; }
        public Bridge Bridge { get; set; }
        public Computer Computer { get; set; }
        public decimal Cost { get; set; }
        public int HullPoints { get { return HullCalculator.CalculateHullPoints(Hull); } }

        public decimal TotalCost =>
            Hull.Cost;

        public decimal TotalCostMCr =>
            TotalCost / 1000000;
    }




    public class JumpDrive : CoreComponent
    {
        public int JumpRating { get; set; }
        public decimal FuelPerParsec { get; set; }

        public override bool ValidateRequirements(Ship ship)
        {
            // Add jump drive specific validation logic
            return base.ValidateRequirements(ship);
        }
    }

    public class ManeuverDrive : CoreComponent
    {
        public int ThrustRating { get; set; }

        public override bool ValidateRequirements(Ship ship)
        {
            // Add maneuver drive specific validation logic
            return base.ValidateRequirements(ship);
        }
    }

    public class PowerPlant : CoreComponent
    {
        public int PowerRating { get; set; }

        public override bool ValidateRequirements(Ship ship)
        {
            // Add power plant specific validation logic
            return base.ValidateRequirements(ship);
        }
    }

    public class Bridge : CoreComponent
    {
        public int MinimumCrew { get; set; }

        public override bool ValidateRequirements(Ship ship)
        {
            // Add bridge specific validation logic
            return base.ValidateRequirements(ship);
        }
    }

    public class Computer : CoreComponent
    {
        public int Rating { get; set; }
        public int Bis { get; set; }
        public decimal FibsRating { get; set; }

        public override bool ValidateRequirements(Ship ship)
        {
            // Add computer specific validation logic
            return base.ValidateRequirements(ship);
        }
    }

    public class Weapon : ShipComponent
    {
        public int DamageValue { get; set; }
        public string Range { get; set; }
        public bool RequiresTurret { get; set; }
        public decimal TurretTonnage { get; set; }
    }

    public class DefensiveSystem : ShipComponent
    {
        public int DefenseRating { get; set; }
        public string DefenseType { get; set; }
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