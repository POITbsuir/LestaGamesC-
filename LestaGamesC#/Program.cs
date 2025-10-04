using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace LestaGamesC_
{
    
    class Program
    {
        static int getNumber(int MAX, int MIN)
        {
            while (true)
            {
                string userInput = Console.ReadLine()?.Trim() ?? "";
                if (int.TryParse(userInput, out int playerChoise))
                { 
                    if(playerChoise >= MIN && playerChoise <= MAX)
                    {
                        return playerChoise;
                    }
                }
                Console.WriteLine("Ошибка!!! Введите число от 1 до заданного диапозона...");
            }
            
        }

        static void ApplyLevelUpBenefits(Character character)
        {
            if (character is Pillager pillager)
            {
                if (pillager._levelCharacter == 2)
                {
                    pillager._agility++;
                    Console.WriteLine("Разбойник получил +1 к ловкости!");
                }
                else if (pillager._levelCharacter == 3)
                {
                    Console.WriteLine("Разбойник научился использовать яд!");
                }
            }
            else if (character is Warrior warrior)
            {
                if (warrior._levelCharacter == 2)
                {
                   
                    Console.WriteLine("Воин получил щит!");
                }
                else if (warrior._levelCharacter == 3)
                {
                    warrior._strenghtPlayer++;
                    Console.WriteLine("Воин получил +1 к силе!");
                }
            }
            else if (character is Barbarian barbarian)
            {
                if (barbarian._levelCharacter == 2)
                {
                   
                    Console.WriteLine("Варвар научился впадать в ярость!");
                }
                else if (barbarian._levelCharacter == 3)
                {
                    barbarian._endurance++;
                    Console.WriteLine("Варвар получил +1 к выносливости!");
                }
            }

            
            ResetCharacterHealth(character);
        }

        static void ResetCharacterHealth(Character character)
        {
            if (character is Pillager)
            {
                character._healthCharacter = 4;
            }
            else if (character is Warrior || character is Barbarian)
            {
                character._healthCharacter = 5;
            }
        }

        static bool Play(Character characterGame, Enemy enemyGame)
        {
            char input;
            Console.WriteLine("Для начала игры нажмите клавишу P: ");
            input = char.Parse(Console.ReadLine());
            if (input == 'P' || input == 'p')
            {
                Fight gameFight = new Fight(characterGame, enemyGame);
                return gameFight.Attack(); 
            }
            return false;
        }


        static void Main(string[] args)
        {
            Random randomNew = new Random();
            bool playAgain = true;

            while (playAgain)
            {
                Console.Clear();
                Console.WriteLine("=== ГЛАВНОЕ МЕНЮ ===");
                Console.WriteLine("Добро пожаловать в игру! Выберите персонажа (от 1 до 3):");
                Console.WriteLine("1 - Разбойник");
                Console.WriteLine("2 - Воин");
                Console.WriteLine("3 - Варвар");
               

                int choise = getNumber(3, 1);

                Character characterGame;
                int victoryCount = 0; 

                
                switch (choise)
                {
                    case 1:
                        characterGame = new Pillager();
                        Console.WriteLine("Отличный выбор! Вы выбрали персонажа - Разбойник!");
                        break;
                    case 2:
                        characterGame = new Warrior();
                        Console.WriteLine("Отличный выбор! Вы выбрали персонажа - Воин!");
                        break;
                    case 3:
                        characterGame = new Barbarian();
                        Console.WriteLine("Отличный выбор! Вы выбрали персонажа - Варвар!");
                        break;
                    
                    default:
                        characterGame = new Pillager();
                        break;
                }

                characterGame._agility = randomNew.Next(1, 7);    
                characterGame._strenghtPlayer = randomNew.Next(1, 7); 
                characterGame._endurance = randomNew.Next(1, 7);  

                Console.WriteLine($"\nСгенерированные характеристики:");
                Console.WriteLine($"Ловкость: {characterGame._agility}");
                Console.WriteLine($"Сила: {characterGame._strenghtPlayer}");
                Console.WriteLine($"Выносливость: {characterGame._endurance}");
                Console.WriteLine($"Здоровье: {characterGame._healthCharacter}");

                while (victoryCount < 3) 
                {
                    Console.WriteLine($"\n=== БИТВА {victoryCount + 1} ===");
                    Console.WriteLine($"Уровень персонажа: {characterGame._levelCharacter}");
                    Console.WriteLine($"Побед: {victoryCount}/3");

                    List<Enemy> enemyList = new List<Enemy>();
                    enemyList.Add(new Goblin());
                    enemyList.Add(new Scelet());
                    enemyList.Add(new Slime());
                    enemyList.Add(new Ghost());
                    enemyList.Add(new Golem());
                    enemyList.Add(new Dragon());
                    Random random = new Random();
                    int randomIndex = random.Next(0, enemyList.Count);
                    Enemy enemyGame = enemyList[randomIndex];

                    Console.WriteLine($"Вы встретили: {enemyGame.nameEnemy}");

                    Console.WriteLine("Пожалуйста, выберите режим для дальнейшей игры:");
                    Console.WriteLine("1 - Пошаговый режим");
                    Console.WriteLine("2 - Автоматический режим");
                    int choose = getNumber(2, 1);

                    bool playerWon = Play(characterGame, enemyGame);

                    if (playerWon) 
                    {
                        victoryCount++;
                        characterGame._levelCharacter++; 
                        ApplyLevelUpBenefits(characterGame);

                        Console.WriteLine($"\n=== ПОБЕДА! Уровень повышен до {characterGame._levelCharacter} ===");
                        Console.WriteLine($"Побед: {victoryCount}/3");

                        if (victoryCount >= 3)
                        {
                            Console.WriteLine("\n ПОЗДРАВЛЯЕМ! ВЫ ПРОШЛИ ИГРУ! ");
                            Console.WriteLine($"Ваш {characterGame.nameCharacter} достиг максимального уровня!");
                            break;
                        }

                        Console.WriteLine("1 - Продолжить с новым противником");
                        Console.WriteLine("2 - Вернуться в главное меню");

                        int continueChoice = getNumber(2, 1);
                        if (continueChoice == 2)
                        {
                            break; 
                        }
                    }
                    else 
                    {
                        Console.WriteLine("\n=== ВЫ ПРОИГРАЛИ ===");
                        Console.WriteLine("1 - Попробовать снова с этим персонажем");
                        Console.WriteLine("2 - Выбрать другого персонажа");
                        Console.WriteLine("3 - Выйти из игры");

                        int restartChoice = getNumber(3, 1);

                        switch (restartChoice)
                        {
                            case 1:
                                ResetCharacterHealth(characterGame);
                                break;
                            case 2:
                                victoryCount = 0; 
                                break; 
                            case 3:
                                playAgain = false;
                                Console.WriteLine("Спасибо за игру! До свидания!");
                                return; 
                        }

                        if (restartChoice == 2) break; 
                    }
                }

                if (victoryCount >= 3)
                {
                    Console.WriteLine("\nХотите сыграть еще?");
                    Console.WriteLine("1 - Выбрать нового персонажа");
                    Console.WriteLine("2 - Выйти из игры");

                    int finalChoice = getNumber(2, 1);
                    if (finalChoice == 2)
                    {
                        playAgain = false;
                        Console.WriteLine("Спасибо за игру! До свидания!");
                    }
                }
            }
        }

    }
}
