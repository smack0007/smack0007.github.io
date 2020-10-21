---
Title: WMI - Converting datetime values to DateTime objects
Date: 2011-07-12
Tags: .net, wmi 
---

Just a quick post to hopefully save anyone who finds this some time. If you need to convert a datetime value from a WMI query to a native DateTime object in C#, you can use the method ToDateTime() on the ManagementDateTimeConverter class.

```c#
ManagementDateTimeConverter.ToDateTime(dmtfDate);
```
