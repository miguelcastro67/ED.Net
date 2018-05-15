using System;
using System.Linq.Expressions;

namespace PickEd
{
    public class CommandBase
    {
        public virtual bool PreProcess(string cmd)
        {
            return true;
        }
        public virtual bool Process(string cmd, TextFile textFile, EditorValues editorValues)
        {
            return false;
        }

        protected void Output(string message)
        {
            //OnOutputMessage(new OutputMessageEventArgs(message));
        }

        protected int GetRepeatLines(string command)
        {
            int repeatLines = 0;
            int numPos = 1;
            bool noMoreNumbers = false;
            string numValue = string.Empty;
            while (!noMoreNumbers)
            {
                if (numPos < command.Length)
                {
                    string character = command.Substring(numPos, 1);
                    if (IsNumeric(character))
                    {
                        numValue += character;
                        numPos++;
                    }
                    else
                        noMoreNumbers = true;
                }
                else
                    noMoreNumbers = true;
            }

            if (numValue != string.Empty)
                repeatLines = int.Parse(numValue);

            return repeatLines;
        }

        protected string GetDelimiter(string command)
        {
            int delimPos = 0;
            int repeatLines = GetRepeatLines(command);
            if (repeatLines > 0)
                delimPos = repeatLines.ToString().Length + 1;
            else
                delimPos = 1;

            return command.Substring(delimPos, 1);
        }

        protected bool IsNumeric(string character)
        {
            int value = 0;
            bool ret = int.TryParse(character, out value);
            return ret;
        }
    }
}
