using System;
using System.Collections.Generic;
using System.Text;

namespace PickEd
{
    public class TextLine
    {
        private const string STR_EqualsError = "TextLine object can only be compared to another TextLine object.";
        
        public TextLine()
        {
        }

        public TextLine(string value)
        {
            _Value = value;
        }

        private string _Value = string.Empty;

        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        public override string ToString()
        {
            return _Value;
        }

        public override bool Equals(object obj)
        {
            if(!(obj is TextLine))
                throw new ArgumentException(STR_EqualsError);

            TextLine other = obj as TextLine;
            bool equals = (this.Value == other.Value);

            return equals;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
