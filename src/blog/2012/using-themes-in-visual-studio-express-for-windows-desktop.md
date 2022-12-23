---
Title: Using Themes in Visual Studio Express for Windows Desktop
Date: 2012-09-15
Tags: vs, vs2012
---

I just want to post some information that took me while to find. Check out the original article [here](http://alinconstantin.blogspot.de/2012/09/using-color-themes-with-visual-studio.html) with pictures and more information.

<blockquote>
1) First, download the zip file http://www.alinconstantin.net/download/VS2012Themes.zip – it contains the 7 pkgdef files defining the colors of the default themes from Matt’s extension.

2) Now, create a folder under "%ProgramFiles%\Microsoft Visual Studio 11.0\Common7\IDE\WDExpressExtensions”, and lets name it “Themes”. Unpack the zip file in that folder.

3) Open a ‘Developer Command Prompt for VS2012” window. In the command line, type “wdexpress.exe /updateconfiguration”. This will make Visual Studio to read the pkgdef files on next restart, and import the color themes into registry.

4) Launch Visual Studio Express, and now you should be able to see the new themes and switch them in Tools/Options dialog, Environment/General tab
</blockquote>

[Source](http://alinconstantin.blogspot.de/2012/09/using-color-themes-with-visual-studio.html)
