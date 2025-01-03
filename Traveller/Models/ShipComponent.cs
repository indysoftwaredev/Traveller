using System.ComponentModel.DataAnnotations;

namespace Traveller.Models
{
    public abstract class ShipComponent
    {
        public int Id { get; set; }

        [Required]
        public virtual string Name { get; set; } = "ComponentName";

        [Required]
        public virtual decimal PowerRequired { get; set; }

        [Required]
        public virtual decimal TonsDisplacement { get; set; }

        [Required]
        [Display(Name = "Cost (MCr)")]
        public virtual decimal CostMCr { get => Cost / 1000000; }

        public virtual decimal Cost { get; set; }

        public virtual int TechLevel { get; set; }
    }
}
