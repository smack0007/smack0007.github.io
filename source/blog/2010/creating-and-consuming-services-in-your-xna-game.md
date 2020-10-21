---
Title: Creating and consuming services in your XNA Game
Date: 2010-02-18
Tags: .net
---

The [GameServiceContainer](http://msdn.microsoft.com/en-us/library/microsoft.xna.framework.gameservicecontainer.aspx) implements the [IServiceProvider](http://msdn.microsoft.com/en-us/library/system.iserviceprovider.aspx) interface and the MSDN documentation says about the IServiceProvider interface:

<blockquote>Defines a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</blockquote>

This article will "*attempt*" to describe how can you use the GameServiceContainer in your XNA game, in both your GameComponent(s) and your game's entity objects.
<!--more-->
The most obvious place to use the GameServiceContainer is in your GameComponent(s). But first, lets talk about "[Coupling](http://en.wikipedia.org/wiki/Coupling_(computer_science))". Let's assume you have the following components:

```c#
class FooComponent : GameComponent
{
	public FooComponent(Game game)
		: base(game)
	{
	}
	
	public int DoFoo()
	{
		// Do something and return an int.
	}
}

class BarComponent : GameComponent
{
	FooComponent foo;

	public BarComponent(Game game)
		: base(game)
	{
		this.foo = new FooComponent(game);
	}
	
	public void DoBar()
	{
		int result = this.foo.DoFoo();
		// Do something based on result.
	}
}
```

There's nothing wrong with the code, but BarComponent has a dependency on FooComponent. BarComponent directly interacts with FooComponent and therefore any change made to FooComponent indirectly affects BarComponent. For instance, let's assume the constructor for FooComponent needs to be modified. That means we now have to update not only the FooComponent class but as well the BarComponent class. Throw in a few more components with dependencies on FooComponent and you could start to get headache really fast. This design is highly coupled.

Let's try a slight redesign:

```c#
class FooComponent : GameComponent
{
	public FooComponent(Game game)
		: base(game)
	{
	}
	
	public int DoFoo()
	{
		// Do something and return an int.
	}
}

class BarComponent : GameComponent
{
	FooComponent foo;

	public BarComponent(Game game, FooComponent foo)
		: base(game)
	{
		this.foo = foo;
	}
	
	public void DoBar()
	{
		int result = this.foo.DoFoo();
		// Do something based on result.
	}
}
```

We've now eliminated the construction of the FooComponent from within the BarComponent. The design is better but still not that great. BarComponent is still directly relying on and communicating with FooComponent. We want to change BarComponent so that it has no direct dependency on a concrete implementation of FooComponent. We'll create an interface:

```c#
interface IFooService
{
	int DoFoo();
}

class FooComponent : GameComponent, IFooService
{
	public FooComponent(Game game)
		: base(game)
	{
	}
	
	public int DoFoo()
	{
		// Do something and return an int.
	}
}

class BarComponent : GameComponent
{
	IFooService foo;

	public BarComponent(Game game, IFooService foo)
		: base(game)
	{
		this.foo = foo;
	}
	
	public void DoBar()
	{
		int result = this.foo.DoFoo();
		// Do something based on result.
	}
}
```

We can now change FooComponent as much as we want and BarComponent will be unaffected. BarComponent now communicates with FooComponent through the IFooService interface. This also allows us to have multiple implementations of DoFoo():

```c#
class SimpleFooComponent : GameComponent, IFooService
{
	public SimpleFooComponent(Game game)
		: base(game)
	{
	}
	
	public int DoFoo()
	{
		return 5; // The class says "Simple"
	}
}

class ComplexFooComponent : GameComponent, IFooService
{
	public ComplexFooComponent(Game game)
		: base(game)
	{
	}
	
	public int DoFoo()
	{
		int result = 0;
		// Do some very complex calculation
		return result;
	}
}
```

We can pass BarComponent an instance of SimpleFooComponent or ComplexFooComponent. Whatever the situation may call for.

Where does GameServiceContainer fit into all of this? You can use the GameServiceContainer to hold all your "Services". Add whatever class will implement the IFooService and then from within your BarComponent you can query for it:

```c#
class BarComponent : GameComponent
{
	IFooService foo;

	public BarComponent(Game game)
		: base(game)
	{
	}
	
	public override void Initialize()
	{
		this.foo = this.Game.Services.GetService(typeof(IFooService)) as IFooService;
		
		if(this.foo == null)
			throw new InvalidOperationException("IFooService not found.");
	}
	
	public void DoBar()
	{
		int result = this.foo.DoFoo();
		// Do something based on result.
	}
}

// In your Game's constructor.
this.Services.AddService(typeof(IFooService), new SimpleFooComponent(this));
```

Not only does BarComponent no longer require an instance of IFooService in its constructor, it also no longer matters if the instance of IFooService is constructed before or after the BarComponent. So long as all the services BarComponent requires are in the GameServiceContainer before Initialize() is called, it doesn't matter what order your components are constructed in. Now, suppose that BarComponent didn't necessarily depend on IFooService and instead the behavior of DoBar() is changed based on whether or not IFooService is available:

```c#
class BarComponent : GameComponent
{
	IFooService foo;

	public BarComponent(Game game)
		: base(game)
	{
	}
	
	public override void Initialize()
	{
		this.foo = this.Game.Services.GetService(typeof(IFooService)) as IFooService;
	}
	
	public intDoBar()
	{
		// If the IFooService is available, delegate to the DoFoo() method.
		if(this.foo != null)
			return this.foo.DoFoo();
		
		int result = 0;
		// Otherwise do some other calculation.
		return result;
	}
}
```

Service providers don't always have to be GameComponent(s). Our BarComponent needs a Camera class now:

```c#
interface ICamera
{
	Matrix Transform { get; }
}

class IdentityCamera : ICamera
{
	public Matrix Transform
	{
		get { return Matrix.Identity; }
	}
}

class MovingCamera : ICamera
{
	public Matrix Transform
	{
		get;
		set;
	}
}

class BarComponent : DrawableGameComponent
{
	ICamera camera;

	public BarComponent(Game game)
		: base(game)
	{
	}
	
	public override void Initialize()
	{
		this.camera = this.Game.Services.GetService(typeof(ICamera)) as ICamera;
	}
	
	public override void Draw(GameTime gameTime)
	{
		Matrix transform = this.camera.Transform;
		// Draw based on the transform matrix
	}
}

// In your Game's constructor.
this.Services.AddService(typeof(ICamera), new MovingCamera());
```

BarComponent uses the camera's Transform matrix and doesn't care how it is calculated. It's completely decoupled from the camera's implementation.

In closing, using the GameServiceContainer and interfaces makes your classes more loosely coupled. This makes it easier to make changes to the way your game works. Your classes also become more reusable as you can now mix and match service providers and consumers as needed. If you need a specific implementation of a camera for your game, you can still use the BarComponent so long as your camera class implements the ICamera interface.

Loosely coupling your classes has the added benefit of making them more testable. That's another blog post though.

