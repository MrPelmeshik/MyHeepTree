using System;
using System.Collections.Generic;
using System.Linq;

namespace MyHeapTree
{
    public class BTree
    {
        private class Node
        {
            public int value;
            public Node parent,
                left,
                right;

            public Node(Node parent, int value)
            {
                this.parent = parent;
                this.value = value;
            }

            public Node(
                Node parent,
                int value,
                Node left,
                Node right)
            {
                this.parent = parent;
                this.value = value;
                this.left = left;
                this.right = right;
            }

            public Node(Node node)
            {
                this.parent = node.parent;
                this.value = node.value;
                this.left = node.left;
                this.right = node.right;
            }
        }

        private Node _root;

        public BTree(){}

        public void IntegrationTest_1()
        {
            _root = new Node(null, 101);
            
            
            _root.left = new Node(_root, 102);
            _root.right = new Node(_root, 103);
            
            
            _root.right.left = new Node(_root.right, 104);
            _root.right.right = new Node(_root.right, 105);
            
            
            _root.right.left.left = new Node(_root.right.left, 106);
            _root.right.left.right = new Node(_root.right.left, 107);

            
            _root.right = _RotateRight(_root.right);
            _root = _RotateLeft(_root);
        }
        
        public void IntegrationTest_2()
        {
            _root = new Node(null, 101);
            
            Add(102);
            Add(103);
            
            Add(104);
            Add(105);
            Add(106);
            Add(107);
            
            Add(108);
            Add(109);
            Add(110);
            Add(111);
            Add(112);
            Add(113);
            Add(114);
            Add(115);
        }
        
        public void IntegrationTest_3()
        {
            _root = new Node(null, 115);
            
            Add(114);
            Add(113);
            
            Add(112);
            Add(111);
            Add(110);
            Add(109);
            
            Add(108);
            Add(107);
            Add(106);
            Add(105);
            Add(104);
            Add(103);
            Add(102);
            Add(101);
        }
        
        public void OutputInfo()
        {
            Console.Out.Write($"\nOutputInfo():\nУровней дерева: {_GetDepth(_root)}");
            
            OutputElement(_root);

            void OutputElement(Node node, int lvl = 0, string tab = "")
            {
                if (node == null) return;
                Console.Out.Write($"\n{lvl,3}|   {tab}{node.value,3}:{(node.parent == null ? String.Empty : node.parent.value),3}");
                OutputElement(node.left, lvl + 1, tab + " |    ");
                OutputElement(node.right, lvl + 1, tab + " |    ");
            }
        }

        public void OutputBTreeInColumn()
        {
            Console.Out.Write($"\nOutputBTreeInColumn():\nУровней дерева: {_GetDepth(_root)}");
            
            OutputElement(_root);

            void OutputElement(Node node, int lvl = 0, string tab = "")
            {
                if (node == null) return;
                Console.Out.Write($"\n{lvl,3}|   {tab}{node.value,3}");
                OutputElement(node.left, lvl + 1, tab + " |    ");
                OutputElement(node.right, lvl + 1, tab + " |    ");
            }
        }
        
        public void OutputBTree()
        {
            int depth = _GetDepth(_root);
            Console.Out.Write($"\nДерево:");
            //Console.Out.Write($"\nУровней дерева: {depth}");
            var elementsFromDepth = new List<Node> {_root};

            for (int i = 0; i < depth; i++)
            {
                var elementsFromNextDepth = new List<Node>();
                int countEmpty = (int) Math.Pow(2, depth - i) - 1;

                Console.Out.Write($"\n{i,2}|   ");
                for (int j = 0; j < 0; j++)
                {
                    Console.Out.Write("   ");
                }

                for (int j = 0; j < elementsFromDepth.Count; j++)
                {
                    OutputElement(elementsFromDepth[j] != null ? elementsFromDepth[j].value.ToString() : "", countEmpty/2);
                    if (elementsFromDepth[j] == null)
                    {
                        elementsFromNextDepth.Add(null);
                        elementsFromNextDepth.Add(null);
                    }
                    else
                    {
                        elementsFromNextDepth.Add(elementsFromDepth[j].left);
                        elementsFromNextDepth.Add(elementsFromDepth[j].right);
                    }
                }
                
                if (elementsFromNextDepth.Any(w => w != null)) 
                    elementsFromDepth = elementsFromNextDepth;
                else 
                    return;
            }

            void OutputElement(string value = "", int countEmpty = 0)
            {
                for (int i = 0; i < countEmpty; i++)
                {
                    if(i < countEmpty/2 || value == "")
                        Console.Out.Write("   ");
                    else
                        Console.Out.Write("___");
                }

                Console.Out.Write($"{value,3}");
                for (int i = 0; i < countEmpty; i++)
                {
                    if(i > countEmpty/2 || value == "")
                        Console.Out.Write("   ");
                    else
                        Console.Out.Write("___");
                }

                Console.Out.Write("   ");
            }
        }

        public void Add(int value)
        {
            OutputBTree();
            Console.Out.Write($"\nLOG>> Добавление значения {value}");
            if (_root == null)
                _root = new Node(null, value);
            else
                _root = _Add(_root, value);
            
            _root = _Balance(_root);
            _Sort(_root);
        }

        private Node _Add(Node parent, int value)
        {
            if (parent.value > value)
            {
                if (parent.left == null)
                {
                    Console.Out.Write($"\nLOG>> Добавлено значение слева: {value}");
                    parent.left = new Node(parent, value);
                }
                else
                {
                    parent.left = _Add(parent.left, value);
                    parent.left = _Balance(parent.left);
                    
                }
            }
            else
            {
                if (parent.right == null)
                {
                    Console.Out.Write($"\nLOG>> Добавлено значение справа: {value}");
                    parent.right = new Node(parent, value);
                    
                }
                else
                {
                    parent.right = _Add(parent.right, value);
                    parent.right = _Balance(parent.right);
                }
            }

            parent = _Balance(parent);
            return parent;
        }

        private Node _RotateLeft(Node node)
        {
            Console.Out.Write($"\nLOG>> Поворот влева для узла ({node.value})");

            Node newNode = new Node(
                parent: node.parent,
                value: node.right.value,
                left: null,
                right: node.right.right);
            
            Node newNodeLeft = new Node(
                parent: newNode,
                value: node.value,
                left: node.left,
                right: node.right.left);
            
            newNode.left = newNodeLeft;
            if(newNode.right != null)
                newNode.right.parent = newNode;
            if(newNode.left.right != null)
                newNode.left.right.parent = newNodeLeft;
            
            return newNode;
        }

        private Node _RotateRight(Node node)
        {
            Console.Out.Write($"\nLOG>> Поворот вправа для узла ({node.value})");

            Node newNode = new Node(
                parent: node.parent,
                value: node.left.value,
                left: node.left.left,
                right: null);
            
            Node newNodeRight = new Node(
                parent: newNode,
                value: node.value,
                left: node.left.right,
                right: node.right);
            
            newNode.right = newNodeRight;
            if(newNode.left != null)
                newNode.left.parent = newNode;
            if(newNode.right.left != null)
                newNode.right.left.parent = newNodeRight;
            
            return newNode;
        }

        private Node _Balance(Node node)
        {
            if (_GetHeight(node.right) - _GetHeight(node.left) == 2)
            {
                if (node.right != null && _GetHeight(node.right.right) - _GetHeight(node.right.left) < 0)
                    node = _RotateRight(node.right);
                node = _RotateLeft(node);
            }
            if (_GetHeight(node.right) - _GetHeight(node.left) == -2)
            {
                if (node.left != null && _GetHeight(node.left.right) - _GetHeight(node.left.left) > 0)
                    node = _RotateLeft(node.left);
                node = _RotateRight(node);
            }
            
            return node;
        }

        private void _Sort(Node node)
        {
            if (node.left == null && node.right == null) return; // это крайний элемент

            if (node.left != null)
                _Sort(node.left);
            if (node.right != null) 
                _Sort(node.right);

            if (node.left != null && node.right != null)
            {
                if (node.left.value > node.right.value)
                {
                    if (node.left.value > node.value)
                        (node.value, node.left.value) = (node.left.value, node.value);
                }
                else
                {
                    if (node.right.value > node.value)
                        (node.value, node.right.value) = (node.right.value, node.value);
                }
            }
            else if (node.left != null)
            {
                if (node.left.value > node.value)
                    (node.value, node.left.value) = (node.left.value, node.value);
            }
            else
            {
                if (node.right.value > node.value)
                    (node.value, node.right.value) = (node.right.value, node.value); 
            }


            /*int? leftValue = null, rightValue = null;
                
                if (node.left != null)
                {
                    _Sort(node.left);
                    leftValue = node.left.value;
                }
                
                if (node.right != null) 
                {
                    _Sort(node.right);
                    rightValue = node.right.value;
                }
    
                int? maxValue = null;
                if (leftValue != null && rightValue != null)
                {
                    if (leftValue > rightValue)
                    {
                        maxValue = leftValue;
                    }
                    else
                    {
                        
                    }
                }
    
    
                if (node.left != null && node.right != null)
                {
                    
                }*/

            /*if (node == null) return;
            if (node.parent == null) return;
            if (node.value <= node.parent.value) return;
            
            Console.Out.Write($"\n\nLOG>> Смена узлов {node.value} - {node.parent.value}");
            
            (node.value, node.parent.value) = (node.parent.value, node.value);*/
        }
        
        private int _GetHeight(Node node)
        {
            if (node == null) return 0;
            int leftNodeHeight = _GetHeight(node.left);
            int rightNodeHeight = _GetHeight(node.right);

            return (leftNodeHeight > rightNodeHeight ? leftNodeHeight : rightNodeHeight) + 1;
        }

        private int _GetDepth(Node root)
        {
            if (root == null) return 0;
            int depth = 0;
            var nodes = new Queue<Node>();
            nodes.Enqueue(root);
            do
            {
                depth++;
                var nextNodes = new Queue<Node>();
                do
                {
                    var node = nodes.Dequeue();
                    if (node.left != null)
                        nextNodes.Enqueue(node.left);
                    if (node.right != null)
                        nextNodes.Enqueue(node.right);
                } while (nodes.Any());

                nodes = nextNodes;
            } while (nodes.Any());

            return depth;
        }
    }

    public class MaxHeapTree
    {
        private int[] _tree;
        private int _size;
        private int _depth;

        public MaxHeapTree(int[] arr)
        {
            _tree = arr;
            _size = _tree.Length;
            _depth = (int) Math.Sqrt(_size);
            _SortNode();
            NLR();
        }
        
        private MaxHeapTree(){}

        public void Test()
        {
            Console.Out.Write($"\n\nИтоговое дерево:");
            Output();

            Console.Out.Write($"\n\nОбход дерева (NLR):");
            NLR();
        }

        public static int[] SortArray(int[] arr)
        {
            MaxHeapTree maxHeapTree = new MaxHeapTree(arr);
            maxHeapTree.Output();
            
            int size = maxHeapTree._size;
            for (int i = 0; i < size; i++)
            {
                maxHeapTree._SortNode();
                (maxHeapTree._tree[0], maxHeapTree._tree[size - i - 1]) =
                    (maxHeapTree._tree[size - i - 1], maxHeapTree._tree[0]);
                maxHeapTree._size--;
            }

            maxHeapTree._size = size;

            return maxHeapTree._tree;
        }

        public void Output()
        {
            Console.Out.Write($"\nДерево:");
            Console.Out.Write($"\n\tУровней дерева: {_depth}");
            Console.Out.Write($"\n\tЭлементов в дереве: {_size}");
            Console.Out.WriteLine("");
            
            var elementsFromDepth = new List<int> {0};
            
            OutputElement(elementsFromDepth);

            void OutputElement(List<int> elementsFromDepth, int nowDepth = 0)
            {
                int countEmpty = (int) Math.Pow(2, _depth - nowDepth) - 1;
                var elementsFromNextDepth = new List<int>();
                
                foreach (var elementFromDepth in elementsFromDepth)
                {
                    for (int i = 0; i < countEmpty; i++)
                    {
                        if (i < countEmpty / 2)
                            Console.Out.Write("   ");
                        else
                            Console.Out.Write("___");
                    }

                    Console.Out.Write($"{_tree[elementFromDepth],3}");
                    for (int i = 0; i < countEmpty; i++)
                    {
                        if (i > countEmpty / 2)
                            Console.Out.Write("   ");
                        else
                            Console.Out.Write("___");
                    }

                    Console.Out.Write("   ");

                    int i_l = _GetIndexLeft(elementFromDepth);
                    int i_r = _GetIndexRight(elementFromDepth);
                    if (i_l < _size)
                    {
                        elementsFromNextDepth.Add(i_l);
                        if(i_r < _size)
                            elementsFromNextDepth.Add(i_r);
                    }
                }

                Console.Out.WriteLine("");
                if(elementsFromNextDepth.Any()) 
                    OutputElement(elementsFromNextDepth, nowDepth + 1);

            }
        }

        public void NLR(int i = 0)
        {
            if (i == 0)
                Console.Out.Write("\nNLR:\n\t");
            if (i >= _size) return;
            Console.Out.Write($"{_tree[i],3} ");
            NLR(_GetIndexLeft(i));
            NLR(_GetIndexRight(i));
        }

        private int _GetIndexLeft(int i) => 2 * i + 1;
        private int _GetIndexRight(int i) => 2 * i + 2;

        private void _SortNode(int i = 0)
        {
            if (i >= _size) return;

            int i_l = _GetIndexLeft(i);
            int i_r = _GetIndexRight(i);
            
            _SortNode(i_l);
            _SortNode(i_r);
            
            if (i_l < _size && i_r < _size)
            {
                if (_tree[i_l] > _tree[i_r])
                {
                    if (_tree[i_l] > _tree[i])
                    {
                        _Swap(i, i_l);
                        _SortNode(i_l);
                    }
                }
                else
                {
                    if (_tree[i_r] > _tree[i])
                    {
                        _Swap(i, i_r);
                        _SortNode(i_r);
                    }
                }
            }
            else if (i_l < _size)
            {
                if (_tree[i_l] > _tree[i])
                {
                    _Swap(i, i_l);
                    _SortNode(i_l);
                }
            } 
            else if (i_r < _size)
            {
                if (_tree[i_r] > _tree[i])
                {
                    _Swap(i, i_r);
                    _SortNode(i_r);
                }
            }
        }

        private void _Swap(int i, int j)
        {
            /*Console.Out.Write($"\n\nЗамена {_tree[i]} на {_tree[j]}");*/
            (_tree[i], _tree[j]) = (_tree[j], _tree[i]);
            /*Console.Out.Write($"\nПромежуточный результат:");
            Output();*/
        }
    }
}