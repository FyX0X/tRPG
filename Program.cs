using System;
using System.IO;
using System.Text.RegularExpressions;
using Entity;
using Props;
using Stage;
using tRPG.GameSystem;

namespace tRPG
{


    class Program
    {
        public static bool runGame = true;
        public static Player player = null;
        
        static void Main(string[] args)
        {            

            
            Console.Title = "Text RPG";

            Initialize();
            player = Start();

            //initialize Stage

            while (runGame)
            {

                Update();
                switch (player.worldPos)
                {
                    default:
                        player.worldPos = "village";
                        break;
                    case "village":
                        Village.VillageMenu();
                        break;
                    case "forest":
                        Forest.ForestMenu();
                        break;
                }
            }
        }

        public static void Update(EnemyEntity enemy = null) {
            player.PlayerUpdate();
            if (enemy != null ) enemy.EnemyUpdate();
        }


        static void Initialize()
        {
            string currDir = Directory.GetCurrentDirectory();
            Directory.CreateDirectory(currDir+@"\data\save");
        }

        // START THE GAME AT NEW GAME MENU TO CREATE/LOAD/DELETE GAMES 
        static Player Start() {
            

            Player player = null;


            string[] saves;
            string username;
            string file;
            Regex regexFilter = new Regex(@"^[a-zA-Z_-][a-zA-Z0-9_-]+$");            
            int index;
            bool isIndex;

            Console.WriteLine("Welcome to Text RPG");        
            string playInput;
            while (true) {
                Console.Write(">>");
                playInput = Console.ReadLine().ToUpper();
                switch (playInput) {
                    default:
                        Console.WriteLine("\n--------NEW GAME MENU HELP--------");
                        Console.WriteLine("To choose your next action, just input the corresponding key:");
                        Console.WriteLine("Create a new game: C");
                        Console.WriteLine("Load an existing game: L");
                        Console.WriteLine("Delete an existing game: D");
                        Console.WriteLine("Quit the game: Q\n");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You can't have two game with the same username. \n(creating a new game with an existing username will overwrite the other game.)");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case "C":                        
                        Console.WriteLine("Enter the name of your Player");
                        while (true) {
                            username = Console.ReadLine();
                            if (regexFilter.IsMatch(username)) {
                                player = new Player(username);
                                break;
                            } else Console.WriteLine("Invalid name: you can't start with a digit and only put letter, digits, dashes and underscores");
                        }
                        Console.WriteLine("Player created.");

                        //player.GainXP(10000); //=======================================================

                        SaveSystem.SavePlayer(player);

                        return player;
                    case "L":
                        saves = Directory.GetFiles(@"data\save\", "*.save");
                        Console.WriteLine("Current games:");
                        for (int i = 0; i < saves.Length; i++)
                        {
                            Console.WriteLine($"{i}: {Path.GetFileNameWithoutExtension(saves[i])}");                  
                        }
                        Console.Write("Enter the name or the index of the caracter you want to play.\n>>");
                        
                        username = Console.ReadLine();

                        isIndex = int.TryParse(username, out index);

                        if (isIndex) {
                            if (0 <= index || index <= saves.Length) {
                                player = SaveSystem.LoadPlayer(Path.GetFileNameWithoutExtension(saves[index]));
                                Console.WriteLine("Player loaded\n");
                                return player;
                            } else {
                                Console.WriteLine("You specified a incorrect index");
                            }
                        } else {
                            
                            file = @"data\save\" + username + ".save";

                            if (Array.Exists(saves, elem => elem == file)) {
                                player = SaveSystem.LoadPlayer(username);
                                Console.WriteLine("Player loaded\n");
                                
                                return player;
                            } else {
                                Console.WriteLine ("cancel.");
                            }
                        }
                        break;

                    case "D":
                        saves = Directory.GetFiles(@"data\save\", "*.save", SearchOption.TopDirectoryOnly);
                        Console.WriteLine("Current games:");
                        for (int i = 0; i < saves.Length; i++)
                        {
                            Console.WriteLine($"{i}: {Path.GetFileNameWithoutExtension(saves[i])}");                  
                        }
                        Console.Write("Enter the name or the index of the caracter you want to delete.\n>>");

                        username = Console.ReadLine();

                        isIndex = int.TryParse(username, out index);

                        if (isIndex) {
                            if (0 <= index || index <= saves.Length) {
                                File.Delete(saves[index]);
                                Console.WriteLine("Game deleted\n");
                            } else {
                                Console.WriteLine("You specified a incorrect index");
                            }
                        } else {
                            
                            file = @"data\save\" + username + ".save";

                            if (Array.Exists(saves, elem => elem == file)) {
                                File.Delete(file);
                                Console.WriteLine("Game deleted\n");
                            } else {
                                Console.WriteLine ("cancel.");
                            }
                        }
                        break;

                    case "Q":
                        Quit();
                        return null;
                                
                }
            }

        }

        public static void Save(Player player) {            
            if (player != null) SaveSystem.SavePlayer(player);
        }

        public static void Quit(Player player = null) {

            Save(player);

            Environment.Exit(0);
        }
    }

}
