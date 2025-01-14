using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using NativeFileDialogCore.Native;

namespace NativeFileDialogCore
{
    public class DialogNamedFilter
    {
        public string Name { get; set; }
        public List<DialogAnonymousFilter> Filters { get; set; }
    }

    public class DialogAnonymousFilter
    {
        
    }
    
    public class DialogFilter
    {
        public DialogFilter()
        {
            
        }

        public DialogFilter(string filter)
        {
            
        }
        
        internal string Serialize()
        {
            return string.Empty;
        }

        public static implicit operator DialogFilter(string filter) => new DialogFilter(filter);
        public static implicit operator string(DialogFilter filter) => filter.Serialize();
    }
    
    public static class Dialog
    {
        private static readonly bool need32bit = Is32BitWindowsOnNetFramework();
        private static readonly MemoryPool<byte> _memoryPool = MemoryPool<byte>.Shared;
        
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

        private static byte[] ToUtf8(string s)
        {
            return Encoding.UTF8.GetBytes($"{s}\0");
        }
        
        private static unsafe int GetNullTerminatedStringLength(byte* nullTerminatedString)
        {
            int count = 0;
            byte* ptr = nullTerminatedString;
            while (*ptr != 0)
            {
                ptr++;
                count++;
            }

            return count;
        }

        private static unsafe string FromUtf8(byte* nullTerminatedString)
        {
            return Encoding.UTF8.GetString(nullTerminatedString, GetNullTerminatedStringLength(nullTerminatedString));
        }
        
        private static IMemoryOwner<byte> RentUtf8Buffer(string text)
        {
            int byteCount = Encoding.UTF8.GetByteCount(text) + 1;
            IMemoryOwner<byte> owner = _memoryPool.Rent(byteCount);
            Encoding.UTF8.GetBytes(text, owner.Memory.Span);
            owner.Memory.Span[byteCount - 1] = 0;
            return owner;
        }
        
        /// <summary>
        /// Opens a styled dialog, prompting user to select a file.
        /// </summary>
        /// <param name="filterList"></param>
        /// <param name="filter"></param>
        /// <param name="dialogTitle"></param>
        /// <param name="fileNameLabel"></param>
        /// <param name="selectButtonLabel"></param>
        /// <param name="cancelButtonLabel"></param>
        /// <returns></returns>
        public static unsafe DialogResult FileOpenEx(string? filterList = null, string? filter = null, string? dialogTitle = null, string? fileNameLabel = null, string? selectButtonLabel = null, IntPtr? parentWindow = null, string? cancelButtonLabel = null)
        {
            using IMemoryOwner<byte>? filterListOwner = filterList is not null ? RentUtf8Buffer(filterList) : null;
            using IMemoryOwner<byte>? defaultPathOwner = filter is not null ? RentUtf8Buffer(filter) : null;
            using IMemoryOwner<byte>? titleOwner = dialogTitle is not null ? RentUtf8Buffer(dialogTitle) : null;
            using IMemoryOwner<byte>? fileNameLabelOwner = fileNameLabel is not null ? RentUtf8Buffer(fileNameLabel) : null;
            using IMemoryOwner<byte>? selectButtonLabelOwner = selectButtonLabel is not null ? RentUtf8Buffer(selectButtonLabel) : null;
            using IMemoryOwner<byte>? cancelButtonLabelOwner = cancelButtonLabel is not null ? RentUtf8Buffer(cancelButtonLabel) : null;
            
            fixed (byte* filterListPtr = filterList is null ? null : filterListOwner.Memory.Span)
            fixed (byte* defaultPathPtr = defaultPathOwner is null ? null : defaultPathOwner.Memory.Span)
            fixed (byte* titlePtr = titleOwner is null ? null : titleOwner.Memory.Span)
            fixed (byte* fileNameLabelPtr = fileNameLabelOwner is null ? null : fileNameLabelOwner.Memory.Span)
            fixed (byte* selectButtonLabelPtr = selectButtonLabelOwner is null ? null : selectButtonLabelOwner.Memory.Span)
            fixed (byte* cancelButtonLabelPtr = cancelButtonLabelOwner is null ? null : cancelButtonLabelOwner.Memory.Span)

            {
                string? path = null;
                string? errorMessage = null;
                
                nfdresult_t result = need32bit 
                    ? NativeFunctions32.NFD_OpenDialogEx(filterListPtr, defaultPathPtr, titlePtr, fileNameLabelPtr, selectButtonLabelPtr, cancelButtonLabelPtr, parentWindow ?? IntPtr.Zero, out IntPtr outPathIntPtr)
                    : NativeFunctions.NFD_OpenDialogEx(filterListPtr, defaultPathPtr, titlePtr, fileNameLabelPtr, selectButtonLabelPtr, cancelButtonLabelPtr, parentWindow ?? IntPtr.Zero, out outPathIntPtr);
                
                switch (result)
                {
                    case nfdresult_t.NFD_ERROR:
                        errorMessage = FromUtf8(NativeFunctions.NFD_GetError());
                        break;
                    case nfdresult_t.NFD_OKAY:
                    {
                        byte* outPathNts = (byte*)outPathIntPtr.ToPointer();
                        path = FromUtf8(outPathNts);
                        NativeFunctions.NFD_Free(outPathIntPtr);
                        break;
                    }
                }

                return new DialogResult(result, path, null, errorMessage);
            }
        }

        public static unsafe DialogResult FileOpen(string filterList = null, string defaultPath = null)
        {
            fixed (byte* filterListNts = filterList != null ? ToUtf8(filterList) : null)
            fixed (byte* defaultPathNts = defaultPath != null ? ToUtf8(defaultPath) : null)
            {
                string path = null;
                string errorMessage = null;
                nfdresult_t result = need32bit 
                    ? NativeFunctions32.NFD_OpenDialog(filterListNts, defaultPathNts, out IntPtr outPathIntPtr)
                    : NativeFunctions.NFD_OpenDialog(filterListNts, defaultPathNts, out outPathIntPtr);
                
                switch (result)
                {
                    case nfdresult_t.NFD_ERROR:
                        errorMessage = FromUtf8(NativeFunctions.NFD_GetError());
                        break;
                    case nfdresult_t.NFD_OKAY:
                    {
                        byte* outPathNts = (byte*)outPathIntPtr.ToPointer();
                        path = FromUtf8(outPathNts);
                        NativeFunctions.NFD_Free(outPathIntPtr);
                        break;
                    }
                }

                return new DialogResult(result, path, null, errorMessage);
            }
        }

        public static unsafe DialogResult FileSave(string filterList = null, string defaultPath = null)
        {
            fixed (byte* filterListNts = filterList != null ? ToUtf8(filterList) : null)
            fixed (byte* defaultPathNts = defaultPath != null ? ToUtf8(defaultPath) : null)
            {
                string path = null;
                string errorMessage = null;
                nfdresult_t result = need32bit 
                    ? NativeFunctions32.NFD_SaveDialog(filterListNts, defaultPathNts, out IntPtr outPathIntPtr) 
                    : NativeFunctions.NFD_SaveDialog(filterListNts, defaultPathNts, out outPathIntPtr);
                
                switch (result)
                {
                    case nfdresult_t.NFD_ERROR:
                        errorMessage = FromUtf8(NativeFunctions.NFD_GetError());
                        break;
                    case nfdresult_t.NFD_OKAY:
                    {
                        byte* outPathNts = (byte*)outPathIntPtr.ToPointer();
                        path = FromUtf8(outPathNts);
                        NativeFunctions.NFD_Free(outPathIntPtr);
                        break;
                    }
                }

                return new DialogResult(result, path, null, errorMessage);
            }
        }

        public static unsafe DialogResult FolderPicker(string defaultPath = null)
        {
            fixed (byte* defaultPathNts = defaultPath != null ? ToUtf8(defaultPath) : null)
            {
                string path = null;
                string errorMessage = null;
                nfdresult_t result = need32bit
                    ? NativeFunctions32.NFD_PickFolder(defaultPathNts, out IntPtr outPathIntPtr)
                    : NativeFunctions.NFD_PickFolder(defaultPathNts, out outPathIntPtr);
                
                switch (result)
                {
                    case nfdresult_t.NFD_ERROR:
                        errorMessage = FromUtf8(NativeFunctions.NFD_GetError());
                        break;
                    case nfdresult_t.NFD_OKAY:
                    {
                        byte* outPathNts = (byte*)outPathIntPtr.ToPointer();
                        path = FromUtf8(outPathNts);
                        NativeFunctions.NFD_Free(outPathIntPtr);
                        break;
                    }
                }

                return new DialogResult(result, path, null, errorMessage);
            }
        }

        public static unsafe DialogResult FileOpenMultiple(string filterList = null, string defaultPath = null)
        {
            fixed (byte* filterListNts = filterList != null ? ToUtf8(filterList) : null)
            fixed (byte* defaultPathNts = defaultPath != null ? ToUtf8(defaultPath) : null)
            {
                List<string> paths = null;
                string errorMessage = null;
                nfdpathset_t pathSet;
                nfdresult_t result = need32bit
                    ? NativeFunctions32.NFD_OpenDialogMultiple(filterListNts, defaultPathNts, &pathSet)
                    : NativeFunctions.NFD_OpenDialogMultiple(filterListNts, defaultPathNts, &pathSet);
                
                switch (result)
                {
                    case nfdresult_t.NFD_ERROR:
                        errorMessage = FromUtf8(NativeFunctions.NFD_GetError());
                        break;
                    case nfdresult_t.NFD_OKAY:
                    {
                        int pathCount = (int)NativeFunctions.NFD_PathSet_GetCount(&pathSet).ToUInt32();
                        paths = new List<string>(pathCount);
                        for (int i = 0; i < pathCount; i++)
                        {
                            paths.Add(FromUtf8(NativeFunctions.NFD_PathSet_GetPath(&pathSet, new UIntPtr((uint)i))));
                        }

                        NativeFunctions.NFD_PathSet_Free(&pathSet);
                        break;
                    }
                }

                return new DialogResult(result, null, paths, errorMessage);
            }
        }
    }

    public class DialogResult
    {
        private readonly nfdresult_t result;

        public string Path { get; }

        public IReadOnlyList<string> Paths { get; }

        public bool IsError => result == nfdresult_t.NFD_ERROR;

        public string ErrorMessage { get; }

        public bool IsCancelled => result == nfdresult_t.NFD_CANCEL;

        public bool IsOk => result == nfdresult_t.NFD_OKAY;

        internal DialogResult(nfdresult_t result, string path, IReadOnlyList<string> paths, string errorMessage)
        {
            this.result = result;
            Path = path;
            Paths = paths;
            ErrorMessage = errorMessage;
        }
    }
}