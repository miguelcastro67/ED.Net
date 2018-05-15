using System;

namespace PickEd
{
    [CommandFilter(Prefix = "D")]
    public class DeleteCommand : CommandBase
    {
        public override bool Process(string cmd, TextFile textFile, EditorValues editorValues)
        {
            editorValues.RepeatLines = GetRepeatLines(cmd);

            if (editorValues.CurrentLine > 0)
            {
                if (editorValues.RepeatLines == 0)
                {
                    // delete current line
                    textFile.Delete(editorValues.CurrentLine);
                }
                else
                {
                    // delete multiple lines

                    if (editorValues.CommandInfo.IndexOf(editorValues.Delim) > -1)
                    {
                        int lastLine = editorValues.CurrentLine + editorValues.RepeatLines - 1;
                        if (lastLine > textFile.Lines)
                            lastLine = textFile.Lines;

                        for (int i = editorValues.CurrentLine; i <= lastLine; i++)
                            textFile.Delete(editorValues.CurrentLine);
                    }
                }
            }

            return false;
        }
    }
}
