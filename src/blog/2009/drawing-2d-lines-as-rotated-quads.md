---
Title: Drawing 2D Lines as Rotated Quads
Date: 2009-02-07
Tags: .net
---

I haven't had much time lately with work but one question I've seen asked many many times is how to draw lines of different widths. So, to cut to the chase I'll share the code I've used to do it.

```c#
public void DrawLine(Vector3 p1, Color c1, Vector3 p2, Color c2, int width)
{
	float distance = Vector3.Distance(p1, p2);
	float halfDistance = distance / 2.0f;
	float halfWidth = width / 2.0f;

	Vector3 difference = p2 - p1;
	Vector3 destination = new Vector3(p1.X + difference.X / 2.0f, p1.Y + difference.Y / 2.0f, p1.Z + difference.Z);

	// Calculate angle between two points
	float angle = (float)Math.Atan2(difference.Y, difference.X);

	Vector3 v1, v2, v3, v4;

	v1 = new Vector3(-halfDistance, -halfWidth, 0); // Top Left
	v2 = new Vector3(halfDistance, -halfWidth, 0); // Top Right
	v3 = new Vector3(halfDistance, halfWidth, 0); // Bottom Right
	v4 = new Vector3(-halfDistance, halfWidth, 0); // Bottom Left

	Matrix m =
		Matrix.Identity *
		Matrix.CreateRotationZ(angle) *
		Matrix.CreateTranslation(destination);

	v1 = Vector3.Transform(v1, m);
	v2 = Vector3.Transform(v2, m);
	v3 = Vector3.Transform(v3, m);
	v4 = Vector3.Transform(v4, m);

	DrawQuad(v1, c1, v2, c2, v3, c2, v4, c1);
}
```

I've left a lot of fluff code out. I usually check if the line is a width of 1 and draw a normal line. I also left out the code on how to draw a quad as that can be found many other places already.
