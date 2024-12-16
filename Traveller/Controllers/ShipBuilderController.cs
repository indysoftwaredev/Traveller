using Microsoft.AspNetCore.Mvc;
using Traveller.Models;
using Traveller.Extensions;

namespace Traveller.Controllers
{
    public class ShipBuilderController : Controller
    {
        private const string ShipSessionKey = "_Ship";

        public IActionResult Index()
        {
            var ship = HttpContext.Session.Get<Ship>(ShipSessionKey) ?? new Ship();
            return View("Step1_Hull", ship);
        }

        [HttpPost]
        public IActionResult SaveHull(Ship ship)
        {
            if (!ModelState.IsValid)
                return View("Step1_Hull", ship);

            HttpContext.Session.Set(ShipSessionKey, ship);
            return RedirectToAction("Step2_Drives");
        }

        public IActionResult Step2_Drives()
        {
            var ship = HttpContext.Session.Get<Ship>(ShipSessionKey);
            if (ship == null)
                return RedirectToAction("Index");

            return View(ship);
        }

        [HttpPost]
        public IActionResult SaveDrives(JumpDrive jumpDrive, ManeuverDrive maneuverDrive)
        {
            var ship = HttpContext.Session.Get<Ship>(ShipSessionKey);
            if (ship == null)
                return RedirectToAction("Index");

            ship.JumpDrive = jumpDrive;
            ship.ManeuverDrive = maneuverDrive;

            HttpContext.Session.Set(ShipSessionKey, ship);
            return RedirectToAction("Step3_PowerPlant");
        }

        // Add similar actions for other steps...

        public IActionResult Summary()
        {
            var ship = HttpContext.Session.Get<Ship>(ShipSessionKey);
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
