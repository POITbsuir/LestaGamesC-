using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LestaGamesC_
{
   
    class Enemy 
    {
        public int enemyDamage { get; set; }
        public int healthEnemy { get; set; }
        public int strenghtEnemy { get; set; }
        public int agilityEnemy { get; set; }
        public int endurance { get; set; }
        public string nameEnemy { get; set; }
        public Weapon revortWeapon = new Weapon();
        public virtual void ApplyPeculiarity(Character playerCharacter, Weapon playerWeapon, int incomingDamage, out int modifiedDamage)
        {
            modifiedDamage = incomingDamage; 
        }
        public virtual void ApplyAttackPeculiarity(Fight fight) { }

        public Weapon GetWeaponDrop()
        {
            if (this is Goblin)
            {
                return new Dagger();
            }
            else if (this is Scelet)
            {
                return new Sword();
            }
            else if (this is Slime)
            {
                return new Club();
            }
            else if (this is Ghost)
            {
                return new Spear();
            }
            else if (this is Golem)
            {
                return new Axe();
            }
            else if (this is Dragon)
            {
                return new LegendarySword();
            }

            return null;
        }
    }

    class Goblin : Enemy
    {
        public Goblin()
        {
            nameEnemy = "Гоблин";
            healthEnemy = 5;
            enemyDamage = 2;
            agilityEnemy = 1;
            strenghtEnemy = 1;
            endurance = 1;
        }
    }

    class Scelet : Enemy
    {
        public Scelet()
        {
            nameEnemy = "Скелет";
            healthEnemy = 10;
            enemyDamage = 2;
            agilityEnemy = 2;
            strenghtEnemy = 2;
            endurance = 1;
        }

        public override void ApplyPeculiarity(Character playerCharacter, Weapon playerWeapon, int incomingDamage, out int modifiedDamage)
        {
            if (playerWeapon._crushing)
            {
                modifiedDamage = incomingDamage * 2;
                Console.WriteLine("---------------------");
                Console.WriteLine($"Особенность скелета: получает двойной урон от дробящего оружия! Урон увеличен до {modifiedDamage}!");
                Console.WriteLine("---------------------");
            }
            else
            {
                modifiedDamage = incomingDamage;
            }
        }
    }

    class Slime : Enemy
    {
        public Slime()
        {
            nameEnemy = "Слайм";
            healthEnemy = 8;
            enemyDamage = 1;
            agilityEnemy = 1;
            strenghtEnemy = 3;
            endurance = 2;
        }

        public override void ApplyPeculiarity(Character playerCharacter, Weapon playerWeapon, int incomingDamage, out int modifiedDamage)
        {
            int weaponDamage = playerWeapon._damage;
            int strengthAndBonusDamage = incomingDamage - weaponDamage;

            if (playerWeapon._chopping)
            {
                modifiedDamage = strengthAndBonusDamage;
                Console.WriteLine("---------------------");
                Console.WriteLine($" Особенность слайма: иммунитет к рубящему урону оружия! " +
                                  $"Урон от оружия заблокирован, но урон от силы и особенностей +" +
                                  $" ({strengthAndBonusDamage}) проходит!");
                Console.WriteLine("---------------------");
            }
            else
            {
                modifiedDamage = incomingDamage;
            }
        }
    }

    class Ghost : Enemy
    {
        public Ghost()
        {
            nameEnemy = "Призрак";
            healthEnemy = 6;
            enemyDamage = 3;
            agilityEnemy = 3;
            strenghtEnemy = 1;
            endurance = 3;
        }

        private int attackCount = 0;

        public override void ApplyAttackPeculiarity(Fight fight)
        {
            attackCount++;
            if (attackCount % 3 == 0)
            {
                int originalDamage = enemyDamage;
                enemyDamage *= 2;
                Console.WriteLine("---------------------");
                Console.WriteLine($"Особенность призрака: призрачная ярость! Урон увеличен с {originalDamage} до {enemyDamage}!");
                Console.WriteLine("---------------------");
            }
        }
    }

    class Golem : Enemy
    {
        public Golem()
        {
            nameEnemy = "Голем";
            healthEnemy = 10;
            enemyDamage = 1;
            agilityEnemy = 1;
            strenghtEnemy = 3;
            endurance = 3;
        }

        public override void ApplyPeculiarity(Character playerCharacter, Weapon playerWeapon, int incomingDamage, out int modifiedDamage)
        {
            int reducedDamage = Math.Max(0, incomingDamage - endurance);
            if (reducedDamage < incomingDamage)
            {
                Console.WriteLine("---------------------");
                Console.WriteLine($"Особенность голема: каменная кожа снижает урон на {endurance}! " +
                                  $"Урон уменьшен с {incomingDamage} до {reducedDamage}!");
                Console.WriteLine("---------------------");
            }
            modifiedDamage = reducedDamage;
        }
    }

    class Dragon : Enemy
    {
        private int moveCount = 0;
        public Dragon()
        {
            nameEnemy = "Дракон";
            healthEnemy = 20;
            enemyDamage = 4;
            agilityEnemy = 3;
            strenghtEnemy = 3;
            endurance = 3;
        }

        public override void ApplyAttackPeculiarity(Fight fight)
        {
            moveCount++;
            if (moveCount % 3 == 0)
            {
                int originalDamage = enemyDamage;
                enemyDamage += 3;
                Console.WriteLine("---------------------");
                Console.WriteLine($"Особенность дракона: огненное дыхание! Урон увеличен с {originalDamage} до {enemyDamage}!");
                Console.WriteLine("---------------------");
            }
        }
    }
}