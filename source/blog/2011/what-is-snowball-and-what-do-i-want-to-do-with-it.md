---
Title: What is Snowball and what do I want to do with it?
Layout: Post
Permalink: 2011/10/16/what-is-snowball-and-what-do-i-want-to-do-with-it.html
Date: 2011-10-16
Category: .NET
Tags: 2d, C#, CodePlex, Open Source, SlimDX, Xna 
Comments: true
---

I originally got the idea for Snowball after working with the Xna Framework. The Xna Framework is a good piece of software for what it is but there are some things about which I just do not agree with:

<ul>
<li>The content pipeline only works with content in the serialized .xnb format.</li>
<li>There are certain content types which can only be loaded via the content pipeline.</li>
<li>Certain features don't exist on the PC because they don't exist on the XBox or Windows Phone 7.</li>
</ul>

Xna was designed as an abstraction layer for all the 3 platforms mentioned in the last point, so that one is somewhat understandable. I don't want to write games for my XBox right now though, so why should things like drawing lines not be available to me?

With these points in mind I started working on [Snowball](http://snowball.codeplex.com/). It's designed to be an Xna like framework for making 2D games. It uses SlimDX on the backend, but that is completely abstracted away from consumers of the framework. What I want to do is design the API so that the backend can be swapped out **somewhat** painlessly.

I still have a ways to go before I will consider it a version 1.0 release. As of this writing, I'm transitioning to more of a ContentLoader class style for loading your game's content. Any resource type from within the framework can be loaded by hand if you want, the ContentLoader class will just make it easier. After that I have a few other features like GamePad and Music which I would like to implement before saying I have a Beta type release.

The future after that is up in the air. I would love to try and have different implementations of the API for Xna and/or OpenTK.

I recommend for anyone who is interested as to why an API designer choose to implement the API in the way they did to try it for themselves. I have learned many things from this project including why certain design decisions were made by the Xna Framework team. 


