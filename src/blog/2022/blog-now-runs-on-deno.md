---
Title: Now powered by deno
Subtitle:
Date: 2022-12-23
Tags: deno
---

I finally sat down and got my blog compiling with [deno](https://deno.land/). Now that there
is official npm support I didn't need to completely rewrite the compile which was what was
stopping me before.

<!--more-->

Besides updating the fs / path api calls the only real roadblock that I had was using
[node-sass](https://www.npmjs.com/package/node-sass) as I ran into
[this issue](https://github.com/sass/dart-sass/issues/1841).

I just ended up calling node-sass via the cli as I only have to really compile one file:

```ts
const result = new TextDecoder().decode(
  await Deno.run({
    cmd: [
      IS_WINDOWS ? "npx.cmd" : "npx",
      "node-sass",
      join(INPUT_DIRECTORY, "css", "style.scss"),
      "--output-style=compressed",
    ],
    stdout: "piped",
  }).output()
);
```

Overall I really like the direction deno is heading and I think it is the future of local
JavaScript runtimes.
