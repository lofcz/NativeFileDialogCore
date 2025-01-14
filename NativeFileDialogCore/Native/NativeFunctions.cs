using System;
using System.Runtime.InteropServices;

namespace NativeFileDialogCore.Native
{
    internal struct nfdpathset_t
    {
        public IntPtr buf;
        public IntPtr indices;
        public UIntPtr count;
    }

    internal enum nfdresult_t
    {
        NFD_ERROR,
        NFD_OKAY,
        NFD_CANCEL
    }

    internal static partial class NativeFunctions
    {
        private const string LibraryName = "nfd64";

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static unsafe partial nfdresult_t NFD_OpenDialog(byte* filterList, byte* defaultPath, out IntPtr outPath);
        
        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static unsafe partial nfdresult_t NFD_OpenDialogEx(byte* filterList, byte* defaultPath, byte* dialogTitle, byte* fileNameLabel, byte* selectButtonLabel, byte* cancelButtonLabel, IntPtr parentWindow, out IntPtr outPath);

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static unsafe partial nfdresult_t NFD_OpenDialogMultiple(byte* filterList, byte* defaultPath, nfdpathset_t* outPaths);

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static unsafe partial nfdresult_t NFD_SaveDialog(byte* filterList, byte* defaultPath, out IntPtr outPath);

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static unsafe partial nfdresult_t NFD_PickFolder(byte* defaultPath, out IntPtr outPath);

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static unsafe partial byte* NFD_GetError();

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static unsafe partial UIntPtr NFD_PathSet_GetCount(nfdpathset_t* pathSet);

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static unsafe partial byte* NFD_PathSet_GetPath(nfdpathset_t* pathSet, UIntPtr index);

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static unsafe partial void NFD_PathSet_Free(nfdpathset_t* pathSet);

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial void NFD_Dummy();

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static partial void NFD_Free(IntPtr ptr);
    }
    
    internal static partial class NativeFunctions32
    {
        private const string LibraryName = "nfd_x86";

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static unsafe partial nfdresult_t NFD_OpenDialog(byte* filterList, byte* defaultPath, out IntPtr outPath);

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static unsafe partial nfdresult_t NFD_OpenDialogEx(byte* filterList, byte* defaultPath, byte* dialogTitle, byte* fileNameLabel, byte* selectButtonLabel, byte* cancelButtonLabel, IntPtr parentWindow, out IntPtr outPath);
        
        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static unsafe partial nfdresult_t NFD_OpenDialogMultiple(byte* filterList, byte* defaultPath, nfdpathset_t* outPaths);

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static unsafe partial nfdresult_t NFD_SaveDialog(byte* filterList, byte* defaultPath, out IntPtr outPath);

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static unsafe partial nfdresult_t NFD_PickFolder(byte* defaultPath, out IntPtr outPath);

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static unsafe partial byte* NFD_GetError();

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static unsafe partial UIntPtr NFD_PathSet_GetCount(nfdpathset_t* pathSet);

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static unsafe partial byte* NFD_PathSet_GetPath(nfdpathset_t* pathSet, UIntPtr index);

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static unsafe partial void NFD_PathSet_Free(nfdpathset_t* pathSet);

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static partial void NFD_Dummy();

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static partial IntPtr NFD_Malloc(UIntPtr bytes);

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static partial void NFD_Free(IntPtr ptr);
    }
}