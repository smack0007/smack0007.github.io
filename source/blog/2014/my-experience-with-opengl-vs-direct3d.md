---
Title: My Experience with OpenGL vs Direct3D
Date: 2014-11-18
Tags: graphics, opengl, direct3D 
---

I've struggled a lot with myself over whether I should use OpenGL or Direct3D. There is no blanket answer to this question that anyone can tell you.
Both APIs have a very different feel and which one you prefer can only be decided on your own.

<!--more-->

Although Direct3D seems more structured and honestly makes more sense to me most of the time, I find the barrier to entry is much greater than that of OpenGL.
With OpenGL it's easy to find lot's of [great](http://www.opengl-tutorial.org/) [tutorials](http://www.arcsynthesis.org/gltut/) that help you get started, although
the there are some [outdated](http://nehe.gamedev.net) tutorials which teach you the old way of doing things. Sorting between the two of them is the hardest part. The modern
way of doing things is with buffers and shaders.

OpenGL of course has the advantage of being multiplatform but I would say don't let this influence your decision too much. I've had working with OpenGL on some kind of Linux
system on my list of things to do for some time and I still haven't gotten around to it. 95% of my programming is done on Windows and if things are the same for you then
it doesn't matter if Direct3D is only available on Windows.

OpenGL is a C based API which makes it easy to P/Invoke into from C#. You can use something like [OpenTK](http://www.opentk.com/) if you want, but for my needs I just P/Invoke.
I'm working on my own C# OpenGL API which I call [Samurai](https://github.com/smack0007/Samurai). It's not a 1 to 1 binding like OpenTK but an attempt at creating a higher
level API for OpenGL.
