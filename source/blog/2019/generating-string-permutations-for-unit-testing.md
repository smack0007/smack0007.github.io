---
Title: Generating string permutations for unit testing
Subtitle: 
Date: 2019-02-11
Category: c#
Tags: c#, nunit, math
---

I needed to parse a string containing 6 characters. Each character should only
be one of 3 possibilities: '?' for null, '0' for false or '1' for true. The problem
sounded easy enough to generate a whole bunch of unit tests for.

<!--more-->

I'm wrapping the string in a struct and simply reading the values for any given field.
That class looks sort of like this:

```c#
public struct MyData
{
    public bool? Field1 { get; }

    public bool? Field2 { get; }

    public bool? Field3 { get; }

    public bool? Field4 { get; }

    public bool? Field5 { get; }

    public bool? Field6 { get; }
}
```

I wanted to generate unit tests for every possibly permuation of the string. We're using
NUnit so this can be acheived using TestCaseSource:

```c#
public IEnumerable<object[]> TestValues()
{
    return
        from f1 in FieldValues()
        from f2 in FieldValues()
        from f3 in FieldValues()
        from f4 in FieldValues()
        from f5 in FieldValues()
        from f6 in FieldValues()
        select new object[]
        {
            new string(new char[] { f1.Key, f2.Key, f3.Key, f4.Key, f5.Key, f6.Key }),
            f1.Value,
            f2.Value,
            f3.Value,
            f4.Value,
            f5.Value,
            f6.Value
        };
}

public IEnumerable<KeyValuePair<char, bool?>> FieldValues()
{
    yield return new KeyValuePair<char, bool?>('?', null);
    yield return new KeyValuePair<char, bool?>('0', false);
    yield return new KeyValuePair<char, bool?>('1', true);
}

[TestCaseSource(nameof(TestValues))]
public void ValuesAreCorrect(
    string value,
    bool? f1,
    bool? f2,
    bool? f3,
    bool? f4,
    bool? f5,
    bool? f6)
{
    var data = new MyData(value);
    Assert.AreEqual(data.Field1, f1);
    Assert.AreEqual(data.Field2, f2);
    Assert.AreEqual(data.Field3, f3);
    Assert.AreEqual(data.Field4, f4);
    Assert.AreEqual(data.Field5, f5);
    Assert.AreEqual(data.Field6, f6);
}
```

This geneartes 721 unit tests which is the correct number of tests according to
the formula [___n<sup>k</sup>___](https://www.mathsisfun.com/combinatorics/combinations-permutations.html).