using Microsoft.IdentityModel.Tokens;
using Traveller.Models;

namespace Traveller.Calculators
{
    public class HullCalculator
    {
        public static decimal CalculateCost(Hull hull)
        {
            decimal base_cost = CalculateBaseCost(hull);

            decimal gravityHullModifier = hull.GravityHull ? 1 : .5m;
            decimal constructionModifier = CalculateConstructionCostModifier(hull);
            decimal hullTypeModifier = CalculateHullTypeCostModifier(hull);

            return base_cost * gravityHullModifier * constructionModifier * hullTypeModifier;
        }

        private static decimal CalculateBaseCost(Hull hull)
        {
            decimal costPerTon = 50000;
            if (hull.HullConfiguration == HullConfiguration.Planetoid || hull.HullConfiguration == HullConfiguration.BufferedPlanetoid)
            {
                costPerTon = 4000;
            }
            return hull.TonsDisplacement * costPerTon;

        }

        private static decimal CalculateHullTypeCostModifier(Hull hull)
        {
            decimal hullTypeModifier = 1;
            switch (hull.HullConfiguration)
            {
                case HullConfiguration.Streamlined: hullTypeModifier = 1.2m; break;
                case HullConfiguration.Sphere: hullTypeModifier = .8m; break;
                case HullConfiguration.CloseStructure: hullTypeModifier = .9m; break;
                case HullConfiguration.DispersedStructure: hullTypeModifier = .5m; break;
            }
            return hullTypeModifier;
        }

        private static decimal CalculateConstructionCostModifier(Hull hull)
        {
            decimal constructionModifier = 1;
            if (hull.Construction == Construction.Reinforced)
            {
                constructionModifier = 1.5m;
            }
            if (hull.Construction == Construction.Light)
            {
                constructionModifier = .75m;
            }
            return constructionModifier;
        }

        public static int CalculateHullPoints(Hull hull)
        {
            decimal tonsPerHullPoint = 2.5m;
            decimal constructionModifier = CalculateConstructionHullPointsModifier(hull);
            decimal hullTypePointsModifier = CalculateHullTypePointsModifier(hull);

            if (hull.TonsDisplacement >= 100000)
            {
                tonsPerHullPoint = 1.5m;
            }
            else if (hull.TonsDisplacement >= 25000)
            {
                tonsPerHullPoint = 2;
            }

            return (int)(hull.TonsDisplacement / tonsPerHullPoint * constructionModifier * hullTypePointsModifier);
        }

        private static decimal CalculateHullTypePointsModifier(Hull hull)
        {
            decimal hullTypeModifier = 1;
            switch (hull.HullConfiguration)
            {
                case HullConfiguration.CloseStructure: hullTypeModifier = 1.1m; break;
                case HullConfiguration.DispersedStructure: hullTypeModifier = .9m; break;
                case HullConfiguration.Planetoid: hullTypeModifier = 1.25m; break;
                case HullConfiguration.BufferedPlanetoid: hullTypeModifier = 1.5m; break;

            }
            return hullTypeModifier;

        }

        private static decimal CalculateConstructionHullPointsModifier(Hull hull)
        {
            decimal constructionModifier = 1;
            if (hull.Construction == Construction.Reinforced)
            {
                constructionModifier = 1.1m;
            }
            if (hull.Construction == Construction.Light)
            {
                constructionModifier = .9m;
            }
            return constructionModifier;
        }

        public static decimal CalculateUsableTonnage(Hull hull)
        {
            decimal tonnageModifier = 1;
            if (hull.HullConfiguration == HullConfiguration.Planetoid) { tonnageModifier = .8m; }
            if (hull.HullConfiguration == HullConfiguration.BufferedPlanetoid) { tonnageModifier = .65m; }
            return hull.TonsDisplacement * tonnageModifier;
        }
    }
}
