---
Title: Git Quick Push
Subtitle: 
Date: 2018-03-29
Category: git
Tags: git, batch
---

Here's a quick one liner to quickly stage all your changes in the current git repo, commit them and then push
the commit to origin master branch.

<!--more-->

```bat
git add -A && git commit -m %1 && git push origin master
```

The first argument to the script is the message to use for the commit.

Example usage: `push "Added Git Quick Push"`.