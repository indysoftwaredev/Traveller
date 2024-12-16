using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Traveller.Calculators;

namespace Traveller.Models
{
    public class Hull : ShipComponent
    {
        public override string Name { get; set; } = "Hull";

        [Range(10, int.MaxValue, ErrorMessage = "Tonnage must be greater than 10.")]
        public override int TonsDisplacement { get; set; } = 10;

        public override decimal CostMCr { get { return Cost / 1000000; } }

        public override decimal Cost { get { return HullCalculator.CalculateCost(this); } }

        public bool GravityHull { get; set; } = true;

        [DisplayName("Hull Configuration")]
        public HullConfiguration HullConfiguration { get; set; }

        [DisplayName("Hull Construction")]
        public Construction Construction { get; set; }

        public decimal UsableTonnage
        {
            get
            {
                return HullCalculator.CalculateUsableTonnage(this);
            }
        }

        public string Notes { 
            get 
            {
                return "";
            } 
        }
    }

    public enum HullConfiguration
    {
        Standard,
        Streamlined,
        Sphere,
        [Display(Name = "Close Structure")]
        CloseStructure,
        [Display(Name = "Dispersed Structure")]
        DispersedStructure,
        Planetoid,
        [Display(Name = "Buffered Planetoid")]
        BufferedPlanetoid
    }

    public enum Construction
    {
        Standard,
        Reinforced,
        Light
    }
}
