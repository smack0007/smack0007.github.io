---
Title: Using the HP Laser Jet Pro MFP M26nw on DietPI via CUPS
Subtitle:
Date: 2026-07-10
Tags: hp-m26nw, dietpi, cups
---

I've got an old (2016) HP Laser Jet Pro MFP M26nw printer that I do my best to keep
printing even though more and more obstacles keep getting put in my way. In MacOS 27
the drivers for the printer will officially be marked as deprecated and should cease
to stop working in MacOS 28. We used to be able to print to from our old Samsung tablet
but that also stopped working when we replaced the tablet with an iPad. [CUPS](https://openprinting.github.io/cups/)
implements the AirPrint protocol though so switching to that fixes the tablet problem
as well.

<!--more-->

I've got a Raspberry Pi 3 running [DietPi](https://dietpi.com) and so I decided to hook the printer
up to that. After installing CUPS via the `dietpi-software` command I added the `dietpi` user
to the `lpadmin` group with the following command:

```shell
sudo usermod -a -G lpadmin dietpi
```

A Debian package for working with HP printers can be installed with:

```shell
sudo apt install hplip
```

And then after installing that package I could get the printer configured in CUPS with:

```shell
hp-setup -i
```

The `-i` flag runs the command in terminal mode only.
