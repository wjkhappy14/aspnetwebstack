using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wintellect.PowerCollections
{

    /// <summary>
    /// The class that is each node in the red-black tree.
    /// </summary>
    [Serializable]
    public class Node<T>
    {
        public Node<T> Left, Right;
        public T Item;

        private const uint REDMASK = 0x80000000;
        private uint count;

        /// <summary>
        /// Is this a red node?
        /// </summary>
        public bool IsRed
        {
            get { return (count & REDMASK) != 0; }
            set
            {
                if (value)
                    count |= REDMASK;
                else
                    count &= ~REDMASK;
            }
        }

        /// <summary>
        /// Get or set the Count field -- a 31-bit field
        /// that holds the number of nodes at or below this
        /// level.
        /// </summary>
        public int Count
        {
            get { return (int)(count & ~REDMASK); }
            set { count = (count & REDMASK) | (uint)value; }
        }

        /// <summary>
        /// Add one to the Count.
        /// </summary>
        public void IncrementCount()
        {
            ++count;
        }

        /// <summary>
        /// Subtract one from the Count. The current
        /// Count must be non-zero.
        /// </summary>
        public void DecrementCount()
        {
            Debug.Assert(Count != 0);
            --count;
        }

        /// <summary>
        /// Clones a node and all its descendants.
        /// </summary>
        /// <returns>The cloned node.</returns>
        public Node<T> Clone()
        {
            Node<T> newNode = new Node<T>();
            newNode.Item = Item;

            newNode.count = count;

            if (Left != null)
                newNode.Left = Left.Clone();

            if (Right != null)
                newNode.Right = Right.Clone();

            return newNode;
        }
    }

}
