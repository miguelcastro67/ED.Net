using System;

namespace PickEd
{
    [CommandFilter(Prefix = "FI")]
    public class FileExitCommand : CommandBase
    {
        public override bool Process(string cmd, TextFile textFile, EditorValues editorValues)
        {
            textFile.Flip();
            textFile.Save();

            Output("Saved");

            return true;
        }
    }
}
