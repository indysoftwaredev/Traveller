using Microsoft.AspNetCore.Mvc;
using Traveller.Models;
using Traveller.Extensions;
using Traveller.Calculators;

namespace Traveller.Controllers
{
    public class ShipBuilderController : Controller
    {
        private readonly ILogger<ShipBuilderController> _logger;

        public ShipBuilderController(ILogger<ShipBuilderController> logger)
        {
            _logger = logger;
        }

        private Ship GetShipFromSession()
        {
            return HttpContext.Session.Get<Ship>(ShipSessionKey) ?? new Ship();
        }

        private void SaveShipToSession(Ship ship)
        {
            HttpContext.Session.Set(ShipSessionKey, ship);
        }

        private const string ShipSessionKey = "_Ship";

        public IActionResult Index()
        {
            Ship ship = GetShipFromSession();
            return View("ShipBuilder", ship);
        }

        [HttpPost]
        public async Task<IActionResult> CalculateHull([FromForm] Hull hull)
        {
            var ship = GetShipFromSession();
            ship.Hull = hull;
            SaveShipToSession(ship);

            return await Task.FromResult(PartialView("~/Views/ShipBuilder/_ShipSummary.cshtml", ship));
        }

        [HttpGet]
        public IActionResult CalculateArmor(ArmorType armorType, int protectionLevel)
        {
            var ship = GetShipFromSession();
            var armor = ShipArmorCalculator.CalculateArmor(armorType, protectionLevel, ship);

            return Json(new
            {
                costMCr = armor.Cost,
                tonsDisplacement = armor.TonsDisplacement,
                techLevel = armor.TechLevel
            });
        }

        [HttpPost]
        public IActionResult AddArmor(ArmorType armorType, int protectionLevel)
        {
            var ship = GetShipFromSession();
            Armor armor = ShipArmorCalculator.CalculateArmor(armorType, protectionLevel, ship);
            ship.Components.Add(armor);
            SaveShipToSession(ship);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult RecalculateComponent(string componentType, int index)
        {
            var ship = GetShipFromSession();
            var component = ship.Components[index];

            //TODO: there's an opportunity to refactor here once we add more components            

           if (component is Armor armor)
            {
                var recalculated = ShipArmorCalculator.CalculateArmor(armor.ArmorType, armor.ProtectionLevel, ship);
                component.Cost = recalculated.Cost;
                component.TonsDisplacement = recalculated.TonsDisplacement;
                component.TechLevel = recalculated.TechLevel;
            }

            SaveShipToSession(ship);

            return Json(new
            {
                costMCr = component.CostMCr,
                tonsDisplacement = component.TonsDisplacement,
                techLevel = component.TechLevel,
                powerRequired = component.PowerRequired
            });
        }

        [HttpPost]
        public IActionResult DeleteComponent([FromBody] DeleteComponentModel model)
        {
            var ship = GetShipFromSession();
            if (ship != null)
            {
                var component = ship.Components.FirstOrDefault(c => c.Id == model.ComponentId);
                if (component != null)
                {
                    ship.Components.Remove(component);
                    SaveShipToSession(ship);
                }
            }
            return PartialView("_ShipSummary", ship);
        }

        public class DeleteComponentModel
        {
            public int ComponentId { get; set; }
        }
    }
}
