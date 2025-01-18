using System;
using System.Runtime.InteropServices;

namespace NativeFileDialogCore.Native
{
    internal static partial class NativeFunctions
    {
        private const string LibraryName = "nfd64";

        [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static partial nfdresult_t NFD_OpenDialog(string? filterList, string? defaultPath, out IntPtr outPath);

        [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static partial nfdresult_t NFD_OpenDialogEx(string? filterList, string? defaultPath, string? dialogTitle, string? fileNameLabel, string? selectButtonLabel, string? cancelButtonLabel, IntPtr parentWindow, out IntPtr outPath);

        [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static partial nfdresult_t NFD_OpenDialogMultiple(string? filterList, string? defaultPath, out nfdpathset_t outPaths);

        [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static partial nfdresult_t NFD_SaveDialog(string? filterList, string? defaultPath, out IntPtr outPath);

        [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static partial nfdresult_t NFD_PickFolder(string? defaultPath, out IntPtr outPath);

        [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static partial string NFD_GetError();

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static partial UIntPtr NFD_PathSet_GetCount(in nfdpathset_t pathSet);

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static partial IntPtr NFD_PathSet_GetPath(in nfdpathset_t pathSet, UIntPtr index);

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static partial void NFD_PathSet_Free(ref nfdpathset_t pathSet);

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        public static partial void NFD_Dummy();

        [LibraryImport(LibraryName)]
        [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
        internal static partial void NFD_Free(IntPtr ptr);
    }
}