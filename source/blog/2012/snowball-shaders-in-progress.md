---
Title: Snowball - Shaders in progress
Date: 2012-06-22
Tags: .net, open-source
---

It's been a long couple of months. I'm in the middle of switching jobs, been on vacation a bit, and have been playing around with OpenGL a bit to get a feel for how that API works compared to Direct3D. As of yesterday I started working on implementing shaders in Snowball.

In order to implement shaders or Effect(s), there may have to be a few changes to the API / interface of the Renderer class. Nothing significant I don't think but mainly changes to the Begin() method overloads. Today I pushed the branch which contains my initial implementation. 
