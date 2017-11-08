---
Title: Snowball: A Slightly Different Direction
Layout: Post
Permalink: 2011/07/02/snowball-a-slightly-different-direction.html
Date: 2011-07-02
Category: .NET
Tags: CodePlex 
Comments: true
---

At first, I had imagined [Snowball](http://snowball.codeplex.com/), now located on Codeplex, to be a framework which would define how your game objects look. I.E. I had a class called GameEntity which I imagined would handle a lot of boiler plate code for you such as setting up Initialize(), Update(), Draw(), etc. 

I've decided to move away from that and let Snowball purely focus on the subsystems of a game, such as Graphics, Sound, Input, etc. Extending from the Game class completely sets up these aspects of your game for you. I think I will include helper components, such as a collision detection system, but I will not force you to use them.
