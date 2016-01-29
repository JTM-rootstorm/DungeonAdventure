using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engine.Items;

namespace Engine.Items.Player
{
    public class HealingPotion : Item
    {
        public int amountToHeal { get; set; }

        public HealingPotion(int ID, string name, string namePlural, int amountToHeal) : base(ID, name, namePlural)
        {
            this.amountToHeal = amountToHeal;
        }
    }
}
