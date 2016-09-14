using System.Threading;
using System.Web.Http;
using TotalApi.Core.Authentication;
using TotalApi.Utils.Mvc.Web;

namespace WebApp.Api
{
    [AppExceptionFilter]
    public class AppApiController : ApiController
    {
        public AppApiController()
        {
            // Add TotalApi-authorization info to the current identity.
            if (!TotalApiAuth.IsAuthenticated)
                TotalApiAuth.ConfigAuth(Thread.CurrentPrincipal.Identity);
        }
    }
}