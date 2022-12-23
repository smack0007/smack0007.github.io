---
Title: XAudio2: Stop Currently Playing Sound
Date: 2011-07-02
Tags: .net, slimdx, xaudio2 
---

Using SlimDX, if you want to stop a sound which is currently playing via XAudio2, use the following methods on the SourceVoice object:

```c#
sourceVoice.Stop();
sourceVoice.FlushSourceBuffers();
```
