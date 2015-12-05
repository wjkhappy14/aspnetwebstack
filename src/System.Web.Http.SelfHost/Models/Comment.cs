using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Wintellect.PowerCollections;

namespace System.Web.Http.SelfHost.Models
{

    public class Comment
    {

        public Comment()
        {
            CommentTree = new RedBlackTree<Comment>(Comparer<Comment>.Default);
            // CommentTree.Root = new Node<Comment>() { Item = this }; ;
        }
        private Guid _id = Guid.NewGuid();

        /// <summary>
        /// 事件是特殊类型的多路广播委托，仅可从声明它们的类或结构（发行者类）中调用。
        /// </summary>
        public event EventHandler<CommentArgs> CommentAdd = (x, y) =>
        {
            System.Diagnostics.Debug.WriteLine(y);
        };
        /// <summary>
        /// 记录日志
        /// </summary>
        public event EventHandler<CommentArgs> Log = (sender, args) =>
        {

            System.Diagnostics.Debug.Print(" Begin Do Log AddCommentEvent");

        };


        public void Do()
        {
            System.Diagnostics.Debug.Print(" Begin Do AddCommentEvent");
            // Copy to a temporary variable to be thread-safe.
            EventHandler<CommentArgs> temp = CommentAdd;
            //激活事件
            if (temp != null)
            {
                temp(this, new CommentArgs(this));
            }
            if (Log != null)
            {
                Log(this, new CommentArgs(this));
            }
            System.Diagnostics.Debug.Print("End  Do AddCommentEvent");

        }

        public Guid Id { get { return _id; } private set { _id = Guid.NewGuid(); } }



        [Required(AllowEmptyStrings = false, ErrorMessage = "Title 　是必须的")]
        public string Title { get; set; }
        public DateTime CreatedTime
        {
            get { return DateTime.Now; }
            private set { value = DateTime.Now; }
        }


        /// <summary>
        /// 评论树
        /// </summary>
        public RedBlackTree<Comment> CommentTree { get; }

    }
}
