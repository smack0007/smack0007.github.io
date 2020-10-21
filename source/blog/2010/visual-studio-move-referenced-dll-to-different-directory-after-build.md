---
Title: Visual Studio - Move referenced DLL to different directory after Build
Date: 2010-07-20
Tags: .net, msbuild 
---

If you need to move a referenced DLL to a different directory after build, add these commands to the "**Post Build event command line**" box in the "**Build Events**" tab of the project properties:

```
mkdir $(TargetDir)dir
move $(TargetDir)myDLL.dll $(TargetDir)dir\myDLL.dll
```
