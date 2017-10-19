---
Title: Calculating an angle from a Vector2
Layout: Post
Permalink: 2009/02/28/calculating-an-angle-from-a-vector2.html
Date: 2009-02-28
Category: .NET
Tags: Vectors, Xna 
Comments: true
---

When you need to calculate an angle from a Vector2 structure, you can use this piece of code:

```c#
public static class Vector2Helper
{
	public static float CalculateAngle(Vector2 v)
	{
		float angle = 0.0f;

		if(v != Vector2.Zero)
		{
			v.Normalize();

			angle = (float)Math.Acos(v.Y);

			if(v.X &lt; 0.0f)
			angle = -angle;
		}

		return angle;
	}
}
```

I used this to calculate an angle from the Vector2 of the Left Stick.

The original credit for this source code comes from [here](http://xnagamer.spaces.live.com/blog/cns!EC20BAAE6808B682!139.entry).
