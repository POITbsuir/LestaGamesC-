using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LestaGamesC_
{
    class Fight
    {
        public int _move { get; set; }
        public int _damageNPC { get; set; }
        public int _strenghtNPC;

        Character character;
        Enemy enemy;
        Random randomNumber;
        int playerWeaponDamage;
        Weapon currentPlayerWeapon;

        public Fight(Character character, Enemy enemy)
        {
            this.character = character;
            this.enemy = enemy;
            this.randomNumber = new Random();
            this.currentPlayerWeapon = character.CurrentWeapon;
            this.playerWeaponDamage = currentPlayerWeapon._damage;
        }

        private Weapon GetPlayerStartingWeapon()
        {
            if (character is Pillager)
            {
                return new Dagger();
            }
            else if (character is Warrior)
            {
                return new Sword();
            }
            else if (character is Barbarian)
            {
                return new Club();
            }
            return new Weapon();
        }

        private bool OfferWeaponChoice(Weapon droppedWeapon)
        {
            Console.WriteLine($"\nС противника выпало оружие: {droppedWeapon.nameWeapon}");
            Console.WriteLine($"Урон нового оружия: {droppedWeapon._damage}");
            Console.WriteLine($"Ваше текущее оружие: {currentPlayerWeapon.nameWeapon}");
            Console.WriteLine($"Урон текущего оружия: {currentPlayerWeapon._damage}");

            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1 - Взять новое оружие");
            Console.WriteLine("2 - Оставить текущее оружие");

            int choice;
            while (true)
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out choice) && (choice == 1 || choice == 2))
                {
                    break;
                }
                Console.WriteLine("Ошибка! Введите 1 или 2:");
            }

            if (choice == 1)
            {
                currentPlayerWeapon = droppedWeapon;
                playerWeaponDamage = currentPlayerWeapon._damage;
                character.CurrentWeapon = currentPlayerWeapon;
                Console.WriteLine($"Вы используете оружие: {currentPlayerWeapon.nameWeapon}!");
                return true;
            }
            else
            {
                Console.WriteLine("Вы оставили свое текущее оружие."); 
                return false;
            }
        }

        private bool PlayerTurn()
        {
            Console.WriteLine("\nХод игрока");
            character.StartTurn(enemy);

            int sumAttack = character._agility + enemy.agilityEnemy;
            double chanceHitting = randomNumber.Next(1, sumAttack);

            if (chanceHitting <= enemy.agilityEnemy)
            {
                Console.WriteLine("Игрок промахнулся, следующий шаг противника");
                return false;
            }
            else
            {
                Console.WriteLine("Игрок попал!");

                int baseDamage = playerWeaponDamage + character._strenghtPlayer;
                int characterBonusDamage = character.ApplyAttackPeculiarity(enemy, baseDamage);
                enemy.ApplyPeculiarity(character, currentPlayerWeapon, characterBonusDamage, out int finalDamage);

                enemy.healthEnemy -= finalDamage;
                Console.WriteLine($"Противнику нанесен урон: {finalDamage}");
                Console.WriteLine($"Здоровье противника: [{enemy.healthEnemy}хп]");
                return enemy.healthEnemy <= 0;
            }
        }

        private bool EnemyTurn()
        {
            Console.WriteLine("\nХод противника");
            enemy.ApplyAttackPeculiarity(this);

            int sumAttack = character._agility + enemy.agilityEnemy;
            double chanceHitting = randomNumber.Next(1, sumAttack);

            if (chanceHitting <= character._agility)
            {
                Console.WriteLine("Противник промахнулся, следующий шаг игрока");
                return false;
            }
            else
            {
                Console.WriteLine("Противник попал!");

               
                int baseDamage = enemy.enemyDamage + enemy.strenghtEnemy;

                int finalDamage = character.ApplyDefensePeculiarity(enemy, baseDamage);

                character._healthCharacter -= finalDamage;
                Console.WriteLine($"Игроку нанесен урон: {finalDamage}");
                Console.WriteLine($"Здоровье игрока: [{character._healthCharacter}хп]");
                return character._healthCharacter <= 0;
            }
        }

        private void ExecutePlayerAttack()
        {
            bool enemyDefeated = PlayerTurn();
            if (enemyDefeated)
            {
                Console.WriteLine("\nИГРОК ВЫИГРАЛ БИТВУ!");
                character._levelCharacter++;

                Weapon droppedWeapon = enemy.GetWeaponDrop();
                if (droppedWeapon != null)
                {
                    OfferWeaponChoice(droppedWeapon);
                }
                else
                {
                    Console.WriteLine("С противника ничего не выпало.");
                }

                // Восстановление здоровья после победы
                if (character is Pillager)
                {
                    character._healthCharacter = 4;
                }
                else if (character is Warrior || character is Barbarian)
                {
                    character._healthCharacter = 5;
                }

                Console.WriteLine($"{character.nameCharacter} восстановил здоровье до {character._healthCharacter} HP");
                Console.WriteLine($"Текущее оружие: {currentPlayerWeapon.nameWeapon} (Урон: {playerWeaponDamage})");
            }
        }

        private void ExecuteEnemyAttack()
        {
            bool playerDefeated = EnemyTurn();
            if (playerDefeated)
            {
                Console.WriteLine("\nБИТВУ ВЫИГРАЛ ПРОТИВНИК...");
                if (enemy is Goblin)
                {
                    enemy.healthEnemy = 5;
                }
                else if (enemy is Scelet)
                {
                    enemy.healthEnemy = 10;
                }
                else if (enemy is Slime)
                {
                    enemy.healthEnemy = 8;
                }
                else if (enemy is Ghost)
                {
                    enemy.healthEnemy = 6;
                }
                else if (enemy is Golem)
                {
                    enemy.healthEnemy = 10;
                }
                else if (enemy is Dragon)
                {
                    enemy.healthEnemy = 20;
                }

                Console.WriteLine($"{enemy.nameEnemy} восстановил здоровье до {enemy.healthEnemy} HP");
            }
        }

        private void HandlePlayerPriorityTurn()
        {
            ExecutePlayerAttack();
            if (enemy.healthEnemy > 0)
            {
                ExecuteEnemyAttack();
            }
        }

        private void HandleEnemyPriorityTurn()
        {
            ExecuteEnemyAttack();
            if (character._healthCharacter > 0)
            {
                ExecutePlayerAttack();
            }
        }

        private void HandleEqualAgilityTurn()
        {
            ExecutePlayerAttack();
            if (enemy.healthEnemy > 0)
            {
                ExecuteEnemyAttack();
            }
        }

        public bool Attack()
        {
            Console.WriteLine("\nНачало битвы!");
            Console.WriteLine($"{character.nameCharacter} (Уровень: {character._levelCharacter}) против {enemy.nameEnemy}");
            Console.WriteLine($"Здоровье игрока: {character._healthCharacter}");
            Console.WriteLine($"Здоровье противника: {enemy.healthEnemy}");

            while (character._healthCharacter > 0 && enemy.healthEnemy > 0)
            {
                Console.WriteLine($"\n--- Ход {_move + 1} ---");

                if (character._agility > enemy.agilityEnemy)
                {
                    HandlePlayerPriorityTurn();
                }
                else if (character._agility < enemy.agilityEnemy)
                {
                    HandleEnemyPriorityTurn();
                }
                else
                {
                    HandleEqualAgilityTurn();
                }

                _move++;

                if (character._healthCharacter <= 0 || enemy.healthEnemy <= 0)
                {
                    break;
                }
            }

            if (character._healthCharacter > 0)
            {
                Console.WriteLine("\n Поздравляем! Вы победили!");
                return true;
            }
            else
            {
                Console.WriteLine("\n К сожалению, вы проиграли...");
                return false;
            }
        }
    }
}