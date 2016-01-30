namespace Engine.Items
{
    public class Item
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string namePlural { get; set; }

        public Item(int ID, string name, string namePlural)
        {
            this.ID = ID;
            this.name = name;
            this.namePlural = namePlural;
        }
    }
}
