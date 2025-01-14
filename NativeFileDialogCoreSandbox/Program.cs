using NativeFileDialogCore;
using System;
using System.Runtime.InteropServices;

namespace NativeFileDialogCoreSandbox
{
    internal class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("test");

            IntPtr? consoleHandle = null;
            
            #if WINDOWS
            consoleHandle = GetConsoleWindow();
            #endif            

            PrintResult(Dialog.FileOpenEx(null, null, "C# dialog title!", selectButtonLabel: "ok dokie", cancelButtonLabel: "no no go back", parentWindow: consoleHandle));
                
            return;
            
            PrintResult(Dialog.FileOpenMultiple("pdf", null));
            PrintResult(Dialog.FileOpen(null));
            PrintResult(Dialog.FileSave(null));
            PrintResult(Dialog.FolderPicker(null));
        }

        static void PrintResult(DialogResult result)
        {
            Console.WriteLine($"Path: {result.Path}, IsError {result.IsError}, IsOk {result.IsOk}, IsCancelled {result.IsCancelled}, ErrorMessage {result.ErrorMessage}");
            if (result.Paths != null)
            {
                Console.WriteLine("Paths");
                Console.WriteLine(string.Join("\n", result.Paths));
            }
        }
    }
}
