using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Description;

namespace System.Web.Http.SelfHost.Documents
{
    public class JsonDocumentationProvider : IDocumentationProvider
    {


        public JsonDocumentationProvider()
        {



        }
        public string GetDocumentation(HttpParameterDescriptor parameterDescriptor)
        {
            throw new NotImplementedException();
        }

        public string GetDocumentation(HttpActionDescriptor actionDescriptor)
        {
            throw new NotImplementedException();
        }

        public string GetDocumentation(HttpControllerDescriptor controllerDescriptor)
        {
            throw new NotImplementedException();
        }

        public string GetResponseDocumentation(HttpActionDescriptor actionDescriptor)
        {
            throw new NotImplementedException();
        }
    }
}
