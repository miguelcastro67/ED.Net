using System;

namespace PickEd
{
    [CommandFilter(Prefix = "?")]
    public class HelpCommand : CommandBase
    {
        public override bool Process(string cmd, TextFile textFile, EditorValues editorValues)
        {
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
            Output("   Performs an append opperation to a line or two [xx] number of lines.");
            Output("   The delimiter specified by the '/' can be any character in order to");
            Output("   accomodate any text to be appended.");
            Output("D[xx]");
            Output("   Deletes the current line, or [xx] number of lines.");
            Output("X");
            Output("   Rolls back the all changes since the last 'flip'");
            Output("   (see the commands below).");
            Output("F");
            Output("   Performs a 'flip' operation.  When any 'replace', insert', 'append',");
            Output("   or 'delete' command is performed, the operations can be rolled back");
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

            return false;
        }
    }
}
