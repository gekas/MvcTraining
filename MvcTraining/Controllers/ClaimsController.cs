using Microsoft.Owin.Security;
using MvcTraining.Models.View;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Users.Controllers
{
    [Authorize]
    public class ClaimsController : Controller
    {
        public ActionResult Index()
        {
            ClaimsIdentity ident = HttpContext.User.Identity as ClaimsIdentity;
            if (ident == null)
            {
                return View("Error", new string[] { "No claims available" });
            }
            else
            {
                return View(ident.Claims.OrderBy(x => x.Issuer).ToList());
            }
        }

        [HttpPost]
        public ActionResult Update(List<ClaimViewModel> claimsAfterUpdate)
        {
            ClaimsIdentity ident = HttpContext.User.Identity as ClaimsIdentity;

            foreach (var oldClaim in ident.Claims)
            {
                if(oldClaim.Issuer != ClaimsIdentity.DefaultIssuer)
                    ident.TryRemoveClaim(oldClaim);
            }

            var newClaims = claimsAfterUpdate.Where(c => c.Issuer != ClaimsIdentity.DefaultIssuer)
                                             .Select(c => new Claim(c.Type, c.Value, c.ValueType, c.Issuer))
                                             .ToList();
            ident.AddClaims(newClaims);

            var AuthenticationManager = HttpContext.GetOwinContext().Authentication;
            AuthenticationManager.SignOut();
            AuthenticationManager.SignIn(new AuthenticationProperties
            {
                IsPersistent = false
            }, ident);

            return RedirectToAction("Index");
        }
    }
}