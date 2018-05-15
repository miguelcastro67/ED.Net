using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace PickEd
{
    public class CommandProcessor
    {
        public CommandProcessor(TextFile textFile) 
            : this(textFile, null, null)
        {
        }

        public CommandProcessor(TextFile textFile,
            EventHandler<OutputMessageEventArgs> outputMessageHandler,
            EventHandler<ReceiveInputEventArgs> receiveInputHandler)
        {
            _TextFile = textFile;

            OutputMessage += outputMessageHandler;
            ReceiveInput += receiveInputHandler;

            _Commands = GetCommands();

        }

        TextFile _TextFile;

        int pageSize = 20;
        int currentLine = 0;

        IEnumerable<CommandMetadata> _Commands = null;

        public event EventHandler<OutputMessageEventArgs> OutputMessage;
        public event EventHandler<ReceiveInputEventArgs> ReceiveInput;

        protected virtual void OnReceiveInput(ReceiveInputEventArgs e)
        {
            if (this.ReceiveInput != null)
                this.ReceiveInput(this, e);
        }

        protected virtual void OnOutputMessage(OutputMessageEventArgs e)
        {
            if (this.OutputMessage != null)
                this.OutputMessage(this, e);
        }
        
        public bool ProcessCommand2(string command)
        {
            bool exit = false;

            string commandToProcess = command.ToUpper();

            EditorValues editorValues = new EditorValues()
            {
                CommandInfo = string.Empty,
                RepeatLines = 0,
                Delim = string.Empty
            };
             
            int lineNumber = 0;
            bool isLineNuber = int.TryParse(command, out lineNumber);
            if (isLineNuber && lineNumber <= _TextFile.Lines)
            {
                // entered a line number
                editorValues.CurrentLine = lineNumber;
                commandToProcess = ".";
            }
            
            CommandMetadata commandMetadata = _Commands.FirstOrDefault(item => commandToProcess.StartsWith(item.Prefix));
            if (commandMetadata != null)
            {
                CommandBase commandObject = Activator.CreateInstance(commandMetadata.CommandType) as CommandBase;
                if (commandObject != null)
                {
                    if (commandObject.PreProcess(command))
                        exit = commandObject.Process(commandToProcess, _TextFile, editorValues);
                }
            }

            return exit;
        }

        public bool ProcessCommand(string command)
        {
            bool exit = false;
            string commandToProcess = command.ToUpper();
            string info = string.Empty;
            int repeatLines = 0;
            string delim = string.Empty;

            // perform pre-processing
            
            int lineNumber = 0;
            bool isLineNuber = int.TryParse(command, out lineNumber);
            if (isLineNuber && lineNumber <= _TextFile.Lines)
            {
                // entered a line number
                currentLine = lineNumber;
                commandToProcess = ".";
            }
            else if (command.Length > 1 && command.Substring(0, 1).ToUpper() == "L")
            {
                // change page size and list page

                string temp = command.Substring(1);
                int lines = 0;
                bool linesOk = int.TryParse(temp, out lines);
                if (lines > 0)
                    pageSize = lines;

                commandToProcess = "L";
            }
            else if (command.Length >= 2 && command.ToUpper().Substring(0, 2) == "EX")
            {
                commandToProcess = "EX";
            }
            else if (command.Length > 1 && command.Substring(0, 1).ToUpper() == "R")
            {
                // possible "replace" command

                repeatLines = GetRepeatLines(command);
                delim = GetDelimiter(command);

                if (repeatLines == 0)
                {
                    repeatLines = 1;
                    info = command.Substring(2);
                    commandToProcess = "R";
                }
                else
                {
                    string[] sections = command.Substring(1).Split(Convert.ToChar(delim));
                    if (sections.Length >= 2)
                    {
                        int pos = command.IndexOf(delim);
                        string lines = command.Substring(1, pos - 1);
                        repeatLines = 1;
                        bool linesOk = int.TryParse(lines, out repeatLines);
                        if (linesOk)
                        {
                            info = command.Substring(pos + 1);
                            commandToProcess = "R";
                        }
                        else
                            commandToProcess = null;
                    }
                    else
                        commandToProcess = null;
                }
            }
            else if (command.Length > 1 && command.Substring(0, 1).ToUpper() == "D")
            {
                // delete commandToProcess
                repeatLines = GetRepeatLines(command);
                commandToProcess = "D";
            }

            // process command

            switch (commandToProcess)
            {
                case ".":
                    // display current line
                    if (currentLine > 0)
                        Output(_TextFile.DisplayLine(currentLine, true));

                    break;

                case "":
                    // next line

                    if (currentLine < _TextFile.Lines)
                        currentLine++;

                    Output(_TextFile.DisplayLine(currentLine, true));

                    break;

                case "T":
                    // top of the text file
                    currentLine = 0;
                    Output("Top");

                    break;

                case "L":
                    // list next page

                    if (_TextFile.Lines > 0)
                    {
                        int startLine = currentLine + 1;
                        int endLine = startLine + pageSize;

                        if (endLine > _TextFile.Lines)
                            endLine = _TextFile.Lines;

                        if (startLine < endLine)
                        {
                            for (int i = startLine; i < endLine; i++)
                                Output(_TextFile.DisplayLine(i, true));
                        }
                        else
                            Output(_TextFile.DisplayLine(currentLine, true));

                        currentLine += pageSize;
                        if (currentLine > _TextFile.Lines - 1)
                            currentLine = _TextFile.Lines - 1;
                    }
                    else
                        currentLine = 0;

                    break;

                case "R":
                    // replace

                    if (currentLine > 0)
                    {
                        if (info == string.Empty)
                        {
                            // replace entire line

                            Console.Write(currentLine.ToString() + " ");
                            string newLine = Console.ReadLine();

                            _TextFile.Replace(currentLine, newLine);
                        }
                        else
                        {
                            // perform calculated replace

                            if (info.IndexOf(delim) > -1)
                            {
                                string[] replacementInfo = info.Split(Convert.ToChar(delim));
                                string oldText = replacementInfo[0];
                                string newText = replacementInfo[1];

                                for (int i = currentLine; i <= currentLine + repeatLines - 1; i++)
                                {
                                    if (oldText == "" && newText != "")
                                    {
                                        // insert new text in front of the line
                                        _TextFile.Insert(i, newText);
                                    }
                                    else if (oldText != "" && newText != "")
                                    {
                                        // replace old text with new text
                                        _TextFile.Replace(i, oldText, newText);
                                    }
                                    else if (oldText != "" && newText == "" && replacementInfo.Length > 2)
                                    {
                                        // remove old text
                                        _TextFile.Replace(i, oldText, "");
                                    }

                                    Output(_TextFile.DisplayLine(i, true));
                                }
                            }

                            currentLine += repeatLines - 1;
                        }
                    }

                    break;

                case "I":
                    // insert lines

                    bool exitInsert = false;
                    while (!exitInsert)
                    {
                        Console.Write(string.Format("{0:000}+", currentLine));
                        string newLine = Console.ReadLine();
                        if (newLine != string.Empty)
                        {
                            _TextFile.Insert(currentLine, newLine, true);
                            currentLine++;
                        }
                        else
                            exitInsert = true;
                    }

                    break;

                case "D":
                    // delete lines

                    if (currentLine > 0)
                    {
                        if (repeatLines == 0)
                        {
                            // delete current line
                            _TextFile.Delete(currentLine);
                        }
                        else
                        {
                            // delete multiple lines

                            if (info.IndexOf(delim) > -1)
                            {
                                int lastLine = currentLine + repeatLines - 1;
                                if (lastLine > _TextFile.Lines)
                                    lastLine = _TextFile.Lines;

                                for (int i = currentLine; i <= lastLine; i++)
                                    _TextFile.Delete(currentLine);
                            }
                        }
                    }

                    break;

                case "X":
                    // cancel changes

                    _TextFile.Cancel();

                    if (currentLine > _TextFile.Lines)
                        currentLine = _TextFile.Lines;

                    break;

                case "F":
                    // flip

                    _TextFile.Flip();

                    break;

                case "FS":
                    // file-save and stay

                    _TextFile.Flip();
                    _TextFile.Save();

                    Output("Saved");

                    break;

                case "FI":
                    // file-save and exit

                    _TextFile.Flip();
                    _TextFile.Save();

                    Output("Saved");

                    exit = true;

                    break;

                case "EX":
                    // exit
                    
                    if (_TextFile.HasChanged)
                    {
                        ReceiveInputEventArgs args = new ReceiveInputEventArgs("File has changed.  Exit without saving (Y/N)? ");
                        OnReceiveInput(args);
                        if (args.InputValue.Substring(0, 1).ToUpper() == "Y")
                            exit = true;
                    }
                    else
                        exit = true;

                    break;

                case "?":
                    // help

                    Output("-- ED.Net");
                    Output("-- A Pick-like line editor developed in C#");
                    Output("-- www.melvicorp.com");
                    Output("");
                    Output("Available commands and instructions:");
                    Output("");
                    Output(".");
                    Output("   Displays the current line.");
                    Output("{enter}");
                    Output("   Displays the current line and increments the current line number.");
                    Output("L[xx]");
                    Output("   Lists the next page of text lines based on the current page size");
                    Output("   (20 by default).  If [xx] is specfied, then [xx] number of lines");
                    Output("   are listed and the current page size is reset to [xx] number of lines.");
                    Output("T");
                    Output("   Sets the current line counter to the top of the file.");
                    Output("R[xx][/old/new");
                    Output("   Performs a 'replace' operation.");
                    Output("   If 'R' is used on its own then you are given the opportunity to");
                    Output("   replace the entire line.  Specifying [xx] will perform the 'replace'");
                    Output("   operation for [xx] number of lines.");
                    Output("   The 'old' and 'new' specify the text you want to replace and with");
                    Output("   what you it to be replaced.  The delimiter specified as '/' can be");
                    Output("   any character you like in order to use any other characters");
                    Output("   in the 'old' and 'new' text.  Specifying no 'old' text will insert");
                    Output("   the 'new' text at the beginning of the line(s).");
                    Output("   Usage examples:");
                    Output("      R/Using/Imports");
                    Output("        Replaces all instances of 'Using' with the text 'Imports'.");
                    Output("      R12/Using/Imports");
                    Output("        Performs the same operation for the next 12 lines.");
                    Output("      R5//''");
                    Output("        Inserts two tick marks in front of of the next 5 lines.");
                    Output("      R5..//");
                    Output("        Inserts two slashes in front of the next five lines.");
                    Output("      R.1/5/2008.01/05/2008");
                    Output("        Replaces the text '1/5/2008' with the text '01/05/2008'.");
                    Output("I[xx]");
                    Output("   Places the editor into 'Insert' mode at a point immediately following");
                    Output("   the current line.  Hitting {enter} on a line witout typing any text");
                    Output("   will exit 'Insert' mode.  Specifying [xx] will end 'Insert' mode");
                    Output("   after [xx] number of lines are inserted.");
                    Output("AL[xx]/text");
                    Output("   Performs an append opperation to a line or to [xx] number of lines.");
                    Output("   The delimiter specified by the '/' can be any character in order to");
                    Output("   accomodate any text to be appended (similarly to the 'R' command).");
                    Output("D[xx]");
                    Output("   Deletes the current line or [xx] number of lines.");
                    Output("X");
                    Output("   Rolls back the all changes since the last 'flip'");
                    Output("   (see the commands below).");
                    Output("F");
                    Output("   Performs a 'flip' operation.  When any 'replace', insert', 'append',");
                    Output("   or 'delete' command is performed, the operations are not permanent in memory ");
                    Output("   until they are 'flipped', so they can be rolled back");
                    Output("   using the 'X' command.  Performing a 'flip' operation brings those");
                    Output("   changes to permanent status.  Note, permanent does not signify 'saved'.");
                    Output("FS");
                    Output("   Performs a 'flip' operation followed by a 'save' operation.");
                    Output("FI");
                    Output("   Performs a 'flip' operation followed by a 'save' operation; afterwhich");
                    Output("   the edit is exited.");
                    Output("EX");
                    Output("   Exits the editor in the current state of the file.  If the file has");
                    Output("   changed since it was initially read in, the user will receive a");
                    Output("   confirmation message.");
                    Output("");

                    break;
            }

            return exit;
        }

        private void Output(string message)
        {
            OnOutputMessage(new OutputMessageEventArgs(message));
        }
        
        private int GetRepeatLines(string command)
        {
            int repeatLines = 0;
            int numPos = 1;
            bool noMoreNumbers = false;
            string numValue = string.Empty;
            while (!noMoreNumbers)
            {
                if (numPos < command.Length)
                {
                    string character = command.Substring(numPos, 1);
                    if (IsNumeric(character))
                    {
                        numValue += character;
                        numPos++;
                    }
                    else
                        noMoreNumbers = true;
                }
                else
                    noMoreNumbers = true;
            }

            if (numValue != string.Empty)
                repeatLines = int.Parse(numValue);

            return repeatLines;
        }

        private string GetDelimiter(string command)
        {
            int delimPos = 0;
            int repeatLines = GetRepeatLines(command);
            if (repeatLines > 0)
                delimPos = repeatLines.ToString().Length + 1;
            else
                delimPos = 1;

            return command.Substring(delimPos, 1);
        }

        private bool IsNumeric(string character)
        {
            int value = 0;
            bool ret = int.TryParse(character, out value);
            return ret;
        }

        IEnumerable<CommandMetadata> GetCommands()
        {
            List<CommandMetadata> commandTypes = new List<CommandMetadata>();

            Type[] types = this.GetType().Assembly.GetTypes();

            foreach (Type type in types)
            {
                if (type.IsSubclassOf(typeof(CommandBase)))
                {
                    CommandFilterAttribute attr = type.GetCustomAttribute<CommandFilterAttribute>();
                    if (attr != null)
                    {
                        CommandMetadata commandMetadata = new CommandMetadata()
                        {
                            Prefix = attr.Prefix,
                            CommandType = type
                        };

                        commandTypes.Add(commandMetadata);
                    }
                }
            }

            return commandTypes;
        }
    }
}
