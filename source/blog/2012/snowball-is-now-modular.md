---
Title: Snowball is now modular
Date: 2012-11-06
Tags: .net, game-programming 
---

The version of Snowball currently on GitHub under the "develop" branch has been split into multiple projects. There is now an assembly for each major piece of Snowball, such as Graphics, Input, Sound. Although this means having to reference more assemblies, the amount of code your project depends on is now smaller. This also makes code maintenance a bit easier as it's more clear now what parts of the library depend on other parts of the library.

The parts of the library which really make up a Game Framework has also been split out into their own library. This allows for using Snowball as a just a simple set of libraries or a full blown game framework, depending on what your situation calls for.
