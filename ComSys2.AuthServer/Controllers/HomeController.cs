using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ComSys2.AuthServer.IdentityEntities;
using Microsoft.AspNet.Identity.Owin;

namespace ComSys2.AuthServer.Controllers
{
    public class HomeController : Controller
    {
		private SignInManager _signInManager;
		private UserManager _userManager;

		public HomeController()
		{
		}

		public HomeController(UserManager userManager, SignInManager signInManager)
		{
			UserManager = userManager;
			SignInManager = signInManager;
		}

		public SignInManager SignInManager
		{
			get
			{
				return _signInManager ?? HttpContext.GetOwinContext().Get<SignInManager>();
			}
			private set
			{
				_signInManager = value;
			}
		}

		public UserManager UserManager
		{
			get
			{
				return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>();
			}
			private set
			{
				_userManager = value;
			}
		}

		// GET: Home
		[HttpGet]
        public ActionResult Login()
        {
            return View();
        }

		[HttpPost]
		public async Task<ActionResult> Login(string userName, string password, string returnUrl)
		{
			var result = await SignInManager.PasswordSignInAsync(userName, password, false, shouldLockout: false);
			switch (result)
			{
				case SignInStatus.Success:
					return Redirect(returnUrl);
				case SignInStatus.LockedOut:
					return View();
				case SignInStatus.RequiresVerification:
					return View();
				case SignInStatus.Failure:
				default:	
					return View();
			}
		}
	}
}