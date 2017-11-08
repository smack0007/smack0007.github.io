---
Title: WMI: Converting datetime values to DateTime objects
Layout: Post
Permalink: 2011/07/12/wmi-converting-datetime-values-to-datetime-objects.html
Date: 2011-07-12
Category: .NET
Tags: WMI 
Comments: true
---

Just a quick post to hopefully save anyone who finds this some time. If you need to convert a datetime value from a WMI query to a native DateTime object in C#, you can use the method ToDateTime() on the ManagementDateTimeConverter class.

```c#
ManagementDateTimeConverter.ToDateTime(dmtfDate);
```
