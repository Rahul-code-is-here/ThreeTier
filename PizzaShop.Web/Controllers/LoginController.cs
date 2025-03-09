using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using PizzaShop.Domain.DataModels;
using PizzaShop.Service.Implementaion;
using PizzaShop.Repository.Interface;
using PizzaShop.Service.Interface;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using PizzaShop.Domain.DataContext;
using PizzaShop.Domain.ViewModels;
// using PizzaShop.Web.Models;

namespace PizzaShop.Web.Controllers;


public class LoginController : Controller
{
    // private readonly IUnitOfWork _unitOfWork;
    private readonly PizzaShemaContext dbo;

    private readonly IAuthService _authService;

    private readonly IUserServices _userServices;

    private readonly IEmailSender _emailSender;

    private readonly IAuthTokenService _authTokenService;

    public LoginController(IAuthService authService, IAuthTokenService authTokenService, IEmailSender emailSender, PizzaShemaContext dbContext, IUserServices userServices)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _authTokenService = authTokenService;
        _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        dbo = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _userServices = userServices;
    }


    [HttpGet]
    public IActionResult Login()
    {
        var token = Request.Cookies["AuthToken"];

        if (!string.IsNullOrEmpty(token))
        {
            string email = ExtractEmailFromToken(token);

            if (!string.IsNullOrEmpty(email))
            {
                var user = dbo.Users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    return user.RoleId == 1
                        ? RedirectToAction("Dashboard", "Home")
                        : RedirectToAction("Privacy", "Home");
                }
            }
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        if (!ModelState.IsValid) return View("Login");

        var user = await _authService.Login(loginModel.Email, loginModel.password);

        if (user == null)
        {
            ModelState.AddModelError("Email", "Wrong Email or Password");
            return View("Login");
        }

        string token = _authTokenService.GenerateJwtToken(user, loginModel.RememberMe);

        Response.Cookies.Append("AuthToken", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTime.UtcNow.AddHours(2)
        });

        // Extract Role ID from Token
        string roleId = ExtractRoleFromToken(token);

        // Redirect Based on Role
        if (roleId == "1")
        {
            return RedirectToAction("Dashboard", "Home");
        }
        else if (roleId == "2")
        {
            return RedirectToAction("Privacy", "Home");
        }

        return RedirectToAction("Login");
    }


    public string GetEmailBody(string link)
    {
        return $@"
            <html>
            <head>
                <title>Email Title</title>
            </head>
            <body>
                <div style='background-color: #0073C6; text-align:center; padding: 20px;'>
                    <h1>PIZZA SHOP</h1>
                </div>
                <div>Welcome to pizza shop</div>
                <div>Please click <a href='{link}' style='color:blue'>here</a> to reset your account password.</div>
                <div>If you encounter any issue or have any questions, please do not hesitate to contact our support team.</div>
                <div><span style='color:yellow'>Important Note:</span> For security reasons, the link will expire in 24 hours. If you did not request a password reset, please ignore this email or contact our support team.</div>
            </body>
            </html>";
    }

    [HttpGet]
    public IActionResult Forgot()
    {
        return View();
    }

 

    [HttpPost]
    public async Task<IActionResult> Forgot(ForgotPasswordModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Forgot", model);
        }

        var user = await _authService.GetUserAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError("Email", "Email not found");
            return View("Forgot", model);
        }

        // Generate token
        var token = _authService.GenerateJwtToken(user, rememberMe: false);

        // Create reset link with properly encoded token
        string resetLink = Url.Action("ResetPassword", "Login",
                                     new { token = Uri.EscapeDataString(token) },
                                     Request.Scheme);

        // Send email with the reset link
        string emailBody = GetEmailBody(resetLink);
        bool result = await _emailSender.SendEmailAsync(model.Email, "Reset Password", emailBody);

        if (!result)
        {
            ModelState.AddModelError("Email", "Failed to send email");
            return View("Forgot", model);
        }

        return RedirectToAction("Login", "Login");
    }


    [HttpGet]
    public IActionResult ResetPassword(string token)
    {
        // Validate the token and show the reset password form
        var model = new ResetPasswordModel { Token = token };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _authService.ResetPasswordAsync(model.Token, model.NewPassword);
        if (!result)
        {
            ModelState.AddModelError(string.Empty, "Failed to reset password");
            return View(model);
        }

        return RedirectToAction("Login", "Login");
    }



    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["error"] = "Invalid input. Please try again.";
            return View(model);
        }

        var token = Request.Cookies["AuthToken"];
        var email = ExtractEmailFromToken(token);

        if (string.IsNullOrEmpty(email))
            return RedirectToAction("Login", "Auth");

        var success = await _authService.ChangePasswordAsync(email, model);
        if (!success)
        {
            TempData["error"] = "Current password is incorrect.";
            return View(model);
        }

        TempData["success"] = "Password changed successfully!";
        return RedirectToAction("ChangePassword");
    }



    private string ExtractEmailFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

        return emailClaim?.Value;
    }

    private string ExtractRoleFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

        return roleClaim?.Value;
    }


    [HttpPost]
    public IActionResult Logout()
    {
        // Remove the AuthToken cookie
        Response.Cookies.Delete("AuthToken");
        return RedirectToAction("Login", "Login");
    }

}

