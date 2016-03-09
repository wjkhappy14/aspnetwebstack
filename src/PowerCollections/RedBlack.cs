//******************************
// Written by Peter Golde
// Copyright (c) 2004-2007, Wintellect
//
// Use and restribution of this code is subject to the license agreement 
// contained in the file "License.txt" accompanying this file.
//******************************

using System;
using System.Diagnostics;
using System.Collections.Generic;
using Wintellect.PowerCollections;

namespace Wintellect.PowerCollections
{


    /// <summary>
    /// Created By:Angkor 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingleRedBlackTree<T>
    {
      private static  Lazy<RedBlackTree<T>> lazyTree = new Lazy<RedBlackTree<T>>(() => Init(), true);
        private SingleRedBlackTree()
        {
            //Do Nothding
        }
        /// <summary>
        /// Return  RedBlackTree Instance 
        /// </summary>
        public  static RedBlackTree<T> Instance
        {
            get { return lazyTree.Value; }
        }
        /// <summary>
        /// init RedBlackTree
        /// </summary>
        /// <returns></returns>
        private static RedBlackTree<T> Init()
        {
            RedBlackTree<T> tree = new RedBlackTree<T>(Comparer<T>.Default);
            return tree;
        }
    }

    /// <summary>
    /// Describes what to do if a key is already in the tree when doing an
    /// insertion.
    /// </summary>
    public enum DuplicatePolicy
    {
        InsertFirst,               // Insert a new node before duplicates
        InsertLast,               // Insert a new node after duplicates
        ReplaceFirst,            // Replace the first of the duplicate nodes
        ReplaceLast,            // Replace the last of the duplicate nodes
        DoNothing                // Do nothing to the tree
    };

    /// <summary>
    /// The base implementation for various collections classes that use Red-Black trees
    /// as part of their implementation. This class should not (and can not) be 
    /// used directly by end users; it's only for internal use by the collections package.
    /// </summary>
    /// <remarks>
    /// The Red-Black tree manages items of type T, and uses a IComparer&lt;T&gt; that
    /// compares items to sort the tree. Multiple items can compare equal and be stored
    /// in the tree. Insert, Delete, and Find operations are provided in their full generality;
    /// all operations allow dealing with either the first or last of items that compare equal. 
    ///</remarks>
    [Serializable]
    public class RedBlackTree<T> : IEnumerable<T>
    {
        private readonly IComparer<T> comparer;         // interface for comparing elements, only Compare is used.
        public Node<T> Root;                    // The root of the tree. Can be null when tree is empty.
        private int count;						// The count of elements in the tree.

        private int changeStamp;        // An integer that is changed every time the tree structurally changes.
                                        // Used so that enumerations throw an exception if the tree is changed
                                        // during enumeration.

        private Node<T>[] stack;               // A stack of nodes. This is cached locally to avoid constant re-allocated it.

        /// <summary>
        /// Create an array of Nodes big enough for any path from top 
        /// to bottom. This is cached, and reused from call-to-call, so only one
        /// can be around at a time per tree.
        /// </summary>
        /// <returns>The node stack.</returns>
        private Node<T>[] GetNodeStack()
        {
            // Maximum depth needed is 2 * lg count + 1.
            int maxDepth;
            if (count < 0x400)
                maxDepth = 21;
            else if (count < 0x10000)
                maxDepth = 41;
            else
                maxDepth = 65;

            if (stack == null || stack.Length < maxDepth)
                stack = new Node<T>[maxDepth];

            return stack;
        }

        /// <summary>
        /// Must be called whenever there is a structural change in the tree. Causes
        /// changeStamp to be changed, which causes any in-progress enumerations
        /// to throw exceptions.
        /// </summary>
        internal void StopEnumerations()
        {
            ++changeStamp;
        }

        /// <summary>
        /// Checks the given stamp against the current change stamp. If different, the
        /// collection has changed during enumeration and an InvalidOperationException
        /// must be thrown
        /// </summary>
        /// <param name="startStamp">changeStamp at the start of the enumeration.</param>
        private void CheckEnumerationStamp(int startStamp)
        {
            if (startStamp != changeStamp)
            {
                throw new InvalidOperationException(Strings.ChangeDuringEnumeration);
            }
        }

        /// <summary>
		/// Initialize a red-black tree, using the given interface instance to compare elements. Only
		/// Compare is used on the IComparer interface.
		/// </summary>
		/// <param name="comparer">The IComparer&lt;T&gt; used to sort keys.</param>
		public RedBlackTree(IComparer<T> comparer)
        {
            this.comparer = comparer;
            this.count = 0;
            this.Root = null;
        }

        /// <summary>
        /// Returns the number of elements in the tree.
        /// </summary>
        public int ElementCount
        {
            get
            {
                return count;
            }
        }

        /// <summary>
        /// Clone the tree, returning a new tree containing the same items. Should
        /// take O(N) take.
        /// </summary>
        /// <returns>Clone version of this tree.</returns>
        public RedBlackTree<T> Clone()
        {
            RedBlackTree<T> newTree = new RedBlackTree<T>(comparer);
            newTree.count = this.count;
            if (this.Root != null)
                newTree.Root = this.Root.Clone();
            return newTree;
        }

        /// <summary>
        /// Finds the key in the tree. If multiple items in the tree have
        /// compare equal to the key, finds the first or last one. Optionally replaces the Item
        /// with the one searched for.
        /// </summary>
        /// <param name="key">Key to search for.</param>
        /// <param name="findFirst">If true, find the first of duplicates, else finds the last of duplicates.</param>
        /// <param name="replace">If true, replaces the Item with key (if function returns true)</param>
        /// <param name="Item">Returns the found Item, before replacing (if function returns true).</param>
        /// <returns>True if the key was found.</returns>
        public bool Find(T key, bool findFirst, bool replace, out T Item)
        {
            Node<T> current = Root;         // current search location in the tree
            Node<T> found = null;           // last node found with the key, or null if none.

            while (current != null)
            {
                int compare = comparer.Compare(key, current.Item);

                if (compare < 0)
                {
                    current = current.Left;
                }
                else if (compare > 0)
                {
                    current = current.Right;
                }
                else
                {
                    // Go Left/Right on equality to find first/last of elements with this key.
                    Debug.Assert(compare == 0);
                    found = current;
                    if (findFirst)
                        current = current.Left;
                    else
                        current = current.Right;
                }
            }

            if (found != null)
            {
                Item = found.Item;
                if (replace)
                    found.Item = key;
                return true;
            }
            else
            {
                Item = default(T);
                return false;
            }
        }

        /// <summary>
        /// Finds the index of the key in the tree. If multiple items in the tree have
        /// compare equal to the key, finds the first or last one. 
        /// </summary>
        /// <param name="key">Key to search for.</param>
        /// <param name="findFirst">If true, find the first of duplicates, else finds the last of duplicates.</param>
        /// <returns>Index of the Item found if the key was found, -1 if not found.</returns>
        public int FindIndex(T key, bool findFirst)
        {
            T dummy;
            if (findFirst)
                return FirstItemInRange(EqualRangeTester(key), out dummy);
            else
                return LastItemInRange(EqualRangeTester(key), out dummy);
        }

        /// <summary>
        /// Find the Item at a particular index in the tree.
        /// </summary>
        /// <param name="index">The zero-based index of the Item. Must be &gt;= 0 and &lt; Count.</param>
        /// <returns>The Item at the particular index.</returns>
        public T GetItemByIndex(int index)
        {
            if (index < 0 || index >= count)
                throw new ArgumentOutOfRangeException("index");

            Node<T> current = Root;			// current search location in the tree

            for (;;)
            {
                int leftCount;

                if (current.Left != null)
                    leftCount = current.Left.Count;
                else
                    leftCount = 0;

                if (leftCount > index)
                    current = current.Left;
                else if (leftCount == index)
                    return current.Item;
                else
                {
                    index -= leftCount + 1;
                    current = current.Right;
                }
            }
        }

        /// <summary>
        /// Insert a new node into the tree, maintaining the red-black invariants.
        /// </summary>
        /// <remarks>Algorithm from Sedgewick, "Algorithms".</remarks>
        /// <param name="Item">The new Item to insert</param>
        /// <param name="dupPolicy">What to do if equal Item is already present.</param>
        /// <param name="previous">If false, returned, the previous Item.</param>
        /// <returns>false if duplicate exists, otherwise true.</returns>
        public bool Insert(T Item, DuplicatePolicy dupPolicy, out T previous)
        {
            Node<T> node = Root;
            Node<T> parent = null, gparent = null, ggparent = null; // parent, grand, a great-grantparent of node.
            bool wentLeft = false, wentRight = false;				// direction from parent to node.
            bool rotated;
            Node<T> duplicateFound = null;

            // The tree may be changed.
            StopEnumerations();

            // We increment counts on the way down the tree. If we end up not inserting an items due
            // to a duplicate, we need a stack to adjust the counts back. We don't need the stack if the duplicate
            // policy means that we will always do an insertion.
            bool needStack = !((dupPolicy == DuplicatePolicy.InsertFirst) || (dupPolicy == DuplicatePolicy.InsertLast));
            Node<T>[] nodeStack = null;
            int nodeStackPtr = 0;  // first free Item on the stack.
            if (needStack)
                nodeStack = GetNodeStack();

            while (node != null)
            {
                // If we find a node with two red children, split it so it doesn't cause problems
                // when inserting a node.
                if (node.Left != null && node.Left.IsRed && node.Right != null && node.Right.IsRed)
                {
                    node = InsertSplit(ggparent, gparent, parent, node, out rotated);

                    if (needStack && rotated)
                    {
                        nodeStackPtr -= 2;
                        if (nodeStackPtr < 0)
                            nodeStackPtr = 0;
                    }
                }

                // Keep track of parent, grandparent, great-grand parent.
                ggparent = gparent; gparent = parent; parent = node;

                // Compare the key and the node. 
                int compare = comparer.Compare(Item, node.Item);

                if (compare == 0)
                {
                    // Found a node with the data already. Check duplicate policy.
                    if (dupPolicy == DuplicatePolicy.DoNothing)
                    {
                        previous = node.Item;

                        // Didn't insert after all. Return counts back to their previous value.
                        for (int i = 0; i < nodeStackPtr; ++i)
                            nodeStack[i].DecrementCount();

                        return false;
                    }
                    else if (dupPolicy == DuplicatePolicy.InsertFirst || dupPolicy == DuplicatePolicy.ReplaceFirst)
                    {
                        // Insert first by treating the key as less than nodes in the tree.
                        duplicateFound = node;
                        compare = -1;
                    }
                    else
                    {
                        Debug.Assert(dupPolicy == DuplicatePolicy.InsertLast || dupPolicy == DuplicatePolicy.ReplaceLast);
                        // Insert last by treating the key as greater than nodes in the tree.
                        duplicateFound = node;
                        compare = 1;
                    }
                }

                Debug.Assert(compare != 0);

                node.IncrementCount();
                if (needStack)
                    nodeStack[nodeStackPtr++] = node;

                // Move to the Left or Right as needed to find the insertion point.
                if (compare < 0)
                {
                    node = node.Left;
                    wentLeft = true; wentRight = false;
                }
                else
                {
                    node = node.Right;
                    wentRight = true; wentLeft = false;
                }
            }

            if (duplicateFound != null)
            {
                previous = duplicateFound.Item;

                // Are we replacing instread of inserting?
                if (dupPolicy == DuplicatePolicy.ReplaceFirst || dupPolicy == DuplicatePolicy.ReplaceLast)
                {
                    duplicateFound.Item = Item;

                    // Didn't insert after all. Return counts back to their previous value.
                    for (int i = 0; i < nodeStackPtr; ++i)
                        nodeStack[i].DecrementCount();

                    return false;
                }
            }
            else
            {
                previous = default(T);
            }

            // Create a new node.
            node = new Node<T>();
            node.Item = Item;
            node.Count = 1;

            // Link the node into the tree.
            if (wentLeft)
                parent.Left = node;
            else if (wentRight)
                parent.Right = node;
            else
            {
                Debug.Assert(Root == null);
                Root = node;
            }

            // Maintain the red-black policy.
            InsertSplit(ggparent, gparent, parent, node, out rotated);

            // We've added a node to the tree, so update the count.
            count += 1;

            return (duplicateFound == null);
        }

        /// <summary>
        /// Split a node with two red children (a 4-node in the 2-3-4 tree formalism), as
        /// part of an insert operation.
        /// </summary>
        /// <param name="ggparent">great grand-parent of "node", can be null near root</param>
        /// <param name="gparent">grand-parent of "node", can be null near root</param>
        /// <param name="parent">parent of "node", can be null near root</param>
        /// <param name="node">Node<T> to split, can't be null</param>
        /// <param name="rotated">Indicates that rotation(s) occurred in the tree.</param>
        /// <returns>Node<T> to continue searching from.</returns>
        private Node<T> InsertSplit(Node<T> ggparent, Node<T> gparent, Node<T> parent, Node<T> node, out bool rotated)
        {
            if (node != Root)
                node.IsRed = true;
            if (node.Left != null)
                node.Left.IsRed = false;
            if (node.Right != null)
                node.Right.IsRed = false;

            if (parent != null && parent.IsRed)
            {
                // Since parent is red, gparent can't be null (root is always black). ggparent
                // might be null, however.
                Debug.Assert(gparent != null);

                // if links from gparent and parent are opposite (Left/Right or Right/Left),
                // then rotate.
                if ((gparent.Left == parent) != (parent.Left == node))
                {
                    Rotate(gparent, parent, node);
                    parent = node;
                }

                gparent.IsRed = true;

                // Do a rotate to prevent two red links in a row.
                Rotate(ggparent, gparent, parent);

                parent.IsRed = false;
                rotated = true;
                return parent;
            }
            else
            {
                rotated = false;
                return node;
            }
        }

        /// <summary>
        /// Performs a rotation involving the node, it's child and grandchild. The counts of 
        /// childs and grand-child are set the correct values from their children; this is important
        /// if they have been adjusted on the way down the try as part of an insert/delete.
        /// </summary>
        /// <param name="node">Top node of the rotation. Can be null if child==root.</param>
        /// <param name="child">One child of "node". Not null.</param>
        /// <param name="gchild">One child of "child". Not null.</param>
        private void Rotate(Node<T> node, Node<T> child, Node<T> gchild)
        {
            if (gchild == child.Left)
            {
                child.Left = gchild.Right;
                gchild.Right = child;
            }
            else
            {
                Debug.Assert(gchild == child.Right);
                child.Right = gchild.Left;
                gchild.Left = child;
            }

            // Restore the counts.
            child.Count = (child.Left != null ? child.Left.Count : 0) + (child.Right != null ? child.Right.Count : 0) + 1;
            gchild.Count = (gchild.Left != null ? gchild.Left.Count : 0) + (gchild.Right != null ? gchild.Right.Count : 0) + 1;

            if (node == null)
            {
                Debug.Assert(child == Root);
                Root = gchild;
            }
            else if (child == node.Left)
            {
                node.Left = gchild;
            }
            else
            {
                Debug.Assert(child == node.Right);
                node.Right = gchild;
            }
        }

        /// <summary>
        /// Deletes a key from the tree. If multiple elements are equal to key, 
        /// deletes the first or last. If no element is equal to the key, 
        /// returns false.
        /// </summary>
        /// <remarks>Top-down algorithm from Weiss. Basic plan is to move down in the tree, 
        /// rotating and recoloring along the way to always keep the current node red, which 
        /// ensures that the node we delete is red. The details are quite complex, however! </remarks>
        /// <param name="key">Key to delete.</param>
        /// <param name="deleteFirst">Which Item to delete if multiple are equal to key. True to delete the first, false to delete last.</param>
        /// <param name="Item">Returns the Item that was deleted, if true returned.</param>
        /// <returns>True if an element was deleted, false if no element had 
        /// specified key.</returns>
        public bool Delete(T key, bool deleteFirst, out T Item)
        {
            return DeleteItemFromRange(EqualRangeTester(key), deleteFirst, out Item);
        }

        /// 
		/// <summary>
		/// Enumerate all the items in-order
		/// </summary>
		/// <returns>An enumerator for all the items, in order.</returns>
        /// <exception cref="InvalidOperationException">The tree has an Item added or deleted during the enumeration.</exception>
        public IEnumerator<T> GetEnumerator()
        {
            return EnumerateRange(EntireRangeTester).GetEnumerator();
        }

        /// <summary>
        /// Enumerate all the items in-order
        /// </summary>
        /// <returns>An enumerator for all the items, in order.</returns>
        /// <exception cref="InvalidOperationException">The tree has an Item added or deleted during the enumeration.</exception>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region Ranges

        /// <summary>
        /// A delegate that tests if an Item is within a custom range. The range must be a contiguous
        /// range of items with the ordering of this tree. The range test function must test
        /// if an Item is before, withing, or after the range.
        /// </summary>
        /// <param name="Item">Item to test against the range.</param>
        /// <returns>Returns negative if Item is before the range, zero if Item is withing the range,
        /// and positive if Item is after the range.</returns>
        public delegate int RangeTester(T Item);

        /// <summary>
        /// Gets a range tester that defines a range by first and last items.
        /// </summary>
        /// <param name="useFirst">If true, bound the range on the bottom by first.</param>
        /// <param name="first">If useFirst is true, the inclusive lower bound.</param>
        /// <param name="useLast">If true, bound the range on the top by last.</param>
        /// <param name="last">If useLast is true, the exclusive upper bound.</param>
        /// <returns>A RangeTester delegate that tests for an Item in the given range.</returns>
        public RangeTester BoundedRangeTester(bool useFirst, T first, bool useLast, T last)
        {
            return delegate (T Item)
            {
                if (useFirst && comparer.Compare(first, Item) > 0)
                    return -1;     // Item is before first.
                else if (useLast && comparer.Compare(last, Item) <= 0)
                    return 1;      // Item is after or equal to last.
                else
                    return 0;      // Item is greater or equal to first, and less than last.
            };
        }

        /// <summary>
        /// Gets a range tester that defines a range by first and last items.
        /// </summary>
        /// <param name="first">The lower bound.</param>
        /// <param name="firstInclusive">True if the lower bound is inclusive, false if exclusive.</param>
        /// <param name="last">The upper bound.</param>
        /// <param name="lastInclusive">True if the upper bound is inclusive, false if exclusive.</param>
        /// <returns>A RangeTester delegate that tests for an Item in the given range.</returns>
        public RangeTester DoubleBoundedRangeTester(T first, bool firstInclusive, T last, bool lastInclusive)
        {
            return delegate (T Item)
            {
                if (firstInclusive)
                {
                    if (comparer.Compare(first, Item) > 0)
                        return -1;     // Item is before first.
                }
                else
                {
                    if (comparer.Compare(first, Item) >= 0)
                        return -1;     // Item is before or equal to first.
                }

                if (lastInclusive)
                {
                    if (comparer.Compare(last, Item) < 0)
                        return 1;      // Item is after last.
                }
                else
                {
                    if (comparer.Compare(last, Item) <= 0)
                        return 1;      // Item is after or equal to last
                }

                return 0;      // Item is between first and last.
            };
        }


        /// <summary>
        /// Gets a range tester that defines a range by a lower bound.
        /// </summary>
        /// <param name="first">The lower bound.</param>
        /// <param name="inclusive">True if the lower bound is inclusive, false if exclusive.</param>
        /// <returns>A RangeTester delegate that tests for an Item in the given range.</returns>
        public RangeTester LowerBoundedRangeTester(T first, bool inclusive)
        {
            return delegate (T Item)
            {
                if (inclusive)
                {
                    if (comparer.Compare(first, Item) > 0)
                        return -1;     // Item is before first.
                    else
                        return 0;      // Item is after or equal to first
                }
                else
                {
                    if (comparer.Compare(first, Item) >= 0)
                        return -1;     // Item is before or equal to first.
                    else
                        return 0;      // Item is after first
                }
            };
        }


        /// <summary>
        /// Gets a range tester that defines a range by upper bound.
        /// </summary>
        /// <param name="last">The upper bound.</param>
        /// <param name="inclusive">True if the upper bound is inclusive, false if exclusive.</param>
        /// <returns>A RangeTester delegate that tests for an Item in the given range.</returns>
        public RangeTester UpperBoundedRangeTester(T last, bool inclusive)
        {
            return delegate (T Item)
            {
                if (inclusive)
                {
                    if (comparer.Compare(last, Item) < 0)
                        return 1;      // Item is after last.
                    else
                        return 0;      // Item is before or equal to last.
                }
                else
                {
                    if (comparer.Compare(last, Item) <= 0)
                        return 1;      // Item is after or equal to last
                    else
                        return 0;      // Item is before last.
                }
            };
        }

        /// <summary>
        /// Gets a range tester that defines a range by all items equal to an Item.
        /// </summary>
        /// <param name="equalTo">The Item that is contained in the range.</param>
        /// <returns>A RangeTester delegate that tests for an Item equal to <paramref name="equalTo"/>.</returns>
        public RangeTester EqualRangeTester(T equalTo)
        {
            return delegate (T Item)
            {
                return comparer.Compare(Item, equalTo);
            };
        }

        /// <summary>
        /// A range tester that defines a range that is the entire tree.
        /// </summary>
        /// <param name="Item">Item to test.</param>
        /// <returns>Always returns 0.</returns>
        public int EntireRangeTester(T Item)
        {
            return 0;
        }

        /// <summary>
        /// Enumerate the items in a custom range in the tree. The range is determined by 
        /// a RangeTest delegate.
        /// </summary>
        /// <param name="rangeTester">Tests an Item against the custom range.</param>
        /// <returns>An IEnumerable&lt;T&gt; that enumerates the custom range in order.</returns>
        /// <exception cref="InvalidOperationException">The tree has an Item added or deleted during the enumeration.</exception>
        public IEnumerable<T> EnumerateRange(RangeTester rangeTester)
        {
            return EnumerateRangeInOrder(rangeTester, Root);
        }

        /// <summary>
        /// Enumerate all the items in a custom range, under and including node, in-order.
        /// </summary>
        /// <param name="rangeTester">Tests an Item against the custom range.</param>
        /// <param name="node">Node<T> to begin enumeration. May be null.</param>
        /// <returns>An enumerable of the items.</returns>
        /// <exception cref="InvalidOperationException">The tree has an Item added or deleted during the enumeration.</exception>
        private IEnumerable<T> EnumerateRangeInOrder(RangeTester rangeTester, Node<T> node)
        {
            int startStamp = changeStamp;

            if (node != null)
            {
                int compare = rangeTester(node.Item);

                if (compare >= 0)
                {
                    // At least part of the range may lie to the Left.
                    foreach (T Item in EnumerateRangeInOrder(rangeTester, node.Left))
                    {
                        yield return Item;
                        CheckEnumerationStamp(startStamp);
                    }
                }

                if (compare == 0)
                {
                    // The Item is within the range.
                    yield return node.Item;
                    CheckEnumerationStamp(startStamp);
                }

                if (compare <= 0)
                {
                    // At least part of the range lies to the Right.
                    foreach (T Item in EnumerateRangeInOrder(rangeTester, node.Right))
                    {
                        yield return Item;
                        CheckEnumerationStamp(startStamp);
                    }
                }
            }
        }

        /// <summary>
        /// Enumerate the items in a custom range in the tree, in reversed order. The range is determined by 
        /// a RangeTest delegate.
        /// </summary>
        /// <param name="rangeTester">Tests an Item against the custom range.</param>
        /// <returns>An IEnumerable&lt;T&gt; that enumerates the custom range in reversed order.</returns>
        /// <exception cref="InvalidOperationException">The tree has an Item added or deleted during the enumeration.</exception>
        public IEnumerable<T> EnumerateRangeReversed(RangeTester rangeTester)
        {
            return EnumerateRangeInReversedOrder(rangeTester, Root);
        }

        /// <summary>
        /// Enumerate all the items in a custom range, under and including node, in reversed order.
        /// </summary>
        /// <param name="rangeTester">Tests an Item against the custom range.</param>
        /// <param name="node">Node<T> to begin enumeration. May be null.</param>
        /// <returns>An enumerable of the items, in reversed oreder.</returns>
        /// <exception cref="InvalidOperationException">The tree has an Item added or deleted during the enumeration.</exception>
        private IEnumerable<T> EnumerateRangeInReversedOrder(RangeTester rangeTester, Node<T> node)
        {
            int startStamp = changeStamp;

            if (node != null)
            {
                int compare = rangeTester(node.Item);

                if (compare <= 0)
                {
                    // At least part of the range lies to the Right.
                    foreach (T Item in EnumerateRangeInReversedOrder(rangeTester, node.Right))
                    {
                        yield return Item;
                        CheckEnumerationStamp(startStamp);
                    }
                }

                if (compare == 0)
                {
                    // The Item is within the range.
                    yield return node.Item;
                    CheckEnumerationStamp(startStamp);
                }

                if (compare >= 0)
                {
                    // At least part of the range may lie to the Left.
                    foreach (T Item in EnumerateRangeInReversedOrder(rangeTester, node.Left))
                    {
                        yield return Item;
                        CheckEnumerationStamp(startStamp);
                    }
                }
            }
        }


        /// <summary>
        /// Deletes either the first or last Item from a range, as identified by a RangeTester
        /// delegate. If the range is empty, returns false.
        /// </summary>
        /// <remarks>Top-down algorithm from Weiss. Basic plan is to move down in the tree, 
        /// rotating and recoloring along the way to always keep the current node red, which 
        /// ensures that the node we delete is red. The details are quite complex, however! </remarks>
        /// <param name="rangeTester">Range to delete from.</param>
        /// <param name="deleteFirst">If true, delete the first Item from the range, else the last.</param>
        /// <param name="Item">Returns the Item that was deleted, if true returned.</param>
        /// <returns>True if an element was deleted, false if the range is empty.</returns>
        public bool DeleteItemFromRange(RangeTester rangeTester, bool deleteFirst, out T Item)
        {
            Node<T> node;			// The current node.
            Node<T> parent;		// Parent of the current node.
            Node<T> gparent;		// Grandparent of the current node.
            Node<T> sib;			// Sibling of the current node.
            Node<T> keyNode;		// Node<T> with the key that is being removed.

            // The tree may be changed.
            StopEnumerations();

            if (Root == null)
            {
                // Nothing in the tree. Go home now.
                Item = default(T);
                return false;
            }

            // We decrement counts on the way down the tree. If we end up not finding an Item to delete
            // we need a stack to adjust the counts back. 
            Node<T>[] nodeStack = GetNodeStack();
            int nodeStackPtr = 0;  // first free Item on the stack.

            // Start at the root.
            node = Root;
            sib = parent = gparent = null;
            keyNode = null;

            // Proceed down the tree, making the current node red so it can be removed.
            for (;;)
            {
                Debug.Assert(parent == null || parent.IsRed);
                Debug.Assert(sib == null || !sib.IsRed);
                Debug.Assert(!node.IsRed);

                if ((node.Left == null || !node.Left.IsRed) && (node.Right == null || !node.Right.IsRed))
                {
                    // node has two black children (null children are considered black).
                    if (parent == null)
                    {
                        // Special case for the root.
                        Debug.Assert(node == Root);
                        node.IsRed = true;
                    }
                    else if ((sib.Left == null || !sib.Left.IsRed) && (sib.Right == null || !sib.Right.IsRed))
                    {
                        // sib has two black children.
                        node.IsRed = true;
                        sib.IsRed = true;
                        parent.IsRed = false;
                    }
                    else
                    {
                        if (parent.Left == node && (sib.Right == null || !sib.Right.IsRed))
                        {
                            // sib has a black child on the opposite side as node.
                            Node<T> tleft = sib.Left;
                            Rotate(parent, sib, tleft);
                            sib = tleft;
                        }
                        else if (parent.Right == node && (sib.Left == null || !sib.Left.IsRed))
                        {
                            // sib has a black child on the opposite side as node.
                            Node<T> tright = sib.Right;
                            Rotate(parent, sib, tright);
                            sib = tright;
                        }

                        // sib has a red child.
                        Rotate(gparent, parent, sib);
                        node.IsRed = true;
                        sib.IsRed = true;
                        sib.Left.IsRed = false;
                        sib.Right.IsRed = false;

                        sib.DecrementCount();
                        nodeStack[nodeStackPtr - 1] = sib;
                        parent.DecrementCount();
                        nodeStack[nodeStackPtr++] = parent;
                    }
                }

                // Compare the key and move down the tree to the correct child.
                do
                {
                    Node<T> nextNode, nextSib;		// Node<T> we've moving to, and it's sibling.

                    node.DecrementCount();
                    nodeStack[nodeStackPtr++] = node;

                    // Determine which way to move in the tree by comparing the 
                    // current Item to what we're looking for.
                    int compare = rangeTester(node.Item);

                    if (compare == 0)
                    {
                        // We've found the node to remove. Remember it, then keep traversing the
                        // tree to either find the first/last of equal keys, and if needed, the predecessor
                        // or successor (the actual node to be removed).
                        keyNode = node;
                        if (deleteFirst)
                        {
                            nextNode = node.Left; nextSib = node.Right;
                        }
                        else
                        {
                            nextNode = node.Right; nextSib = node.Left;
                        }
                    }
                    else if (compare > 0)
                    {
                        nextNode = node.Left; nextSib = node.Right;
                    }
                    else
                    {
                        nextNode = node.Right; nextSib = node.Left;
                    }

                    // Have we reached the end of our tree walk?
                    if (nextNode == null)
                        goto FINISHED;

                    // Move down the tree.
                    gparent = parent;
                    parent = node;
                    node = nextNode;
                    sib = nextSib;
                } while (!parent.IsRed && node.IsRed);

                if (!parent.IsRed)
                {
                    Debug.Assert(!node.IsRed);
                    // moved to a black child.
                    Rotate(gparent, parent, sib);

                    sib.DecrementCount();
                    nodeStack[nodeStackPtr - 1] = sib;
                    parent.DecrementCount();
                    nodeStack[nodeStackPtr++] = parent;

                    sib.IsRed = false;
                    parent.IsRed = true;
                    gparent = sib;
                    sib = (parent.Left == node) ? parent.Right : parent.Left;
                }
            }

            FINISHED:
            if (keyNode == null)
            {
                // We never found a node to delete.

                // Return counts back to their previous value.
                for (int i = 0; i < nodeStackPtr; ++i)
                    nodeStack[i].IncrementCount();

                // Color the root black, in case it was colored red above.
                if (Root != null)
                    Root.IsRed = false;

                Item = default(T);
                return false;
            }

            // Return the Item from the node we're deleting.
            Item = keyNode.Item;

            // At a leaf or a node with one child which is a leaf. Remove the node.
            if (keyNode != node)
            {
                // The node we want to delete is interior. Move the Item from the
                // node we're actually deleting to the key node.
                keyNode.Item = node.Item;
            }

            // If we have one child, replace the current with the child, otherwise,
            // replace the current node with null.
            Node<T> replacement;
            if (node.Left != null)
            {
                replacement = node.Left;
                Debug.Assert(!node.IsRed && replacement.IsRed);
                replacement.IsRed = false;
            }
            else if (node.Right != null)
            {
                replacement = node.Right;
                Debug.Assert(!node.IsRed && replacement.IsRed);
                replacement.IsRed = false;
            }
            else
                replacement = null;

            if (parent == null)
            {
                Debug.Assert(Root == node);
                Root = replacement;
            }
            else if (parent.Left == node)
                parent.Left = replacement;
            else
            {
                Debug.Assert(parent.Right == node);
                parent.Right = replacement;
            }

            // Color the root black, in case it was colored red above.
            if (Root != null)
                Root.IsRed = false;

            // Update Item count.
            count -= 1;

            // And we're done.
            return true;
        }

        /// <summary>
        /// Delete all the items in a range, identified by a RangeTester delegate.
        /// </summary>
        /// <param name="rangeTester">The delegate that defines the range to delete.</param>
        /// <returns>The number of items deleted.</returns>
        public int DeleteRange(RangeTester rangeTester)
        {
            bool deleted;
            int counter = 0;
            T dummy;

            do
            {
                deleted = DeleteItemFromRange(rangeTester, true, out dummy);
                if (deleted)
                    ++counter;
            } while (deleted);

            return counter;
        }

        /// <summary>
        /// Count the items in a custom range in the tree. The range is determined by 
        /// a RangeTester delegate.
        /// </summary>
        /// <param name="rangeTester">The delegate that defines the range.</param>
        /// <returns>The number of items in the range.</returns>
        public int CountRange(RangeTester rangeTester)
        {
            return CountRangeUnderNode(rangeTester, Root, false, false);
        }

        /// <summary>
        /// Count all the items in a custom range, under and including node.
        /// </summary>
        /// <param name="rangeTester">The delegate that defines the range.</param>
        /// <param name="node">Node<T> to begin enumeration. May be null.</param>
        /// <param name="belowRangeTop">This node and all under it are either in the range or below it.</param>
        /// <param name="aboveRangeBottom">This node and all under it are either in the range or above it.</param>
        /// <returns>The number of items in the range, under and include node.</returns>
        private int CountRangeUnderNode(RangeTester rangeTester, Node<T> node, bool belowRangeTop, bool aboveRangeBottom)
        {
            if (node != null)
            {
                if (belowRangeTop && aboveRangeBottom)
                {
                    // This node and all below it must be in the range. Use the predefined count.
                    return node.Count;
                }

                int compare = rangeTester(node.Item);
                int counter;

                if (compare == 0)
                {
                    counter = 1;  // the node itself
                    counter += CountRangeUnderNode(rangeTester, node.Left, true, aboveRangeBottom);
                    counter += CountRangeUnderNode(rangeTester, node.Right, belowRangeTop, true);
                }
                else if (compare < 0)
                {
                    counter = CountRangeUnderNode(rangeTester, node.Right, belowRangeTop, aboveRangeBottom);
                }
                else
                { // compare > 0
                    counter = CountRangeUnderNode(rangeTester, node.Left, belowRangeTop, aboveRangeBottom);
                }

                return counter;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Find the first Item in a custom range in the tree, and it's index. The range is determined
        /// by a RangeTester delegate.
        /// </summary>
        /// <param name="rangeTester">The delegate that defines the range.</param>
        /// <param name="Item">Returns the Item found, if true was returned.</param>
        /// <returns>Index of first Item in range if range is non-empty, -1 otherwise.</returns>
        public int FirstItemInRange(RangeTester rangeTester, out T Item)
        {
            Node<T> node = Root, found = null;
            int curCount = 0, foundIndex = -1;

            while (node != null)
            {
                int compare = rangeTester(node.Item);

                if (compare == 0)
                {
                    found = node;
                    if (node.Left != null)
                        foundIndex = curCount + node.Left.Count;
                    else
                        foundIndex = curCount;
                }

                if (compare >= 0)
                    node = node.Left;
                else
                {
                    if (node.Left != null)
                        curCount += node.Left.Count + 1;
                    else
                        curCount += 1;
                    node = node.Right;
                }
            }

            if (found != null)
            {
                Item = found.Item;
                return foundIndex;
            }
            else
            {
                Item = default(T);
                return -1;
            }
        }

        /// <summary>
        /// Find the last Item in a custom range in the tree, and it's index. The range is determined
        /// by a RangeTester delegate.
        /// </summary>
        /// <param name="rangeTester">The delegate that defines the range.</param>
        /// <param name="Item">Returns the Item found, if true was returned.</param>
        /// <returns>Index of the Item if range is non-empty, -1 otherwise.</returns>
        public int LastItemInRange(RangeTester rangeTester, out T Item)
        {
            Node<T> node = Root, found = null;
            int curCount = 0, foundIndex = -1;

            while (node != null)
            {
                int compare = rangeTester(node.Item);

                if (compare == 0)
                {
                    found = node;
                    if (node.Left != null)
                        foundIndex = curCount + node.Left.Count;
                    else
                        foundIndex = curCount;
                }

                if (compare <= 0)
                {
                    if (node.Left != null)
                        curCount += node.Left.Count + 1;
                    else
                        curCount += 1;
                    node = node.Right;
                }
                else
                    node = node.Left;
            }

            if (found != null)
            {
                Item = found.Item;
                return foundIndex;
            }
            else
            {
                Item = default(T);
                return foundIndex;
            }
        }

        #endregion Ranges

#if DEBUG
        /// <summary>
        /// Prints out the tree.
        /// </summary>
        public void Print()
        {
            PrintSubTree(Root, "", "");
            Console.WriteLine();
        }

        /// <summary>
        /// Prints a sub-tree.
        /// </summary>
        /// <param name="node">Node<T> to print from</param>
        /// <param name="prefixNode">Prefix for the node</param>
        /// <param name="prefixChildren">Prefix for the node's children</param>
        private void PrintSubTree(Node<T> node, string prefixNode, string prefixChildren)
        {
            if (node == null)
                return;

            // Red nodes marked as "@@", black nodes as "..".
            Console.WriteLine("{0}{1} {2,4} {3}", prefixNode, node.IsRed ? "@@" : "..", node.Count, node.Item);

            PrintSubTree(node.Left, prefixChildren + "|-L-", prefixChildren + "|  ");
            PrintSubTree(node.Right, prefixChildren + "|-R-", prefixChildren + "   ");
        }

        /// <summary>
        /// Validates that the tree is correctly sorted, and meets the red-black tree 
        /// axioms.
        /// </summary>
        public void Validate()
        {
            Debug.Assert(comparer != null, "Comparer should not be null");

            if (Root == null)
            {
                Debug.Assert(0 == count, "Count in empty tree should be 0.");

            }
            else
            {
                Debug.Assert(!Root.IsRed, "Root is not black");
                int blackHeight;
                int nodeCount = ValidateSubTree(Root, out blackHeight);
                Debug.Assert(nodeCount == this.count, "Node<T> count of tree is not correct.");
            }
        }

        /// <summary>
        /// Validates a sub-tree and returns the count and black height.
        /// </summary>
        /// <param name="node">Sub-tree to validate. May be null.</param>
        /// <param name="blackHeight">Returns the black height of the tree.</param>
        /// <returns>Returns the number of nodes in the sub-tree. 0 if node is null.</returns>
        private int ValidateSubTree(Node<T> node, out int blackHeight)
        {
            if (node == null)
            {
                blackHeight = 0;
                return 0;
            }

            // Check that this node is sorted with respect to any children.
            if (node.Left != null)
                Debug.Assert(comparer.Compare(node.Left.Item, node.Item) <= 0, "Left child is not less than or equal to node");
            if (node.Right != null)
                Debug.Assert(comparer.Compare(node.Right.Item, node.Item) >= 0, "Right child is not greater than or equal to node");

            // Check that the two-red rule is not violated.
            if (node.IsRed)
            {
                if (node.Left != null)
                    Debug.Assert(!node.Left.IsRed, "Node<T> and Left child both red");
                if (node.Right != null)
                    Debug.Assert(!node.Right.IsRed, "Node<T> and Right child both red");
            }

            // Validate sub-trees and get their size and heights.
            int leftCount, leftBlackHeight;
            int rightCount, rightBlackHeight;
            int ourCount;

            leftCount = ValidateSubTree(node.Left, out leftBlackHeight);
            rightCount = ValidateSubTree(node.Right, out rightBlackHeight);
            ourCount = leftCount + rightCount + 1;

            Debug.Assert(ourCount == node.Count);

            // Validate the equal black-height rule.
            Debug.Assert(leftBlackHeight == rightBlackHeight, "Black heights are not equal");

            // Calculate our black height and return the count
            blackHeight = leftBlackHeight;
            if (!node.IsRed)
                blackHeight += 1;
            return ourCount;
        }
#endif //DEBUG

    }

}
