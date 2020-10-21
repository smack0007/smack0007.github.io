---
Title: LINQ calls IEqualityComparer<T>.GetHashCode() before Equals()
Date: 2014-11-24
Tags: .net, linq
---

This is a problem that has bitten me more than a few times so I thought it was about time to write a blog post about it. It's one of those problems
that makes you scratch your head for a bit and then the light bulb goes on and you remember you've solved this one before. It occurs whenever you
use a LINQ extension method which takes an instance of IEqualityComaparer<T>.

<!--more-->

Here is our sample IEqualityComparer:

```c#
class MethodInfoComparer : IEqualityComparer<MethodInfo>
{		
	public bool Equals(MethodInfo x, MethodInfo y)
	{
		if (x.Name != y.Name)
			return false;

		if (x.ReturnType != y.ReturnType)
			return false;

		var xParameters = x.GetParameters();
		var yParameters = y.GetParameters();

		if (xParameters.Length != yParameters.Length)
			return false;

		for (int i = 0; i < xParameters.Length; i++)
		{
			if (xParameters[i].ParameterType != yParameters[i].ParameterType)
				return false;
		}

		return true;
	}

	public int GetHashCode(MethodInfo obj)
	{
		return obj.GetHashCode();
	}
}
```

Given two instances of MethodInfo, it attempts to compare them using the name, return type, and the parameter names and types. GetHashCode() calls
obj.GetHashCode() for simplicity.

Here is our LINQ statement:

```c#
return this.GetType().GetMethods().Except(typeof(BaseClass).GetMethods(), new MethodInfoComparer());
```

The LINQ statment should return only the public methods of the derived class and filter out those of the base class. It doesn't work though. Attempting to debug the problem
by setting a breakpoint inside of the Equals method reveals the method is never called. This is the part where you usually have to scratch your head for about 2 or 3 minutes.

The problem is in the implementation of the GetHashCode method. LINQ calls the GetHashCode method and compares the results from both objects before calling the Equals method.
This is an optimization as GetHashCode should be fast where as Equals may not be. From MSDN:

> A hash function is used to _quickly_ generate a number (hash code) that corresponds to the value of an object.

MethodInfo doesn't seem to implement GetHashCode based on any values of the instance. Comparing the hash codes of 2 MethodInfo objects which actually represent
the some method always fails and therefore the Equals method is never called.

A better implementation of GetHashCode for this use case is:

```c#
public int GetHashCode(MethodInfo obj)
{
	return obj.Name.GetHashCode() ^ obj.GetParameters().Length;
}
```

obj.GetParameters().Length could probably be left out. I haven't ran any tests to determine if it actually improves the performance or not.
