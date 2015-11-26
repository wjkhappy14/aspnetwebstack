using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Http.SelfHost.Models
{


    /// <summary>
    /// 添加评论 时间参数
    /// </summary>
    public class CommentArgs : EventArgs
    {
        public CommentArgs(Comment comment)
        {
            this.Comment = comment;
        }
        public Comment Comment { get; }
    }
}
