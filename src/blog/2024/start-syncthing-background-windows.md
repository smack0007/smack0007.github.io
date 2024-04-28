---
Title: Start Syncthing in the Background on Windows
Subtitle:
Date: 2024-04-28
Tags: syncthing, windows
---

Just a quick tip for myself (and anyone who is reading this) for the future. To start [syncthing](https://syncthing.net/) in
the background on Windows open up the startup apps folder via the run command (Win+R) with `shell:startup` amd then create
the following shortcut:

```shell
powershell.exe -c "Start-Process -FilePath 'syncthing' -ArgumentList '--no-browser' -WindowStyle Hidden"
```

This starts syncthing in the background after a short lived popup opens. `--no-console` doesn't seem to work for whatever reason.
