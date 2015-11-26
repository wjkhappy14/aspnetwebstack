using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Wintellect.PowerCollections
{


    /// <summary>
    /// 
    /// </summary>
    public class MagicExpressions : Expression
    {
        /// <summary>
        /// 计算阶乘 4x3x2x1
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Expression<Func<T, T>> MakeFactorialExpression<T>()
        {
            var nParam = Expression.Parameter(typeof(T), "n");
            var methodVar = Expression.Variable(typeof(Func<T, T>), "factorial");
            var one = Expression.Convert(Expression.Constant(1), typeof(T));

            return Expression.Lambda<Func<T, T>>(
                Expression.Block(
                    // Func<uint, uint> method;
                    new[] { methodVar },
                    // method = n => ( n <= 1 ) ? 1 : n * method( n - 1 );
                    Expression.Assign(
                        methodVar,
                        Expression.Lambda<Func<T, T>>(
                            Expression.Condition(
                                // ( n <= 1 )
                                Expression.LessThanOrEqual(nParam, one),
                                // 1
                                one,
                                // n * method( n - 1 )
                                Expression.Multiply(
                                    // n
                                    nParam,
                                    // method( n - 1 )
                                    Expression.Invoke(
                                        methodVar,
                                        Expression.Subtract(nParam, one)))),
                            nParam)),
                    // return method( n );
                    Expression.Invoke(methodVar, nParam)),
                nParam);
        }


        /// <summary>
        /// 
        /// </summary>
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Multiply;
            }
        }


    }
}
