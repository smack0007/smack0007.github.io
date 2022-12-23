---
Title: NativeLibraryLoader for Assembly with ModuleInitializer
Subtitle: 
Date: 2020-12-18
Tags: c#, c#9
---

In C# 9 the [`ModuleInitializer`](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-9#support-for-code-generators)
atrribute was introduced which makes it easy to implement loading of native assemblies in your library.

<!--more-->

```c#
internal static class NativeLibraryLoader
{
    [ModuleInitializer]
    public static void Initialize()
    {
        NativeLibrary.SetDllImportResolver(typeof(NativeLibraryLoader).Assembly, ResolveDllImport);
    }

    private static IntPtr ResolveDllImport(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        // Implement all your assembly loading logic here.
        
        if (libraryName == GL.LibraryName)
        {
            return NativeLibrary.Load("opengl32");
        }

        return IntPtr.Zero;
    }
}
```