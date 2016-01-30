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
