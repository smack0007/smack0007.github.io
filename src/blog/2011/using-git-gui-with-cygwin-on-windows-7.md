---
Title: Using git-gui with Cygwin on Windows 7
Date: 2011-04-07
Tags: cygwin, git, windows-7 
---

I've started using git via [cygwin](http://www.cygwin.com/) and was running into trouble trying to pin it to my taskbar in Windows 7.

First I created a .bat file in the c:\cygwin folder which launches the app standalone:

```
@@echo off

C:
chdir C:\cygwin\bin

start run.exe git gui
```

You can change paths accordingly. Now run the batch file and pin the program to the taskbar. You'll notice after you close the app, the icon changes and it won't launch again.

Right click on the shortcut while holding shift and choose properties. Change the target to the batch file we wrote. You can change the icon to the git-gui icon by pointing the shortcut icon to "C:\cygwin\usr\share\git-gui\lib\git-gui.ico".

Now if you click on the icon, the git-gui app should start up. Kill your explorer.exe in task manager and restart. If the icon is still the genie lamp, you'll need to clear your icon cache to get the icon to look right. Credit for that from [here](http://superuser.com/questions/72756/changing-windows-7-pinned-taskbar-icons). Kill your explorer.exe again and while explorer is gone, start cmd.exe. From there enter the following commands:

```
CD /d %userprofile%\AppData\Local

DEL IconCache.db /a

EXIT
```

After that your icon should be there as you want.

