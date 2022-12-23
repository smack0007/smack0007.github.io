---
Title: Powershell time command
Subtitle:
Date: 2022-03-10
Tags: powershell
---

I commbined the best parts of this [StackOverflow answer](https://stackoverflow.com/a/4801509/26566) and this
[Super User answer](https://superuser.com/a/1289962/3465) to come up with this solution for a `time` function
in Powershell:

```powershell
function time {
    $Command = "$args";
    (Measure-Command { Invoke-Expression $Command 2>&1 | Out-Default }).ToString();
}
```

Example usage:

```cmd
# time git pull
00:00:01.0804938
```
