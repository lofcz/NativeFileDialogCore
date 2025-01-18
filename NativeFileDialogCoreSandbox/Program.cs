using NativeFileDialogCore;
using System;
using System.Runtime.InteropServices;

namespace NativeFileDialogCoreSandbox
{
    internal class Program
    {
#if WINDOWS
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();
#endif          
        [STAThread]
        private static void Main(string[] args)
        {
            Console.WriteLine("test");

            IntPtr? consoleHandle = null;
            
            #if WINDOWS
            consoleHandle = GetConsoleWindow();
            #endif

            //PrintResult(Dialog.FileOpen("jpg"));
            //PrintResult(Dialog.FileOpenEx("[Awesome named list|jpg,png]", null, "Select File - NativeFileDialogCore!", selectButtonLabel: "That's the one!", cancelButtonLabel: "Abort mission!", parentWindow: consoleHandle));
            //return;
            
            //PrintResult(Dialog.FileOpenMultiple("pdf"));
            //PrintResult(Dialog.FileOpen());
            //PrintResult(Dialog.FileSave("txt"));
            PrintResult(Dialog.FolderPicker());

            Console.ReadKey();
        }

        private static void PrintResult(DialogResult result)
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
