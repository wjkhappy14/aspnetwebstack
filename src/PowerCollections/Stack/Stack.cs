using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;


namespace Wintellect.PowerCollections.Stack
{
    public class Stack<T> : IEnumerable<T>, ICollection, IEnumerable
    {
        int ICollection.Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }
        


    }
}
