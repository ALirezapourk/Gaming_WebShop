using System.Security.Claims;
using MerchStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace MerchStore.Controllers;

public class AccountController : Controller
{
    // Mocked user "database" for demonstration purposes.
    private const string MockedUsername = "john.doe";
    private const string MockedPassword = "pass"; // Note: NEVER hard-code passwords in real applications.

    // This is a simple login page that allows users to enter their credentials.
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // This action handles the login form submission.
    [HttpPost]
    [ValidateAntiForgeryToken] // This ensures that the form is submitted with a valid anti-forgery token to prevent CSRF attacks.
    public async Task<IActionResult> LoginAsync(LoginViewModel model)
    {
        // Check model validators
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Verify the user's credentials against the mocked database.
        if (model.Username == MockedUsername && model.Password == MockedPassword)
        {
            // Set up the session/cookie for the authenticated user.
            var claims = new[] { new Claim(ClaimTypes.Name, model.Username) };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Sign in the user with the cookie authentication scheme.
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Redirect to a secure area of your application
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt."); // Generic error message for security reasons.
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        // Sign out the user by removing the authentication cookie.
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // Redirect to a public area of your application
        return RedirectToAction("Index", "Home");
    }
}