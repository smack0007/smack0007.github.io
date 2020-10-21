---
Title: Dynamically loaded DLL thows AccessViolationException on first function call
Subtitle: 
Date: 2019-10-22
Tags: .net, c#
---

In my library [GLESDotNet](https://github.com/smack0007/GLESDotNet) I load
the `libegl.dll` and `libglesv2.dll` DLLs dynamically via the Win32 functions
[LoadLibrary](https://docs.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-loadlibraryw)
and [GetProcAddress](https://docs.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-getprocaddress).
I already had everything working via `DLLImport` but I wanted to be able to
load the DLLs from different subdirectories depending on the architecture.

For the  first test I only loaded the `libegl.dll` DLL before moving on to
the `libglesv2.dll`. I made the classic programmer mistake of changing too much
at one time. The first call to any function in `libegl.dll` resulted in an
`AccessViolationException`. I couldn't figure out what was going wrong so I
decided to revert my changes and try again.

I noticed that when the DLLs were in the same directory the
`AccessViolationException` went away. Changing the working directory
to the directory of the DLLs also solved the problem once the DLLs
were placed in subdirectories again. This led me to believe that
loading `libegl.dll` must be implicitly loading `libglesv2.dll`. Loading
`libglesv2.dll` via `LoadLibrary` along with `libegl.dll` solved the problem.

