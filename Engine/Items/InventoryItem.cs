using System.ComponentModel;

namespace Engine.Items
{
    public class InventoryItem : INotifyPropertyChanged
    {
        private Item _details;
        public Item details
        {
            get { return _details; }
            set
            {
                _details = value;
                OnPropertyChanged("details");
            }
        }

        private int _quantity;
        public int quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged("quantity");
                OnPropertyChanged("description");
            }
        }
        
        public string description
        {
            get { return quantity > 1 ? details.namePlural : details.name; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public InventoryItem(Item details, int quantity)
        {
            this.details = details;
            this.quantity = quantity;
        }

        protected void OnPropertyChanged(string name)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
