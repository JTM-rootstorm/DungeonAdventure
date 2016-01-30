using System.ComponentModel;

namespace Engine
{
    public class PlayerQuest : INotifyPropertyChanged
    {
        private Quest _details;
        public Quest details
        {
            get { return _details; }
            set
            {
                _details = value;
                OnPropertyChanged("details");
            }
        }

        private bool _isCompleted;
        public bool isCompleted
        {
            get { return _isCompleted; }
            set
            {
                _isCompleted = value;
                OnPropertyChanged("isCompleted");
                OnPropertyChanged("name");
            }
        }

        public string name
        {
            get { return details.name; }
        }

        public PlayerQuest(Quest details)
        {
            this.details = details;
            isCompleted = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
