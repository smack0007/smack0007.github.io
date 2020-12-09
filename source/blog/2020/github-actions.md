---
Title: GitHub Actions
Subtitle: 
Date: 2020-12-06
Tags: github 
---

My blog is now being generated via GitHub actions in `ubuntu-latest`. This allows me to rebuild my
blog everytime there is a pushed commit.

<!--more-->

The Build step is as follows:

```yml
- name: Build
  run: |
    git clone https://github.com/smack0007/smack0007.github.io.git -b main bin
    cd compiler
    dotnet run -p compiler.csproj ../source ../bin
    cd ..
```

The output of my blog is stored in the `main` branch and the soruce for the blog
is stored in the `source` branch. The `source` branch is a
[detatched head](https://www.git-tower.com/learn/git/faq/detached-head-when-checkout-commit/).

The output of the blog will all be placed in `bin` directory which is where the `main`
branch is checked out. After the blog has been recompiled I can use git to push any changes
if there are any.

```yml
- name: Publish
  run: |
    git log -1 --pretty=%B > commit.msg
    cd bin
    git config --global user.name "smack0007"
    git config --global user.email "zachary.snow@gmail.com"
    git add -A && git commit -F "../commit.msg"
    git push https://smack0007:${{ secrets.API_KEY }}@github.com/smack0007/smack0007.github.io.git
```

First store the commit message from the `source` branch in `commit.msg`. We'll use that for the commit
message in the later step. Then we change directory into the `bin` directory of the `main` branch git
repository. We then configure `user.name` and `user.email` and then make the commit to the `main` branch.
After that we push using an API key from GitHub.