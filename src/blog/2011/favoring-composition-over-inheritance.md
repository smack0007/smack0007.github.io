---
Title: Favoring Composition over Inheritance
Date: 2011-01-18
Tags: design-patterns, composition-over-inheritance
---

You may have heard the expression before "Favor Composition over Inheritance", but do you know what it means and how to apply it? Lets take this code for example:

```c#
abstract class Car
{
	public Color Color { get; protected set; }
	public Engine Engine { get; protected set; }
}

class ElectricCar : Car
{
	public ElectricCar()
	{
		this.Color = Color.Blue;
		this.Engine = new ElectricEngine();
	}
}

class SportsCar : Car
{
	public SportsCar()
	{
		this.Color = Color.Red;
		this.Engine = new V8Engine();
	}
}

class Truck : Car
{
	public Truck()
	{
		this.Color = Color.White;
		this.Engine = new DieselEngine();
	}
}
```

In this contrived example, we've defined three types of cars. Each instance of each of the cars will always have the same color and same engine. What happens when we need a 4th type? We have to define another class. By refactoring this code, we can compose a car type by giving it a color and an engine:

```c#
class Car
{
	public Color Color { get; private set; }
	public Engine Engine { get; private set; }
	
	public Car(Color color, Engine engine)
	{
		this.Color = color;
		this.Engine = engine;
	}
}

Car electricCar = new Car(Color.Blue, new ElectricEngine());
Car sportsCar = new Car(Color.Red, new V8Engine());
Car truck = new Car(Color.White, new DieselEngine());
Car familyCar = new Car(Color.Black, new V4Engine());
```

Now we can compose many car types, with any combination of colors and engines and we've only defined one class.

