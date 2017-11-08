---
Title: Zune Playlist / SMIL Copier
Layout: Post
Permalink: 2011/06/27/zune-playlist-smil-copier.html
Date: 2011-06-27
Category: .NET
Tags: .NET, Playlist, SMIL, Windows Media Player, Zune 
Comments: true
---

I needed a tool that would copy the contents of a Zune playlist to a given directory. I did a bit of googling and I couldn't find anything, so like any good nerd I wrote a tool that did.

<a href="/images/SMILCopier.png"><img alt="SMILCopier" src="/images/SMILCopier.png" title="SMILCopier" class="alignnone" width="411" height="167" /></a>

The format of Zune playlists is a simple XML format known as [SMIL](http://www.w3.org/AudioVideo/). I think Windows Media Player also stores playlists in this format but I haven't confirmed that yet.

I wrote the tool very quickly and have only tested it on my machine with my test data, but I'll provide the source if you'd like to modify for yourself. The source is C#.

[Download Executable](/files/SMILCopier.zip)
[Download Source](/files/SMILCopier-src.zip)
