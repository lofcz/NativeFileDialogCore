# NativeFileDialog.Core

.NET Core 8+ bindings for [nativefiledialog](https://github.com/lofcz/nativefiledialog). The successor of [NativeFileDialogSharp](https://github.com/milleniumbug/nativefiledialogsharp) for modern runtime.

Supported operating systems:

- Windows x86
- Windows x86-64
- Linux x86-64
- macOS x86-64

Features added in NativeFileDialog.Core:

- Compile-time marshalling of native functions via `LibraryImport`.
- Reduced allocations, using `MemoryPool` internally.
- Support for named filters.
- Label customization in dialogs (confirm/cancel buttons, title..)
- Dialogs can be attached to any window.
