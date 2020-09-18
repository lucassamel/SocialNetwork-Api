using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkBLL.Identity;
using SocialNetworkBLL.Models;
using SocialNetworkBLL.Requests;

namespace SocialNetworkAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            // Copia os dados do RegisterViewModel para o IdentityUser
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };
            // Armazena os dados do usuário na tabela AspNetUsers
            var result = await userManager.CreateAsync(user, model.Password);
            // Se o usuário foi criado com sucesso, faz o login do usuário
            // usando o serviço SignInManager e redireciona para o Método Action Index
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                return Ok();
            }
            // Se houver erros então inclui no ModelState
            // que será exibido pela tag helper summary na validação
            return StatusCode(StatusCodes.Status400BadRequest, result.Errors);  
        }

        //Método que faz o Logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var result = await signInManager.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe, false);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Login Inválido");
            }
            
            return Ok();
        }

    }
}
