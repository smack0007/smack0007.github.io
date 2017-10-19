---
Title: Xna: Load Texture2D from Embedded Resource
Layout: Post
Permalink: 2010/07/03/xna-load-texture2d-from-embedded-resource.html
Date: 2010-07-03
Category: .NET
Tags: Embedded Resources 
Comments: true
---

If you're writing an app which uses Xna, you may need to load a texture from an embedded resource. Here's how:

First embed the resource in your app. Do so by choosing **Embedded Resource** as the **Build Action** in the properties of the resource.

<a href="http://zacharysnow.net/wp-content/uploads/2010/07/embed-resource.png"><img src="http://zacharysnow.net/wp-content/uploads/2010/07/embed-resource.png" alt="Properties Dialog for a File" title="embed-resource" width="281" height="175" class="alignnone size-full wp-image-339" /></a>

After that you can load the **Texture2D** using a stream handle to the embedded file.

```c#
Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream("AppNamespace.Folder.font.bmp");
return Texture2D.FromFile(graphicsDevice, stream);
```

**GetCallingAssembly()** can be exchanged with **GetExecutingAssembly()** if needed. The name of the resource must be fully qualified with the app's namespace and folders. I usually keep my resources in a folder **Resources** so I would have: AppNamespace.Resources.font.bmp.

