using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using netCoreMvcTest.Ioc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Collections.Generic;
using netCoreMvcTest.models;
using System.Threading.Tasks;
using netCoreMvcTest.Data;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using netCoreMvcTest.Email;
using System.Web;

namespace netCoreMvcTest.Controllers
{

    public class AuthorizeToken : AuthorizeAttribute
    {
        public AuthorizeToken()
        {
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }
    }

    /// <summary>
    /// Manage web api requests
    /// </summary>
    public class ApiController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ApiController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        //list of stored tokens to invalidate when logout 
        public static List<(string id, string type)> storedTokens = new List<(string id, string type)>();

        [HttpPost]
        [Route("/api/register")]
        public async Task<ApiResponse<RegisterResultApiModel>> RegisterAsync([FromBody]RegisterCredentialsApiModel model)
        {
            var invalidUserNameOrPassword = "Please provide all required details for register";

            var errorResponse = new ApiResponse<RegisterResultApiModel>
            {
                ErrorMessage = invalidUserNameOrPassword
            };

            if (model == null)
            {
                return errorResponse;
            }

            if (string.IsNullOrWhiteSpace(model.UserName))
            {
                return errorResponse;
            }


            //create user from model
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
            };

            //try register

            var result = await _userManager.CreateAsync(user, model.Password);

            //if registration is successfull
            if (result.Succeeded)
            {
                //get detail user details

                var userAccount = await _userManager.FindByNameAsync(model.UserName);

                var emailVerificationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var confirmationUrl = $"http://{Request.Host.Value}/api/verify/email/{HttpUtility.UrlEncode(userAccount.Id)}/{HttpUtility.UrlEncode(emailVerificationCode)}";

                //send user email verification
               await Task.Run(async () =>
                {
                   await AppEmailSender.SendUserVerificationEmailAsync(null, userAccount.Email, confirmationUrl);
                });

                return new ApiResponse<RegisterResultApiModel>
                {
                    Response = new RegisterResultApiModel
                    {
                        FirstName = userAccount.FirstName,
                        LastName = userAccount.LastName,
                        Email = userAccount.Email,
                        Token = userAccount.GenerateJwtToken()
                    }
                };
            }

            //if we failed 
            return new ApiResponse<RegisterResultApiModel>
            {
                //list of all errors
                ErrorMessage = result.Errors?.ToList()
                .Select(eror => eror.Description)
                .Aggregate((a, b) => $"{a}{Environment.NewLine}{b}")
            };
        }

        [Route("/api/verify/email/{userid}/{code}")]
        public async Task<IActionResult> VerifyEmail(string userid, string code)
        {
            //verify if id matches
            var user = await _userManager.FindByIdAsync(userid);

            code = code.Replace("%2f", "/").Replace("%2F", "/");

            if (user == null || code == null)
            {
                return Content("User not found");
            }
            //verify code
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return Content("Email verified");
            }

            return Content("Invalid token");
        }

        [Route("api/login")]
        public async Task<ApiResponse<LoginResultApiModel>> LogInAsync([FromBody]LoginCredentialsApiModel model)
        {
            var invalidUserNameOrPassword = "Invalid password or username";

            var errorResponse = new ApiResponse<LoginResultApiModel>
            {
                ErrorMessage = invalidUserNameOrPassword
            };

            //make sure if we have userName
            if (model.UserNameOrEmail == null || string.IsNullOrWhiteSpace(model.Password))
            {
                //Error Message
                return errorResponse;
            }

            // validate user
            var isEmail = model.UserNameOrEmail.Contains("@");

            var user = isEmail ?
               //find by email
               await _userManager.FindByEmailAsync(model.UserNameOrEmail) :
               //find my username
               await _userManager.FindByNameAsync(model.UserNameOrEmail);


            if (user == null)
            {
                return errorResponse;
            }

            var isValidPassword = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!isValidPassword)
            {
                return errorResponse;
            }

            //if we got here we passed corret login and password

            var userName = user.UserName;

            //return token to user
            return new ApiResponse<LoginResultApiModel>
            {
                //pass back user details and a token
                Response = new LoginResultApiModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Token = user.GenerateJwtToken()
                }
            };
        }
        //After returning token to client then in each request u write headers with "Authorization: Bearer {generatedToken}"
        //to get role based authorization like [Authorize(Roles ="admin")] you have to write claims with roles in token
        //see https://stackoverflow.com/questions/42036810/asp-net-core-jwt-mapping-role-claims-to-claimsidentity
        //for more infomation


        [AuthorizeToken]
        public IActionResult Private()
        {
            return Content($"private! hello ");
        }
    }
}
