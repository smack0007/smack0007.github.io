---
Title: Disable C# null checks for a field or property
Subtitle: 
Date: 2020-01-02
Category: c#
Tags: c#, nrt, nullable
---

The new nullable reference types in C# are great but sometimes you might need to disable it for a
single field or property. You can do that by assigning the value to `null` and using the `!` operator.

<!--more-->

```c#
public class Renderer
{
    private bool _drawInProgress = false;
    private Surface _drawTarget = null!;

    public Renderer()
    {
    }

    public void BeginDraw(Surface surface)
    {
        if (_drawInProgress)
            throw new InvalidOperationException("Draw currently in progress.");

        _drawInProgress = true;
        _drawTarget = surface;
    }

    private void EnsureDrawInProgress()
    {
        if (!_drawInProgress)
            throw new InvalidOperationException("Draw not currently in progress.");
    }

    public void EndDraw()
    {
        EnsureDrawInProgress();

        _drawInProgress = false;
        _drawTarget = null!;
    }

    public void DrawCaret()
    {
        EnsureDrawInProgress();
        Console.WriteLine("^");
    }
}
```

`_drawTarget` will not be null if `_drawInProgress` is true therefore I don't want to have to check
it in every Draw method. Using this trick the null checks are only disabled for the `_drawTarget` field
and not for the entire file.
