using System;
using tRPG;

namespace Entity
{
    
    public enum EnemyID
    {
        slime,
        goblin
    }

    public class EnemyEntity
    {
        public string name;
        public int level;
        public int maxHealth;
        public int health;
        public bool isAlive;
        public int damage;
        public int defence;
        public int xpDrop;
        //public double regeneration;

        public void EnemyUpdate() {
            if (health <= 0 && isAlive) {
                Die();
            }
        }

        void Die() {
            isAlive = false;
            Console.WriteLine($"{name} died");
            Program.player.GainXP(xpDrop);
        }
        
        public void Attack(Player player) {
            Console.WriteLine($"{this.name} attacked you.");
            player.Attacked(this);

        }


        public void Damaged(int _damage) {            
            ConsoleColor previousConsoleColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            int damageReceived = _damage - defence;
            health -= damageReceived;
            Console.WriteLine($"{this.name} took {damageReceived} damages.");
            Console.ForegroundColor = previousConsoleColor;
            EnemyUpdate();
        }

    }


    public class Slime : EnemyEntity
    {

        public Slime(int _level) {
            name = "slime";
            level = _level;
            maxHealth = 10 + 3 * level;
            health = maxHealth;
            isAlive = true;
            damage = 5 + level;
            if (level > 4) defence = level-4;
            xpDrop = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(level)) * 10);
            //regeneration = (level-4)/10;
        }

        public void SLimeTalk() {
            Console.WriteLine("slime talk");
        }
    }

    
    public class Goblin : EnemyEntity
    {

        public Goblin(int _level) {
            name = "goblin";
            level = _level;
            maxHealth = 8 + 4 * level;
            health = maxHealth;
            isAlive = true;
            damage = Convert.ToInt32(4 + 1.2*level);
            if (level > 3) defence = level-3;
            xpDrop = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(level)) * 11);
            //regeneration = (level-4)/10;
        }
    }
}