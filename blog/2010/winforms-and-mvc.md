---
Title: WinForms and MVC
Layout: Post
Permalink: 2010/05/26/winforms-and-mvc.html
Date: 2010-05-26
Category: .NET
Tags: MVC, WinForms 
Comments: true
---

I recently became interested in doing MVC inside of a Windows Forms app. I found a few MVC frameworks which work with WinForms ([see here](http://stackoverflow.com/questions/2406/looking-for-a-mvc-sample-for-winforms)) but they didn't really interest me. Too heavy I felt for what I was looking to do. I ended up with a solution looking something like this:

<a href="http://zacharysnow.net/wp-content/uploads/2010/05/WinFormsMvcSolution.png"><img src="http://zacharysnow.net/wp-content/uploads/2010/05/WinFormsMvcSolution-150x150.png" alt="WinForms MVC Solution" title="WinForms MVC Solution" width="150" height="150" class="alignnone size-thumbnail wp-image-305" /></a>

There is really only one controller and that is the "Application" class. It contains all the methods your app can call to manipulate your models, which are in the "Data" folder / namespace. The "WinFormsApplication" class inherits from the "Application" class and just sets the view to an instance of "WinFormsView". The "Application" class communicates with the view through the "IView" interface. The "WinFormsView" class is a Windows Forms implementation of that view. The "Application" class and your models are not coupled in any way to your Windows Forms implementation of the view.

If you want you view to be as dumb as possible, your view can communicate with the "Application" class only through events. In my case though, I choose to go with a smart view and have the view call back to methods in the "Application" class. The "Application" class tells the view when models are loaded and unloaded. The view subscribes to events on the models and reacts to the events.

All of my forms and controls communicate with each other through the "WinFormsView" class. One control might change the value of a property in the "WinFormsView" class and another control might subscribe to a change event and update as necessary. This keeps the controls and forms slightly less coupled.

It's not a perfect implementation of MVC but it keeps my model logic decoupled enough from my view logic that I can later implement a WPF version of the view I think.
