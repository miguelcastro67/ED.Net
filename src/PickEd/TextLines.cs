using System;
using System.Collections.Generic;
using System.Text;

namespace PickEd
{
    public class TextLines : List<TextLine>
    {
        private const string STR_EqualsError = "TextLines object can only be compared to another TextLines object.";

        public TextLines Clone()
        {
            TextLines newTextLines = new TextLines();

            foreach (TextLine item in this)
                newTextLines.Add(new TextLine(item.Value));

            return newTextLines;
        }

        public override bool Equals(object obj)
        {
            if(!(obj is TextLines))
                throw new ArgumentException(STR_EqualsError);

            TextLines other = obj as TextLines;

            if (this.Count != other.Count)
                return false;

            int lines = this.Count;
            bool equals = true;

            for (int i = 0; i < lines; i++)
            {
                equals = this[i].Equals(other[i]);
                if (!equals)
                    break;
            }

            return equals;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
