using System.Collections.Generic;
using UnityEngine;

namespace DynamicLOD
{
    public enum QuadTreeSide : byte
    {
        TopLeft = 0,
        TopRight = 1,
        BottomLeft = 2,
        BottomRight = 3,
    }

    public class QuadTree
    {
        private readonly QuadTreeNode _parent;
        private readonly HashSet<QuadTreeNode> _leafs;

        public QuadTree(Vector2 position, float size, int depth)
        {
            _parent = new QuadTreeNode(position, size * 0.5f);
            _leafs = new HashSet<QuadTreeNode>();
            _parent.Subdivide(depth);
        }

        public HashSet<QuadTreeNode> GetLeafs()
        {
            _leafs.Clear();
            AddLeafsRecursive(_parent);
            return _leafs;
        }

        public void Insert(Vector2 position)
        {
            _parent.Insert(position, 7);
        }

        private void AddLeafsRecursive(QuadTreeNode node)
        {
            if (node is null) return;
            
            if (node.IsLeaf)
            {
                _leafs.Add(node);
                return;
            }

            foreach (QuadTreeNode child in node.Children)
            {
                if(child is null) 
                    continue;
                
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

        private static QuadTreeSide GetBoundedSide(Vector2 lookUpPosition, Vector2 position)
        {
            int index = 0;
            index |= lookUpPosition.y > position.y ? 2 : 0;
            index |= lookUpPosition.x > position.x ? 1 : 0;
            return (QuadTreeSide)index;
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

            public void Insert(Vector2 position, int depth)
            {
                QuadTreeSide quadTreeSide = GetBoundedSide(_position, position);

                if (IsLeaf)
                    this.Subdivide(0);
                
                for (byte i = 0; i < _children.Length; i++)
                {
                    Vector2 newPosition = _position;
                    newPosition.y += (IsBottomBitSet(i) ? (-_size) : (_size)) * 0.5f;
                    newPosition.x += (IsRightBitSet(i) ? (-_size) : (_size)) * 0.5f;

                    if (quadTreeSide == (QuadTreeSide)i && depth > 0)
                    {
                        Children[i].Insert(position, depth - 1);
                    }
                }
            }

            public void Subdivide(int depth)
            {
                _children = new QuadTreeNode[4];

                for (byte i = 0; i < _children.Length; i++)
                {
                    Vector2 newPosition = _position;
                    newPosition.y += (IsBottomBitSet(i) ? (-_size) : (_size)) * 0.5f;
                    newPosition.x += (IsRightBitSet(i) ? (-_size) : (_size)) * 0.5f;
                    Children[i] = new QuadTreeNode(newPosition, _size * 0.5f);
                }

                if (depth == 0) return;

                foreach (QuadTreeNode child in Children)
                {
                    child.Subdivide(depth - 1);
                }
            }

            private bool IsBottomBitSet(byte i) => (i & 2) == 2;
            private bool IsRightBitSet(byte i) => (i & 1) == 1;
        }
    }
}