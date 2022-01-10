using System;
using tRPG;
using Entity;

namespace Props
{
    
    class Chest
    {
        //public int rarityLevel; //0: common, 1: uncommon, 2:rare 
        public int xpGain;
        int coinsInside;
        //add item (not yet in the game)
        Random rng = new Random();
        
        public Chest() {
            /*
            int rarity = rng.Next(0, 100);
            if (rarity < 70) rarityLevel = 0;
            else if (rarity < 97) rarityLevel = 1;
            else rarityLevel = 2;*/

            xpGain = Convert.ToInt32( Math.Round(Math.Pow( rng.NextDouble() * 100, 4.5 )/10000000 + 10) );
            coinsInside = rng.Next(0, 21);

            
        }

        public void Loot() {
            Program.player.GainXP(xpGain);
            if (coinsInside > 0) Program.player.GainMoney(coinsInside);
            
        }
        
    }
}