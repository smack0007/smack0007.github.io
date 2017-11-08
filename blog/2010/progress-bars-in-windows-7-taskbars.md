---
Title: Progress Bar in Windows 7 Taskbars
Layout: Post
Permalink: 2010/06/01/progress-bars-in-windows-7-taskbars.html
Date: 2010-06-01
Category: .NET
Tags: .NET, C#, Windows 7 
Comments: true
---

I decided to add progress bar to the Windows 7 Taskbar in my Timer app.

<a href="http://zacharysnow.net/wp-content/uploads/2010/06/TimerProgressBar.png"><img src="http://zacharysnow.net/wp-content/uploads/2010/06/TimerProgressBar.png" alt="" title="TimerProgressBar" width="139" height="40" class="alignnone size-full wp-image-309" /></a>

I started by downloading and compiling the [Windows API Code Pack](http://code.msdn.microsoft.com/WindowsAPICodePack) in Release mode. I then added a reference to the Microsoft.WindowsAPICodePack.dll and Microsoft.WindowsAPICodePack.Shell.dll files to the project. After that add the lines:

```c#
using Microsoft.WindowsAPICodePack.Taskbar;
```

to your using statements. When the clock starts running I create the progress bar in the taskbar with:

```c#
// Initialize progress bar
if(TaskbarManager.IsPlatformSupported)
{
	TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
	TaskbarManager.Instance.SetProgressValue(0, (int)this.totalTime.TotalSeconds, this.Handle);
}
```

to stop the progress bar:

```c#
// Stop progress bar
if(TaskbarManager.IsPlatformSupported)
	TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
```

and finally to update the progress bar on each tick:

```c#
// Update progress bar
if(TaskbarManager.IsPlatformSupported)
	TaskbarManager.Instance.SetProgressValue((int)this.totalTime.TotalSeconds - (int)this.time.TotalSeconds, (int)this.totalTime.TotalSeconds, this.Handle);
```

[Download Binary](/files/TimerWin7Taskbar.zip)
[Download Source](/files/TimerWin7TaskbarSrc.zip)
