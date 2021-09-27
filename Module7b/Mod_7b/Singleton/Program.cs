using System;
using System.Threading;

namespace Singleton
{
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

        public int getArea() {
            this.area = 50000;
            return area;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
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
            //public delegate void firstDel(int area_);
            Thread threadFirst = new Thread(delegate() { TestThreadSingleton(5); });
            //Thread threadSecond = new Thread(TestThreadSingleton(10));

            threadFirst.Start();
        }

        public static void TestThreadSingleton(int area_)
        {
            SingletonThreaded singleton = SingletonThreaded.GetInstance(area_);

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



        // The Singleton's instance is stored in a static field. There there are
        // multiple ways to initialize this field, all of them have various pros
        // and cons. In this example we'll show the simplest of these ways,
        // which, however, doesn't work really well in multithreaded program.
        private static SingletonThreaded _instance;

        private static readonly object _lock = new object();

        // This is the static method that controls the access to the singleton
        // instance. On the first run, it creates a singleton object and places
        // it into the static field. On subsequent runs, it returns the client
        // existing object stored in the static field.
        public static SingletonThreaded GetInstance(int area_)
        {
            if (_instance == null)
            {
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

        public int getArea() {
            this.area = 50000;
            return area;
        }
    }

}
