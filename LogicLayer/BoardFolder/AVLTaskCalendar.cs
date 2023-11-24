namespace myFirstAppSol.LogicLayer.BoardFolder
{
    public class Node
    {
        internal TaskCalendarModel value;
        internal Node left; 
        internal Node right;


        public Node(TaskCalendarModel value, Node left, Node right)
        {
            this.value = value;
            this.left = left;
            this.right = right;
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
                root = new Node(model, null, null);
            }
            else
            {
                Insert(root, model);
            }
            Console.WriteLine(model.Task.Name + " =+=+=+=+ task added to the avl tree --------------------+++++++++++++++++++-----------");

        }


        private void Insert(Node node, TaskCalendarModel model)
        {
            if (node.value.compareTo(model) > 0)
            {
                if (node.left == null) {
                    node.left = new Node(model,null,null);
                    UpdateBalance(node);
                }
                else
                {
                    Insert(node.left, model);
                    
                }
            }
            else
            {
                if (node.right == null)
                {
                    node.right = new Node(model, null, null);
                    UpdateBalance(node);
                }
                else
                {
                    Insert(node.right, model);
                    
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
                    Console.WriteLine("RR");
                    RightRotate(node);
                }
                else
                {
                    Console.WriteLine("RL");
                    RightLeftRotate(node);
                }
            }
            else if (balanceFactor > 1)
            {
                if (GetBalanceFactor(node.right) > 0)
                {
                    Console.WriteLine("LL");
                    LeftRotate(node);
                }
                else
                {
                    Console.WriteLine("LR");
                    LeftRightRotate(node);
                }
            }
        }

        private void RightRotate(Node node)
        {
            Console.WriteLine("RR");
            Node leftChild = node.left;
            node.left = leftChild.right;
            leftChild.right = node;

            UpdateBalance(node);
            UpdateBalance(leftChild);
        }

        private void RightLeftRotate(Node node)
        {
            Console.WriteLine("RL");
            LeftRotate(node.right);
            RightRotate(node);
        }

        private void LeftRotate(Node node)
        {
            Console.WriteLine("LL");
            Node rightChild = node.right;
            node.right = rightChild.left;
            rightChild.left = node;

            UpdateBalance(node);
            UpdateBalance(rightChild);
        }

        private void LeftRightRotate(Node node)
        {
            Console.WriteLine("LR");
            RightRotate(node.left);
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



        private int GetHeight(Node node)
        {
            if (node == null) return 0;
            int leftHeight = GetHeight(node.left);
            int rightHeight = GetHeight(node.right);

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
