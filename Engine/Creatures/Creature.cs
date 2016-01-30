using System;
using System.ComponentModel;

namespace Engine.Creatures
{
    public class Creature : INotifyPropertyChanged
    {
        private int _currentHitPoints;
        public int currentHitPoints
        {
            get { return _currentHitPoints; }
            set
            {
                _currentHitPoints = value;
                OnPropertyChanged("currentHitPoints");
            }
        }
        private int _maximumHitPoints;
        public int maximumHitPoints
        {
            get { return _maximumHitPoints; }
            set
            {
                _maximumHitPoints = value;
                OnPropertyChanged("maximumHitPoints");
            }
        } 

        public int strength { get; set; } = 8;
        public int dexterity { get; set; } = 8;
        public int constitution { get; set; } = 8;
        public int intelligence { get; set; } = 8;
        public int wisdom { get; set; } = 8;
        public int charisma { get; set; } = 8;

        public int attackMod { get; set; }

        public string name { get; set; }

        public Creature(int currentHitPoints, int maximumHitPoints, string name)
        {
            this.currentHitPoints = currentHitPoints;
            this.maximumHitPoints = maximumHitPoints;
            this.name = name;
        }

        public Creature(int currentHitPoints, int maximumHitPoints, int attackMod, string name)
        {
            this.currentHitPoints = currentHitPoints;
            this.maximumHitPoints = maximumHitPoints;
            this.attackMod = attackMod;
            this.name = name;
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

        public int FindAttModifier(int stat)
        {
            return (int)Math.Floor((stat - 10.0d) / 2.0d);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
