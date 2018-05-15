using System;

namespace PickEd
{
    [CommandFilter(Prefix = "EX")]
    public class ExitCommand : CommandBase
    {
        public override bool PreProcess(string cmd)
        {
            return (cmd.Length > 1);
        }

        public override bool Process(string cmd, TextFile textFile, EditorValues editorValues)
        {
            bool exit = false;

            if (textFile.HasChanged)
            {
                ReceiveInputEventArgs args = new ReceiveInputEventArgs("File has changed.  Exit without saving (Y/N)? ");
                //OnReceiveInput(args);
                if (args.InputValue.Substring(0, 1).ToUpper() == "Y")
                    exit = true;
            }
            else
                exit = true;

            return exit;
        }
    }
}
