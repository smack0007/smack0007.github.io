---
Title: Add the current time to your command prompt
Subtitle: 
Date: 2020-11-26
Tags: cmd
---

I am someone who never made the jump to Powershell and to this day still use my
cmd shell with pride. My cmd is very personalized at this point and is available
on [GitHub](https://github.com/smack0007/cmd) though I'm not sure how useful it is
for anyone but me. I use git and GitHub to synchronize my cmd between my different
machines.

<!--more-->

When you open cmd and have not personalized it all you are presented with a prompt
that looks like this:

```cmd
C:\Users\smack0007>_
```

The folder might be different of course. If you start typing the input will be placed
directly after the `>` symbol. The string `C:\Users\smack0007>` is what is known as the
prompt and this string can be customized by either using the `PROMPT` command or setting
the environment variable `PROMPT`. If you enter the command `PROMPT /?` a short description
will be displayed explaining which escape codes are available. The escape codes all start
with `$`.

The default PROMPT string is `$P$G`:

```cmd
C:\Users\smack0007>PROMPT $P$G

C:\Users\smack0007>_
```

A newline can be added to the PROMPT by using the escape code `$_`:

```cmd
C:\Users\smack0007>PROMPT $P$G$_

C:\Users\smack0007>
_
```

The time can be added by using the escape code `$T`. We'll insert the `$T` between
the `$P` and the `$G`. We'll also wrap the `$T` in brackets and add a space between
the time and the path:

```cmd
C:\Users\smack0007>PROMPT $P [$T]$G$_

C:\Users\smack0007 [10:01:53,36]>
_
```

The default format of the `$T` escape code is not the output I want to have. In this case
it would be nice to cut off the `,36`. The escape code `$H` will perform a backspace. We
can use 3 `$H` to remove the `,36`:

```cmd
C:\Users\zachary>PROMPT $P [$T$H$H$H]$G$_

C:\Users\zachary [10:05:27]>
_
```

If you close the cmd window and reopen the prompt would be reset. We can set the environment
variable `PROMPT` with the command `SETX`:

```cmd
SETX "PROMPT=$P [$T$H$H$H]$G$_"
```

Now your prompt will always contain the time.