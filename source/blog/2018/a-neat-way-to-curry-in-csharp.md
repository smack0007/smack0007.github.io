---
Title: A neat way to curry in C#
Date: 2018-05-25
Category: c#
Tags: c#, fp
---

I was reading an older blogpost from [Mike Hadlow](http://mikehadlow.blogspot.de/2015/09/partial-application-in-c.html) about
[Partial Application in C#](http://mikehadlow.blogspot.de/2015/09/partial-application-in-c.html) in which he discusses how
[Partial Application](https://en.wikipedia.org/wiki/Partial_application) can be implemented in C# via
[Currying](https://en.wikipedia.org/wiki/Currying). Although I appreciate his example of implementing currying via extsion
methods, the syntax is hideous. There is a suggestion in the comments though that I found to be a much better solution.

```c#
// Define a local function Add.
int Add(int a, int b) => a + b;

// Here we do the currying.
Func<int, int> add3 = (b) => Add(3, b);

// This will print 5.
Console.WriteLine(add3(2));

// Curry one more time so that we have
// a function that simply produces 5.
Func<int> five = () => add3(2);

// This will also print 5.
Console.WriteLine(five());
```



