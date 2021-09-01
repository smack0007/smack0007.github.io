---
Title: Use bash.exe to run bash scripts in Windows
Subtitle:
Date: 2021-09-01
Tags: windows, wsl, bash
---

If you have the Windows Subsystem for Linux (WSL) installed then you can use `wsl.exe` to start
any linux distribution you have installed from the command prompt. What I recently learned about
though is that there is a `bash.exe` as well which can be used to execute a bash script directly.

<!--more-->

```cmd
bash -li -c "./myscript.sh"
```

The `-li` flag starts the `bash` instance as a login shell and the `-c` flag is the command to execute.
In this case, execute our script directly.
