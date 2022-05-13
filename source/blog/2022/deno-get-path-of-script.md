---
Title: deno: Get the path of the current script in
Subtitle:
Date: 2022-05-13
Tags: deno, typescript, javascript
---

In [deno](https://deno.land/) obtaining the path of the current script is done via the
`import.meta.url` api:

```ts
const scriptPath = new URL(import.meta.url).pathname;
```

On Windows though this will return a path with unix style path seperators and a leading `/`:

```
/D:/Code/deno-path-of-script/main.ts
```

<!--more-->

This cannot be used for example to read the contents of the current script. We'll need a better
solution.

Whether or not the script is running on Windows can be detected via the `Deno.build.os` api:

```ts
const isWindows = Deno.build.os === "windows";
```

So we can use that to correct the path seperators and remove the leading `/`:

```ts
const isWindows = Deno.build.os === "windows";

const scriptPath = new URL(import.meta.url).pathname
  .replaceAll("/", isWindows ? "\\" : "/")
  .substring(isWindows ? 1 : 0);

console.info(scriptPath);
console.info(await Deno.readTextFile(scriptPath));
```
