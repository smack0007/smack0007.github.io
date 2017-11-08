---
Title: Keeping SplitContainer SplitterDistance consistent
Layout: Post
Permalink: 2011/02/09/keeping-splitcontainer-splitterdistance-consistent.html
Date: 2011-02-09
Category: .NET
Tags: Split Container, WinForms 
Comments: true
---

If you're having trouble keeping the SplitterDistance property of a SplitContainer consistent across app sessions, you can set the FixedPanel property of the splitter to FixedPanel.Panel1.

```c#
splitter.FixedPanel = FixedPanel.Panel1;
```

I guess this could also work with FixedPanel.Panel2 as well but I haven't given it a try. Credit [this stackoverflow post](http://stackoverflow.com/questions/129362/restoring-splitterdistance-inside-tabcontrol-is-inconsistent).
