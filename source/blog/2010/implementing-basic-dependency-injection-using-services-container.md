---
Title: Implementing basic Dependency Injection using a Service Container
Layout: Post
Permalink: 2010/06/21/implementing-basic-dependency-injection-using-services-container.html
Date: 2010-06-21
Category: .NET
Tags: .NET, C#, Dependency Injection, Design Patterns, Service Continer 
Comments: true
---

By extending your Service Container class, a very basic version of dependency injection can be implemented. We'll implement two forms of dependency injection: constructor and property injection. 

<!--more-->

We'll start by defining the **Injectable** attribute. 

```c#
[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Property,
	AllowMultiple = false, Inherited = true)]
public class InjectableAttribute : Attribute
{
}
```

We'll use this attribute to mark our constructors and properties for dependency injection. Next we'll define an interface for our dependency injector:

```c#
public interface IDependencyInjector
{
	T Construct&lt;T&gt;();
	void Inject(object instance);
}
```

We'll define our service container like so:

```c#
public class ServiceContainer : IDependencyInjector, IServiceProvider
{
	Dictionary&lt;Type, Object&gt; services;

	public ServiceContainer()
		: base()
	{
		this.services = new Dictionary&lt;Type, object&gt;();
	}

	public void AddService(Type type, Object provider)
	{
		if(null == type)
			throw new ArgumentNullException("type");

		if(null == provider)
			throw new ArgumentNullException("provider");

		if(this.services.ContainsKey(type))
			throw new InvalidOperationException("A provider is already registered the type " + type);

		var providerType = provider.GetType();

		if(!type.IsAssignableFrom(providerType))
			throw new InvalidOperationException(providerType + " is not an instance of " + type);

		this.services.Add(type, provider);
	}

	public object GetService(Type type)
	{
		if(null == type)
			throw new ArgumentNullException("type");

		if(this.services.ContainsKey(type))
			return this.services[type];
					
		return null;
	}

	public void RemoveService(Type type)
	{
		if(null == type)
			throw new ArgumentNullException("type");

		this.services.Remove(type);
	}

	protected object GetInjectableService(Type type)
	{
		if(type == typeof(IDependencyInjector) ||
		   type == typeof(IServiceProvider))
		{
			return this;
		}
		else
		{
			object service = this.GetService(type);

			if(service == null)
				throw new InvalidOperationException("Failed to find " + type + " depenedency.");

			return service;
		}
	}

	public T Construct&lt;T&gt;()
	{
		ConstructorInfo injectableConstructor = null;
		foreach(ConstructorInfo constructor in typeof(T).GetConstructors())
		{
			foreach(Attribute attribute in constructor.GetCustomAttributes(true))
			{
				if(attribute is InjectableAttribute)
				{
					injectableConstructor = constructor;
					break;
				}
			}

			if(injectableConstructor != null)
				break;
		}

		if(injectableConstructor == null)
			throw new InvalidOperationException("No injectable constructor found.");

		var parameters = injectableConstructor.GetParameters();
		var services = new object[parameters.Length];

		int i = 0;
		foreach(ParameterInfo parameter in parameters)
			services[i++] = GetInjectableService(parameter.ParameterType);

		return (T)injectableConstructor.Invoke(services);
	}

	public void Inject(object instance)
	{
		foreach(PropertyInfo property in instance.GetType().GetProperties())
		{
			foreach(Attribute attribute in property.GetCustomAttributes(true))
			{
				if(attribute is InjectableAttribute)
				{
					if(!property.CanWrite)
						throw new InvalidOperationException(property.Name + " is marked as Injectable but not writable.");

					property.SetValue(instance, GetInjectableService(property.PropertyType), null);
				}
			}
		}
	}
}
```

You can now construct new instances and inject dependencies on existing instances. Some usage examples:

```c#
public interface IFoo
{
	int Value { get; }
}

public class Foo : IFoo
{
	public int Value
	{
		get;
		set;
	}

	[Injectable]
	public Foo()
	{
	}

	public void DoIt()
	{
		Console.WriteLine(this.Value);
	}
}

public interface IBar
{
	string Value { get; }
}

public class Bar : IBar
{
	IFoo foo;

	public string Value
	{
		get;
		set;
	}

	[Injectable]
	public Bar(IFoo foo)
	{
		this.foo = foo;
	}

	public void DoIt()
	{
		Console.WriteLine(this.Value + ": " + this.foo.Value);
	}
}

public class Baz
{
	[Injectable]
	public IFoo Foo
	{
		get;
		set;
	}

	[Injectable]
	public IBar Bar
	{
		get;
		set;
	}
							
	public void DoIt()
	{
		Console.WriteLine(this.Bar.Value + " | " + this.Foo.Value);
	}
}

class Program
{
	static void Main(string[] args)
	{
		var container = new ServiceContainer();

		var foo = container.Construct&lt;Foo&gt;();
		foo.Value = 5;
		container.AddService(typeof(IFoo), foo);

		var bar = container.Construct&lt;Bar&gt;();
		container.AddService(typeof(IBar), bar);
		bar.Value = "Hello World!";
		bar.DoIt();

		var baz = new Baz();
		container.Inject(baz);
		baz.DoIt();
	}
}
```

