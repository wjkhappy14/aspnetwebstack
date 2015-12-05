namespace BTree.UnitTest
{
    using System.Linq;

    using Wintellect.PowerCollections.BTree;

    public class BTreeTest
    {
        private const int Degree = 2;

        private readonly int[] testKeyData = new int[] { 10, 20, 30, 50 };
        private readonly int[] testPointerData = new int[] { 50, 60, 40, 20 };
        public int InsertOneNode()
        {
            var btree = new BTree<int, int>(Degree);
            this.InsertTestDataAndValidateTree(btree, 0);
            return btree.Height;//1
        }

        public int InsertMultipleNodesToSplit()
        {
            var btree = new BTree<int, int>(Degree);

            for (int i = 0; i < this.testKeyData.Length; i++)
            {
                this.InsertTestDataAndValidateTree(btree, i);
            }
            return btree.Height;
        }

        public int DeleteNodes()
        {
            var btree = new BTree<int, int>(Degree);

            for (int i = 0; i < this.testKeyData.Length; i++)
            {
                this.InsertTestData(btree, i);
            }

            for (int i = 0; i < this.testKeyData.Length; i++)
            {
                btree.Delete(this.testKeyData[i]);
                TreeValidation.ValidateTree(btree.Root, Degree, this.testKeyData.Skip(i + 1).ToArray());
            }

            return btree.Height;//1
        }

        public void DeleteNonExistingNode()
        {
            var btree = new BTree<int, int>(Degree);

            for (int i = 0; i < this.testKeyData.Length; i++)
            {
                this.InsertTestData(btree, i);
            }

            btree.Delete(99999);
            TreeValidation.ValidateTree(btree.Root, Degree, this.testKeyData.ToArray());
        }

        public void SearchNodes()
        {
            var btree = new BTree<int, int>(Degree);

            for (int i = 0; i < this.testKeyData.Length; i++)
            {
                this.InsertTestData(btree, i);
                this.SearchTestData(btree, i);
            }
        }

        public Entry<int, int> SearchNonExistingNode()
        {
            var btree = new BTree<int, int>(Degree);

            // search an empty tree
            Entry<int, int> nonExisting = btree.Search(9999);
            for (int i = 0; i < this.testKeyData.Length; i++)
            {
                this.InsertTestData(btree, i);
                this.SearchTestData(btree, i);
            }

            // search a populated tree
           return  nonExisting = btree.Search(9999);
        }

        #region Private Helper Methods
        private void InsertTestData(BTree<int, int> btree, int testDataIndex)
        {
            btree.Insert(this.testKeyData[testDataIndex], this.testPointerData[testDataIndex]);
        }

        private void InsertTestDataAndValidateTree(BTree<int, int> btree, int testDataIndex)
        {
            btree.Insert(this.testKeyData[testDataIndex], this.testPointerData[testDataIndex]);
            TreeValidation.ValidateTree(btree.Root, Degree, this.testKeyData.Take(testDataIndex + 1).ToArray());
        }

        private Entry<int, int>  SearchTestData(BTree<int, int> btree, int testKeyDataIndex)
        {
            Entry<int, int> entry = new Entry<int, int>();
            for (int i = 0; i <= testKeyDataIndex; i++)
            {
                entry = btree.Search(this.testKeyData[i]);
              
            }

            return entry;
        }

        #endregion
    }
}
