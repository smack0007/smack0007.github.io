---
Title: Install Linux Mint Cinnamon Themes on Debian
Subtitle:
Date: 2024-08-23
Tags: linux, debian, linux-mint, cinnamon
---

As a long time Windows user it probably is no surprise that I enjoy using the [Cinnamon](https://projects.linuxmint.com/cinnamon/) Desktop
Environment. Although I have used and enjoyed using [Linux Mint](https://linuxmint.com/) one goal I had when I started replacing Windows
throughout my house was to be the same distribution on differnt machines. So I settled on installing [Debian](https://www.debian.org/)
everywhere and customizing it to each systems individual needs.

I wanted to run [Cinnamon](https://projects.linuxmint.com/cinnamon/) on my Thinkpad Laptop and [Debian](https://www.debian.org/) can install
it out of the box. The only problem is the defaykt theme leaves a lot to be deisred. Luckily the theme from [Linux Mint](https://linuxmint.com/)
can installed using a few commands from the terminal.  

<!--more-->

```shell
cd ~/Downloads # Could also be /tmp

git clone --depth=1 --branch 2.1.8 git@github.com:linuxmint/mint-themes.git
cd mint-themes
sudo apt install -y pysassc # Needed by generate.py
./generate.py
sudo cp -r ./usr/share/themes/* /usr/share/themes
cd ..
rm -rf mint-themes

git clone --depth=1 --branch 1.7.1 git@github.com:linuxmint/mint-x-icons.git
cd mint-x-icons
sudo cp -r ./usr/share/icons/* /usr/share/icons
cd ..
rm -rf mint-x-icosn

git clone --depth=1 --branch 1.7.7 git@github.com:linuxmint/mint-y-icons.git
cd mint-y-icons
sudo cp -r ./usr/share/icons/* /usr/share/icons
cd ..
rm -rf mint-y-icons

git clone --depth=1 git@github.com:linuxmint/mint-cursor-themes.git
cd mint-cursor-theme
sudo cp -r ./usr/share/icons/* /usr/share/icons
cd ..
rm -rf mint-cursor-themes
```

After running these commands the [Linux Mint](https://linuxmint.com/) will be available for selection in the Themes application. The current versions
of the individual repositories can be checked on GitHub.