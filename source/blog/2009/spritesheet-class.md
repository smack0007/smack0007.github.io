---
Title: SpriteSheet Class
Date: 2009-01-09
Tags: .NET, SpriteSheet, XNA
---

I've been talking with a guy on the creator forums lately about SpriteSheets and so I decided it might be a good idea to post my SpriteSheet class.

It's very simple. Only reads sprites from left to right and assumes all Sprites are the same width and height.

```csharp
#region Using
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Snow.Xna.Graphics
{
	/// <summary>
	/// Spritesheet class.
	/// </summary>
	public class SpriteSheet
	{
		#region Fields

		string name;

		Texture2D texture;

		Rectangle[] rectangles;

		int spriteWidth, spriteHeight;

		#endregion

		#region Properties

		/// <summary>
		/// The name of this SpriteSheet.
		/// </summary>
		public string Name
		{
			get { return name; }
		}

		/// <summary>
		/// The texture for this SpriteSheet.
		/// </summary>
		public Texture2D Texture
		{
			get { return texture; }
		}

		/// <summary>
		/// Returns a rectangle for a sprite in the SpriteSheet.
		/// </summary>
		/// <param name="i">index</param>
		/// <returns></returns>
		public Rectangle this[int i]
		{
			get { return rectangles[i]; }
		}

		/// <summary>
		/// The number of sprites in this SpriteSheet.
		/// </summary>
		public int Count
		{
			get { return rectangles.Length; }
		}

		/// <summary>
		/// The width of the texture.
		/// </summary>
		public int Width
		{
			get { return texture.Width; }
		}

		/// <summary>
		/// The width of each sprite in the SpriteSheet.
		/// </summary>
		public int SpriteWidth
		{
			get { return spriteWidth; }
		}

		/// <summary>
		/// The height of the texture.
		/// </summary>
		public int Height
		{
			get { return texture.Height; }
		}

		/// <summary>
		/// The height of each sprite in the SpriteSheet.
		/// </summary>
		public int SpriteHeight
		{
			get { return spriteHeight; }
		}

		#endregion

		/// <summary>
		/// Create a new SpriteSheet and determine the number of sprites in the sheet.
		/// </summary>
		/// <param name="texture"></param>
		/// <param name="spriteWidth">Width of each sprite.</param>
		/// <param name="spriteHeight">Height of each sprite.</param>
		public SpriteSheet(string name, Texture2D texture, int spriteWidth, int spriteHeight)
			: this(name, texture, spriteWidth, spriteHeight, 0)
		{
		}

		/// <summary>
		/// Create a new SpriteSheet.
		/// </summary>
		/// <param name="texture"></param>
		/// <param name="spriteWidth">Width of each sprite.</param>
		/// <param name="spriteHeight">Height of each sprite.</param>
		/// <param name="count">The number of sprites in the sheet.</param>
		public SpriteSheet(string name, Texture2D texture, int spriteWidth, int spriteHeight, int count)
		{
			this.name = name;
			this.texture = texture;
			this.spriteWidth = spriteWidth;
			this.spriteHeight = spriteHeight;

			if(count == 0)
			{
				int numX = texture.Width / spriteWidth;
				int numY = texture.Height / spriteHeight;

				rectangles = new Rectangle[numX * numY];
			}
			else
			{
				rectangles = new Rectangle[count];
			}

			int x = 0, y = 0;
			for(int i = 0; i < rectangles.Length; i++)
			{
				rectangles[i] = new Rectangle(x, y, spriteWidth, spriteHeight);

				x += spriteWidth;
				if(x >= texture.Width)
				{
					x = 0;
					y += spriteHeight;
				}
			}
		}

		public static implicit operator Texture2D(SpriteSheet spriteSheet)
		{
			return spriteSheet.Texture;
		}
	}
}
```

You can create a new SpriteSheet and use it like this:

`
```c#
SpriteSheet spriteSheet = new SpriteSheet("tiles", Content.Load<texture2D>("tiles"), 64, 64);

spriteBatch.Begin();

spriteBatch.Draw(spriteSheet,
		        new Rectangle(0, 0, spriteSheet.SpriteWidth, spriteSheet.SpriteHeight),
			spriteSheet[0],
			Color.White);

spriteBatch.End();
```
`

Which loads a spritesheet with sprites of size 64x64. It then draws the first Sprite in the SpriteSheet. You of course wouldn't want to load the spritesheet every frame as well.

Feel free to use this code without restriction.

**Edit:** I copied and pasted the second piece of code from somewhere else so I fixed two typos.
