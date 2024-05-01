namespace CodingSolutions.Cache.MultiEvictionPolicyCache
{
    public class BinaryTreeNode
    {
        public string Key;
        public int Value;
        public BinaryTreeNode Left;
        public BinaryTreeNode Right;

        public BinaryTreeNode(string key, int value)
        {
            Key = key;
            Value = value;
        }
    }

    public class BinaryTree
    {
        public BinaryTreeNode binaryTreeNode;

        public void Insert(int val, string key)
        {
            if (binaryTreeNode == null)
            {
                binaryTreeNode = new BinaryTreeNode(key, val);
            }
            else
            {
                var current = binaryTreeNode;
                while (current != null)
                {
                    // insert into the right subtree
                    if (val > current.Value)
                    {
                        // insert right now
                        if (current.Right == null)
                        {
                            current.Right = new BinaryTreeNode(key, val);
                            break;
                        }
                        else
                        {
                            current = current.Right;
                        }
                    }
                    else
                    {
                        // insert left now
                        if (current.Left == null)
                        {
                            current.Left = new BinaryTreeNode(key, val);
                            break;
                        }
                        else
                        {
                            current = current.Left;
                        }
                    }
                }
            }
        }

        public void Update(int val, string key)
        {
            DeleteFromBinaryTree(val, key);
            Insert(val, key);
        }

        public void DeleteFromBinaryTree(int value, string key)
        {
            DeleteNode(binaryTreeNode, value, key);
        }

        public BinaryTreeNode SearchForValueLessThen(int val)
        {
            var current = binaryTreeNode;
            while (current != null && val! < current.Value)
            {
                current = val < current.Value ? current.Left : current.Right;
            }
            return current;
        }

        private BinaryTreeNode DeleteNode(BinaryTreeNode root, int value, string key)
        {
            if (root == null)
                return root;

            if (value < root.Value)
            {
                root.Left = DeleteNode(root.Left, value, key);
            }
            else if (value > root.Value)
            {
                root.Right = DeleteNode(root.Right, value, key);
            }
            else if (value == root.Value && root.Key == key)
            {
                // Value is equal to root
                if (root.Left == null && root.Right == null)
                {
                    root = null;
                }
                else if (root.Left == null || root.Right == null)
                {
                    BinaryTreeNode ret = root.Left != null ? root.Left : root.Right;
                    root = ret;
                }
                else
                {
                    //finding the left most node of right
                    BinaryTreeNode tmp = root.Right;
                    while (tmp.Left != null)
                    {
                        tmp = tmp.Left;
                    }
                    root.Value = tmp.Value;
                    root.Key = tmp.Key;
                    root.Right = DeleteNode(root.Right, tmp.Value, key);
                }
            }
            else
            {
                DeleteNode(root.Left, value, key);
            }

            return root;
        }
    }
}