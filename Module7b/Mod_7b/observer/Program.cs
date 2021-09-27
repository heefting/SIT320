using System;
using System.Threading;
using System.Collections.Generic;

namespace observer
{

	// Nayyar / John
	public interface Subscriber
	{ 
		void Update(IPublisher subject);
	}

	class ConcreteSubscriberA : Subscriber
	{
		public void Update(IPublisher subject)
		{            
			Console.WriteLine("Mobile Device (display): " + (subject as Publisher).Wickets + "/" + (subject as Publisher).Score);
		}
	}

	class ConcreteSubscriberB : Subscriber
	{
		public void Update(IPublisher subject)
		{
			Console.WriteLine("Laptop Device (display): " + (subject as Publisher).Wickets + "/" + (subject as Publisher).Score);
		}
	}

	public interface IPublisher
	{
		// Attach an observer to the subject.
		void Attach(Subscriber observer);

		// Detach an observer from the subject.
		void Detach(Subscriber observer);

		// Notify all observers about an event.
		void Notify();
	}

	public class Publisher : IPublisher
	{
		public int Wickets { get; set; } = -0;
		public int Score { get; set; } = -0;

		// List of subscribers. In real life, the list of subscribers can be
		// stored more comprehensively (categorized by event type, etc.).
		private List<Subscriber> subscribers = new List<Subscriber>();

		// The subscription management methods.
		public void Attach(Subscriber observer)
		{
			Console.WriteLine("Subject: Attached an observer.");
			this.subscribers.Add(observer);
		}

		public void Detach(Subscriber observer)
		{
			this.subscribers.Remove(observer);
			Console.WriteLine("Subject: Detached an observer.");
		}

		// Trigger an update in each subscriber.
		public void Notify()
		{
			Console.WriteLine("Subject: Notifying observers...");

			foreach (var subscriber in subscribers)
			{
				subscriber.Update(this);
			}
		}

		public void wicketFallen()
		{
			Thread.Sleep(10000);

			this.Wickets += 1;

			Console.WriteLine("Subject: Wicket Fallen: " + this.Wickets);
			this.Notify();
		}

		public void scoreIncrease(int score)
		{
			Thread.Sleep(10000);

			this.Score += score;

			Console.WriteLine("Subject: Score Changed: " + this.Score);
			this.Notify();
		}

	}

	class Program
	{
		static void Main(string[] args)
		{
			// The client code.
			// new game between england and newzealand is started:
			var publisher = new Publisher();
			
			// Nayyar has opened his website
			var subscriberA = new ConcreteSubscriberA();
			publisher.Attach(subscriberA);

			// Aya opening her website
			var subscriberB = new ConcreteSubscriberB();
			publisher.Attach(subscriberB);

			/* Game has started */
			publisher.scoreIncrease(1);
			publisher.scoreIncrease(1);
			publisher.scoreIncrease(6);
			
			publisher.wicketFallen();
			
			publisher.Detach(subscriberB);
			
			publisher.scoreIncrease(1);
			publisher.scoreIncrease(6);
			publisher.wicketFallen();

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


		}
	}


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
			Thread.Sleep(10000);

			this.Wickets += 1;

			Console.WriteLine("Subject: Wicket Fallen: " + this.Wickets);

			this.OnScoreChange(new WicketsEventArgs("Wickets fallen", this.Wickets, this.Score));
		}

		public virtual void scoreIncrease(int score)
		{
			Thread.Sleep(10000);

			this.Score += score;

			Console.WriteLine("Subject: Score Changed: " + this.Score);

			this.OnScoreChange(new WicketsEventArgs("Score Changed", this.Wickets, this.Score));
		}

	}

	public class NetPublisherA : NetPublisher
	{

		public override void scoreIncrease(int score)
		{
			Thread.Sleep(10000);

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





}
