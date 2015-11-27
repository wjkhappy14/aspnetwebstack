using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost.DelegatingHandlers;

namespace System.Web.Http.SelfHost.Controllers
{


    /// <summary>
    /// 
    /// </summary>
    public class BlogController : ApiController
    {


        public BlogController()
        {
            
        }

        [HttpGet]
        public IHttpActionResult Config()
        {
            return Json(this.Configuration);
        }

        [HttpGet]
        public IHttpActionResult This()
        {

            return Json(this);
        }


        [HttpGet]
        public IHttpActionResult ActionContextInfo()
        {

            return Json(base.ActionContext);

        }

        [HttpGet]
        public IHttpActionResult RequestUri()
        {
            return Json(this.Request.RequestUri);
        }



        [HttpGet]
        public HttpResponseMessage Cookies()
        {
            string sessionId = Request.Properties[SessionIdHandler.SessionIdToken] as string;
            return new HttpResponseMessage()
            {
                Content = new StringContent("Your session ID = " + sessionId)
            };
         
        }


        public async Task<HttpResponseMessage> PostFormData()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartFormDataStreamProvider(Configuration.VirtualPathRoot);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                    Trace.WriteLine("Server file path: " + file.LocalFileName);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
