using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost.Expressions;
using Wintellect.PowerCollections;

namespace System.Web.Http.SelfHost.Controllers
{
    public class ExpressionController : ApiController
    {


        public ExpressionController()
        {

            MagicExpressions express = new MagicExpressions();
            var factorial = express.MakeFactorialExpression<UInt64>().Compile();
            var val = factorial(10);

        }




        [HttpGet]
        public IHttpActionResult Block()
        {


            BlockExpression blockExpr = Expression.Block(
               Expression.Call(
               null,
               typeof(Console).GetMethod("Write", new Type[] { typeof(String) }),
               Expression.Constant("Hello ")
         ),
            Expression.Call(
                  null,
                  typeof(Console).GetMethod("WriteLine", new Type[] { typeof(String) }),
                  Expression.Constant("World!")
         ),
           Expression.Constant(42)
         );


            return Json("");

        }



        [HttpGet]
        public IHttpActionResult CreateInstance()
        {

            var func = MyTypeActivator.Create<String>(typeof(System.DateTime));
            func.Invoke();
            return Json(func);

        }

    }
}
