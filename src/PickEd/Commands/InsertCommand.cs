using System;

namespace PickEd
{
    [CommandFilter(Prefix = "I")]
    public class InsertCommand : CommandBase
    {
        public override bool Process(string cmd, TextFile textFile, EditorValues editorValues)
        {
            bool exitInsert = false;
            while (!exitInsert)
            {
                Console.Write(string.Format("{0:000}+", editorValues.CurrentLine));
                string newLine = Console.ReadLine();
                if (newLine != string.Empty)
                {
                    textFile.Insert(editorValues.CurrentLine, newLine, true);
                    editorValues.CurrentLine++;
                }
                else
                    exitInsert = true;
            }

            return false;
        }
    }
}
