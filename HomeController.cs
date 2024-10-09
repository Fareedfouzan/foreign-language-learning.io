using ConversationApp.Framework.Interfaces;
using ConversationApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ConversationApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHistoryService historyService;
        private readonly IUserService userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(IHistoryService historyService, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            this.historyService = historyService;
            this.userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task Login()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
            
        }

        public async Task<ActionResult> GoogleResponse()
        {
            try{
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });
            var identity = (ClaimsIdentity)User.Identity;
            string userEmail = identity.FindFirst(ClaimTypes.Email).Value;
            string userName = identity.FindFirst(ClaimTypes.Name).Value;

            
            // Retrieve user from the database or create if not exists
            var user = userService.GetUserByEmail(userEmail);

            DateTime registerDate = DateTime.Now.Date;

            int languageId = 1;

            if (!userService.UserExistsInDatabase(userEmail))
            {
                userService.AddUserRecord(userEmail, userName, registerDate, languageId);

                // Set session variables
               _httpContextAccessor.HttpContext.Session.SetString("UserEmail", userEmail);
                _httpContextAccessor.HttpContext.Session.SetString("UserName", userName);
            }
            else{

              DateTime joinDate = DateTime.Now;
              historyService.InsertHistoryRecord($"User {userEmail} loged in", joinDate);
              
              // Set session variables
              _httpContextAccessor.HttpContext.Session.SetString("UserEmail", userEmail);
              _httpContextAccessor.HttpContext.Session.SetString("UserName", userName);

            }
            
            return View("~/Views/User/Home.cshtml");
             
            }
             catch (Exception ex)
            {
                Console.WriteLine($"Error in GoogleResponse: {ex.Message}");
                // Log the exception or handle it appropriately
                return View("Error"); // You might want to redirect to an error page
            }
        }


        public async Task<ActionResult> Logout()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
            await HttpContext.SignOutAsync();
            return View("~/Views/User/Login.cshtml");
        }
    }
}