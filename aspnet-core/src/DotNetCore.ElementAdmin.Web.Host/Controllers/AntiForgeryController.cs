using Microsoft.AspNetCore.Antiforgery;
using DotNetCore.ElementAdmin.Controllers;

namespace DotNetCore.ElementAdmin.Web.Host.Controllers
{
    public class AntiForgeryController : ElementAdminControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
