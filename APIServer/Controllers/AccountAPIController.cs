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

namespace APIServer.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class AccountAPI : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountAPI(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration,
        RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
                //var check = _userManager.FindByEmailAsync(model.Email);
                //if (check != null)
                //{
                //    return StatusCode(409, new { Error = "Email Already Exists" });
                //}
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
                    // change this to not auto login after register
                    await _signInManager.SignInAsync(user, false);
                    ApplicationUser appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
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
                    ApplicationUser appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                    // the following line works strangely, why?
                    // ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
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

        private async Task<object> AuthOkWithToken(ApplicationUser appUser)
        {
            string role = await _userManager.IsInRoleAsync(appUser, "PROFESSOR") ? "PROFESSOR": "STUDENT";
            string token = await GenerateJwtToken(appUser);
            return Ok(new { token, role });
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
    }
}