---
Title: Xna: Load Texture2D from Embedded Resource
Date: 2010-07-03
Tags: .net, embedded-resources 
---

If you're writing an app which uses Xna, you may need to load a texture from an embedded resource. Here's how:

First embed the resource in your app. Do so by choosing **Embedded Resource** as the **Build Action** in the properties of the resource.

After that you can load the **Texture2D** using a stream handle to the embedded file.

```c#
Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream("AppNamespace.Folder.font.bmp");
return Texture2D.FromFile(graphicsDevice, stream);
```

**GetCallingAssembly()** can be exchanged with **GetExecutingAssembly()** if needed. The name of the resource must be fully qualified with the app's namespace and folders. I usually keep my resources in a folder **Resources** so I would have: AppNamespace.Resources.font.bmp.

