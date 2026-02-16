---
Title: deno: The node API is the standard library
Subtitle:
Date: 2026-02-16
Tags: deno, node
---

One of the things I've always struggled with in [deno](https://deno.com/) was that fact almost every time you wanted to do
something non trivial you end up having to import from the `@std` module. This used to be hosted on deno.land but now it 
have moved to [jsr](https://jsr.io/). This always felt like a poor solution to the standard library problem to me. As soon
as I have to import from `@std` I at least need a `deno.json` file. This discouraged me from writing all but the simplest
scripts in deno.

Now with version 2.0 the node compatibility has gotten so good that I just always reach for the node APIs. I don't have
to create an extra `deno.json` file and I can get on with the task at hand.
