using System;
using System.Threading;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

			Console.WriteLine("\n\n -- OBSERVER -- \n\n");
			Console.WriteLine(" Hamish's C# net event handler, shows some a similar thing that C# likes to do. \n");

			// Net publisher
			var publisherA = new NetPublisher();
			var publisherB = new NetPublisherA();

			var subA = new NetSubA();
			var subB = new NetSubB();
			var subC = new NetSubC();
			var subD = new NetSubD();

			subA.SubToEvent(publisherA);
			subB.SubToEvent(publisherA);
			subC.SubToEvent(publisherA);
			subD.SubToEvent(publisherB);

			/* Game has started */
			publisherA.scoreIncrease(1);
			publisherA.scoreIncrease(1);
			publisherA.scoreIncrease(6);
			
			publisherA.wicketFallen();
			
			subD.UnsubFromEvent(publisherB);
			
			publisherA.scoreIncrease(1);
			publisherA.scoreIncrease(6);
			publisherA.wicketFallen();

			publisherB.scoreIncrease(6);
			publisherB.wicketFallen();

			Thread.Sleep(10000);


			FactoryExample();


			SingletonTesting singletonTesting = new SingletonTesting();
			SingletonTesting.MainTest();

		}


		static void FactoryExample()
		{
			Console.WriteLine("\n\n -- FACTORY STUFF -- \n\n");
			new Client().Main();
			new ClientTransport().Main();
		}
    }



	// -------------------------------- OBSERVER -------------------------------- //

	// -- My Events Implementation -- //
	/*
	 * I created a similar implementation to the above using c#'s inbuilt events,
	 * i did this because i think it is a good example of how different langages
	 * strucutre thier design and create certin systems to allign with design patterns
	*/

	public class WicketsEventArgs : EventArgs
	{
		public WicketsEventArgs(string message, int wickets, int score)
		{
			Message = message;
			Wickets = wickets;
			Score = score;
		}

		public string Message { get; }
		public int Wickets { get; }
		public int Score { get; }
	}

	public interface INetPublisher
	{
		event EventHandler<WicketsEventArgs> ScoreChange;

	}


	public class NetPublisher : INetPublisher
	{
		public int Wickets { get; set; } = -0;
		public int Score { get; set; } = -0;

		public event EventHandler<WicketsEventArgs> ScoreChange;

		protected virtual void OnScoreChange(WicketsEventArgs e)
		{

			if (ScoreChange != null)
			{
				//e.Message = "Score changed";
				//e.Wickets = this.Wickets;
				//e.Score = this.Score;
				ScoreChange.Invoke(this, e);
			}
		}

		public virtual void wicketFallen()
		{
			Thread.Sleep(2000);

			this.Wickets += 1;

			Console.WriteLine("Subject: Wicket Fallen: " + this.Wickets);

			this.OnScoreChange(new WicketsEventArgs("Wickets fallen", this.Wickets, this.Score));
		}

		public virtual void scoreIncrease(int score)
		{
			Thread.Sleep(1000);

			this.Score += score;

			Console.WriteLine("Subject: Score Changed: " + this.Score);

			this.OnScoreChange(new WicketsEventArgs("Score Changed", this.Wickets, this.Score));
		}

	}

	public class NetPublisherA : NetPublisher
	{

		public override void scoreIncrease(int score)
		{
			Thread.Sleep(2000);

			this.Score += score + 3;

			Console.WriteLine("SPORTS BALL!!!: " + this.Score);

			base.scoreIncrease(score);
		}

	}

	// Nayyar / John
	public interface INetSubscriber
	{
		void SubToEvent(INetPublisher publisher);
		void UnsubFromEvent(INetPublisher publisher);
	}

	class NetSubA : INetSubscriber
	{
		public void SubToEvent(INetPublisher publisher)
		{
			publisher.ScoreChange += this.HandleWicketsUpdate;
		}

		public void UnsubFromEvent(INetPublisher publisher)
		{
			publisher.ScoreChange -= this.HandleWicketsUpdate;
		}

		void HandleWicketsUpdate(object sender, WicketsEventArgs e)
		{
			Console.WriteLine("Mobile Device (display): " + e.Wickets + "/" + e.Score);
		}
	}

	class NetSubB : INetSubscriber
	{
		public void SubToEvent(INetPublisher publisher)
		{
			publisher.ScoreChange += this.HandleWicketsUpdate;
		}

		public void UnsubFromEvent(INetPublisher publisher)
		{
			publisher.ScoreChange -= this.HandleWicketsUpdate;
		}

		void HandleWicketsUpdate(object sender, WicketsEventArgs e)
		{
			Console.WriteLine("Laptop Device (display): " + e.Wickets + "/" + e.Score);
		}
	}

	class NetSubC : INetSubscriber
	{
		public void SubToEvent(INetPublisher publisher)
		{
			publisher.ScoreChange += this.HandleWicketsUpdate;
		}

		public void UnsubFromEvent(INetPublisher publisher)
		{
			publisher.ScoreChange -= this.HandleWicketsUpdate;
		}

		void HandleWicketsUpdate(object sender, WicketsEventArgs e)
		{
			Console.WriteLine("PC Device (display): " + e.Wickets + "/" + e.Score);
		}
	}

	class NetSubD : INetSubscriber
	{
		public void SubToEvent(INetPublisher publisher)
		{
			publisher.ScoreChange += this.HandleWicketsUpdate;
		}

		public void UnsubFromEvent(INetPublisher publisher)
		{
			publisher.ScoreChange -= this.HandleWicketsUpdate;
		}

		void HandleWicketsUpdate(object sender, WicketsEventArgs e)
		{
			Console.WriteLine("Console Device (display): " + e.Wickets + "/" + e.Score);
		}
	}



	// -------------------------------- FACTORYS -------------------------------- //
    
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
			Console.WriteLine("\n\n -- Abstract Factory Stuff example -- \n\n");
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

	// --- My implementation as a way to understand --- //

	// Coded as example here: https://refactoring.guru/design-patterns/factory-method , very helpful

	class ClientTransport
	{
		public void Main()
        {
			Console.WriteLine("\n\n -- Hamish's ClientTransport -- \n\n");
            Console.WriteLine("ClientTransport: Testing client code with the 1st factory type...");
            ClientMethod(new RoadLogistics());
            Console.WriteLine();

            Console.WriteLine("ClientTransport: Testing the same client code with the 2nd factory type...");
            ClientMethod(new SeaLogistics());
			Console.WriteLine();

			Console.WriteLine("ClientTransport: Testing the same client code with the 3rd factory type...");
            ClientMethod(new AirLogistics());
			Console.WriteLine();
        }

        public void ClientMethod(ILogistics logistics)
        {
            var transport = logistics.createTransport();
            var container = logistics.createContainer();

            Console.WriteLine(transport.loadContainer(container));
			Console.WriteLine(transport.deliver());
		}
	}


	public interface ITransport
	{
		string deliver();

        string loadContainer(IContainer container);
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

	public interface ILogistics
	{
		ITransport createTransport();

        IContainer createContainer();
	}



	abstract class Logistics : ILogistics
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
		string checkContents();
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
		public string checkContents()
		{
            string loadDiscription = "air packages";
            return loadDiscription;
		}
	}

	class Singleton
	{
		// The Singleton's constructor should always be private to prevent
		// direct construction calls with the `new` operator.
		private Singleton()
		{
			// nothing here
		}

		// private members
		private int area;

		// The Singleton's instance is stored in a static field. There there are
		// multiple ways to initialize this field, all of them have various pros
		// and cons. In this example we'll show the simplest of these ways,
		// which, however, doesn't work really well in multithreaded program.
		private static Singleton _instance;

		// This is the static method that controls the access to the singleton
		// instance. On the first run, it creates a singleton object and places
		// it into the static field. On subsequent runs, it returns the client
		// existing object stored in the static field.
		public static Singleton GetInstance()
		{
			if (_instance == null)
			{
				_instance = new Singleton();
			}
			return _instance;
		}

		// Finally, any singleton should define some business logic, which can
		// be executed on its instance.
		public void someBusinessLogic()
		{
			Console.WriteLine("We are one but we are many ... Australia");
		}

		public int getArea()
		{
			this.area = 50000;
			return area;
		}
	}

	class SingletonTesting
	{
		public static void MainTest()
		{

			Console.WriteLine("\n\n -- Singleton Example -- \n\n");

			// The client code.
			Singleton s1 = Singleton.GetInstance();
			Singleton s2 = Singleton.GetInstance();

			if (s1 == s2)
			{
				Console.WriteLine("Singleton works, both variables contain the same instance.");
			}
			else
			{
				Console.WriteLine("Singleton failed, variables contain different instances.");
			}

			s1.someBusinessLogic();

			Console.WriteLine("Area = " + s1.getArea());

			ThreadedExample();
		}


		static void ThreadedExample()
		{

			Console.WriteLine("\n\n -- Threaded Example of singleton -- \n\n");

			Thread thread1 = new Thread(delegate () { TestThreadSingleton(5); });
			Thread thread2 = new Thread(delegate () { TestThreadSingleton(10); });
			Thread thread3 = new Thread(delegate () { TestThreadSingleton(15); });

			thread1.Start();
			thread2.Start();
			thread3.Start();


			// Block tell done
			thread1.Join();
			thread2.Join();
			thread3.Join();
		}

		public static void TestThreadSingleton(int area_)
		{
			SingletonThreaded singleton = SingletonThreaded.GetInstance(area_);
			Console.WriteLine("Area = " + singleton.getArea());

		}
	}

	class SingletonThreaded
	{
		// The Singleton's constructor should always be private to prevent
		// direct construction calls with the `new` operator.
		private SingletonThreaded()
		{
			// nothing here
		}

		// private members
		private int area;




		private static SingletonThreaded _instance;

		// Allows interaction via locking thread executions.
		private static readonly object _lock = new object();

		public static SingletonThreaded GetInstance(int area_)
		{
			// Only init if null
			if (_instance == null)
			{
				// This locks the threads, basically the other threads cannot use, r/w blocking
				// When the first instance reaches this, it locks and the others pass over, this means that at initial runtime, only the first thread locks and runs.
				// Subsiquint runs from all other threads can then use the fist if statment.
				lock (_lock)
				{
					if (_instance == null)
					{
						_instance = new SingletonThreaded();
						_instance.area = area_;
					}
				}
			}
			return _instance;
		}

		// Finally, any singleton should define some business logic, which can
		// be executed on its instance.
		public void someBusinessLogic()
		{
			Console.WriteLine("We are one but we are many ... Australia");
		}

		public int getArea()
		{
			//this.area = 50000;
			return area;
		}
	}
}
