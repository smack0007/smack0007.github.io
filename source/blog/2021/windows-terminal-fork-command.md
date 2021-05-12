---
Title: Windows Terminal fork command
Subtitle: 
Date: 2021-05-12
Tags: windows-terminal, powershell
---

I wanted a way to [fork](https://man7.org/linux/man-pages/man2/fork.2.html) the current tab in [Windows Terminal](https://github.com/microsoft/terminal) and
so I've come up with this PowerShell function:

```shell
function fork { wt -w 0 -d "$(Get-Location)" }
```
