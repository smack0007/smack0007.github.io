---
Title: Filter ItemGroup by extension in MSBuild
Subtitle: 
Date: 2020-11-04
Tags: msbuild
---

Just a quick snippet that shows how to filter an exiting ItemGroup by an extension.

<!--more-->

```xml
<ItemGroup>
    <FilteredItems Include="@(None)" Condition="'%(Extension)' == '.ext'" />
</ItemGroup>
```

None is the name of the ItemGroup you'd like to filter and you need to change the extension
to the file extension you are looking for.
