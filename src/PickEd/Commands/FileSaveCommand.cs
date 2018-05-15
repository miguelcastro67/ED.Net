﻿using System;

namespace PickEd
{
    [CommandFilter(Prefix = "FS")]
    public class FileSaveCommand : CommandBase
    {
        public override bool Process(string cmd, TextFile textFile, EditorValues editorValues)
        {
            textFile.Flip();
            textFile.Save();

            Output("Saved");

            return false;
        }
    }
}
