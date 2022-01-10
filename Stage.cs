using System;
using Entity;
using tRPG;
using tRPG.GameSystem;
using Props;

namespace Stage
{
    static class Village
    {
        public static void EnterVillage() {
            Program.player.worldPos = "village";
        }

        
        public static void VillageMenu() {
            
            Random rng = new Random();
            
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Input you next action.");
            string input = Console.ReadLine().ToLower();
            switch (input) {
                default:
                    ConsoleColor previousConsoleColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("\n--------VILLAGE MENU HELP--------");
                    Console.WriteLine("To choose your next action, just input the corresponding key:");
                    Console.WriteLine("See your status: status");
                    Console.WriteLine("See your inventory: inventory");
                    Console.WriteLine("Enter the forest to Combat with monster: forest");
                    Console.WriteLine("quit the game: quit\n");
                    Console.ForegroundColor = previousConsoleColor;
                    break;
                case "status":
                    Program.player.Status();
                    break;
                case "inventory":
                    Program.player.Inventory();
                    break;
                case "forest":
                    Forest.EnterForest();
                    break;
                case "quit":
                    Program.Quit(Program.player);
                    break;
                case "chest":                        
                    Chest chest = new Chest();
                    chest.Loot();
                    break;
            }

        }
    }
    
    public static class Forest
    {

        public static void ForestMenu() {
                        
            Random rng = new Random();

            
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            Console.WriteLine("Input you next action.");
            string input = Console.ReadLine().ToLower();
            switch (input) {
                default:
                    ConsoleColor previousConsoleColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("\n--------FOREST MENU HELP--------");
                    Console.WriteLine("To choose your next action, just input the corresponding key:");
                    Console.WriteLine("See your status: status");
                    Console.WriteLine("See your inventory: inventory");
                    Console.WriteLine("Leave the forest and go to the Village: village");
                    Console.WriteLine("To continue just press the key: Enter");
                    Console.WriteLine("quit the game: quit\n");
                    Console.ForegroundColor = previousConsoleColor;
                    break;
                case "status":
                    Program.player.Status();
                    break;
                case "inventory":
                    Program.player.Inventory();
                    break;
                case "village":
                    Village.EnterVillage();
                    break;
                case "quit":
                    Program.Quit(Program.player);
                    break;
                case "":                        
                    Advance();
                    break;
            }
        }

        static void Advance() {
            Console.WriteLine("You advance in the forest...");
            Random rng = new Random();
            bool wonTheCombat;
            if (rng.NextDouble() > 0.1) {
                wonTheCombat = CombatSystem.Combat(Program.player, (EnemyID)rng.Next(0, 2));
            } else {
                Console.WriteLine("you found a chest.");
                Chest chest = new Chest();
                chest.Loot();
            }

        }

        public static void EnterForest() {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
            
            Program.player.worldPos = "forest";
            if (Program.player.forestProgress > 0) {
                Console.WriteLine("You already went in the forest and progress to the zone {0}", Program.player.forestProgress);
                Console.WriteLine("How far will you go ?");
                string input;
                int distanceInput;
                input = Console.ReadLine();
                try
                {
                    distanceInput = Convert.ToInt32(input);
                    ProgressTo(distanceInput);
                }
                catch (System.Exception)
                {
                    Console.WriteLine("You didn't go this far yet. Please input a lower value");
                }

            } else {
                Console.WriteLine("This is your first time in the forest");
            }
        }

        public static void ProgressTo(int _distance) {
            Program.player.forestCurrentProgress = _distance;
        }
        
    }
}