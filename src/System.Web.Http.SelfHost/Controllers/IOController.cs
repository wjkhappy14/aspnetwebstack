using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Web.Http.SelfHost.Controllers
{
    public class IOController : ApiController
    {

        public IOController()
        {

            
        }



        /// <summary>
        /// 非托管 非托管内存块
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        unsafe public IHttpActionResult Unmanaged()
        {
            //使用 UnmanagedMemoryStream 类读取和写入非托管内存。 使用 Marshal 类分配和解除分配非托管内存块。
            // Create some data to read and write.
            byte[] message = UnicodeEncoding.Unicode.GetBytes("Hello Angkor");

            // Allocate a block of unmanaged memory and return an IntPtr object.	
            IntPtr memIntPtr = Marshal.AllocHGlobal(message.Length);

            // Get a byte pointer from the IntPtr object.
            byte* memBytePtr = (byte*)memIntPtr.ToPointer();

            // Create an UnmanagedMemoryStream object using a pointer to unmanaged memory.
            UnmanagedMemoryStream writeStream = new UnmanagedMemoryStream(memBytePtr, message.Length, message.Length, FileAccess.Write);

            // Write the data.
            writeStream.Write(message, 0, message.Length);
            // Close the stream.
            writeStream.Close();
            // Create another UnmanagedMemoryStream object using a pointer to unmanaged memory.
            UnmanagedMemoryStream readStream = new UnmanagedMemoryStream(memBytePtr, message.Length, message.Length, FileAccess.Read);
            // Create a byte array to hold data from unmanaged memory.
            byte[] outMessage = new byte[message.Length];
            // Read from unmanaged memory to the byte array.
            readStream.Read(outMessage, 0, message.Length);
            // Close the stream.
            readStream.Close();
            // Display the data to the console.

            var msg = UnicodeEncoding.Unicode.GetString(outMessage);
            // Free the block of unmanaged memory.
            Marshal.FreeHGlobal(memIntPtr);
            return Json(msg);


        }


    }
}
