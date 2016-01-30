using System;

namespace Engine.Creatures
{
    public class Creature
    {       
        public int currentHitPoints { get; set; }
        public int maximumHitPoints { get; set; }      

        public int strength { get; set; } = 8;
        public int dexterity { get; set; } = 8;
        public int constitution { get; set; } = 8;
        public int intelligence { get; set; } = 8;
        public int wisdom { get; set; } = 8;
        public int charisma { get; set; } = 8;

        public Creature(int currentHitPoints, int maximumHitPoints)
        {
            this.currentHitPoints = currentHitPoints;
            this.maximumHitPoints = maximumHitPoints;
        }

        public void SetAttributes(int str, int dex, int con, int intel, int wis, int charisma)
        {
            strength = str;
            dexterity = dex;
            constitution = con;
            intelligence = intel;
            wisdom = wis;
            this.charisma = charisma;
        }

        public static int FindAttModifier(int stat)
        {
            return (int)Math.Floor((stat - 10.0d) / 2.0d);
        }
    }
}
