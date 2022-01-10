using System;
using Entity;

namespace tRPG.GameSystem
{
    class CombatSystem
    {
        public static bool Combat(Player player, EnemyID enemyID, int enemyLevel = -1) {
            ConsoleColor previousConsoleColor = Console.ForegroundColor;
            player.inCombat = true;
            Random rng = new Random();

            if(enemyLevel == -1) enemyLevel = Math.Max(1, rng.Next(player.level - 3, player.level + 5));

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("In Combat.");
            EnemyEntity enemy = null;


            switch (enemyID)
            {
                case EnemyID.slime:
                    enemy = new Slime(enemyLevel);
                    break;
                case EnemyID.goblin:
                    enemy = new Goblin(enemyLevel);
                    break;
            }

            Console.WriteLine($"You encountered {1} {(EnemyID) enemyID} level {enemy.level}");

            string inputAction;

            while (player.inCombat) {

                Console.WriteLine("Enter your next action:");
                inputAction = Console.ReadLine().ToLower();

                switch (inputAction) { //player's action
                    default:
                        CombatHelp();
                        break;
                    case "attack":
                        player.Attack(enemy);
                        break;
                    case "parry":
                        player.isParrying = true;
                        break;
                    case "inventory":
                        player.Inventory();
                        break;
                
                }
                //update health and stuff of player and enemies
                Program.Update(enemy);
                if(enemy.isAlive) {
                    enemy.Attack(player); // enemy's action                          
                    //update health and stuff of player and enemies
                    Program.Update(enemy);
                }

                if (player.isAlive && !enemy.isAlive) {
                    player.inCombat = false;
                    Console.WriteLine("Enemy died. => combat end.");
                    Console.ForegroundColor = previousConsoleColor;
                    return true;
                } else if (!player.isAlive && enemy.isAlive) {
                    player.inCombat = false;
                    Console.WriteLine("Player died. => combat end.");
                    Console.ForegroundColor = previousConsoleColor;
                    return false;
                } else {
                    player.inCombat = true;
                }

            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Error occured: combat issue unknown => return player won");
            return true;
        }

        public static void CombatHelp() {
            
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\n--------COMBAT HELP--------");
            Console.WriteLine("Here are your next possible actions, just input the corresponding key:");
            Console.WriteLine("See your status: status");
            Console.WriteLine("See your inventory: inventory");
            Console.WriteLine("Attack an eneny: attack");
            Console.WriteLine("Parry the eneny's attack: parry");
            Console.WriteLine("Leave the forest to the village (if in combat counted as retreat): leave\n");
            Console.WriteLine("Warning: checking your inventory for using items will cost you a turn\n");
            Console.ForegroundColor = ConsoleColor.Cyan;
        }
    }
}