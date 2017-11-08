---
Title: Drawing Rectangles with SpriteBatch
Layout: Post
Permalink: 2010/03/29/drawing-rectangles-with-spritebatch.html
Date: 2010-03-29
Category: .NET
Tags: C#, Code Snippets, Xna 
Comments: true
---

Just a quick code snippet which adds an extension method for drawing Rectangles to SpriteBatch:

```c#
public static class SpriteBatchHelper
{
	static Texture2D pixel;

	private static void LoadPixel(GraphicsDevice graphicsDevice)
	{
		if(pixel == null)
		{
			pixel = new Texture2D(graphicsDevice, 1, 1);
			pixel.SetData&lt;Color&gt;(new Color[] { Color.White });
		}
	}

	public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
	{
		LoadPixel(spriteBatch.GraphicsDevice);
		spriteBatch.Draw(pixel, rectangle, color);
	}
}
```
