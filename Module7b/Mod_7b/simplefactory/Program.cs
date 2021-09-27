using System;

namespace simplefactory
{

	/* -- PIZZA CLASSES -- */

	public interface Pizza
	{
		string prepare();
	}


	/* -- PIZZA SUB-CLASSES -- */

	class CheesePizza : Pizza
	{
		public string prepare()
		{
			return "Preparing a yummy Cheese Pizza";
		}
	}

	class PepperoniPizza : Pizza
	{
		public string prepare()
		{
			return "Preparing a yummy Pepperoni Pizza";
		}
	}

	/* -- SIMPLE FACTORY (Cringe) -- */

	class SimplePizzaFactory {

		public Pizza createPizza(String type) {
			
			Pizza pizza = null;

			if (type.Equals("Cheese")) {
				pizza = new CheesePizza();
			} else if (type.Equals("Pepperoni")) {
				pizza = new PepperoniPizza();
			} 

			return pizza;

		}

	}

	/* -- FACTORY METHOD PATTERN (Based) -- */

	abstract class AbstractPizzaFactory {

		abstract public Pizza factoryMethod();

		public string SomeOperation()
		{
			// Call the factory method to create a Product object.
			Pizza pizza = factoryMethod();
			// Now, use the product.
			var result = " -- " + pizza.prepare();

			return result;
		}

	}
	class CheesePizzaCreator : AbstractPizzaFactory
	{
		public override Pizza factoryMethod()
		{
			return new CheesePizza();
		}
	}

	class PepperoniPizzaCreator : AbstractPizzaFactory
	{
		public override Pizza factoryMethod()
		{
			return new PepperoniPizza();
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Welcome to World's Best Pizza!");

			String type = "Cheese";

			/* Bad way of ordering pizza */
			Pizza pizza = orderPizza(type);

			Console.WriteLine(pizza.prepare());

			/* Ugly way of ordering pizza [Simple Factory] */
			SimplePizzaFactory factory = new SimplePizzaFactory();
			pizza = factory.createPizza(type);

			Console.WriteLine(pizza.prepare());

			/* Good way of ordering pizza [Factory Method] */
			AbstractPizzaFactory afactory = new CheesePizzaCreator();
			pizza = afactory.factoryMethod();

			Console.WriteLine(pizza.prepare());
		}


		/* -- STRING IF-ELSE (Bad) -- */

		static Pizza orderPizza(String type) {
			Pizza pizza = null;

			if (type.Equals("Cheese")) {
				pizza = new CheesePizza();
			} else if (type.Equals("Pepperoni")) {
				pizza = new PepperoniPizza();
			} 

			return pizza;
		}
	}


	// --- My implementation as a way to understand --- //

	// Coded as example here: https://refactoring.guru/design-patterns/factory-method , very helpful


	public interface ITransport
	{
		string deliver();
	}

	class Truck : ITransport
	{
		public string deliver()
		{
			string deliverDiscription = "Drive";
			return deliverDiscription;
		}
	}

	class Ship : ITransport
	{
		public string deliver()
		{
			string deliverDiscription = "Float";
			return deliverDiscription;
		}
	}

	class Plane : ITransport
	{
		public string deliver()
		{
			string deliverDiscription = "Fly";
			return deliverDiscription;
		}
	}

	abstract class Logistics
	{
		// This is the factory method
		abstract public ITransport createTransport();

		// This is inhereted by the other logi classes
		public ITransport planDelivery()
		{
			// Because "createTransport is overriden, when used it will create the specified class"
			ITransport transport = this.createTransport();
			return transport;
		}
	}

	class RoadLogistics : Logistics
	{
        public override ITransport createTransport()
        {
            return new Truck();
        }
    }

	class SeaLogistics : Logistics
	{
        public override ITransport createTransport()
        {
            return new Ship();
        }
    }

	class AirLogistics : Logistics
	{
        public override ITransport createTransport()
        {
            return new Plane();
        }
    }
}
