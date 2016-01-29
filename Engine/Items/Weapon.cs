using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Items
{
    public class Weapon : Item
    {
        public int minimumDamage { get; set; }
        public int maximumDamage { get; set; }

        public Weapon(int ID, string name, string namePlural, int minimumDamage, int maximumDamage) : base(ID, name, namePlural)
        {
            this.minimumDamage = minimumDamage;
            this.maximumDamage = maximumDamage;
        }
    }

}
