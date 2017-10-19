---
Title: XAudio2: Stop Currently Playing Sound
Layout: Post
Permalink: 2011/07/02/xaudio2-stop-currently-playing-sound.html
Date: 2011-07-02
Category: .NET
Tags: SlimDX, XAudio2 
Comments: true
---

Using SlimDX, if you want to stop a sound which is currently playing via XAudio2, use the following methods on the SourceVoice object:

```c#
sourceVoice.Stop();
sourceVoice.FlushSourceBuffers();
```
