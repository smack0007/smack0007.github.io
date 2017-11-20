---
Title: Snowball: Now based on shaders
Layout: Post
Permalink: 2012/06/26/snowball-now-based-on-shaders.html
Date: 2012-06-26
Category: .NET
Tags: Shaders, Game Programming 
Comments: true
---

I've now merged the "Shaders" branch back into the "master" branch. All rendering is now based on shaders and no longer on the fixed function pipeline.

The function of the Renderer class was essentially been reduced to pushing data to the GPU and therefore I decided to rename the class to GraphicsBatch. The Begin() overload which would allow you to specify RendererSettings has been removed and been replaced with an overload which allows you to specify an Effect file to use. Also, the DrawLine() method has been removed, although vertical and horizontal lines can still be drawn using the DrawFilledRectangle() method. Better line drawing should be possible through shaders and I hope to eventually make a sample which provides an example.

I've added a sample (pictured above) which demonstrates using a custom shader. By default, GraphicsBatch uses a BasicEffect class which is basically the old way of rendering implemented in a shader. 

In order for shaders to work properly when using GraphicsBatch, the GraphicsBatch class must pass a few parameters to the shader. At the moment, this only includes a transform matrix but may include more parameters in the future. The GraphicsBatchEffectWrapper can be used to wrap custom effects which you write in order to work with GraphicsBatch correctly. GraphicsBatchEffectWrapper will pass the parameters to your shader by following a standard naming convention. For example, the transform matrix is passed to your shader through a parameter named "TransformMatrix".  You can also write you own class which implements IGraphicsBatchEffect. See the sample for an example of using the wrapper class.
