using ConversationApp.Framework.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

public class SessionManager
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionManager(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public UserInputModel GetUserInputModelFromSession()
    {
        // Access the current HttpContext from IHttpContextAccessor
        HttpContext httpContext = _httpContextAccessor.HttpContext;

        // Check if the session contains the model data
        if (httpContext.Session.TryGetValue("UserInputModel", out byte[] modelData))
        {
            // Deserialize the model data from JSON
            string jsonData = System.Text.Encoding.UTF8.GetString(modelData);
            UserInputModel userInputModel = JsonConvert.DeserializeObject<UserInputModel>(jsonData);

            return userInputModel;
        }
        else
        {
            // If the model data is not found in session, return null or handle as appropriate
            return null;
        }
    }
}
