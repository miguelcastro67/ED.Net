using System;
using System.IO;
using System.Linq;

namespace PickEd
{
    public class Editor
    {
        public Editor()
        {
        }

        public Editor(string[] args)
        {
            _Args = args;
        }

        string[] _Args = new string[] { };
        CommandProcessor _CommandProcessor = null;

        public virtual string Lbl_New
        {
            get { return "New"; }
        }

        public virtual string Lbl_Prompt
        {
            get { return "."; }
        }

        public virtual string lbl_File
        {
            get { return "File: "; }
        }

        public event EventHandler<ObtainFileNameEventArgs> ObtainFileName;
        public event EventHandler<ReceiveInputEventArgs> ReceiveInput;
        public event EventHandler<OutputMessageEventArgs> Output;

        protected virtual void OnOutput(string message)
        {
            Output?.Invoke(this, new OutputMessageEventArgs(message));
        }

        public TextFile Enter()
        {
            return Enter(_Args);
        }
        
        public TextFile Enter(string[] args)
        {
            string fileName = string.Empty;

            if (args.Length == 0)
            {
                // no file specified
                ObtainFileNameEventArgs eventArgs = new ObtainFileNameEventArgs();
                if (this.ObtainFileName != null)
                {
                    this.ObtainFileName(this, eventArgs);
                    fileName = eventArgs.FileName;
                }
            }
            else
                fileName = args[0];

            TextFile textFile = null;

            if (fileName != string.Empty)
            {
                OnOutput("(" + fileName + ")");
                if (!File.Exists(fileName))
                {
                    textFile = new TextFile(fileName);
                    OnOutput(Lbl_New);
                }
                else
                {
                    textFile = new TextFile();
                    textFile.ReadFromFile(fileName);
                }
            }

            _CommandProcessor = new CommandProcessor(textFile, 
                (s, e) => Output(this, e), 
                (s, e) => ReceiveInput(this, e));

            return textFile;
        }

        public bool Command(string command)
        {
            if (_CommandProcessor != null)
                return _CommandProcessor.ProcessCommand(command);
            else
                return false;
        }
    }
}
