using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LestaGamesC_
{
    interface IWeapon
    {
         bool _chopping { get; set; }//рубящий
         bool _crushing { get; set; }//дробящий
         bool _piercing { get; set; } //колющий 
         int _damage { get; set; }
        
    }
    public class Weapon : IWeapon
    {
        public bool _chopping { get; set; }
        public bool _crushing { get; set; }
        public bool _piercing { get; set; }
        public int _damage { get; set; }

        public string nameWeapon { get; set; }

    }
    class Sword : Weapon
    {
        public Sword()
        {
            nameWeapon = "Меч";
            _damage = 3;
            _chopping = true;
        }
    }
    class Club : Weapon
    {
        public Club()
        {
            nameWeapon = "Дубина";
            _damage = 3;
            _crushing = true;
        }
    }
    class Dagger : Weapon 
    {
        public Dagger()
        {
            nameWeapon = "Кинжал";
            _damage = 2;
            _piercing = true;
        }
    }
    class Axe : Weapon
    {
        public Axe()
        {
            nameWeapon = "Топор";
            _damage = 4;
            _chopping = true;
        }
    }
    class Spear : Weapon
    {
        public Spear()
        {
            nameWeapon = "Копье";
            _damage = 3;
            _crushing = true;
        }
    }
    class LegendarySword : Weapon 
    {
        public LegendarySword ()
        {
            nameWeapon = "Легендарный меч";
            _damage = 5;
            _chopping = true;
        }
    }

}
