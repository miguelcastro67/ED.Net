using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PickEd
{
    public class TextFile
    {
        private TextLines _Orig;
        private TextLines _Temp;
        private TextLines _Perm;
        private string _FileName;

        public TextFile()
        {
        }
        
        public TextFile(string fileName)
        {
            _FileName = fileName;
            _Temp = new TextLines();
            _Perm = new TextLines();
            _Orig = new TextLines();
        }

        public void ReadFromFile(string fileName)
        {
            _FileName = fileName;

            using(StreamReader reader = new StreamReader(fileName))
            {
                _Temp = new TextLines();

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    AddTextLine(line);
                }
            }

            _Perm = _Temp.Clone();
            _Orig = _Temp.Clone();

        }

        public void AddTextLine(string text)
        {
            _Temp.Add(new TextLine(text));
        }

        public int Lines
        {
            get
            {
                return _Temp.Count;
            }
        }

        public string DisplayLine(int lineNumber)
        {
            return DisplayLine(lineNumber, true);
        }

        public string DisplayLine(int lineNumber, bool displayLineNumber)
        {
            // _Lines is an option-base 0 collection but line number will imply option-base 1
            string line = string.Empty;

            if(displayLineNumber)
                line = string.Format("{0:000} {1}", lineNumber, _Temp[lineNumber - 1].Value);
            else
                line = string.Format("{0}", _Temp[lineNumber - 1].Value);

            return line;
        }

        public TextLine this[int index]
        {
            get
            {
                return _Temp[index - 1];
            }
        }

        public void Insert(int lineNumber, string text)
        {
            Insert(lineNumber, text, false);
        }

        public void Insert(int lineNumber, string text, bool newLine)
        {
            if(!newLine)
                _Temp[lineNumber - 1].Value = text + _Temp[lineNumber - 1].Value;
            else
                _Temp.Insert(lineNumber, new TextLine(text));
        }

        public void Replace(int lineNumber, string newLine)
        {
            _Temp[lineNumber - 1].Value = newLine;
        }

        public void Replace(int lineNumber, string oldText, string newText)
        {
            _Temp[lineNumber - 1].Value = _Temp[lineNumber - 1].Value.Replace(oldText, newText);
        }

        public void Delete(int lineNumber)
        {
            _Temp.RemoveAt(lineNumber - 1);
        }

        public void Cancel()
        {
            _Temp = _Perm.Clone();
        }

        public void Flip()
        {
            _Perm = _Temp.Clone();
        }

        public void Save()
        {
            Save(_FileName);
        }

        public void Save(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName, false))
            {
                foreach (TextLine item in _Temp)
                    writer.WriteLine(item.Value);
            }
        }

        public bool HasChanged
        {
            get
            {
                return !(_Temp.Equals(_Orig));
            }
        }

    }
}
