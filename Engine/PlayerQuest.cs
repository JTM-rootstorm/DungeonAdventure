namespace Engine
{
    public class PlayerQuest
    {
        public Quest details { get; set; }
        public bool isCompleted { get; set; }

        public PlayerQuest(Quest details)
        {
            this.details = details;
            isCompleted = false;
        }
    }
}
