---
Title: Cherry picking only certain files from one branch to another 
Subtitle:
Date: 2025-05-21
Tags: git, bash
---

At work I had a branch with a large amount of updated VRT (Visual Regressen Tests) (.png) files and this made the review
process in GitLab near impossible. To make the Merge Request reviewable I wanted to rebase the branch into 2 commits. The
first one containing just the code chages and the second one containing all the updated VRT(s). 

<!--more-->

To get started I rebased the branch (FB1) on our develop branch and squashed all commits into a single commit. Then I created a 
second branch (FB2) and cherry picked all the code changes out of the first branch using the following command:

```shell
git show FB1 -- $(git diff --name-only FB1 | grep -v .png | tr '\n' ' ') | git apply -
```

Then I commited the changes to create the code only commit. After that I could rebase FB1 on FB2 in order to create the commit
containing just the VRTs.
