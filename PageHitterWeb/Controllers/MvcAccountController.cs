using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PageHitterWeb;
using PageHitterWeb.Helpers;
using PageHitterWeb.Models;
using PageMonitorRepository;

namespace PageHitterWeb.Controllers
{
//    public class MvcAccountController : Controller
//    {
//        // GET: MvcAccount
//        public ActionResult Index()
//        {
//            return View();
//        }
//    }
//}


//namespace PageHitterWeb.Controllers
//{
	[Authorize]
	public class MvcAccountController : Controller
	{
		private ApplicationSignInManager _signInManager;
		private ApplicationUserManager _userManager;
		//private ApplicationRoleManager _roleManager;

		public MvcAccountController()
		{
		}

		public MvcAccountController(ApplicationUserManager userManager,
								 ApplicationSignInManager signInManager /*,
								 ApplicationRoleManager roleManager */)
		{
			UserManager = userManager;
			SignInManager = signInManager;
			//RoleManager = roleManager;
		}

		//public ApplicationRoleManager RoleManager
		//{
		//    get
		//    {
		//		return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
		//	}

		//	private set { _roleManager = value; }
		//   }
		public ApplicationSignInManager SignInManager
		{
			get
			{
				return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
			}
			private set
			{
				_signInManager = value;
			}
		}


		[AllowAnonymous]
		public ActionResult Test()
		{
			var owinContext = HttpContext.GetOwinContext();
			var iden = owinContext.Authentication.User.Identities.ToList();

			string name;

			if (iden.Any())
			{
				var authenticated = iden.First().IsAuthenticated;

				if (authenticated)
				{
					name = iden.First().Name;
				}

			}

			var isAuthenticated = owinContext.Request.User.Identity.IsAuthenticated;
			name = owinContext.Request.User.Identity.Name;
			var userName = owinContext.Request.User.Identity.GetUserName();

			var isAdmin = owinContext.Request.User.IsInRole("admin");


			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult Test(string userName)
		{
			var owinContext = HttpContext.GetOwinContext();
			var iden = owinContext.Authentication.User.Identities.ToList();

			var isAuthenticated = owinContext.Request.User.Identity.IsAuthenticated;

			return View();
		}


		public ApplicationUserManager UserManager
		{
			get
			{
				var owinContext = HttpContext.GetOwinContext();

				return _userManager ?? owinContext.GetUserManager<ApplicationUserManager>();
			}
			private set
			{
				_userManager = value;
			}
		}


		[HttpGet]
		[Authorize]
		public ActionResult ChangeAdminPassword()
		{
			var userName = System.Web.HttpContext.Current.User.Identity.Name;
			var model = new AdminModel { UserName = userName };

			return View(model);
		}

		[HttpPost]
		[Authorize]
		public ActionResult ChangeAdminPassword(FormCollection collection)
		{
			try
			{
				var model = new AdminModel();

				UpdateModel(model);

				using (var db = new PageMonitorDb())
				{
					//db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

					var hasher = new PasswordHasher();

					var securityAdmin = db.AspNetUsers.FirstOrDefault(u => u.UserName == model.UserName);

					if (securityAdmin == null) return RedirectToAction("Index");

					securityAdmin.PasswordHash = hasher.HashPassword(model.Password);
					db.AspNetUsers.AddOrUpdate(securityAdmin);
					db.SaveChanges();
				}

				return RedirectToAction("Index", "Home");
			}
			catch (Exception ex)
			{
				// ReSharper disable once UnusedVariable
				var msg = ex.Message;
				return View();
			}
		}





		//
		// GET: /Account/Login
		[HttpGet]
		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		//
		// POST: /Account/Login
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			try
			{

				//	// This doesn't count login failures towards account lockout
				//	// To enable password failures to trigger account lockout, change to shouldLockout: true

				var signInManager = SignInManager;

				var context = HttpContext.GetOwinContext();

				var appSigninManager = context.Get<ApplicationSignInManager>();


				var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);

				switch (result)
				{
					case SignInStatus.Success:
						return RedirectToLocal(returnUrl);
					case SignInStatus.LockedOut:
						return View("Lockout");
					case SignInStatus.RequiresVerification:
						return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
					case SignInStatus.Failure:
					default:
						ModelState.AddModelError("", "Invalid login attempt.");
						return View(model);
				}

			}
			catch (Exception ex)
			{
				var msg = ex.Message;

				throw;
			}
		}

			//
			// GET: /Account/VerifyCode
			//[AllowAnonymous]
			//public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
			//{
			//	// Require that the user has already logged in via username/password or external login
			//	if (!await SignInManager.HasBeenVerifiedAsync())
			//	{
			//		return View("Error");
			//	}
			//	return System.Web.UI.WebControls.View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
			//}

			//
			// POST: /Account/VerifyCode
			//[HttpPost]
			//[AllowAnonymous]
			//[ValidateAntiForgeryToken]
			//public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
			//{
			//	if (!ModelState.IsValid)
			//	{
			//		return System.Web.UI.WebControls.View(model);
			//	}

			//	// The following code protects for brute force attacks against the two factor codes. 
			//	// If a user enters incorrect codes for a specified amount of time then the user account 
			//	// will be locked out for a specified amount of time. 
			//	// You can configure the account lockout settings in IdentityConfig
			//	var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
			//	switch (result)
			//	{
			//		case SignInStatus.Success:
			//			return RedirectToLocal(model.ReturnUrl);
			//		case SignInStatus.LockedOut:
			//			return View("Lockout");
			//		case SignInStatus.Failure:
			//		default:
			//			ModelState.AddModelError("", "Invalid code.");
			//			return System.Web.UI.WebControls.View(model);
			//	}
			//}

			//
			// GET: /Account/Register
		[AllowAnonymous]
		public ActionResult Register()
		{
			return View();
		}

		//
		// POST: /Account/Register
		//[HttpPost]
		//[AllowAnonymous]
		//[ValidateAntiForgeryToken]
		//public async Task<ActionResult> Register(RegisterViewModel model)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
		//		var result = await UserManager.CreateAsync(user, model.Password);
		//		if (result.Succeeded)
		//		{
		//			await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

		//			// For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
		//			// Send an email with this link
		//			// string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
		//			// var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
		//			// await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

		//			return RedirectToAction("Index", "Home");
		//		}
		//		AddErrors(result);
		//	}

		//	// If we got this far, something failed, redisplay form
		//	return System.Web.UI.WebControls.View(model);
		//}

		//
		// GET: /Account/ConfirmEmail
		[AllowAnonymous]
		public async Task<ActionResult> ConfirmEmail(string userId, string code)
		{
			if (userId == null || code == null)
			{
				return View("Error");
			}
			var result = await UserManager.ConfirmEmailAsync(userId, code);
			return View(result.Succeeded ? "ConfirmEmail" : "Error");
		}

		//
		// GET: /Account/ForgotPassword
		[AllowAnonymous]
		public ActionResult ForgotPassword()
		{
			return View();
		}

		//
		// POST: /Account/ForgotPassword
		//[HttpPost]
		//[AllowAnonymous]
		//[ValidateAntiForgeryToken]
		//public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		var user = await UserManager.FindByNameAsync(model.Email);
		//		if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
		//		{
		//			// Don't reveal that the user does not exist or is not confirmed
		//			return View("ForgotPasswordConfirmation");
		//		}

		//		// For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
		//		// Send an email with this link
		//		// string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
		//		// var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
		//		// await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
		//		// return RedirectToAction("ForgotPasswordConfirmation", "Account");
		//	}

		//	// If we got this far, something failed, redisplay form
		//	return System.Web.UI.WebControls.View(model);
		//}

		//
		// GET: /Account/ForgotPasswordConfirmation
		[AllowAnonymous]
		public ActionResult ForgotPasswordConfirmation()
		{
			return View();
		}

		//
		// GET: /Account/ResetPassword
		[AllowAnonymous]
		public ActionResult ResetPassword(string code)
		{
			return code == null ? View("Error") : View();
		}

		//
		// POST: /Account/ResetPassword
		//[HttpPost]
		//[AllowAnonymous]
		//[ValidateAntiForgeryToken]
		//public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
		//{
		//	if (!ModelState.IsValid)
		//	{
		//		return System.Web.UI.WebControls.View(model);
		//	}
		//	var user = await UserManager.FindByNameAsync(model.Email);
		//	if (user == null)
		//	{
		//		// Don't reveal that the user does not exist
		//		return RedirectToAction("ResetPasswordConfirmation", "Account");
		//	}
		//	var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
		//	if (result.Succeeded)
		//	{
		//		return RedirectToAction("ResetPasswordConfirmation", "Account");
		//	}
		//	AddErrors(result);
		//	return View();
		//}

		//
		// GET: /Account/ResetPasswordConfirmation
		[AllowAnonymous]
		public ActionResult ResetPasswordConfirmation()
		{
			return View();
		}

		//
		// POST: /Account/ExternalLogin
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ExternalLogin(string provider, string returnUrl)
		{
			// Request a redirect to the external login provider
			return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
		}

		//
		// GET: /Account/SendCode
		//[AllowAnonymous]
		//public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
		//{
		//	var userId = await SignInManager.GetVerifiedUserIdAsync();
		//	if (userId == null)
		//	{
		//		return View("Error");
		//	}
		//	var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
		//	var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
		//	return System.Web.UI.WebControls.View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
		//}

		//
		// POST: /Account/SendCode
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> SendCode(SendCodeViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			// Generate the token and send it
			if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
			{
				return View("Error");
			}
			return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
		}

		//
		// GET: /Account/ExternalLoginCallback
		//[AllowAnonymous]
		//public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
		//{
		//	var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
		//	if (loginInfo == null)
		//	{
		//		return RedirectToAction("Login");
		//	}

		//	// Sign in the user with this external login provider if the user already has a login
		//	var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
		//	switch (result)
		//	{
		//		case SignInStatus.Success:
		//			return RedirectToLocal(returnUrl);
		//		case SignInStatus.LockedOut:
		//			return View("Lockout");
		//		case SignInStatus.RequiresVerification:
		//			return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
		//		case SignInStatus.Failure:
		//		default:
		//			// If the user does not have an account, then prompt the user to create an account
		//			ViewBag.ReturnUrl = returnUrl;
		//			ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
		//			return System.Web.UI.WebControls.View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
		//	}
		//}

		//
		// POST: /Account/ExternalLoginConfirmation
		//[HttpPost]
		//[AllowAnonymous]
		//[ValidateAntiForgeryToken]
		//public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
		//{
		//	if (User.Identity.IsAuthenticated)
		//	{
		//		return RedirectToAction("Index", "Manage");
		//	}

		//	if (ModelState.IsValid)
		//	{
		//		// Get the information about the user from the external login provider
		//		var info = await AuthenticationManager.GetExternalLoginInfoAsync();
		//		if (info == null)
		//		{
		//			return View("ExternalLoginFailure");
		//		}
		//		var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
		//		var result = await UserManager.CreateAsync(user);
		//		if (result.Succeeded)
		//		{
		//			result = await UserManager.AddLoginAsync(user.Id, info.Login);
		//			if (result.Succeeded)
		//			{
		//				await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
		//				return RedirectToLocal(returnUrl);
		//			}
		//		}
		//		AddErrors(result);
		//	}

		//	ViewBag.ReturnUrl = returnUrl;
		//	return System.Web.UI.WebControls.View(model);
		//}

		//
		// POST: /Account/LogOff
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
			return RedirectToAction("Index", "Home");
		}

		//
		// GET: /Account/ExternalLoginFailure
		[AllowAnonymous]
		public ActionResult ExternalLoginFailure()
		{
			return View();
		}


		public ActionResult RoleCreate()
		{
			return View();
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult RoleCreate(string roleName)
		{
			using (var context = new ApplicationDbContext())
			{
				var roleStore = new RoleStore<IdentityRole>(context);
				var roleManager = new RoleManager<IdentityRole>(roleStore);

				if (roleManager.RoleExists(roleName))
				{
					ViewBag.ResultMessage = "Role already exists";
					return RedirectToAction("RoleIndex", "Account");
				}

				roleManager.Create(new IdentityRole(roleName));
				context.SaveChanges();

				ViewBag.ResultMessage = "Role created successfully";
				return RedirectToAction("RoleIndex", "Account");
			}
		}

		public ActionResult RoleIndex()
		{
			List<string> roles;

			using (var context = new ApplicationDbContext())
			{
				var roleStore = new RoleStore<IdentityRole>(context);
				var roleManager = new RoleManager<IdentityRole>(roleStore);

				roles = roleManager.Roles.Select(r => r.Name).ToList();
			}

			return View(roles.ToList());
		}

		public ActionResult RoleDelete(string roleName)
		{
			using (var context = new ApplicationDbContext())
			{
				var roleStore = new RoleStore<IdentityRole>(context);
				var roleManager = new RoleManager<IdentityRole>(roleStore);

				var role = roleManager.FindByName(roleName);

				if (role != null)
				{
					roleManager.Delete(role);
					context.SaveChanges();
					ViewBag.ResultMessage = "Role deleted successfully";
				}
				else
				{
					ViewBag.ResultMessage = "Role not found";
				}
			}

			return RedirectToAction("RoleIndex", "Account");
		}

		public ActionResult RoleAddToUser()
		{
			List<string> roles;
			List<string> users;

			using (var context = new ApplicationDbContext())
			{
				var roleStore = new RoleStore<IdentityRole>(context);
				var roleManager = new RoleManager<IdentityRole>(roleStore);

				var userStore = new UserStore<ApplicationUser>(context);
				var userManager = new UserManager<ApplicationUser>(userStore);

				users = userManager.Users.Select(u => u.UserName).ToList();
				roles = roleManager.Roles.Select(r => r.Name).ToList();
			}

			ViewBag.Roles = new SelectList(roles);
			ViewBag.Users = new SelectList(users);

			return View();
		}


		[Authorize(Roles = "Admin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult RoleAddToUser(string roleName, string userName)
		{
			List<string> roles;
			List<string> users;

			using (var context = new ApplicationDbContext())
			{
				var roleStore = new RoleStore<IdentityRole>(context);
				var roleManager = new RoleManager<IdentityRole>(roleStore);

				var userStore = new UserStore<ApplicationUser>(context);
				var userManager = new UserManager<ApplicationUser>(userStore);

				users = (from u in userManager.Users select u.UserName).ToList();
				roles = (from r in roleManager.Roles select r.Name).ToList();

				var user = userManager.FindByName(userName);

				if (user == null)
				{
					//return View("Index");
					throw new Exception("User not found!");
				}

				var role = roleManager.FindByName(roleName);

				if (role == null)
				{
					//return View("Index");
					throw new Exception("Role not found!");
				}

				if (userManager.IsInRole(user.Id, role.Name))
				{
					ViewBag.ResultMessage = "This user already has role specified";
				}
				else
				{
					userManager.AddToRole(user.Id, role.Name);
					context.SaveChanges();

					ViewBag.ResultMessage = "Username added to the role";
				}

				roles = roleManager.Roles.Select(r => r.Name).ToList();
			}

			ViewBag.Roles = new SelectList(roles);
			ViewBag.Users = new SelectList(users);

			return View();
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult GetRoles(string userName)
		{
			if (!string.IsNullOrEmpty(userName))
			{
				List<string> userRoles;
				List<string> roles;
				List<string> users;

				using (var context = new ApplicationDbContext())
				{
					var roleStore = new RoleStore<IdentityRole>(context);
					var roleManager = new RoleManager<IdentityRole>(roleStore);

					roles = (from r in roleManager.Roles select r.Name).ToList();

					var userStore = new UserStore<ApplicationUser>(context);
					var userManager = new UserManager<ApplicationUser>(userStore);

					users = (from u in userManager.Users select u.UserName).ToList();

					var user = userManager.FindByName(userName);
					if (user == null)
						throw new Exception("User not found!");

					var userRoleIds = user.Roles.Select(r => r.RoleId);       //(from r in user.Roles select r.RoleId);

					userRoles = userRoleIds
								.Select(id => new { id, r = roleManager.FindById(id) })
								.Select(@t => @t.r.Name)
								.ToList();
				}

				ViewBag.Roles = new SelectList(roles);
				ViewBag.Users = new SelectList(users);
				ViewBag.RolesForThisUser = userRoles;
			}


			return View("RoleAddToUser");
		}


		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteRoleForUser(string userName, string roleName)
		{
			List<string> userRoles;
			List<string> roles;
			List<string> users;
			using (var context = new ApplicationDbContext())
			{
				var roleStore = new RoleStore<IdentityRole>(context);
				var roleManager = new RoleManager<IdentityRole>(roleStore);

				roles = (from r in roleManager.Roles select r.Name).ToList();

				var userStore = new UserStore<ApplicationUser>(context);
				var userManager = new UserManager<ApplicationUser>(userStore);

				users = (from u in userManager.Users select u.UserName).ToList();

				var user = userManager.FindByName(userName);
				if (user == null)
					throw new Exception("User not found!");

				if (userManager.IsInRole(user.Id, roleName))
				{
					userManager.RemoveFromRole(user.Id, roleName);
					context.SaveChanges();

					ViewBag.ResultMessage = "Role removed from this user successfully !";
				}
				else
				{
					ViewBag.ResultMessage = "This user doesn't belong to selected role.";
				}

				var userRoleIds = (from r in user.Roles select r.RoleId);
				userRoles = (from id in userRoleIds
							 let r = roleManager.FindById(id)
							 select r.Name).ToList();
			}

			ViewBag.RolesForThisUser = userRoles;
			ViewBag.Roles = new SelectList(roles);
			ViewBag.Users = new SelectList(users);
			return View("RoleAddToUser");
		}



		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_userManager != null)
				{
					_userManager.Dispose();
					_userManager = null;
				}

				if (_signInManager != null)
				{
					_signInManager.Dispose();
					_signInManager = null;
				}
			}

			base.Dispose(disposing);
		}

		#region Helpers
		// Used for XSRF protection when adding external logins
		private const string XsrfKey = "XsrfId";

		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return HttpContext.GetOwinContext().Authentication;
			}
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
		}

		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			return RedirectToAction("Index", "Home");
		}

		internal class ChallengeResult : HttpUnauthorizedResult
		{
			public ChallengeResult(string provider, string redirectUri)
				: this(provider, redirectUri, null)
			{
			}

			public ChallengeResult(string provider, string redirectUri, string userId)
			{
				LoginProvider = provider;
				RedirectUri = redirectUri;
				UserId = userId;
			}

			public string LoginProvider { get; set; }
			public string RedirectUri { get; set; }
			public string UserId { get; set; }

			public override void ExecuteResult(ControllerContext context)
			{
				var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
				if (UserId != null)
				{
					properties.Dictionary[XsrfKey] = UserId;
				}
				context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
			}
		}
		#endregion
	}
}