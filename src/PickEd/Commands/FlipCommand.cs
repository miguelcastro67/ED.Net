using System;

namespace PickEd
{
    [CommandFilter(Prefix = "F")]
    public class FlipCommand : CommandBase
    {
        public override bool Process(string cmd, TextFile textFile, EditorValues editorValues)
        {
            textFile.Flip();

            return false;
        }
    }
}
