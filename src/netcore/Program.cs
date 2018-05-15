using PickEd;
using System;

namespace PickEdConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Editor editor = new Editor(args);

            editor.ObtainFileName += (s, e) =>
            {
                Console.Write(editor. lbl_File);
                e.FileName = Console.ReadLine();
            };

            editor.ReceiveInput += (s, e) =>
            {
                if (e.Message != string.Empty)
                    Console.WriteLine(e.Message);

                e.InputValue = Console.ReadLine();
            };
            
            editor.Output += (s, e) => Console.WriteLine(e.Message);

            TextFile textFile = editor.Enter();
            if (textFile != null)
            {
                bool exit = false;

                while (!exit)
                {
                    Console.Write(editor.Lbl_Prompt);
                    string command = Console.ReadLine();

                    exit = editor.Command(command);
                }
            }
        }
    }
}
