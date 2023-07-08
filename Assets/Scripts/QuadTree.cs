using System.Collections.Generic;
using UnityEngine;

namespace DynamicLOD
{
    public class QuadTree
    {
        private readonly QuadTreeNode _parent;
        private readonly HashSet<QuadTreeNode> _leafs;


        public QuadTree(Vector2 position, float size, int depth)
        {
            _parent = new QuadTreeNode(position, size);
            _leafs = new HashSet<QuadTreeNode>();
            _parent.Subdivide(depth);
        }

        public HashSet<QuadTreeNode> GetLeafs()
        {
            _leafs.Clear();
            AddLeafsRecursive(_parent);
            return _leafs;
        }

        private void AddLeafsRecursive(QuadTreeNode node)
        {
            if (node.IsLeaf)
            {
                _leafs.Add(node);
                return;
            }

            foreach (QuadTreeNode child in node.Children)
            {
                if (child.IsLeaf)
                {
                    _leafs.Add(child);
                }
                else
                {
                    AddLeafsRecursive(child);
                }
            }
        }

        public class QuadTreeNode
        {
            public bool IsLeaf => Children == null;

            public QuadTreeNode[] Children => _children;

            public Vector2 Position => _position;

            public float Size => _size;

            private QuadTreeNode[] _children;
            private readonly Vector2 _position;
            private readonly float _size;

            public QuadTreeNode(Vector2 position, float size)
            {
                _size = size;
                _position = position;
            }


            public void Subdivide(int depth)
            {
                if (depth == 0) return;

                _children = new QuadTreeNode[4];

                Children[0] = new QuadTreeNode(_position - new Vector2(_size, _size) * 0.5f, _size * 0.5f);
                Children[1] = new QuadTreeNode(_position - new Vector2(-_size, _size) * 0.5f, _size * 0.5f);
                Children[2] = new QuadTreeNode(_position - new Vector2(_size, -_size) * 0.5f, _size * 0.5f);
                Children[3] = new QuadTreeNode(_position - new Vector2(-_size, -_size) * 0.5f, _size * 0.5f);


                foreach (QuadTreeNode child in Children)
                {
                    child.Subdivide(depth - 1);
                }
            }
        }
    }
}