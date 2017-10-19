---
Title: Visual Studio: Move referenced DLL to different directory after Build
Layout: Post
Permalink: 2010/07/20/visual-studio-move-referenced-dll-to-different-directory-after-build.html
Date: 2010-07-20
Category: .NET
Tags: MSBuild 
Comments: true
---

If you need to move a referenced DLL to a different directory after build, add these commands to the "**Post Build event command line**" box in the "**Build Events**" tab of the project properties:

```
mkdir $(TargetDir)dir
move $(TargetDir)myDLL.dll $(TargetDir)dir\myDLL.dll
```
