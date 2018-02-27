using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using APIServer.Models;
using APIServer.Models.AccountViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using APIServer.Services;
using APIServer.Models.ManageViewModels;

namespace APIServer.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountAPI : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;

        public AccountAPI(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }

        // test data action for authorzation 
        [HttpGet]       
        public IEnumerable<string> Users()
        {
            return _userManager.Users.Select(u => u.UserName);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<object> Register([FromBody] RegisterViewModel model)
        {
            // quick hack
            //await CreateInitialRolesAsync();

            if (ModelState.IsValid)
            {
                // check if user exists first
                var check = _userManager.FindByEmailAsync(model.Email);
                if (check.Result != null)
                {
                    return StatusCode(409, new { Error = "Email Already Exists" });
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // default to student role
                    string roleToAdd = String.IsNullOrEmpty(model.Role) ? "STUDENT" : (await _roleManager.RoleExistsAsync(model.Role) ? model.Role : "STUDENT");
                    bool userRoleSucceeded = await AddUserRole(model.Email, roleToAdd);
                    if (!userRoleSucceeded)
                    {
                        return StatusCode(500, new { Message = "Role Assignment Failed." });
                    }

                    // send confirmation email 
                    await SendConfirmationEmail(user);

                    // change this part to not auto login after register
                    await _signInManager.SignInAsync(user, false);
                    //ApplicationUser appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                    ApplicationUser appUser = await _userManager.FindByEmailAsync(model.Email);
                    return await AuthOkWithToken(appUser);
                }
                else
                {
                    // general errors
                    return StatusCode(500, new { result.Errors });
                }    
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<object> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    //ApplicationUser appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                    ApplicationUser appUser = await _userManager.FindByEmailAsync(model.Email);
                    return await AuthOkWithToken(appUser);
                }

                if (result.IsLockedOut)
                {
                    return StatusCode(403, new { Message = "User is locked out due to too many failed attempts." }); ;
                }

                return StatusCode(401, new { Message = "Incorrect username or password." });
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    // return Ok() if don't want to reveal that the user does not exist or is not confirmed
                    return NotFound();
                }
                var result = await _userManager.ConfirmEmailAsync(user, model.Code);
                if (result.Succeeded)
                {
                    return Ok();
                }
                return StatusCode(403, new { result.Errors });
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<object> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(model.Email);
                //if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                if (user == null)
                {
                    // return Ok() if don't want to reveal that the user does not exist or is not confirmed
                    return NotFound();
                }
                string code = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                    $"Please reset your password by using this code: {code}");
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<object> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // return Ok() if don't want to reveal that the user does not exist or is not confirmed
                return NotFound();
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500, new { result.Errors });
            }
            
        }

        [HttpGet]
        public async Task<object> UserInfo()
        {
            string userID = HttpContext.User.Claims.ElementAt(2).Value;
            ApplicationUser user = await _userManager.FindByIdAsync(userID);
            UserInfoViewModel userInfo = await GetUserInfo(user);
            return Ok(userInfo);
        }

        [HttpPost]
        public async Task<object> UpdateProfile([FromBody] UserInfoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string userID = HttpContext.User.Claims.ElementAt(2).Value;
            ApplicationUser user = await _userManager.FindByIdAsync(userID);

            if (model.Email != user.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                if (!setEmailResult.Succeeded)
                {
                    return StatusCode(500, new { setEmailResult.Errors });
                }
                var setUsernameResult = await _userManager.SetUserNameAsync(user, model.Email);
                if (!setUsernameResult.Succeeded)
                {
                    return StatusCode(500, new { setUsernameResult.Errors });
                }
            }
            return Ok();
        }

        #region Helpers
        private async Task<UserInfoViewModel> GetUserInfo(ApplicationUser user)
        {

            string role = await _userManager.IsInRoleAsync(user, "PROFESSOR") ? "PROFESSOR" : "STUDENT";

            UserInfoViewModel model = new UserInfoViewModel
            {
                Username = user.UserName,
                IsEmailConfirmed = user.EmailConfirmed,
                Role = role,
                Email = user.Email
            };
            return model;
        }

        private async Task<bool> SendConfirmationEmail(ApplicationUser user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your account with this code: {code}");
            return true;
        }

        private async Task<object> AuthOkWithToken(ApplicationUser appUser)
        {
            //string role = await _userManager.IsInRoleAsync(appUser, "PROFESSOR") ? "PROFESSOR": "STUDENT";
            UserInfoViewModel userInfo = await GetUserInfo(appUser);
            string token = await GenerateJwtToken(appUser);
            return Ok(new { token, userInfo });
        }

        private async Task<string> GenerateJwtToken(IdentityUser user)
        {
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                            _configuration["TokenInformation:Key"]));
            var expires = DateTime.Now.AddDays(Convert.ToDouble(1));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["TokenInformation:Issuer"],
                audience: _configuration["TokenInformation:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );
            string formattedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return formattedToken;
        }


        public async Task<bool> CreateInitialRolesAsync()
        {
            string[] roleNames = { "Professor", "Student" };
            IdentityResult roleResult;

            foreach(var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!roleResult.Succeeded)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public async Task<bool> AddUserRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);

            IdentityRole applicationRole = await _roleManager.FindByNameAsync(roleName);

            if(applicationRole != null)
            {
                IdentityResult roleResult = await _userManager.AddToRoleAsync(user, roleName);
            }

            return true;
        }

        #endregion
    }
}