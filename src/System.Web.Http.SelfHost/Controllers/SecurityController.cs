using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Http.SelfHost.Controllers
{
    public class SecurityController : ApiController
    {
        private static readonly Lazy<IPrincipal> _anonymousPrincipal = new Lazy<IPrincipal>(
           () => new ClaimsPrincipal(new ClaimsIdentity()), isThreadSafe: true);



        public SecurityController()
        {


        }
    }
}
