---
Title: Snowball now using SharpDX
Layout: Post
Permalink: 2012/04/16/snowball-now-using-sharpdx.html
Date: 2012-04-16
Category: .NET
Tags: SharpDX, Open Source, SlimDX 
Comments: true
---

For a little while, I was thinking about giving up on Snowball. When you're one guy working on a project that gets to a certain size, it can start to feel a little daunting. You find a bug, and you feel like you need to fix it asap. I don't know if anyone reading this has actually tried Snowball, but if you have, please comment to let me know. It would encourage me.

I decided to switch Snowball over to [SharpDX](http://code.google.com/p/sharpdx/). It's not that I was unhappy with SlimDX, it just seems like there is a lot more innovation happening on the SharpDX side. I also like the fact that I can include the DLLs in the repository so end users don't have to download another dependency in order to compile it. The Win8 stuff is also quite interesting, although the SlimDX guys say they are working on that.

I plan to set a road map soon for what I want to include the first release of Snowball. Music and Pixel Shaders are high the list. I've experimented with implementing a UI library but I think I want to push that back for a later release.
