namespace myFirstAppSol.LogicLayer.BoardFolder
{
    public class Node
    {
        internal TaskCalendarModel value;
        internal Node left; 
        internal Node right;
        internal Node father;
        


        public Node(TaskCalendarModel value, Node father)
        {
            this.value = value;
            this.father = father;
        }

    }
    

    public class AVLTaskCalendar
    {
        private Node root;


        public void Insert(TaskCalendarModel model)
        {
            Console.WriteLine(model.Task.Name + " =+=+=+=+ task addING to the avl tree --------------------+++++++++++++++++++-----------");
            if (root == null)
            {
                Console.WriteLine("One root =>");
                root = new Node(model, null);
            }
            else
            {
                Insert(root, model);
            }
            Console.WriteLine(model.Task.Name + " =+=+=+=+ task added to the avl tree --------------------+++++++++++++++++++-----------");

        }


        private void Insert(Node node, TaskCalendarModel model)
        {
            if (node.value.compareTo(model) > 0) //this later than model:=>
            {
                Console.WriteLine("we want to insert " + model.Task.Name + " to the left side node is:" + node.value.Task.Name);
                if (node.left == null) {
                    node.left = new Node(model,node);
                    
                }
                else
                {
                    Insert(node.left, model);
                    printNode(node);
                    UpdateBalance(node);
                }
            }
            else
            {
                Console.WriteLine("we want to insert " + model.Task.Name + " to the Right side node is:"+node.value.Task.Name);
                if (node.right == null)
                {
                    node.right = new Node(model, node);
                    
                }
                else
                {
                    Insert(node.right, model);
                    printNode(node);
                    UpdateBalance(node);
                }
            }
        }


        private void UpdateBalance(Node node)
        {
            int balanceFactor = GetBalanceFactor(node);

            if (balanceFactor < -1)
            {
                if (GetBalanceFactor(node.left) < 0)
                {
                    Console.WriteLine("RR # "+node.value.Task.Name+" #");
                    RightRotate(node);
                    Console.WriteLine("RR # " + node.value.Task.Name + " #DONE");
                }
                else
                {
                    Console.WriteLine("RL # " + node.value.Task.Name + " #");
                    RightLeftRotate(node);
                }
            }
            else if (balanceFactor > 1)
            {
                if (GetBalanceFactor(node.right) > 0)
                {
                    Console.WriteLine("LL # " + node.value.Task.Name + " #");
                    LeftRotate(node);
                }
                else
                {
                    Console.WriteLine("LR # "+node.value.Task.Name+" #");
                    LeftRightRotate(node);
                }
            }
        }

        private void RightRotate(Node node)
        {
            Console.WriteLine("RR");
            Node leftChild = node.left;
            printNode(node);
            printNode(leftChild);
            //to move y subtree to node (right y data => left x data)
            
            
            if (leftChild.right != null)
            {
                leftChild.right.father = node;
            }
            node.left = leftChild.right;
            //after moving right y data we want to set the right side is x
            leftChild.right = node;

            //update father of y
            if (node == root)
            {
                root = leftChild;
                leftChild.father = null;
            }
            else
            {
                leftChild.father = node.father;
                //updating left side of father
                if(node.father.left==node) node.father.left = leftChild;
                else node.father.right = leftChild;
            }
            node.father = leftChild;
            Console.WriteLine("<<<<<<<<<<<< AFTER RR >>>>>>>>>>>>>>>>");
            printNode(node);
            printNode(leftChild);
            printNode(node.right);
            printNode(leftChild.right);
            printNode(node.left);
            printNode(leftChild.left);
            Console.WriteLine("<<<<<<<<<<<<>>>>>>>>>>>>>>>>");
        }


        private void printNode(Node node)
        {
            if (node != null)
            {
                Console.WriteLine($"{getNode(node)}:" + getNode(node));
                Console.WriteLine("    left:" + getNode(node.left) + "  Father:" + getNode(node.father) + "    right:" + getNode(node.right));

            }

        }
        private string getNode(Node node)
        {
            if (node == null) return "null";
            else return node.value.Task.Name;
        }

        private void RightLeftRotate(Node node)
        {
            Console.WriteLine("RL");
            
            Node y = node.left;
            Node z = y.right;
            z.father= node;
            node.left = z;
            y.father= node;
            y.right = z.left;
            //y.right = z.left;
            z.left = y;
            //node.left = z;
            
            RightRotate(node);

        }

        private void LeftRotate(Node node)
        {
            Console.WriteLine("LL");
            Node rightChild = node.right;
            printNode(node);
            printNode(rightChild);
            //to move y subtree to node (right y data => left x data)
            if (rightChild.left != null)
            {
                rightChild.left.father = node;
                
            }
            node.right = rightChild.left;


            //after moving right y data we want to set the right side is x
            rightChild.left = node;
            
            //update father of y
            if (node == root)
            {
                root = rightChild;
                rightChild.father= null;
            }
            else
            {
                rightChild.father = node.father;
                //updating left side of father
                if(node.father.left == node) node.father.left = rightChild;
                else node.father.right = rightChild;
                
            }
            node.father = rightChild;
            Console.WriteLine("<<<<<<<<<<<< AFTER LL >>>>>>>>>>>>>>>>");
            printNode(node);
            printNode(rightChild);
            printNode(node.left);
            printNode(rightChild.left);
            printNode(node.right);
            printNode(rightChild.right);
            Console.WriteLine("<<<<<<<<<<<<>>>>>>>>>>>>>>>>");

        }

        private void LeftRightRotate(Node node)
        {
            Console.WriteLine("LR");
            Node y = node.right;
            Node z = y.left;
            z.father= node;
            node.right = z;
            y.father= z;
            y.left= z.right;
            z.right= y;

            //y.left = z.right;
            //node.right = z;
            //z.right = y;
            
            LeftRotate(node);



        }

        private int GetBalanceFactor(Node node)
        {
            if (node == null)
            {
                return 0;
            }

            int leftHeight = GetHeight(node.left);
            int rightHeight = GetHeight(node.right);

            return rightHeight - leftHeight;
        }
        private int counter = 0;


        private int GetHeight(Node node)
        {

            if (node == null) return 0;
            //Console.WriteLine("get height of " + node.value.Task.Name);
            if (counter < 100)
            {
                printNode(node);
                counter++;
            }
            int leftHeight=0;
            int rightHeight = 0;
            if (node.left != null) leftHeight = GetHeight(node.left);
            
            if (node.right != null) rightHeight = GetHeight(node.right);

            return Math.Max(leftHeight, rightHeight) + 1;
        }



        public bool Search(TaskCalendarModel value)
        {
            return Search(root, value);
        }

        private bool Search(Node node, TaskCalendarModel value)
        {
            if (node == null)
            {
                return false;
            }

            if (value.compareTo(node.value) < 0)
            {
                return Search(node.left, value);
            }
            else if (value.compareTo(node.value) > 0)
            {
                return Search(node.right, value);
            }
            else
            {
                return true;
            }
        }

        public Node delete(TaskCalendarModel value)
        {
            return Delete(root, value);
        }


        private Node Delete(Node node, TaskCalendarModel value)
        {
            if (node == null)
            {
                return null;
            }

            if (value.compareTo(node.value) < 0)
            {
                node.left = Delete(node.left, value);
            }
            else if (value.compareTo(node.value) > 0)
            {
                node.right = Delete(node.right, value);
            }
            else
            {

                //found the node
                if (node.left == null && node.right == null)
                {
                    node = null;
                }
                else if (node.left == null)
                {
                    node = node.right;
                }
                else if (node.right == null)
                {
                    node = node.left;
                }
                else
                {
                    Node successor = FindMin(node.right);
                    node.value = successor.value;
                    node.right = Delete(node.right, successor.value);
                }
            }

            if (node != null)
            {
                UpdateBalance(node);
            }

            return node;
        }



        private Node FindMin(Node node)
        {
            if (node.left == null)
            {
                return node;
            }

            return FindMin(node.left);
        }


        public List<TaskCalendarModel> Read()
        {
            return ReadFromMinToMax(root);
        }
        public List<TaskCalendarModel> ReadFromMinToMax(Node node)
        {
            if (node == null)
            {
                return new List<TaskCalendarModel>();
            }

            List<TaskCalendarModel> results = new List<TaskCalendarModel>();
            results.AddRange(ReadFromMinToMax(node.left));
            results.Add(node.value);
            results.AddRange(ReadFromMinToMax(node.right));
            Console.WriteLine(results.Count + " this is the number of the elements in the avl tree ------------");
            return results;
        }

    }

    
}
