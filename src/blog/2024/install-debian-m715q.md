---
Title: Installing debian on a Lenovo ThinkCentre M715q 
Subtitle:
Date: 2024-08-31
Tags: linux, debian, lenovo
---

I recently purchased a [Lenovo ThinkCentre M715q](https://www.lenovo.com/us/en/p/desktops/thinkcentre/m-series-tiny/thinkcentre-m715q-tiny/11tc1mt715q) and wanted to install debian on it but after starting the installer the screen
was completely corrupted. [This forum post](https://forums.debian.net/viewtopic.php?t=155182) provided the solution. I
had to press `tab` and add `vga=normal fb=false` to the boot string. After that the installer was displayed normally.  