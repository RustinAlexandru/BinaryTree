using System;

namespace BinaryTree
{
    class BinaryTreeNode<E>
    {
        private E element;
        private BinaryTreeNode<E> parent;
        private BinaryTreeNode<E> left;
        private BinaryTreeNode<E> right;

        public BinaryTreeNode(E e, BinaryTreeNode<E> above, BinaryTreeNode<E> leftChild, BinaryTreeNode<E> rightChild)
        {
            this.element = e;
            this.parent = above;
            this.left = leftChild;
            this.right = rightChild;
        }



        public E Element
        {
            get
            {
                return element;
            }
            set
            {
                element = value;
            }
        }

        public BinaryTreeNode<E> Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }

        public BinaryTreeNode<E> Left
        {
            get
            {
                return left;
            }
            set
            {
                left = value;
            }
        }

        public BinaryTreeNode<E> Right
        {
            get
            {
                return right;
            }
            set
            {
                right = value;
            }
        }


        public bool IsLeaf()
        {
            return (this.CountChildNodes() == 0);
        }

        public bool IsLeftLeaf()
        {
            return (this.Parent != null && this.Parent.Left == this);
        }

        public bool IsRightLeaf()
        {
            return (this.Parent != null && this.Parent.Right == this);
        }

        public bool HasLeftChild()
        {
            return (this.Left != null);
        }


        public bool HasRightChild()
        {
            return (this.Right != null); 
        }

        public int CountChildNodes()
        {
            int count = 0;
            if (this.Left != null)
                count++;
            if (this.Right != null)
                count++;
            return count;
        }
    }



    class BinaryTree<E> where E: IComparable
    {
        private BinaryTreeNode<E> root;
        private int size;

        public BinaryTree()
        {
            this.root = null;
            this.size = 0;
        }

        public BinaryTreeNode<E> GetRoot()
        {
            return root;
        }

        public int Size()
        {
            return size;
        }

        public bool IsEmpty()
        {
            return size == 0;
        }

        private int CountNodes(BinaryTreeNode<E> root)
        {
            if (root == null)
            {
                return 0;
            }
            else 
            {
                int count = 1;
                count += CountNodes(root.Left);
                count += CountNodes(root.Right);
                return count;
            }
        }

        public int CountTreeNodes()
        {
            return CountNodes(this.root);  
        }



        // return node or node's element??
        private BinaryTreeNode<E> FindNode(BinaryTreeNode<E> root, E value)
        {
            if (root == null)
            {
                Console.WriteLine("Value not found in the tree!");
                return null;
            }
            if (root.Element.CompareTo(value) == 0) 
            {
                Console.WriteLine("Value found in the tree!");
                return root;
            }

            if (value.CompareTo(root.Element)  < 0)
                return FindNode(root.Left, value);
            else
                return FindNode(root.Right, value);


        }

        public BinaryTreeNode<E> Find(E value)
        {
            return FindNode(this.root, value);
        }

        private bool ContainsNode(BinaryTreeNode<E> root, E value)
        {
            if (root == null)
            {
                return false;
            }
            if (root.Element.CompareTo(value) == 0) 
            {
                return true;
            }

            if (value.CompareTo(root.Element)  < 0)
                return FindNode(root.Left, value);
            else
                return FindNode(root.Right, value);


        }

        public BinaryTreeNode<E> Contains(BinaryTreeNode<E> nodeSearched)
        {
            return ContainsNode(this.root, nodeSearched.Element);
        }

        // Insertions, deletions
        private BinaryTreeNode<E> InsertNode(E value, BinaryTreeNode<E> t)
        {
            // recursion  base case
            if (t == null)
            {
                t = new BinaryTreeNode<E>(value,null,null,null);
                size++;

            } // left tree recursion
            else if ((value as IComparable).CompareTo(t.Element) < 0)
            {
                t.Left = InsertNode(value, t.Left);
                t.Left.Parent = t;

            } // right tree recursion
            else if ((value as IComparable).CompareTo(t.Element) > 0)
            {
                t.Right = InsertNode(value, t.Right);
                t.Right.Parent = t;

            }
            else
            {
                throw new Exception("Duplicate item");
            }


            return t;
        }

        public void Insert(E value) 
        {

            this.root = InsertNode(value,this.root);
        }

        public bool RemoveNode(BinaryTreeNode<E> removedNode)
        {
            // Check if node to be deleted is in tree or not
            if (this.Contains(removedNode) == null || removedNode == null)
                return false; 

            bool wasHead = (removedNode == this.root);
            // Node to be removed is the root 
            if (this.CountTreeNodes() == 1)
            {
                this.root = null;
                this.size--;
            }  
            else if (removedNode.IsLeaf())  // Case 1: node is leaf
            {
                if (removedNode.IsLeftLeaf())
                {
                    removedNode.Parent.Left = null;
                }
                else
                {
                    removedNode.Parent.Right = null;
                }
                removedNode.Parent = null;
                size--;
                    
            }
            else if (removedNode.CountChildNodes() == 1) // Case 2: node has one leaf
            {
                if (removedNode.HasLeftChild())
                {
                    removedNode.Left.Parent = removedNode.Parent;
                

                    if (wasHead)
                    {
                        this.root = removedNode.Left;
                    }

                    if (removedNode.IsLeftLeaf())
                    {
                        removedNode.Parent.Left = removedNode.Left;
                    }
                    else
                    {
                        removedNode.Parent.Right = removedNode.Left;
                    }
                }
                else
                {
                    removedNode.Right.Parent = removedNode.Parent;


                    if (wasHead)
                    {
                        this.root = removedNode.Right;
                    }

                    if (removedNode.IsLeftLeaf())
                    {
                        removedNode.Parent.Left = removedNode.Right;
                    }
                    else
                    {
                        removedNode.Parent.Right = removedNode.Right;
                    }
                }

                removedNode.Parent = null;
                removedNode.Left = null;
                removedNode.Right = null;
                size--;

            }
            else  // Case 3: node has two leaves
            {
                // Find right most node in the left subtree
                BinaryTreeNode<E> succesor = removedNode.Left;
                while (succesor.Right != null)
                {
                    succesor = succesor.Right;
                }

                // Replace node to be removed with the value found and delete duplicate
                removedNode.Element = succesor.Element;
                this.RemoveNode(succesor);
            }
            return true;

        }

        public  bool Remove(E value)
        {
            BinaryTreeNode<E> removeNode = Find(value);

            return this.RemoveNode(removeNode);
        }

        // Tree traversals
        private void InOrder(BinaryTreeNode<E> root)
        {
            if (root != null)
            {
                InOrder(root.Left);
                Console.WriteLine(root.Element + " ");
                InOrder(root.Right);
            }
        }

        public void InOrderPrint()
        {
            InOrder(this.root);
        }

        private void PreOrder(BinaryTreeNode<E> root)
        {
            if (root != null)
            {
                Console.WriteLine(root.Element + " ");
                PreOrder(root.Left);
                PreOrder(root.Right);
            }
        }

        public void PreOrderPrint()
        {
            PreOrder(this.root);
        }

        private void PostOrder(BinaryTreeNode<E> root)
        {
            if (root != null)
            {
                PostOrder(root.Left);
                PostOrder(root.Right);
                Console.WriteLine(root.Element + " ");
            }
        }

        public void PostOrderPrint()
        {
            PostOrder(this.root);
        }

    }


    class MainClass
    {
        public static void Main(string[] args)
        {
            BinaryTree<int> myTree = new BinaryTree<int>();

            myTree.Insert(5);
            myTree.Insert(6);
            myTree.Insert(3);
            myTree.Insert(4);
            myTree.Insert(1);
            //myTree.InOrderPrint();
            //myTree.PreOrderPrint();
            //myTree.PostOrderPrint();
         //   Console.WriteLine("My tree has:" + myTree.CountTreeNodes() + " nodes!");
            // Console.WriteLine(myTree.FindNode(0).Element);
            //Console.WriteLine(myTree.GetRoot().Left.IsLeaf());
            //myTree.Remove(1);
            //myTree.Remove(4);
            //myTree.Remove(3);
            myTree.Remove(3);


        }
    }
}
