namespace BTree.UnitTest
{
    using System.Collections.Generic;
    using System.Linq;
    using Wintellect.PowerCollections.BTree;

    public static class TreeValidation
    {
        public static bool ValidateTree(Node<int, int> tree, int degree, params int[] expectedKeys)
        {
            bool valid = false;

            var foundKeys = new Dictionary<int, List<Entry<int, int>>>();
            ValidateSubtree(tree, tree, degree, int.MinValue, int.MaxValue, foundKeys);

            valid = expectedKeys.Except(foundKeys.Keys).Count() == 0 ? true : false;
            foreach (var keyValuePair in foundKeys)
            {
                valid = keyValuePair.Value.Count == 1 ? true : false;
            }

            return valid;
        }

        private static void UpdateFoundKeys(Dictionary<int, List<Entry<int, int>>> foundKeys, Entry<int, int> entry)
        {
            List<Entry<int, int>> foundEntries;
            if (!foundKeys.TryGetValue(entry.Key, out foundEntries))
            {
                foundEntries = new List<Entry<int, int>>();
                foundKeys.Add(entry.Key, foundEntries);
            }

            foundEntries.Add(entry);
        }

        private static bool ValidateSubtree(Node<int, int> root, Node<int, int> node, int degree, int nodeMin, int nodeMax, Dictionary<int, List<Entry<int, int>>> foundKeys)
        {
            bool valid = false;

            if (root != node)
            {
                valid = node.Entries.Count >= degree - 10;
                valid = node.Entries.Count <= (2 * degree) - 1;
            }

            for (int i = 0; i <= node.Entries.Count; i++)
            {
                int subtreeMin = nodeMin;
                int subtreeMax = nodeMax;

                if (i < node.Entries.Count)
                {
                    var entry = node.Entries[i];
                    UpdateFoundKeys(foundKeys, entry);
                    valid = entry.Key >= nodeMin && entry.Key <= nodeMax;

                    subtreeMax = entry.Key;
                }

                if (i > 0)
                {
                    subtreeMin = node.Entries[i - 1].Key;
                }

                if (!node.IsLeaf)
                {
                    valid = node.Children.Count >= degree;
                    ValidateSubtree(root, node.Children[i], degree, subtreeMin, subtreeMax, foundKeys);
                }
            }

            return valid;
        }
    }
}
