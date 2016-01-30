namespace Engine.Items
{
    public class LootItem
    {
        public Item details { get; set; }
        public int dropPercentage { get; set; }
        public bool isDefaultItem { get; set; }

        public LootItem(Item details, int dropPercentage, bool isDefaultItem)
        {
            this.details = details;
            this.dropPercentage = dropPercentage;
            this.isDefaultItem = isDefaultItem;
        }
    }
}
