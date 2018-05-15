using System;
using System.Collections.Generic;
using System.Text;

namespace PickEd
{
    public class OutputMessageEventArgs : EventArgs
    {
        public OutputMessageEventArgs(string message)
        {
            _Message = message;
        }
        
        private string _Message = string.Empty;

        public string Message
        {
            get
            {
                return _Message;
            }
            set
            {
                _Message = value;
            }
        }
    }
}
