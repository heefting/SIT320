using System;

namespace abstractfactory
{

    /* -- CLASS -- */

    public interface Pizza
    {
        string prepare();
    }

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

    public interface Burger
    {
        string prepare();

        string Combo(Pizza pizza);
    }

    class CheeseBurger : Burger
    {
        public string prepare()
        {
            return "Preparing a yummy Cheese Burger";
        }

        public string Combo(Pizza pizza)
        {
            return pizza.prepare() + prepare();

        }
    }

    class PepperoniBurger : Burger
    {
        public string prepare()
        {
            return "Preparing a yummy Pepperoni Burger";
        }

        public string Combo(Pizza pizza)
        {
            return pizza.prepare() + prepare();

        }
    }

    /* -- INTERFACE -- */

    public interface IAbstractFactory
    {
        Pizza CreatePizza();

        Burger CreateBurger();
    }

    /* -- FACTORIES -- */

    class CheeseFactory : IAbstractFactory
    {
        public Pizza CreatePizza()
        {
            return new CheesePizza();
        }

        public Burger CreateBurger()
        {
            return new CheeseBurger();
        }
    }

    class PepperoniFactory : IAbstractFactory
    {
        public Pizza CreatePizza()
        {
            return new PepperoniPizza();
        }

        public Burger CreateBurger()
        {
            return new PepperoniBurger();
        }
    }

    class Client
    {
        public void Main()
        {
            Console.WriteLine("Client: Testing client code with the first factory type...");
            ClientMethod(new CheeseFactory());
            Console.WriteLine();

            Console.WriteLine("Client: Testing the same client code with the second factory type...");
            ClientMethod(new PepperoniFactory());
        }

        public void ClientMethod(IAbstractFactory factory)
        {
            var pizza = factory.CreatePizza();
            var burger = factory.CreateBurger();

            Console.WriteLine(pizza.prepare());
            Console.WriteLine(burger.prepare());
            Console.WriteLine(burger.Combo(pizza));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            new Client().Main();
        }
    }

    // --- My implementation as a way to understand --- //

	// Coded as example here: https://refactoring.guru/design-patterns/factory-method , very helpful


	public interface ITransport
	{
		string deliver();

        string loadContainer();
	}

	class Truck : ITransport
	{
		public string deliver()
		{
			string deliverDiscription = "Drive";
			return deliverDiscription;
		}

        public string loadContainer(IContainer container)
        {
			return container.checkContents();
        }
	}

	class Ship : ITransport
	{
		public string deliver()
		{
			string deliverDiscription = "Float";
			return deliverDiscription;
		}

        public string loadContainer(IContainer container)
        {
			return container.checkContents();
        }
	}

	class Plane : ITransport
	{
		public string deliver()
		{
			string deliverDiscription = "Fly";
			return deliverDiscription;
		}

        public string loadContainer(IContainer container)
        {
			return container.checkContents();
        }
	}

	abstract class Logistics
	{
		// This is a factory method
		abstract public ITransport createTransport();

        // This is a factory method
		abstract public IContainer createContainer();

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

        public override IContainer createContainer()
        {
            return new ShippingContainer();
        }
    }

	class SeaLogistics : Logistics
	{
        public override ITransport createTransport()
        {
            return new Ship();
        }

        public override IContainer createContainer()
        {
            return new ShippingContainer();
        }
    }

	class AirLogistics : Logistics
	{
        public override ITransport createTransport()
        {
            return new Plane();
        }

        public override IContainer createContainer()
        {
            return new AirContainer();
        }
    }


	public interface IContainer
	{
		void checkContents();
	}

	class ShippingContainer : IContainer
	{
		public string checkContents()
		{
            string loadDiscription = "shipping container";
            return loadDiscription;
		}
	}

	class AirContainer : IContainer
	{
		public void checkContents()
		{
            string loadDiscription = "air packages";
            return loadDiscription;
		}
	}

}
