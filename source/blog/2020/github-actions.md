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

