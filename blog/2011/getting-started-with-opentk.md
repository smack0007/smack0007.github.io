---
Title: Getting started with OpenTK
Layout: Post
Permalink: 2011/03/29/getting-started-with-opentk.html
Date: 2011-03-29
Category: .NET
Tags: OpenGL 
Comments: true
---

I started experimenting with [OpenTK](http://www.opentk.com) and I had to look in a few places to put this code together, so I'm posting it here for anyone who might be looking for an easy getting started lesson.

I've set up a window similar to what I've been used to in Xna (CornflowerBlue 4 life). I've also set up a 2D projection matrix and drawn a triangle in a 2D fashion. You'll need to add a reference to the OpenTK assembly for your project in Visual Studio.

```c#
using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenTKApp1
{
	public class AppWindow : GameWindow
	{
		public AppWindow()
		{
			this.Title = "OpenTK App 1";
			this.WindowBorder = WindowBorder.Fixed;
			this.ClientSize = new Size(800, 600);			
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);

			GL.ClearColor(Color.CornflowerBlue);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0, 800, 600, 0, -1, 1);
			GL.Viewport(0, 0, 800, 600);
			
			GL.Begin(BeginMode.Triangles);
			GL.Color3(Color.Red);
			GL.Vertex3(400, 150, 0);
			GL.Color3(Color.Green);
			GL.Vertex3(600, 450, 0);
			GL.Color3(Color.Blue);
			GL.Vertex3(200, 450, 0);
			GL.End();

			GL.Flush();
			this.SwapBuffers();
		}

		[STAThread]
		public static void Main()
		{
			AppWindow window = new AppWindow();
			window.Run();
		}
	}
}
```
