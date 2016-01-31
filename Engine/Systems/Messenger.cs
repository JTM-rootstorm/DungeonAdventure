using System;

namespace Engine.Systems
{
    public class Messenger
    {
        public event EventHandler<MessageEventArgs> OnMessage;

        public Messenger()
        {

        }

        public void RaiseMessage(string message, bool addExtraNewLine = false)
        {
            if (OnMessage != null)
            {
                OnMessage(this, new MessageEventArgs(message, addExtraNewLine));
            }
        }
    }
}
