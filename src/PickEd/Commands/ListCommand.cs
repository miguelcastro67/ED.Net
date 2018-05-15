using System;
using System.Linq.Expressions;

namespace PickEd
{ 
    [CommandFilter(Prefix = "L")]
    public class ListCommand : CommandBase
    {
        public override bool PreProcess(string cmd)
        {
            return (cmd.Length > 1);
        }

        public override bool Process(string cmd, TextFile textFile, EditorValues editorValues)
        {
            // change page size and list page

            string temp = cmd.Substring(1);
            int lines = 0;
            bool linesOk = int.TryParse(temp, out lines);
            if (lines > 0)
                editorValues.PageSize = lines;

            if (textFile.Lines > 0)
            {
                int startLine = editorValues.CurrentLine + 1;
                int endLine = startLine + editorValues.PageSize;

                if (endLine > textFile.Lines)
                    endLine = textFile.Lines;

                if (startLine < endLine)
                {
                    for (int i = startLine; i < endLine; i++)
                        Output(textFile.DisplayLine(i, true));
                }
                else
                    Output(textFile.DisplayLine(editorValues.CurrentLine, true));

                editorValues.CurrentLine += editorValues.PageSize;
                if (editorValues.CurrentLine > textFile.Lines - 1)
                    editorValues.CurrentLine = textFile.Lines - 1;
            }
            else
                editorValues.CurrentLine = 0;

            return false;
        }
    }
}
