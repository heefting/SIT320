using System;
using System.Collections.Generic;

namespace _7._3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // --

            Console.WriteLine("A Gun \n\n");
            Projectile gun_ammoType = new Bullet();
            Weapon gun = new Gun(gun_ammoType);
            for (int i = 0; i < 10; i++)
            {
                gun.Fire();
            }

            Console.WriteLine("\n\n A Machine Gun \n\n");
            Projectile mm_ammoType = new Bullet();
            Weapon machineGun = new MachineGun(mm_ammoType);
            for (int i = 0; i < 10; i++)
            {
                machineGun.Fire();
            }

            Console.WriteLine("\n\n A Rocket Launcher \n\n");
            Projectile rl_ammoType = new Rocket();
            Weapon rocketLauncher = new RocketLauncher(rl_ammoType, 5);
            for (int i = 0; i < 10; i++)
            {
                rocketLauncher.Fire();
            }


        }
    }


    abstract class Projectile
    {
        abstract public void Spawn();
        abstract public void Hit();
    }


    class Bullet : Projectile
    {

        override public void Spawn()
        {
            Console.WriteLine("The bullet flys out of the barrel of the gun...");
            this.Hit();
        }

        override public void Hit()
        {
            Console.WriteLine("Ping, the bullet deflects of a surface");
        }

    }

    class Rocket : Projectile
    {

        override public void Spawn()
        {
            Console.WriteLine("The rocket flys through the air leaving a trail of smoke behind it...");
            this.Hit();
        }

        override public void Hit()
        {
            Console.WriteLine("BOOM, the rocket EXPLODES!");
        }

    }

    class Rocket2 : Rocket
    {

        override public void Spawn()
        {
            Console.WriteLine("Rocket2?");
            this.Hit();
        }

        override public void Hit()
        {
            Console.WriteLine("BOOM, the rocket EXPLODES!");
        }

    }

    class LaserBeam : Projectile
    {

        override public void Spawn()
        {
            Console.WriteLine("PEW PEW the laser fires and ...");
            this.Hit();
        }

        override public void Hit()
        {
            Console.WriteLine("Instantly hits ...");
        }

    }


    abstract class Weapon
    {

        protected Projectile projectile;

        int ammoRemaining = 4;
        int clip = 2;
        int clipSize = 2;

        List<Projectile> projectiles = new List<Projectile>();

        public Weapon(Projectile projectile)
        {
            Console.WriteLine("Constructor of Weapon");
            this.projectile = projectile;
        }

        virtual public bool Fire()
        {
            Console.WriteLine("Weapon Fires");

            bool didFire = false;

            if (this.clip > 0)
            {
                Console.WriteLine("Bang!");
                Projectile newProjectile = (Projectile)Activator.CreateInstance(this.projectile.GetType());
                newProjectile.Spawn();
                this.projectiles.Add(newProjectile);

                this.clip -= 1;

                didFire = true;
            }
            else if (this.ammoRemaining > 0)
            {
                this.Reload();
            }
            else
            {
                Console.WriteLine("Click. Click ...");
            }

            return didFire;
        }

        virtual public void Reload()
        {
            Console.WriteLine("Reload ...");
            if (this.ammoRemaining < this.clipSize)
            {
                this.clip = this.ammoRemaining;
                this.ammoRemaining = 0;
            }
            else
            {
                this.clip = this.clipSize;
                this.ammoRemaining -= this.clipSize;
            }
        }

    }

    class Gun : Weapon
    {

        public Gun(Projectile projectile) : base(projectile)
        {
            Console.WriteLine("I am a Gun");
        }

    }

    class RocketLauncher : Weapon
    {
        int explosionRadius = 5;

        public RocketLauncher(Projectile projectile, int explosionRadius) : base(projectile)
        {
            Console.WriteLine("I am a Rocket Launcher");
            this.explosionRadius = explosionRadius;
        }

        public override bool Fire()
        {
            bool didFire = base.Fire();

            if (didFire)
            {
                Console.WriteLine("The Rocket flys forward");
            }

            return didFire;
        }

    }

    class MachineGun : Weapon
    {

        public MachineGun(Projectile projectile) : base(projectile)
        {
            Console.WriteLine("I am a Machine Gun");
        }

        public override bool Fire()
        {
            Console.WriteLine("Rapid fire..");

            bool didFire = base.Fire();

            Console.WriteLine("..");

            didFire = base.Fire();

            return true;
        }

    }

}
