using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.SelfHost.Streams;

namespace System.Web.Http.SelfHost.Controllers
{


    /// <summary>
    /// WaitHandle封装等待对共享资源的独占访问的操作系统特定的对象。
    /// </summary>
    public class WaitHandleController : ApiController
    {
        private PushStreamContent _pushStreamContent = new PushStreamContent((stream, content, transport) =>
        {
            byte[] buffer = new byte[stream.Length];
            return stream.ReadAsync(buffer, 0,buffer.Length);
        });


        /// <summary>
        /// WaitHandle
        /// </summary>

        public WaitHandleController()
        {





        }
        [HttpGet]
        public async Task<IHttpActionResult> PushStream()
        {
            string content = await _pushStreamContent.ReadAsStringAsync();
            return Json(content);
        }



        [HttpGet]
        public HttpResponseMessage Video(string name, string ext)
        {
            ext = ".pdb";
            name = "System.Web.Http.SelfHost";
            var currentDir=  Environment.CurrentDirectory;
            var filePath = System.IO.Path.Combine(currentDir,name);
            var video = new VideoStream(name, ext);
            var response = Request.CreateResponse();
            onStreamAvailable = video.WriteToStream;
            PushStreamContent videoContent = new PushStreamContent(onStreamAvailable, new MediaTypeHeaderValue("txt/json"));
            response.Content = videoContent;
            return response;
        }
        private Action<Stream, HttpContent, TransportContext> onStreamAvailable;


    }
}
