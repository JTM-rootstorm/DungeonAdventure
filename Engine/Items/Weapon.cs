namespace Engine.Items
{
    public class Weapon : Item
    {
        public int minimumDamage { get; set; }
        public int maximumDamage { get; set; }
        public bool finesse { get; set; }

        public Weapon(int ID, string name, string namePlural, int minimumDamage, int maximumDamage, bool finesse) : base(ID, name, namePlural)
        {
            this.minimumDamage = minimumDamage;
            this.maximumDamage = maximumDamage;
            this.finesse = finesse;
        }
    }

}
