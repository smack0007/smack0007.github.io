---
Title: Predictable Random Number Generation in TypeScript
Subtitle: Lehmer
Date: 2020-08-25
Tags: typescript, rng, lehmer
---

I got the idea for using this type of RNG from the javidx9 video
[Procedural Generation: Programming The Universe](https://www.youtube.com/watch?v=ZZY9YE7rZJw).
It's a really good video as are most of the videos he produces so
I recommend you give it a watch. The algorithm he talks about in
the video is the [Lehmer random number generator](https://en.wikipedia.org/wiki/Lehmer_random_number_generator).

I'm implementing a simple match the blocks game in TypeScript and I wanted to implement the algorithm 
mentioned there in TypeScript.

<!--more-->

Here's the code:

```typescript
export class RNG {
    private static readonly a = 16807;
    private static readonly m = 2147483647;
    private static readonly q = 127773;
    private static readonly r = 2836;

    constructor(private _seed: number) {
        if (this._seed <= 0 || this._seed === Number.MAX_VALUE) {
            throw new Error("Seed out of range.");
        }
    }

    public nextDouble(): number {
        const hi = this._seed / RNG.q;
        const lo = this._seed % RNG.q;

        this._seed = (RNG.a * lo) - (RNG.r * hi);

        if (this._seed <= 0) {
            this._seed = this._seed + RNG.m;
        }

        return (this._seed * 1.0) / RNG.m;
    }

    public nextInt(min: number, max: number): number {
        const range = Math.round(max) - Math.round(min);
        return min + Math.round(range * this.nextDouble());
    }
}
```

You can simply new up an instance and call `rng.nextInt()` to
get a random integer based on the provided seed.
