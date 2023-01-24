---
Title: Enums considered harmful
Subtitle:
Date: 2023-01-24
Tags: typescript, enum
---

It seems that torwards the end of 2022 the collective hivemind of the TypeScript
programming world decided that [enums](https://www.youtube.com/watch?v=jjMbPt_H3RQ)
are [terrible](https://www.youtube.com/watch?v=0fTdCSH_QEU). While I mostly agree
with the premise that TypeScript enums are not good the solutions that are
presented often only deal with [String enums](https://www.typescriptlang.org/docs/handbook/enums.html#string-enums).
For my work on [SDL_ts](https://github.com/smack0007/SDL_ts)
I really need a solution for [Numeric enums](https://www.typescriptlang.org/docs/handbook/enums.html#numeric-enums)
as [SDL](http://www.libsdl.org/) is full of them.

<!--more-->

Uses for numeric enums really boil down to two use cases: the value must be either exactly
one of the specified values or some bitwise combination of them. We'll create the type helpers
`Enum` and `Flags` to implement the two use cases.

`Enum` is when the value must be one of the specified values and only one of the specified values and is easy enough to implement:

```ts
export type Enum<T> = T[keyof T];

export const MyEnum = {
//           ^? const MyEnum: { readonly One: 1; readonly Two: 2; }
  One: 1,
  Two: 2
} as const;

export type MyEnum = Enum<typeof MyEnum>;
//          ^? type MyEnum = 1 | 2

// This works.
const foo: MyEnum = MyEnum.Two;

// @ts-expect-error This does not.
const bar: MyEnum = 3;
```

> [TS Playground Link](https://www.typescriptlang.org/play?#code/KYDwDg9gTgLgBDAnmYcCiA7ArgWwDwAqAfHALxwEDaA1sIhAGYUC6A3ALABQXokscAYwgYAzvACyiTLjJwA3lwD0iuKrXq4APQD8XVQHkMwAFxwAjABo9FAO4RTAJi4BfOAEMRg4WI7c-vaHgkFDhJaRxZcLxg4EZQqWwcIl9lDQ0dLkzOVIIACwBLTzsoahEAOi4hUXgGCHt48NkwxLKCO18lFQABGBEAWl5gARgBqChoCgLPABMIYE8MCBgKziqxOAAjNyhTZplyAGZfIA)

The type helper `Enum` just creates a union of all the values of the object. `T` could
also be more restrictive but it doesn't have to be for our purposes.

To use the helper we create the `export const MyEnum` first to hold the values
and mark it `as const`. This changes the type of `MyEnum` from
`const MyEnum: { One: number; Two: number; }` to
`const MyEnum: { readonly One: 1; readonly Two: 2; }`. Then comes the `export type MyEnum`
which passes `typeof MyEnum` to our `Enum` helper. The `typeof MyEnum` tells TypeScript that
we're talking about the `const` that we defined beforehand. The resulting type is a union
of `1 | 2`.

`Flags` is a bit more complicated:

```ts
declare const _: unique symbol;
export type Flags<T, Name> =
  | {
    [K in keyof T]: { [_]: Name } & T[K];
  }[keyof T]
  | number;

export const MyFlags = {
//           ^? const MyFlags: { readonly One: 1; readonly Two: 2; }
  One: 1,
  Two: 2
} as const;


export type MyFlags = Flags<typeof MyFlags, "MyFlags">;
//          ^? type MyFlags = number | ({ [_]: "MyFlags"; } & 1) | ({ [_]: "MyFlags"; } & 2)

// This works.
const foo: MyFlags = MyFlags.Two;

// This also works.
const bar: MyFlags = MyFlags.One | MyFlags.Two;

// Unfortunately this also works though.
const baz: MyFlags = 4;
```

> [TS Playground Link](https://www.typescriptlang.org/play?#code/CYUwxgNghgTiAEYD2A7AzgF3gfQFzwFcUBLARwITQE8BbAIyQgG4BYAKBAA8AHJGLDFW4IAYtADmaADwAVADTwAclBogAfPAC87ePAA+8AN47d8ANoBpeMRTwA1iCpIAZvBkBdfIfPZPSlQgAvvAAZG6W7qxsuoFmDk6uHiYGKAT0IDBR7Fy8-IiomPAAslRiUJJaRuwA9NWm9fUAegD8JgDyKCD4AIxyJjIA7kj4AEzswVBo+egYWWzZPHwCQgglZRWa8OvSgsIuxaUSaAoARGtHJ2pRtQ0NLew1dTIAFsRTQzB2aAB07Mgz8GcSGGB22lXO5R+gyQcxuLze8CgEDQSHgHy+vzY-0KdFg+AhG1BR2+HQQBgJU2+0NhdQAqiggfwiFAMCAIFR4BhXlMkSi0XwvpznkgCOJnpjsVhcQAvfGHSGVAAsUSAA)

The `Flags` type helper looks quite [gnarly](https://www.youtube.com/watch?v=bVfopcz4nas) but
let's go through it step by step. First `declare const _: unique symbol;` just creates a
[Symbol](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Symbol)
that we can use at the type level. Then first part of the union
(`{ [K in keyof T]: { [_]: Name } & T[K]; }[keyof T]`) in the type helper can be thought of
like calling [map](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/map) on the type and performing what is known as
[branding or nominal typing](https://basarat.gitbook.io/typescript/main-1/nominaltyping#nominal-typing)
on each value of the enum using the string provided in the 2nd parameter of the type helper.
The last part of the helper just unions the newly created type together with `number`.

Using `Flags` is almost the same as using `Enum` but we'll have to provide a string for the
branding which I just set to be the same as the name of the enum.

Why do the enum values have to be branded? Without the branding TypeScript
widens the resulting type to just be `number`. The examples still work as before
but the Intellisense is completely lost. The IDE will just show the type `number` with
no mention of the name of the enum. See this [playground link](https://www.typescriptlang.org/play?#code/KYDwDg9gTgLgBDAnmYcBiAbAhgcwM4A8AKgHxwC8cRA2gNbCIQBmVAunAD5wB2ArgLYAjYFADcAWABQU0JFhwAxhG554AWUSZceCnADeUgPSG4ps+bgA9APxTTAeW7AAXHACMAGjtUA7hFcATFIAvnBYOkoqMBLSsbLQ8EgocBpa+LpphEnAzCma2PgkMcYWFjZSRiZEABYAljp+ULR4AHRSkapwTBD+eZm6qQWtRH4xlVR1OlgYeBBwjc1tkh3wglhQroPaA-naLY6oXFvpLSMQY5IlAKrc3bC83FgwwBiICJNhM3MLOjDVELwcNUlis4GsAF6bXbpSgAFlEQA)
