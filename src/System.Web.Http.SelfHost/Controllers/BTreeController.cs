using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost.Models;
using System.Web.Http.SelfHost.WebMatrix;
using Wintellect.PowerCollections;
using Wintellect.PowerCollections.BTree;

namespace System.Web.Http.SelfHost.Controllers
{
    /// <summary>
    /// https://github.com/rdcastro/btree-dotnet
    /// </summary>
    public class BTreeController : ApiController
    {
        BTree<long, Category> bTree = SingleBTree<long, Category>.Instance;
        InterlockedInt32 interLock = new InterlockedInt32(100);
        DynamicDataReader reader = new DynamicDataReader();


        public BTreeController()
        {


            InitCategoryBTree();
        }

        [HttpGet]
        public IHttpActionResult Tree()
        {
            return Json(bTree);
        }

        private void InitCategoryBTree()
        {
          
            var result = reader.Query("Select *   FROM [UserCenter].[dbo].[Members]");
            var list = new List<Topic>();
            foreach (Object record in result)
            {
                Category category = new Category();
                category.Name = record.GetHashCode().ToString();
                if (interLock.TryChange((int)category.Id))
                    bTree.Insert(category.Id, category);
            }
        }

        [HttpGet]
        public IHttpActionResult Add()
        {
            return Json(bTree.Root);
        }

        [HttpGet]
        public IHttpActionResult Search(long key)
        {
            var result = bTree.Search(key);
            return Json(result);
        }

        [HttpGet]
        public IHttpActionResult Info()
        {
            return Json(new
            {
                Degree = bTree.Degree,
                Root = bTree.Root,
                Height = bTree.Height

            });
        }

        public IHttpActionResult Remove(long key)
        {
            bTree.Delete(key);
            return this.Info();
        }


        public IHttpActionResult PostData()
        {

            string cmdText = "";


          int result=   reader.GetDB().Execute(cmdText);
            return Json(new { });


        }

    }
}
