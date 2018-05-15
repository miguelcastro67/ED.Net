using System;

namespace PickEd
{
    [CommandFilter(Prefix = "X")]
    public class CancelCommand : CommandBase
    {
        public override bool Process(string cmd, TextFile textFile, EditorValues editorValues)
        {
            textFile.Cancel();

            if (editorValues.CurrentLine > textFile.Lines)
                editorValues.CurrentLine = textFile.Lines;

            return false;
        }
    }
}
