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
    }

}
