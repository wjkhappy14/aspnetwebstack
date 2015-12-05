using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost.Singletons;
using Wintellect.PowerCollections;
using System.Web.Http.SelfHost.Models;
using System.Web.Http.SelfHost.WebMatrix;
using WebMatrix.Data;

namespace System.Web.Http.SelfHost.Controllers
{
    public class TopicController : ApiController
    {
        RedBlackTree<Topic> _topicTree = SingletonTree<Topic>.RedBlackTree;
        Lazy<IEnumerable<Topic>> lazy = new Lazy<IEnumerable<Topic>>(true);

        public TopicController()
        {
            InitTopics();
        }


        /// <summary>
        /// 
        /// </summary>
        private void InitTopics()
        {
            Topic pre;
            DynamicDataReader reader = new DynamicDataReader();
            var result = reader.Query("Select *   FROM [UserCenter].[dbo].[Members]");
            var list = new List<Topic>();
            foreach (Object record in result)
            {
                Topic temp = new Topic();
                temp.Name = record.GetHashCode().ToString();
                _topicTree.Insert(temp, DuplicatePolicy.ReplaceFirst, out pre);
            }
        }
        [HttpGet]
        public IHttpActionResult Add(string name)
        {

            Topic previous;
            var topic = new Topic();
            topic.Name = name ?? "Computer";
            _topicTree.Insert(topic, DuplicatePolicy.ReplaceFirst, out previous);
            return Json(previous);

        }


        [HttpGet]
        public IHttpActionResult Topics()
        {
            return Json(_topicTree);

        }


        [HttpGet]
        public IHttpActionResult Root()
        {
            var root = _topicTree.Root;
            return Json(root);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Page(int pageIndex = 0, int pageSize = 10)
        {
            PagedList<Topic> pagelist = new PagedList<Topic>(_topicTree.ToList(), pageIndex, pageSize);
            return Json(new
            {
                TotalCount = pagelist.TotalCount,
                Data = pagelist,
                TotalPages = pagelist.TotalPages,
                HasPreviousPage = pagelist.HasNextPage,
                HasNextPage = pagelist.HasNextPage
            });
        }

        [HttpGet]
        public IHttpActionResult Find()
        {
            Topic temp;
            _topicTree.Find(_topicTree.Root.Item, true, true, out temp);
            if (temp == null)
                return NotFound();
            return Json(temp);
        }


    }
}
