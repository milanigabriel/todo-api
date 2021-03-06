﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using todo_api.Data;
using todo_api.Models;

namespace todo_api.Controllers
{
    [Route("api")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        
        public UsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if(result.Succeeded)
            {
                return BuildToken(model);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserToken>> LoginUser([FromBody] UserInfo model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
            if(result.Succeeded)
            {
                return BuildToken(model);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login inválido");
                return BadRequest(ModelState); 
            }
        }
        private UserToken BuildToken(UserInfo userInfo)
        {
            var claims = new[]{
            
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken
                (
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials : creds
                );
            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
        [HttpPut("update")]
        public async Task<ActionResult<ApplicationUser>> UpdateUser(UserInfoDto model)
        {
            var user = await _userManager.GetUserAsync(User);

            user.Email = model.Email;
            user.UserName = model.Name;
            user.NormalizedEmail = model.Email;
            user.NormalizedUserName = model.Name;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { user.UserName, user.Email });
        }
        [HttpPut("logout")]
        public async Task<ActionResult<ApplicationUser>> LogoutUser()
        {
            await _signInManager.SignOutAsync();

            return Ok("Usuário Desconectado com sucesso.");
        }
    }
}
