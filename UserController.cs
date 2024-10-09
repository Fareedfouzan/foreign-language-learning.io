using ConversationApp.Framework.Interfaces;
using ConversationApp.Data;
using ConversationApp.Framework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConversationApp.Controllers
{
    public class UserController : Controller
    {
        private CaDbContext _dbContext;
        private readonly IUserService userService;

        public UserController(IUserService userService,CaDbContext caDbContext)
        {
            this.userService = userService;
            this._dbContext = caDbContext;
        }
        // GET: UserController
        public ActionResult Profile(UserProfileModel model)
        {
          // Check if the user is authenticated
    if (User.Identity != null && User.Identity.IsAuthenticated)
    {
        var identity = (ClaimsIdentity)User.Identity;

        // Check if the "Email" claim exists before attempting to access it
        var emailClaim = identity.FindFirst(ClaimTypes.Email);
        if (emailClaim != null)
        {
            string userEmail = emailClaim.Value;

            if (model.User == null)
            {
                model.User = userService.GetUserByEmail(userEmail);
            }

            return View(model);
        }
    }
     
      // Handle the case when the user is not authenticated or the email claim is not present
      return RedirectToAction("Login"); // Redirect to the login page or another appropriate action
        }

        public IActionResult ChangeProficiencyLevel(UserProfileModel model)
        {
           userService.ChangeProficiencyLevel(model.User, model.ProficiencyLevel);
           return RedirectToAction("Profile", model);
        }


        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Activities()
        {
            return View();
        }

        public IActionResult ConversationContext()
        {
           return View();
        }

        public IActionResult ConversationPerson(string level, string context, string subContext){
            using (var cadbcontext = new CaDbContext())
    {
        var result = cadbcontext.Conversations
                                .Where(x => x.Level == level && x.Context == context && x.Subcontext == subContext)
                                .Select(x => new UserInputModel()
                                {
                                    Conversation = new Conversation()
                                    {
                                        Level = x.Level,
                                        Context = x.Context,
                                        Subcontext = x.Subcontext
                                    }
                                })
                                .FirstOrDefault(); // Fetching a single UserInputModel object

        if (result != null)
        {
            return View(result);
        }
        else{
            return View("Error");
        }
        }
        }

        public ActionResult ConversationStart(string level, string context, string subContext)
{
    
    using (var cadbcontext = new CaDbContext())
    {
    try{
        var result = cadbcontext.Conversations
                                .Include(x => x.Contents)
                                .Where(x => x.Level == level && x.Context == context && x.Subcontext == subContext)
                                .Select(x => new UserInputModel()
                                {
                                    Conversation = new Conversation()
                                    {
                                        Level = x.Level,
                                        Context = x.Context,
                                        Subcontext = x.Subcontext
                                    },
                                    ConversationContent = new ConversationContent(){
                                        Person = x.Contents.FirstOrDefault().Person 
                                    }

                                })
                                .FirstOrDefault(); // Fetching a single UserInputModel object

        if (result != null)
        {
           var conversationContent = result.ConversationContent.Person;
         
            if (conversationContent != null)
            {
                return View(result);
            }
            else{
                return View("Error");
            }
        }
        else{
        // If data is not found or an error occurs, return an appropriate view
        return View("Error");
        }
    }
    catch(Exception e){
        return StatusCode(500, $"An error occurred while processing the conversations: {e.Message}");
    }
  }
}
     
     

       [HttpGet]
        public ActionResult ConversationLine(string level, string context, string subContext)
        {
         using (var cadbcontext = new CaDbContext())
         {
            try{
             var result = cadbcontext.Conversations
                                .Include(x => x.Contents)
                                .Include(x => x.WordHints)
                                .Where(x => x.Level == level && x.Context == context && x.Subcontext == subContext)
                                .Select(x => new UserInputModel()
                                {
                                    Conversation = new Conversation()
                                    {
                                        Level = x.Level,
                                        Context = x.Context,
                                        Subcontext = x.Subcontext
                                    },
                                    ConversationContent = new ConversationContent()
                                    {
                                        Person = x.Contents.FirstOrDefault().Person,
                                        Line = x.Contents.FirstOrDefault().Line 
                                    },
                                    conversationContent = x.Contents.ToList(),

                                    Hints = x.WordHints.ToList()

                                })
                                .FirstOrDefault();

             if (result != null)
            {
                return View(result);
            }
            return View("Error", "Conversation not found.");
            }
            catch(Exception e){
                return StatusCode(500,$"{e.Message}");
            }
         }
        }



        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult ProfileLang()
        {
            return View();
        }

        [HttpPost]
public async Task<ActionResult> SaveLanguage(long languageId)
{
    using (var cadbcontext = new CaDbContext())
    {
    try
    { 
    if (User.Identity != null && User.Identity.IsAuthenticated)
    {
        // Retrieve the selected language from the database
        var selectedLanguage = await cadbcontext.Languages.FirstOrDefaultAsync(lang => lang.Id == languageId);


        if (selectedLanguage != null)
        {
        
        var identity = (ClaimsIdentity)User.Identity;

         // Get the logged-in user's email from the claims
        var emailClaims = identity.FindFirst(ClaimTypes.Email);

        string userEmail = emailClaims.Value;

        // Retrieve the user from the database using the email
        var users = await cadbcontext.Users.FirstOrDefaultAsync(u => u.EmailAddress == userEmail);

       if(users != null){
    
               // Update the user's language
                users.LanguageId = selectedLanguage.Id;
                cadbcontext.SaveChanges();

                // Populate UserProfileModel with user and language data
                var userProfileModel = new UserProfileModel
                {
                    User = users,
                    Language = selectedLanguage
                };

                return Json(new { success = true });
            }
            else{
                 return Json(new { error = "selected language not recognised" });
            }
        }
        else{
             return Json(new { error = "language ID not identified" });
        }
      }
      else{
       // If user is not authenticated or language selection fails
       return Json(new { error = "language not recognised" });
      }
    }
    catch(Exception e){
        return Json(new { error = true, message = e.Message });
    }
  } 
}  


        public ActionResult ProfileSettings()
        {
            return View();
        }

        public ActionResult Progress()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

    }
}
