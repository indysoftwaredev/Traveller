using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traveller.Models;

namespace ShipBuilderCalculatorTests
{
    public class ShipTests
    {
        [Fact]
        public void TestFixtureIsWorking()
        {
            Assert.True(true);
        }

        [Fact]
        public void ProtectionLevel_IsZeroWithoutAnyArmor()
        {
            Assert.Equal(0, new Ship().ProtectionPoints);
        }

        [Fact]
        public void ProtectionLevel_IsEqualTo_ArmorProtectionLevel()
        {
            Ship ship = new Ship();
            Armor armor = new Armor();
            armor.ProtectionLevel = 100;
            ship.Components.Add(armor);
            Assert.Equal(100, ship.ProtectionPoints);
        }

        [Fact]
        public void ProtectionLevel_IsEqualTo_SumOfArmorProtectionLevel()
        {
            Ship ship = new Ship();
            Armor armor1 = new Armor();
            armor1.ProtectionLevel = 100;
            Armor armor2 = new Armor();
            armor2.ProtectionLevel = 100;
            ship.Components.Add(armor1);
            ship.Components.Add(armor2);
            Assert.Equal(200, ship.ProtectionPoints);
        }

    }
}
