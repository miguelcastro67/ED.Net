using System;

namespace PickEd
{
    [CommandFilter(Prefix = "R")]
    public class ReplaceCommand : CommandBase
    {
        public override bool PreProcess(string cmd)
        {
            return (cmd.Length > 1);
        }

        public override bool Process(string cmd, TextFile textFile, EditorValues editorValues)
        {
            bool cont = false;
            editorValues.RepeatLines = GetRepeatLines(cmd);
            editorValues.Delim = GetDelimiter(cmd);

            if (editorValues.RepeatLines == 0)
            {
                editorValues.RepeatLines = 1;
                editorValues.CommandInfo = cmd.Substring(2);
                cont = true;
            }
            else
            {
                string[] sections = cmd.Substring(1).Split(Convert.ToChar(editorValues.Delim));
                if (sections.Length >= 2)
                {
                    int pos = cmd.IndexOf(editorValues.Delim);
                    string lines = cmd.Substring(1, pos - 1);
                    editorValues.RepeatLines = 1;
                    int repeatLines = editorValues.RepeatLines;
                    bool linesOk = int.TryParse(lines, out repeatLines);
                    if (linesOk)
                    {
                        editorValues.CommandInfo = cmd.Substring(pos + 1);
                        editorValues.RepeatLines = repeatLines;
                        cont = true;
                    }
                }
            }

            if (cont)
            {
                if (editorValues.CurrentLine > 0)
                {
                    if (editorValues.CommandInfo == string.Empty)
                    {
                        // replace entire line

                        Console.Write(editorValues.CurrentLine.ToString() + " ");
                        string newLine = Console.ReadLine();

                        textFile.Replace(editorValues.CurrentLine, newLine);
                    }
                    else
                    {
                        // perform calculated replace

                        if (editorValues.CommandInfo.IndexOf(editorValues.Delim) > -1)
                        {
                            string[] replacementInfo = editorValues.CommandInfo.Split(Convert.ToChar(editorValues.Delim));
                            string oldText = replacementInfo[0];
                            string newText = replacementInfo[1];

                            for (int i = editorValues.CurrentLine; i <= editorValues.CurrentLine + editorValues.RepeatLines - 1; i++)
                            {
                                if (oldText == "" && newText != "")
                                {
                                    // insert new text in front of the line
                                    textFile.Insert(i, newText);
                                }
                                else if (oldText != "" && newText != "")
                                {
                                    // replace old text with new text
                                    textFile.Replace(i, oldText, newText);
                                }
                                else if (oldText != "" && newText == "" && replacementInfo.Length > 2)
                                {
                                    // remove old text
                                    textFile.Replace(i, oldText, "");
                                }

                                Output(textFile.DisplayLine(i, true));
                            }
                        }

                        editorValues.CurrentLine += editorValues.RepeatLines - 1;
                    }
                }
            }

            return false;
        }
    }
}
