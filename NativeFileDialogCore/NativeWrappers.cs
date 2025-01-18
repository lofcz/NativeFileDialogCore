using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using NativeFileDialogCore.Native;

namespace NativeFileDialogCore
{
    public static class Dialog
    {
        private static readonly bool need32Bit = Is32BitWindowsOnNetFramework();

        private static bool Is32BitWindowsOnNetFramework()
        {
            try
            {
                // we call a function that does nothing just to test if we can load it properly
                NativeFunctions.NFD_Dummy();
                return false;
            }
            catch
            {
                // a call to a default library failed, let's attempt the other one
                try
                {
                    NativeFunctions32.NFD_Dummy();
                    return true;
                }
                catch
                {
                    // both of them failed so we may as well default to the default one for predictability
                    return false;
                }
            }
        }
        
        /// <summary>
        /// Opens a styled dialog, prompting user to select a file.
        /// </summary>
        /// <param name="filterList"></param>
        /// <param name="defaultPath"></param>
        /// <param name="dialogTitle"></param>
        /// <param name="fileNameLabel"></param>
        /// <param name="selectButtonLabel"></param>
        /// <param name="cancelButtonLabel"></param>
        /// <returns></returns>
        public static DialogResult FileOpenEx(string? filterList = null, string? defaultPath = null, string? dialogTitle = null, string? fileNameLabel = null, string? selectButtonLabel = null, string? cancelButtonLabel = null, IntPtr? parentWindow = null)
        {
            string? path = null;
            string? errorMessage = null;
            
            nfdresult_t result = need32Bit 
                ? NativeFunctions32.NFD_OpenDialogEx(filterList, defaultPath, dialogTitle, fileNameLabel, selectButtonLabel, cancelButtonLabel, parentWindow ?? IntPtr.Zero, out IntPtr outPathIntPtr)
                : NativeFunctions.NFD_OpenDialogEx(filterList, defaultPath, dialogTitle, fileNameLabel, selectButtonLabel, cancelButtonLabel, parentWindow ?? IntPtr.Zero, out outPathIntPtr);
            
            switch (result)
            {
                case nfdresult_t.NFD_ERROR:
                {
                    errorMessage = need32Bit ? NativeFunctions32.NFD_GetError() : NativeFunctions.NFD_GetError();
                    break;
                }
                case nfdresult_t.NFD_OKAY:
                {
                    path = Marshal.PtrToStringUTF8(outPathIntPtr);
                    NativeFunctions.NFD_Free(outPathIntPtr);
                    break;
                }
            }

            return new DialogResult(result, path, null, errorMessage);
        }

        public static DialogResult FileOpen(string? filterList = null, string? defaultPath = null)
        {
            string? path = null;
            string? errorMessage = null;
            
            nfdresult_t result = need32Bit 
                ? NativeFunctions32.NFD_OpenDialog(filterList, defaultPath, out IntPtr outPathIntPtr)
                : NativeFunctions.NFD_OpenDialog(filterList, defaultPath, out outPathIntPtr);
            
            switch (result)
            {
                case nfdresult_t.NFD_ERROR:
                {
                    errorMessage = need32Bit ? NativeFunctions32.NFD_GetError() : NativeFunctions.NFD_GetError();
                    break;
                }
                case nfdresult_t.NFD_OKAY:
                {
                    path = Marshal.PtrToStringUTF8(outPathIntPtr);
                    NativeFunctions.NFD_Free(outPathIntPtr);
                    break;
                }
            }

            return new DialogResult(result, path, null, errorMessage);
        }

        public static DialogResult FileSave(string? filterList = null, string? defaultPath = null)
        {
            string? path = null;
            string? errorMessage = null;
            
            nfdresult_t result = need32Bit 
                ? NativeFunctions32.NFD_SaveDialog(filterList, defaultPath, out IntPtr outPathIntPtr) 
                : NativeFunctions.NFD_SaveDialog(filterList, filterList, out outPathIntPtr);
            
            switch (result)
            {
                case nfdresult_t.NFD_ERROR:
                {
                    errorMessage = need32Bit ? NativeFunctions32.NFD_GetError() : NativeFunctions.NFD_GetError();
                    break;
                }
                case nfdresult_t.NFD_OKAY:
                {
                    path = Marshal.PtrToStringUTF8(outPathIntPtr);
                    NativeFunctions.NFD_Free(outPathIntPtr);
                    break;
                }
            }

            return new DialogResult(result, path, null, errorMessage);
        }

        public static DialogResult FolderPicker(string? defaultPath = null)
        {
            string? path = null;
            string? errorMessage = null;
            nfdresult_t result = need32Bit
                ? NativeFunctions32.NFD_PickFolder(defaultPath, out IntPtr outPathIntPtr)
                : NativeFunctions.NFD_PickFolder(defaultPath, out outPathIntPtr);
            
            switch (result)
            {
                case nfdresult_t.NFD_ERROR:
                {
                    errorMessage = need32Bit ? NativeFunctions32.NFD_GetError() : NativeFunctions.NFD_GetError();
                    break;
                }
                case nfdresult_t.NFD_OKAY:
                {
                    path = Marshal.PtrToStringUTF8(outPathIntPtr);
                    NativeFunctions.NFD_Free(outPathIntPtr);
                    break;
                }
            }

            return new DialogResult(result, path, null, errorMessage);
        }

        public static DialogResult FileOpenMultiple(string? filterList = null, string? defaultPath = null)
        {
            List<string>? paths = null;
            string? errorMessage = null;

            nfdresult_t result = need32Bit
                ? NativeFunctions32.NFD_OpenDialogMultiple(filterList, defaultPath, out nfdpathset_t pathSet)
                : NativeFunctions.NFD_OpenDialogMultiple(filterList, defaultPath, out pathSet);
            
            switch (result)
            {
                case nfdresult_t.NFD_ERROR:
                {
                    errorMessage = need32Bit ? NativeFunctions32.NFD_GetError() : NativeFunctions.NFD_GetError();
                    break;
                }
                case nfdresult_t.NFD_OKAY:
                {
                    int pathCount = (int)NativeFunctions.NFD_PathSet_GetCount(pathSet).ToUInt32();
                    paths = new List<string>(pathCount);
                    
                    for (int i = 0; i < pathCount; i++)
                    {
                        IntPtr pathPtr = NativeFunctions.NFD_PathSet_GetPath(in pathSet, (UIntPtr)i);

                        if (pathPtr == IntPtr.Zero)
                        {
                            continue;
                        }
                        
                        string? path = Marshal.PtrToStringUTF8(pathPtr);
                            
                        if (path is not null)
                        {
                            paths.Add(path);
                        }
                    }

                    NativeFunctions.NFD_PathSet_Free(ref pathSet);
                    break;
                }
            }

            return new DialogResult(result, null, paths, errorMessage);
        }
    }

    public class DialogResult
    {
        private readonly nfdresult_t result;

        public string? Path { get; }

        public IReadOnlyList<string>? Paths { get; }

        public bool IsError => result == nfdresult_t.NFD_ERROR;

        public string? ErrorMessage { get; }

        public bool IsCancelled => result == nfdresult_t.NFD_CANCEL;

        public bool IsOk => result == nfdresult_t.NFD_OKAY;

        internal DialogResult(nfdresult_t result, string? path, IReadOnlyList<string>? paths, string? errorMessage)
        {
            this.result = result;
            Path = path;
            Paths = paths;
            ErrorMessage = errorMessage;
        }
    }
}