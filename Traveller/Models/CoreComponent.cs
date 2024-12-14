using System.ComponentModel.DataAnnotations;

namespace Traveller.Models
{
    public abstract class CoreComponent : ShipComponent
    {
        [Required]
        public bool IsRequired { get; set; } = true;

        public virtual bool ValidateRequirements(Ship ship)
        {
            // Base validation for core components
            return true;
        }
    }
}
