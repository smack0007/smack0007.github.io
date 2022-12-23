---
Title: OpenTK - Simple Movable Sprite
Date: 2011-03-29
Tags: .net, opengl 
---

I wrote my second OpenTK app. This time I'm drawing a sprite which you can move around the screen using the keyboard. I've included the source code after the jump or you can [download it](/files/OpenTKApp2.zip).

<!--more-->

```c#
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace OpenTKApp2
{
	public class AppWindow : GameWindow
	{
		int texture;
		float x, y;

		public AppWindow()
		{
			this.Title = "OpenTK App 2";
			this.WindowBorder = WindowBorder.Fixed;
			this.ClientSize = new Size(800, 600);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			
			GL.ClearColor(Color.CornflowerBlue);
			GL.Ortho(0, 800, 600, 0, -1, 1);
			GL.Viewport(0, 0, 800, 600);

			GL.Enable(EnableCap.Texture2D);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

			GL.GenTextures(1, out this.texture);
			GL.BindTexture(TextureTarget.Texture2D, this.texture);

			Bitmap bitmap = new Bitmap("ship.png");
			bitmap.MakeTransparent(Color.Magenta);

			BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
				ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
				OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

			bitmap.UnlockBits(data);
			bitmap.Dispose();

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

			this.x = 40f;
			this.y = 41.5f;
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame(e);

			float dx = 0.0f;
			float dy = 0.0f;

			if(this.Keyboard[Key.Left])
				dx = -1.0f;
			else if(this.Keyboard[Key.Right])
				dx = 1.0f;

			if(this.Keyboard[Key.Up])
				dy = -1.0f;
			else if(this.Keyboard[Key.Down])
				dy = 1.0f;

			this.x += 100.0f * dx * (float)e.Time;
			this.y += 100.0f * dy * (float)e.Time;
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);
						
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();

			GL.Begin(BeginMode.Quads);
			GL.TexCoord2(0, 0);
			GL.Vertex2(this.x - 40.0f, this.y - 41.5f);
			GL.TexCoord2(1, 0);
			GL.Vertex2(this.x + 40.0f, this.y - 41.5f);
			GL.TexCoord2(1, 1);
			GL.Vertex2(this.x + 40.0f, this.y + 41.5f);
			GL.TexCoord2(0, 1);
			GL.Vertex2(this.x - 40.0f, this.y + 41.5f);
			GL.End();

			GL.Flush();
			this.SwapBuffers();
		}

		[STAThread]
		public static void Main()
		{
			AppWindow window = new AppWindow();
			window.Run(60);
		}
	}
}
```

[Download Visual Studio Project](/files/OpenTKApp2.zip)
