using Microsoft.AspNetCore.Mvc;
using Traveller.Models;
using Traveller.Extensions;
using Traveller.Calculators;

namespace Traveller.Controllers
{
    public class ShipBuilderController : Controller
    {
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
            return View("Step1_Hull", ship);
        }

        [HttpPost]
        public IActionResult SaveHull(Ship ship)
        {
            if (!ModelState.IsValid)
                return View("Step1_Hull", ship);

            SaveShipToSession(ship);
            return RedirectToAction("Step2_Drives");
        }

        public IActionResult Step2_Drives()
        {
            var ship = GetShipFromSession();
            if (ship == null)
                return RedirectToAction("Index");

            return View(ship);
        }

        [HttpPost]
        public IActionResult SaveDrives(JumpDrive jumpDrive, ManeuverDrive maneuverDrive)
        {
            var ship = GetShipFromSession();
            if (ship == null)
                return RedirectToAction("Index");

            ship.JumpDrive = jumpDrive;
            ship.ManeuverDrive = maneuverDrive;

            HttpContext.Session.Set(ShipSessionKey, ship);
            return RedirectToAction("Step3_PowerPlant");
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
            //not sure if this is how it should be - for now, work on the display.

            /*var armor = new Armor
            {
                ArmorType = armorType,
                TonsDisplacement = CalculateArmorTonnage(armorType, protectionLevel),
                Cost = CalculateArmorCost(armorType, protectionLevel),
                TechLevel = GetArmorTechLevel(armorType)
            };*/
                        
            var ship = GetShipFromSession();
            //ship.Components.Add(armor);
            SaveShipToSession(ship);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            var ship = GetShipFromSession();
            if (ship == null)
                return RedirectToAction("Index");

            return View(ship);
        }

        public IActionResult Test()
        {
            return View();
        }
    }
}
