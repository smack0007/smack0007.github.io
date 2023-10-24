---
Title: Typing jest Spies
Subtitle:
Date: 2023-10-24
Tags: typescript, jest
---

It often feels like [TypeScript](https://www.typescriptlang.org/) was an afterthought in [jest](https://jestjs.io/).
Any time I need to type a variable that holds some data structure from jest it feels
like I need to dust off my old book of spells in order to find the correct incantation
to make [eslint](https://eslint.org/) happy. A situation I finally figured
out a solution for is when you want to store the result of a call to `jest.spyOn` in a
shared variable:

```ts
let setTimeoutSpy: jest.SpyInstance;

beforeEach(() => {
  setTimeoutSpy = jest.spyOn(globalThis, 'setTimeout').mockImplementation((callback) => {
    callback();
    return 0 as unknown as ReturnType<typeof setTimeout>;
  });
});

afterEach(() => {
  setTimeoutSpy.mockRestore();
});
```

Even though `jest.SpyInstance` is specified here as the type for `randomSpy` the information about exactly what
function is being spied on gets lost here. That means if elsewhere in the code if you try the following:

```ts
let setTimeoutCallback = () => {};
let setTimeoutDelay: number | undefined = undefined;
setTimeoutSpy.mockImplementation((callback, delay) => {
  setTimeoutCallback = callback;
  setTimeoutDelay = delay;
  return 0 as unknown as ReturnType<typeof setTimeout>;
});
```

`eslint` will complain that `callback` and `delay` have the type `any`. This is again due to the lost type information.
This problem can be resolved by using some TypeScript utility types when the spy variable is declared:

```ts
let setTimeoutSpy: jest.SpyInstance<ReturnType<typeof setTimeout>, Parameters<typeof setTimeout>>;
```

We use the built in `ReturnType` and `Parameters` utility types to perform the voodoo needed in order to make `eslint` happy.