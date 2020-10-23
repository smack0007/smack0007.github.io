---
Title: Enum Extension Methods
Subtitle: 
Date: 2020-07-17
Category: c#
Tags: .net, c#, enum, extension-methods
---

Did you know you can create extension methods for enums?

<!--more-->

```c#
public enum Suit { Diamonds, Hearts, Clubs, Spades }

public static class SuitExtensions
{
    public static string AsSymbol(this Suit suit) => suit switch
    {
        Suit.Diamonds => "\u2666",
        Suit.Hearts => "\u2665",
        Suit.Clubs => "\u2663",
        Suit.Spades => "\u2660",
        _ => ""
    };
}
```

You'll need the extra static class and I usually just
pack the class and the enum in the same file. It feels
like adding methods to your enum values. You can call
the extension method directly on the enum value:

```c#
Console.WriteLine(Suit.Diamonds.AsSymbol());
Console.WriteLine(Suit.Hearts.AsSymbol());
Console.WriteLine(Suit.Clubs.AsSymbol());
Console.WriteLine(Suit.Spades.AsSymbol());
```

To see the output properly you'll need to have a terminal
that properly undertstands unicode output like 
[Windows Terminal](https://github.com/microsoft/terminal).

