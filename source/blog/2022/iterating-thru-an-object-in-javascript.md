---
Title: Iterating thru an Object in JavaScript
Subtitle:
Date: 2022-04-06
Tags: javascript, typescript
---

For the longest time if I wanted to iterate thru an object
in JavaScript I would write code that looked something like
this:

```javascript
const obj = { a: "foo", b: "bar" };
for (const key of Object.keys(obj)) {
    console.info(key, obj[key]);
}
```

Since ES2017 there is a better way of doing this using `Object.entries()`
and destructuring:

```javascript
const obj = { a: "foo", b: "bar" };
for (const [ key, value ] of Object.entries(obj)) {
    console.info(key, value);
}
```

<!--more-->

This example feels more natural and really shines once the example gets
a little more complicated:

```javascript
const obj = {
    1: {
        name: "Joe",
        age: 12
    },
    2: {
        name: "Bob",
        age: 42
    },
};

// Before
for (const key of Object.keys(obj)) {
    const value = obj[key];
    console.info(`${key}.name`, value.name);
    console.info(`${key}.age`, value.age);
}

// After
for (const [ key, value ] of Object.entries(obj)) {
    console.info(`${key}.name`, value.name);
    console.info(`${key}.age`, value.age);
}
```

Being able to save the `const value = obj[key];` and having `[ key, value ]` in
the for loop in my oppinion makes it more clear that we are just interating thru the
object.
