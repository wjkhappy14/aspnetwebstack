using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Http.SelfHost.Controllers
{
    /*
        使用 unsafe 关键字，它使您能够在 Copy 方法中使用指针。
        fixed 语句用于声明指向源数组和目标数组的指针。
        这将锁定源数组和目标数组在内存中的位置，使其不会因为垃圾回收操作而移动。
        这些数组的内存块将在 fixed 块结束时取消锁定。 
        因为此示例中的 Copy 方法使用了 unsafe 关键字，所以必须使用 /unsafe 编译器选项编译此方法。
        若要在 Visual Studio 中设置选项，请右击项目名称，然后单击“属性”。 在“生成”选项卡上，选择“允许不安全代码”。 
        */

    /// <summary>
    /// https://msdn.microsoft.com/zh-cn/library/28k1s2k6(v=vs.110).aspx
    /// </summary>
    unsafe public class unsafeController : ApiController
    {
        /// <summary>
        /// unsafe  相关处理
        /// </summary>
        public unsafeController()
        {

        }

        // The unsafe keyword allows pointers to be used in the following method.

        static unsafe void Copy(byte[] source, int sourceOffset, byte[] target,
            int targetOffset, int count)
        {
            // If either array is not instantiated, you cannot complete the copy.
            if ((source == null) || (target == null))
            {
                throw new System.ArgumentException();
            }

            // If either offset, or the number of bytes to copy, is negative, you
            // cannot complete the copy.
            if ((sourceOffset < 0) || (targetOffset < 0) || (count < 0))
            {
                throw new System.ArgumentException();
            }

            // If the number of bytes from the offset to the end of the array is 
            // less than the number of bytes you want to copy, you cannot complete
            // the copy. 
            if ((source.Length - sourceOffset < count) ||
                (target.Length - targetOffset < count))
            {
                throw new System.ArgumentException();
            }

            // The following fixed statement pins the location of the source and
            // target objects in memory so that they will not be moved by garbage
            // collection.
            fixed (byte* pSource = source, pTarget = target)
            {
                // Set the starting points in source and target for the copying.
                byte* ps = pSource + sourceOffset;
                byte* pt = pTarget + targetOffset;

                // Copy the specified number of bytes from source to target.
                for (int i = 0; i < count; i++)
                {
                    *pt = *ps;
                    pt++;
                    ps++;
                }
            }
        }



        /// <summary>
        /// 非托管代码的基本操作
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public IHttpActionResult Basic()
        {
            byte[] byteArray1 = Encoding.Unicode.GetBytes("Hello Angkor!");
            byte[] byteArray2 = new byte[byteArray1.Length];
            Copy(byteArray1, 0, byteArray2, 0, byteArray2.Length);
            var data = byteArray1.Concat(byteArray2);
            var msg = Encoding.Unicode.GetString(data.ToArray<byte>());
            return Json(msg);


        }

    }
}
