using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Yucom.DTOs;

namespace Yucom.Controllers
{
    [ApiController]
    [Route("api/Cuentas")]
    public class CuentasController : ControllerBase
    {
        public readonly UserManager<IdentityUser> UserManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;
        public CuentasController(UserManager<IdentityUser> UserManager, 
            IConfiguration configuration,
            SignInManager<IdentityUser> signInManager)
        {
            this.UserManager = UserManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            
        }

        [HttpPost("Registrar")]
        public async Task<ActionResult<RespuestaAutenticacion>> Registrar(CredencialesUsuario credencialesUsuario)
        {
            var usuario = new IdentityUser {UserName = credencialesUsuario.Email, 
                Email = credencialesUsuario.Email};
            var resultado = await UserManager.CreateAsync(usuario, credencialesUsuario.Password);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(credencialesUsuario);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<RespuestaAutenticacion>> Login(CredencialesUsuario credencialesUsuario)
        {
            var resultado = await signInManager.PasswordSignInAsync(credencialesUsuario.Email,
                credencialesUsuario.Password, isPersistent: false, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(credencialesUsuario);
            }
            else
            {
                return BadRequest("Login incorrecto");
            }
        }
        private async Task<RespuestaAutenticacion> ConstruirToken(CredencialesUsuario credencialesUsuario)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", credencialesUsuario.Email)
            };
            var usuario = await UserManager.FindByEmailAsync(credencialesUsuario.Email);
            var claimDB = await UserManager.GetClaimsAsync(usuario);
            claims.AddRange(claimDB);
            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddDays(1);

            var SecurityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiracion, signingCredentials: creds);

            return new RespuestaAutenticacion
            {
                Token = new JwtSecurityTokenHandler().WriteToken(SecurityToken),
                Expiracion = expiracion
            };
        }


        [HttpPost("Administrador")]
        public async Task<ActionResult> Administrador(AdministradorDTO administradorDTO)
        {
            var usuario = await UserManager.FindByEmailAsync(administradorDTO.Email);
            await UserManager.AddClaimAsync(usuario, new Claim("Administrador", "1"));
            return NoContent();
        }


        [HttpPost("RemoverAdministrador")]
        public async Task<ActionResult> RemoverAdministrador(AdministradorDTO administradorDTO)
        {
            var usuario = await UserManager.FindByEmailAsync(administradorDTO.Email);
            await UserManager.RemoveClaimAsync(usuario, new Claim("Administrador", "1"));
            return NoContent();
        }
    }
}