namespace Engine.Items
{
    public class QuestCompletionItem
    {
        public Item details { get; set; }
        public int quantity { get; set; }

        public QuestCompletionItem(Item details, int quantity)
        {
            this.details = details;
            this.quantity = quantity;
        }
    }
}
