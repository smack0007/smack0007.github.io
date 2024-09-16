---
Title: Debian 12: WiFi missing after installing update
Subtitle:
Date: 2024-09-16
Tags: debian, debian-12, wifi, network-manager
---

It finally happened to me. The other night after installing an update from [debain](https://www.debian.org/)
[bookworm](https://www.debian.org/releases/bookworm/) and restarting my WiFi was no longer working. On the desktop
there was no longer a signal strength indicator and my browser informed me I was disconnected from the internet.

So I plugged in an ethernet cable and got started researching. After 2 minutes of [searching](https://duckduckgo.com/) I
had the answer. I used `nmcli` (network manager cli) to see the state of my network devices. On my WiFi adapter there
was an error `plugin missing`. Simply reinstalling `network-manager` with `sudo apt reinstall network-manager` fixed the
problem and I could use my WiFi again.