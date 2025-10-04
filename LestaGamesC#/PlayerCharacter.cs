using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LestaGamesC_
{
    class Character : Weapon
    {
        public int _strenghtPlayer { get; set; }
        public int _agility { get; set; }
        public int _endurance { get; set; }
        public int _levelCharacter = 1;
        public double _healthCharacter { get; set; }
        public Weapon CurrentWeapon { get; set; }
        public string nameCharacter { get; set; }

        public int turnCount { get; set; } = 0;

        public Character()
        {
            CurrentWeapon = new Weapon();
        }

        public virtual void UpdateLevel() { }

       
        public virtual int ApplyAttackPeculiarity(Enemy enemy, int baseDamage)
        {
            return baseDamage;
        }

        public virtual int ApplyDefensePeculiarity(Enemy enemy, int incomingDamage)
        {
            return incomingDamage;
        }

        public virtual void StartTurn(Enemy enemy)
        {
            turnCount++;
        }
    }

    class Pillager : Character
    {
        private int poisonStacks = 0; 
        private bool poisonActive = false;

        public Pillager()
        {
            nameCharacter = "Разбойник";
            _healthCharacter = 4;
            CurrentWeapon = new Dagger();
        }

        public override void UpdateLevel()
        {
            switch (_levelCharacter)
            {
                case 2:
                    _agility++;
                    Console.WriteLine("Разбойник получил +1 к ловкости!");
                    break;
                case 3:
                    Console.WriteLine("Разбойник научился использовать яд!");
                    break;
            }
        }

        public override int ApplyAttackPeculiarity(Enemy enemy, int baseDamage)
        {
            int finalDamage = baseDamage;

            if (_levelCharacter >= 1 && _agility > enemy.agilityEnemy)
            {
                finalDamage += 1;
                Console.WriteLine("Скрытая атака: +1 к урону (ловкость выше противника)!");
            }

            if (_levelCharacter >= 3 && poisonActive)
            {
                finalDamage += poisonStacks;
                Console.WriteLine($"Яд: дополнительно +{poisonStacks} урона!");
            }

            return finalDamage;
        }

        public override void StartTurn(Enemy enemy)
        {
            base.StartTurn(enemy);

            if (_levelCharacter >= 3)
            {
                if (turnCount >= 2) 
                {
                    if (!poisonActive)
                    {
                        poisonActive = true;
                        Console.WriteLine("Яд начинает действовать!");
                    }
                    poisonStacks = turnCount - 1; 
                }
            }
        }
    }

    class Warrior : Character
    {
        private bool firstTurn = true;
        private bool shieldActive = false;

        public Warrior()
        {
            nameCharacter = "Воин";
            _healthCharacter = 5;
            CurrentWeapon = new Sword();
        }

        public override void UpdateLevel()
        {
            switch (_levelCharacter)
            {
                case 2:
                    Console.WriteLine("Воин получил щит!");
                    break;
                case 3:
                    _strenghtPlayer++;
                    Console.WriteLine("Воин получил +1 к силе!");
                    break;
            }
        }

        public override int ApplyAttackPeculiarity(Enemy enemy, int baseDamage)
        {
            int finalDamage = baseDamage;

            if (_levelCharacter >= 1 && firstTurn)
            {
                finalDamage *= 2;
                Console.WriteLine("Порыв к действию: двойной урон в первый ход!");
                firstTurn = false;
            }

            return finalDamage;
        }

        public override int ApplyDefensePeculiarity(Enemy enemy, int incomingDamage)
        {
            int finalDamage = incomingDamage;

            
            if (_levelCharacter >= 2 && _strenghtPlayer > enemy.strenghtEnemy)
            {
                shieldActive = true;
                finalDamage = Math.Max(0, incomingDamage - 3);
                if (finalDamage < incomingDamage)
                {
                    Console.WriteLine($"Щит воина: урон снижен с {incomingDamage} до {finalDamage}!");
                }
            }

            return finalDamage;
        }
    }

    class Barbarian : Character
    {
        private bool rageActive = false;
        private int rageBonus = 0;

        public Barbarian()
        {
            nameCharacter = "Варвар";
            _healthCharacter = 5;
            CurrentWeapon = new Club();
        }

        public override void UpdateLevel()
        {
            switch (_levelCharacter)
            {
                case 2:
                    Console.WriteLine("💎 Варвар научился использовать каменную кожу!");
                    break;
                case 3:
                    _endurance++;
                    Console.WriteLine("🏋️ Варвар получил +1 к выносливости!");
                    break;
            }
        }

        public override int ApplyAttackPeculiarity(Enemy enemy, int baseDamage)
        {
            int finalDamage = baseDamage;

            // Уровень 1: Ярость
            if (_levelCharacter >= 1)
            {
                if (turnCount <= 3) // Первые три хода
                {
                    rageActive = true;
                    rageBonus = 2;
                    finalDamage += rageBonus;
                    Console.WriteLine($"😡 Ярость варвара: +{rageBonus} к урону (ход {turnCount}/3)!");
                }
                else // После трех ходов
                {
                    rageBonus = Math.Max(0, rageBonus - 1);
                    if (rageBonus > 0)
                    {
                        finalDamage += rageBonus;
                        Console.WriteLine($"😡 Ярость варвара ослабевает: +{rageBonus} к урону!");
                    }
                    else if (rageActive)
                    {
                        Console.WriteLine("😴 Ярость варвара закончилась!");
                        rageActive = false;
                    }
                }
            }

            return finalDamage;
        }

        public override int ApplyDefensePeculiarity(Enemy enemy, int incomingDamage)
        {
            int finalDamage = incomingDamage;

            // Уровень 2: Каменная кожа
            if (_levelCharacter >= 2)
            {
                finalDamage = Math.Max(0, incomingDamage - _endurance);
                if (finalDamage < incomingDamage)
                {
                    Console.WriteLine($"💎 Каменная кожа: урон снижен с {incomingDamage} до {finalDamage} (выносливость: {_endurance})!");
                }
            }

            return finalDamage;
        }
    }
}