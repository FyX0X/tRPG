using System;
using tRPG;

namespace Entity
{
    
    [Serializable]
    public class Player
    {
        const string ID = "player";
        public string name;
        public int level;
        public int xp;
        public bool isAlive;
        public bool inCombat;
        public bool isParrying;
        int maxHealth;
        int health;
        int baseDamage;
        int damage; // baseDamage + weapon damage
        int defence;
        double critChance; // %
        double critDamage; // final damage = damage * critDamage
        double regeneration; // hp/action (maybe something else)
        int statPoint;
        public string worldPos; //village, forest, shop(add later, dungeon)
        public int forestProgress; //progress make in the forest
        public int forestCurrentProgress;
        int money;

        public Player(string _name) {
            name = _name;
            maxHealth = 100;
            health = maxHealth;
            xp = 0;
            level = 1;
            statPoint = 5;
            isAlive = true;
            inCombat = false;
            isParrying = false;
            baseDamage = 10;
            damage = baseDamage;
            defence = 0;
            critChance = 0.5;
            critDamage = 1.5;
            //regeneration = 0;
            worldPos = "village";
            forestProgress = 0;
            money = 0;
        }
        

        public void Attack(EnemyEntity enemy) {
            Console.WriteLine($"You attacked {enemy.name} ");
            Random rng = new Random();
            double crit = rng.NextDouble() * 100;
            int damageDealt;
            if (crit < critChance) {
                Crit();
                damageDealt = Convert.ToInt32(Math.Round(damage * critDamage));
            }
            else damageDealt = damage;
            enemy.Damaged(damageDealt);
        }

        public int Parry(EnemyEntity enemy) {
            //parry
            Console.WriteLine($"You parried {enemy.name}'s attack");
            int damageReceived =enemy.damage - defence * 3;
            isParrying = false;
                        
            Random rng = new Random();
            double crit = rng.NextDouble() * 100;
            int damageDealt;
            if (crit < critChance) {
                Crit();
                damageDealt = Convert.ToInt32(Math.Round(damage * 0.5 * critDamage));
            }
            else damageDealt = damage / 2;
            enemy.Damaged(damageDealt);            

            return damageReceived;
        }

        void Crit() {
            ConsoleColor prevColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"You landed a Critical Hit ! X{critDamage} DAMAGE!");
            Console.ForegroundColor = prevColor;
        }

        public void Attacked(EnemyEntity enemy) {
            int damageReceived;
            if (isParrying) damageReceived = Parry(enemy);
            else damageReceived = enemy.damage - defence;
            damageReceived = Math.Max(0, damageReceived);
            health -= damageReceived;
            // writeline to player
            ConsoleColor previousConsoleColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            if (damageReceived > 0) Console.WriteLine($"You took {damageReceived} damages");
            else Console.WriteLine($"You completly defended against the {enemy.name}'s attack");
            Console.ForegroundColor = previousConsoleColor;
            PlayerUpdate();
        }

        void Die() {
            isAlive = false;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("You are dead.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Enter something to respawn or RAGEQUIT to quit the game");
            string input = Console.ReadLine().ToLower();
            if (input.Contains("quit")) Program.Quit(this);
            else Respawn();
        }

        void Respawn(int coinPenalty = 0) {
            isAlive = true;
            if (coinPenalty < 0) money = 0;
            else money -= coinPenalty;
            health = maxHealth /2;

        }

        public void PlayerUpdate() {
            damage = baseDamage; //+weaponDamage but not added yet
            if (health <= 0) {
                Die();
            }
        }

        public void GainXP(int xpGain) {
            xp += xpGain;
            Console.WriteLine("You gained {0} XP", xpGain);
            if (xp >= NextLevel()) LevelUp(xp - NextLevel());
            
        }

        public void GainMoney(int moneyGain) {
            money += moneyGain;
            Console.WriteLine("You picked up {0} coins", moneyGain);
        }

        void LevelUp(int additionalXP) {
            level++;
            xp = additionalXP;
            statPoint += 5;
            health = maxHealth;
            //regeneration += 1;
            ConsoleColor previousConsoleColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("You leveled up, you are now level {0}", level);
            if (xp >= NextLevel()) LevelUp(xp - NextLevel()); //check if the player will still level up
            Console.ForegroundColor = previousConsoleColor;
        }

        int NextLevel() {
            int xpNeeded = Convert.ToInt32(Math.Round(10 + Math.Pow(Convert.ToDouble(level), 1.5)*5));
            return xpNeeded;
        }

        public void Inventory() {
            //diplay inventory :
            ConsoleColor previousConsoleColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n--------INVENTORY--------");
            Console.WriteLine("Coins: {0}", money);
            Console.ForegroundColor = previousConsoleColor;

        }

        public void Status() {
            //update stats and additional verification if player is dead
            Program.Update();
            //diplay stats :
            ConsoleColor previousConsoleColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n--------STATUS--------");
            Console.WriteLine("Name: {0}", name);
            Console.WriteLine("Level: {0} ({1}/{2})", level, xp, NextLevel());
            Console.WriteLine("Health: {0}/{1}", health, maxHealth);
            Console.WriteLine("Damage: {0} (+{1})", baseDamage, damage - baseDamage);
            Console.WriteLine("Defence: {0}", defence);
            Console.Write("Critical Chance: {0}%", critChance);
            if (critChance >= 25) Console.Write(" (MAX)");
            Console.Write("\nCritical Damage: {0} ({1}x{2})", critDamage * damage, critDamage, damage);
            if (critDamage >= 10) Console.Write(" (MAX)");
            //diplay StatPoint :
            Console.WriteLine("\nYou currently have {0} StatPoint(s).  (to attribute StatPoints input 'attribute')", statPoint);
            Console.ForegroundColor = previousConsoleColor;

            string input = Console.ReadLine().ToLower();
            switch (input)
            {
                default:
                    break;
                case "attribute":
                    AttributeStatPoint();
                    break;
            }
        }


        void AttributeStatPoint() {

            ConsoleColor previousConsoleColor = Console.ForegroundColor;
            
            Console.ForegroundColor = ConsoleColor.DarkGray;
            
            //diplay StatPoint :
            Console.WriteLine("\nYou currently have {0} StatPoint(s).", statPoint);
            if (statPoint == 0) {
                Console.ForegroundColor = previousConsoleColor;  
                return;
            }
            //attribute StatPoint :
            Console.WriteLine("Please attribute StatPoint(s).");
            string input = Console.ReadLine().ToUpper();
            string[] attribution = input.Split(' ');
            int attributionNumber = 0;
            if (attribution.Length > 1) attributionNumber = Convert.ToInt32(attribution[1]);

            int nbAttributionSucces = 0;

            switch (attribution[0]) {
                default:
                    ConsoleColor attributionConsoleColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("\n--------ATTRIBUTION HELP--------");
                    Console.WriteLine("To attribute StatPoint use this syntax:\nstat x");
                    Console.WriteLine("Replace x by the number of Statpoint to attribute and stat by the corresponding key:");
                    Console.WriteLine("Health: HP, Base Damage: DMG, Defence: DEF, Critical Chance: CC, Critical Damage: CD");
                    Console.WriteLine("Other options:");
                    Console.WriteLine("See your status: status\nClose the attribution menu: 'close'");
                    Console.ForegroundColor = attributionConsoleColor;
                    break;
                case "CLOSE":
                    Console.WriteLine("You quit the Attribution menu");
                    Console.ForegroundColor = previousConsoleColor;  
                    return;
                case "STATUS":
                    Status();
                    break;
                case "HP":
                    if (attributionNumber <= statPoint) {
                        maxHealth += 10 * attributionNumber;
                        Console.WriteLine("You increased your Health by {0} current: {1}", 5 * attributionNumber, maxHealth);
                        statPoint -= attributionNumber;
                    } else Console.WriteLine("You don't have enough StatPoints.");
                    break;
                case "DMG":
                    if (attributionNumber <= statPoint) {
                        baseDamage += 3 * attributionNumber;
                        Console.WriteLine("You increased your Base Damage by {0} current: {1}", 3 * attributionNumber, baseDamage);
                        statPoint -= attributionNumber;
                    } else Console.WriteLine("You don't have enough StatPoints.");
                    break;
                case "DEF":
                    if (attributionNumber <= statPoint) {
                        defence += 1 * attributionNumber;
                        Console.WriteLine("You increased your Defence by {0} current: {1}", 1 * attributionNumber, defence);
                        statPoint -= attributionNumber;
                    } else Console.WriteLine("You don't have enough StatPoints.");
                    break;
                case "CC":
                    if (attributionNumber <= statPoint) {

                        for (int i = 0; i < attributionNumber; i++)
                        {
                            if (critChance < 25) {
                                critChance += 0.5;
                                statPoint--;
                                nbAttributionSucces++;
                            } else {
                                Console.WriteLine("You already achieved the max value for Critical Chance");
                                break;
                            }
                        }
                        if (nbAttributionSucces > 0) {
                            Console.WriteLine("You increased your Critical Chance by {0} current: {1}", 0.5 * attributionNumber, critChance);
                        }
                        
                    } else Console.WriteLine("You don't have enough StatPoints.");
                    break;

                case "CD":
                    if (attributionNumber <= statPoint) {

                        for (int i = 0; i < attributionNumber; i++)
                        {
                            if (critDamage < 10) {
                                critDamage += 0.25;
                                statPoint--;
                                nbAttributionSucces++;
                            } else {
                                Console.WriteLine("You already achieved the max value for Critical Damage");
                                break;
                            }
                        }
                        if (nbAttributionSucces > 0) {
                            Console.WriteLine("You increased your Critical Damage by {0} current: {1}", 0.25 * attributionNumber, critDamage);
                        }
                        
                    } else Console.WriteLine("You don't have enough StatPoints.");
                    break;

            }
            
            Console.ForegroundColor = previousConsoleColor;            

            AttributeStatPoint();

        }

    }

}