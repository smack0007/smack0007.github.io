---
Title: Using Component Glue to build your object graphs automatically
Date: 2013-07-26
Tags: open-Source, dependency-injection, ioc, c#, .net 
---

Component Glue is an IoC container and you use it of course to wire up your object graphs for you. Component Glue can also build your object graphs for you automatically if there are no interfaces involved. Take this example:

<script src="https://gist.github.com/smack0007/6091538.js"></script>

In After.cs, you can see that Component Glue is able to build the entire object graph for us. This will include all future dependencies as well so long as interfaces don't come into play. Should an interface be needed, you can just bind that single component.

This is a very powerful thing. If one component needs to take on a dependency, just ask for it in the constructor and Component Glue will handle it for you.
