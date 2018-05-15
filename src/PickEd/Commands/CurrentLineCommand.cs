using System;

namespace PickEd
{
    [CommandFilter(Prefix = ".")]
    public class CurrentLineCommand : CommandBase
    {
        public override bool Process(string cmd, TextFile textFile, EditorValues editorValues)
        {
            if (editorValues.CurrentLine > 0)
                Output(textFile.DisplayLine(editorValues.CurrentLine, true));

            return false;
        }
    }
}
