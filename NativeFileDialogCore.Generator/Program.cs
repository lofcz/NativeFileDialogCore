﻿using System.Text.RegularExpressions;

namespace NativeFileDialogCore.Generator;

internal class Program
{
    public static void GenerateNativeFunctions32(string inputFilePath, string outputFilePath)
    {
        string content = File.ReadAllText(inputFilePath);
        
        Match nativeFunctionsMatch = Regex.Match(content, @"internal static partial class NativeFunctions\s*{[^}]*}",
            RegexOptions.Singleline);

        if (!nativeFunctionsMatch.Success)
        {
            throw new Exception("Nepodařilo se najít třídu NativeFunctions ve vstupním souboru.");
        }

        string nativeFunctionsContent = nativeFunctionsMatch.Value;
        
        string nativeFunctions32Content = nativeFunctionsContent
            .Replace("NativeFunctions", "NativeFunctions32")
            .Replace("\"nfd64\"", "\"nfd_x86\"");
        
        nativeFunctions32Content = nativeFunctions32Content.TrimEnd('}') + """
                                                                           
                                                                                   [LibraryImport(LibraryName)]
                                                                                   [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
                                                                                   internal static partial IntPtr NFD_Malloc(UIntPtr bytes);
                                                                               }
                                                                           """;
        string result = $$"""
                          // this file is generated by NativeFileDialogCore.Generator utility, do not edit manually
                          using System;
                          using System.Runtime.InteropServices;

                          namespace NativeFileDialogCore.Native
                          {
                              {{nativeFunctions32Content}}
                          }
                          """;

        File.WriteAllText(outputFilePath, result);
    }

    private static void Main(string[] args)
    {
        GenerateNativeFunctions32("../../../../NativeFileDialogCore/Native/NativeFunctions.cs", "../../../../NativeFileDialogCore/Native/NativeFunctions32.cs");
    }
}