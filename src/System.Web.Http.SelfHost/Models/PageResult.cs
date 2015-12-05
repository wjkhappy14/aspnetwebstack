using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Http.SelfHost.Models
{
    public class PageResult<T> : LinkedList<T>, IPagedList<T>
    {



        public PageResult()
        {
            Data = new LinkedList<T>();
        }

        public T this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }


        /// <summary>
        /// 双向链接列表
        /// </summary>
        public LinkedList<T> Data { get; set; }

        public bool HasNextPage
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int PageIndex
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int PageSize
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int TotalCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int TotalPages
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }
    }
}
