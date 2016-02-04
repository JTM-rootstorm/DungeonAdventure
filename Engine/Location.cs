using Engine.Items;
using Engine.Creatures.Monsters;

namespace Engine
{
    public class Location
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public Item itemRequiredToEnter { get; set; }
        public Quest questAvailableHere { get; set; }
        public Monster monsterLivingHere { get; set; }
        public Location locationToNorth { get; set; }
        public Location locationToEast { get; set; }
        public Location locationToSouth { get; set; }
        public Location locationToWest { get; set; }

        public Location(int ID, string name, string description, Item itemRequiredToEnter = null, Quest questAvailableHere = null, Monster monsterLivingHere = null)
        {
            this.ID = ID;
            this.name = name;
            this.description = description;
            this.itemRequiredToEnter = itemRequiredToEnter;
            this.questAvailableHere = questAvailableHere;
            this.monsterLivingHere = monsterLivingHere;
        }

        public void GenerateLocationLinks(Location[,] dungeonFloor, int row, int col, int maxX, int maxY)
        {
            if(row != 0)
            {
                locationToNorth = dungeonFloor[row - 1, col];
            }
            else
            {
                locationToNorth = null;
            }

            if(row != maxX)
            {
                locationToSouth = dungeonFloor[row + 1, col];
            }
            else
            {
                locationToSouth = null;
            }

            if(col != 0)
            {
                locationToWest = dungeonFloor[row, col - 1];
            }
            else
            {
                locationToWest = null;
            }

            if(col != maxY)
            {
                locationToEast = dungeonFloor[row, col + 1];
            }
            else
            {
                locationToEast = null;
            }         
        }
    }
}
