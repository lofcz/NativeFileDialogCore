using System;

namespace NativeFileDialogCore.Native;

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
