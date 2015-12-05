using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Web.Http.SelfHost.Models;
using System.Diagnostics;
using System.Web.Http.SelfHost.Authentications;
using System.Web.Http.Cors;

namespace System.Web.Http.SelfHost.Controllers
{


    [EnableCors(origins: "*", headers: "Name=Angkor", methods: "POST")]
    // [WwwAuthorize]
    public class CommentController : ApiController
    {
        public CommentController()
        {

        }

        [HttpGet]
        public IHttpActionResult Partition()
        {
            var array = Enumerable.Range(1, 1000).ToArray<int>();
            //分隔一维数组的一部分。
            ArraySegment<int> segment = new ArraySegment<int>(array);
            // System.Buffer
            OrderablePartitioner<int> data = Partitioner.Create<int>(array, true);
            return Json(data);
        }
        [HttpGet]
        public IHttpActionResult Add(string title)
        {
            if (!ModelState.IsValid)
            {

            }

            Comment comment = new Comment();
            //  comment.CommentAdd += Comment_CommentAdd;
            comment.Title = title ?? string.Empty;
            comment.Do();
            return Json(comment);
        }

        private void Comment_CommentAdd(object sender, CommentArgs e)
        {
            Debug.WriteLine(e.Comment);
        }
    }
}
