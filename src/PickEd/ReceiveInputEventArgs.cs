using System;
using System.Collections.Generic;
using System.Text;

namespace PickEd
{
    public class ReceiveInputEventArgs : OutputMessageEventArgs
    {
        public ReceiveInputEventArgs(string outputMessage) : base(outputMessage)
        {
        }

        private string _InputValue = string.Empty;

        public string InputValue
        {
            get
            {
                return _InputValue;
            }
            set
            {
                _InputValue = value;
            }
        }
    }
}
