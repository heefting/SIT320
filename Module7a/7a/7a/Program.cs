using System;
using System.Collections.Generic;

namespace _7a
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // --

            /*
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
            */

            Tree treeStructure = new Tree();

            treeStructure.Insert(1);
            treeStructure.Insert(77);
            treeStructure.Insert(55);
            treeStructure.Insert(24);
            treeStructure.Insert(21);
            treeStructure.Insert(5);
            treeStructure.Insert(12);

            treeStructure.Print();


            
            Graph graph = new Graph(new BFS());



            GraphNode new_node = graph.Insert(1);
            GraphNode new_node1 = graph.Insert(77);
            GraphNode new_node2 = graph.Insert(55);
            GraphNode new_node3 = graph.Insert(24);
            GraphNode new_node4 = graph.Insert(21);
            GraphNode new_node5 = graph.Insert(5);
            GraphNode new_node6 = graph.Insert(12);

            graph.addDiEdge(new_node1, new_node);
            graph.addDiEdge(new_node3, new_node6);
            graph.addDiEdge(new_node5, new_node1);
            graph.addDiEdge(new_node2, new_node3);
            graph.addDiEdge(new_node3, new_node5);
            graph.addDiEdge(new_node1, new_node6);
            graph.addDiEdge(new_node4, new_node2);
            graph.addDiEdge(new_node5, new_node1);
            graph.addDiEdge(new_node6, new_node4);
            graph.addDiEdge(new_node, new_node4);

            graph.Print();



            //DataClass dataClass = new DataClass(treeStructure);


        }
    }

    /*
    class DataClass
    {
        DataStructure dataStructure;

        public DataClass(DataStructure dataStructure)
        {
            this.dataStructure = dataStructure;
        }
    }
    */
    ///*
    abstract class DataStructure
    {
        //abstract public Node Insert(int value);

        abstract public void Delete(int value);

        abstract public Node Search(int value);

        abstract public void Print();
        
    } 
    //*/

    class GraphNode : Node
    {
        public List<GraphNode> inNeighbors = new List<GraphNode>();
        public List<GraphNode> outNeighbors = new List<GraphNode>();
        public string status = "unvisited";


        public GraphNode(int value) : base(value)
        {

        }

        public List<GraphNode> getOutNeighbors()
        {
            return this.outNeighbors;
        }

        public List<GraphNode> getInNeighbors()
        {
            return this.inNeighbors;
        }

        public void addInNeighbor(GraphNode v)
        {
            this.inNeighbors.Add(v);
        }

        public void addOutNeighbor(GraphNode v)
        {
            this.outNeighbors.Add(v);
        }
    }


    class Graph : DataStructure
    {
        public List<GraphNode> vertices = new List<GraphNode>();

        private GSearchDelegate gSearchDelegate;

        public Graph(GSearchDelegate gSearchDelegate)
        {
            this.gSearchDelegate = gSearchDelegate;
        }

        public override void Delete(int value)
        {
            throw new NotImplementedException();
        }

        public GraphNode Insert(int value)
        {
            GraphNode node = new GraphNode(value);
            this.vertices.Add(node);

            return node;
        }

        public void addDiEdge(GraphNode u, GraphNode v)
        {
            u.addOutNeighbor(v);
            v.addInNeighbor(u);
        }

        public void addBiEdge(GraphNode u, GraphNode v)
        {
            this.addDiEdge(u, v);
            this.addDiEdge(v, u);
        }

        private List<(GraphNode, GraphNode)> getDirEdges()
        {
            List<(GraphNode, GraphNode)> ret = new List<(GraphNode, GraphNode)>();
            foreach (GraphNode v in this.vertices)
            {
                foreach (GraphNode u in v.getOutNeighbors())
                {
                    ret.Add((v, u));
                }
                
            }
            return ret;
        }

        public override void Print()
        {
            string printOut = "";
            foreach (GraphNode vert in this.vertices)
            {
                printOut += " - " + vert.value.ToString();
            }
            Console.WriteLine(printOut);
        }

        public List<GraphNode> Traverse(GraphNode node)
        {
            List<GraphNode> nodeList = this.gSearchDelegate.Search(node, this);

            return nodeList;
        }

        public override Node Search(int value)
        {
            GraphNode node = null;
            foreach (GraphNode vert in this.vertices)
            {
                if (vert.value == value)
                {
                    node = vert;
                    break;
                }
            }

            if (node == null)
            {
                Console.WriteLine("Node of this value does not exist");
                return null;
            }

            return node;
        }
    }

    abstract class GSearchDelegate
    {
        abstract public List<GraphNode> Search(GraphNode node, Graph graph);
    }

    class BFS : GSearchDelegate
    {
        public override List<GraphNode> Search(GraphNode node, Graph graph)
        {

            // Set to unvisited
            foreach (GraphNode gNode in graph.vertices)
            {
                gNode.status = "unvisited";
            }

            node.status = "visited";

            int n = graph.vertices.Count;

            List<GraphNode>[] queue = new List<GraphNode>[n];

            List<GraphNode> ret = new List<GraphNode>();

            //queue.Add(node);

            queue[0].Add(node);

            for (int i = 0; i < n; i ++)
            {
                for (int uIdx = 0; uIdx < queue[i].Count; uIdx ++)
                {
                    GraphNode u = queue[i][uIdx];

                    foreach (GraphNode v in u.getOutNeighbors())
                    {
                        if (v.status == "unvisited")
                        {
                            v.status = "visited";
                            queue[i + 1].Add(v);
                            ret.Add(v);
                        }
                    }
                }
            }

            string printOut = "";
            foreach (List<GraphNode> verts in queue)
            {
                printOut += "\n";
                foreach (GraphNode vert in verts)
                {
                    printOut += " - " + vert.value.ToString();
                }
                printOut += "-";
            }
            Console.WriteLine(printOut);


            return ret;
        }
    }

    class DFS : GSearchDelegate
    {
        public override List<GraphNode> Search(GraphNode node, Graph graph)
        {
            List<GraphNode> nodes = new List<GraphNode>();
            nodes.Add(node);
            return nodes;
        }
    }

    class Tree : DataStructure
    {

        TreeNode root;

        public override void Print()
        {
            this.RecPrint(root);
            Console.WriteLine("--");
        }

        public List<Node> Traverse(Node node)
        {
            TreeNode treeNode = (TreeNode) node;

            List<Node> nodeList = new List<Node>();

            if (treeNode.left != null)
            {
                nodeList.AddRange(this.Traverse(treeNode.left));
            }

            if (treeNode.right != null)
            {
                nodeList.AddRange(this.Traverse(treeNode.right));
            }


            return nodeList;
        }

        private void RecPrint(TreeNode root_)
        {
            if (root_ != null)
            {
                this.RecPrint(root_.left);
                Console.Write("-{0}-", root_.value);
                this.RecPrint(root_.right);
            }
        }


        public override Node Search(int value)
        {
            TreeNode searchResult = this.RecSearch(this.root, value);

            if (searchResult.value == value)
            {
                return searchResult;
            }

            return null;
        }

        private TreeNode RecSearch(TreeNode root_, int value)
        {
            if (value < root_.value)
            {
                if (value == root_.value)
                {
                    return root_;
                }
                return this.RecSearch(root_.left, value);
            }
            else
            {
                if (value == root_.value)
                {
                    return root_;
                }
                return this.RecSearch(root_.right, value);
            }
            

        }


        public override void Delete(int value)
        {
            this.RecDelete(this.root, value);
        }

        private TreeNode RecDelete(TreeNode root_, int value)
        {
            TreeNode parent;
            if (root_ == null)
            {
                return root_;
            }
            else
            {
                // Left
                if (value < root_.value)
                {
                    root_.left = this.RecDelete(root_.left, value);
                    if (this.getbalance(root_) == -2)
                    {
                        if (this.getbalance(root_.right) <= 0)
                        {
                            // Right right
                            root_ = this.leftRotate(root_);
                        }
                        else
                        {
                            // Right left
                            root_ = this.rightRotate(root_.right);
                            root_ = this.leftRotate(root_);
                        }
                    }
                }
                else if (value > root_.value)
                {
                    root_.right = this.RecDelete(root_.right, value);
                    if (this.getbalance(root_) == 2)
                    {
                        if (this.getbalance(root_.left) >= 0)
                        {
                            // Left left
                            root_ = this.rightRotate(root_);
                        }
                        else
                        {
                            // Left right
                            root_ = this.leftRotate(root_.left);
                            root_ = this.rightRotate(root_);
                        }
                    }
                }
                else
                {
                    if (root_.right == null)
                    {
                        return root_.left;
                    }
                    else
                    {
                        parent = root_.right;
                        while (parent.left != null)
                        {
                            parent = parent.left;
                        }

                        root_.value = parent.value;
                        root_.right = this.RecDelete(root_.right, parent.value);

                        if (this.getbalance(root_) == 2)
                        {
                            if (this.getbalance(root_.left) >= 0)
                            {
                                // Left left
                                root_ = this.rightRotate(root_);
                            }
                            else
                            {
                                // Left right
                                root_ = this.leftRotate(root_.left);
                                root_ = this.rightRotate(root_);
                            }
                        }
                    }
                }
            }

            return root_;
        }

        public TreeNode Insert(int value)
        {
            TreeNode node = new TreeNode(value);

            TreeNode ret = this.root = this.RecInsert(this.root, node);

            return ret;

        }

        private TreeNode RecInsert(TreeNode root_, TreeNode node)
        {
            if (root_ == null)
            {
                return node;
            }
            else if (node.value < root_.value)
            {
                root_.left = this.RecInsert(root_.left, node);
            }
            else
            {
                root_.right = this.RecInsert(root_.right, node);
            }

            root_.height = 1 + Math.Max(this.getHeight(root_.left), this.getHeight(root_.right));

            int balance = this.getbalance(root_);

            if (balance > 1 && node.value < root_.left.value)
            {
                return this.rightRotate(root_);
            }

            if (balance < -1 && node.value < root_.right.value)
            {
                return this.leftRotate(root_);
            }

            if (balance > 1 && node.value < root_.left.value)
            {
                root_.left = this.leftRotate(root_.left);
                return this.rightRotate(root_);
            }

            if (balance < -1 && node.value < root_.right.value)
            {
                root_.right = this.leftRotate(root_.right);
                return this.leftRotate(root_);
            }




            return root_;
        }


        private int getHeight(TreeNode root_)
        {
            if (root_ == null)
            {
                return 0;
            }
            return root_.height;
        }

        private int getbalance(TreeNode root_)
        {
            if (root_ == null)
            {
                return 0;
            }
            return this.getHeight(root_.left) - this.getHeight(root_.right);
        }

        private TreeNode leftRotate(TreeNode z)
        {
            TreeNode y = z.right;
            TreeNode T2 = y.left;

            // Rotate
            y.left = z;
            z.right = T2;

            // Update Heights
            z.height = 1 + Math.Max(this.getHeight(z.left), this.getHeight(z.right));
            y.height = 1 + Math.Max(this.getHeight(y.left), this.getHeight(y.right));


            return y;
        }

        private TreeNode rightRotate(TreeNode z)
        {
            TreeNode y = z.left;
            TreeNode T3 = y.right;

            // Rotate
            y.right = z;
            z.left = T3;

            // Update Heights
            z.height = 1 + Math.Max(this.getHeight(z.left), this.getHeight(z.right));
            y.height = 1 + Math.Max(this.getHeight(y.left), this.getHeight(y.right));


            return y;
        }

        
    }


    ///*
    abstract class Node
    {
        public int value;

        protected Node(int value)
        {
            this.value = value;
        }
    }

    class TreeNode : Node
    {


        public TreeNode left;
        public TreeNode right;
        public int height = 1;

        public TreeNode(int value) : base(value)
        {
        }
    }


    //*/



    // ----------------------------------------- OTHER EXAMPLE ----------------------------------------- //


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



