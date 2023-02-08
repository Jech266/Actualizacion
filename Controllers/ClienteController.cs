using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yucom.Entity;

namespace Yucom.Controllers
{
    [ApiController]
    [Route("api/Cliente")]
    public class ClienteController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public ClienteController(ApplicationDbContext context)
        {
            this.context = context;
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrador")]
        public async Task<ActionResult<Cliente>> Get(string Usuario)
        {
            var ExisteUsuario = await context.Clientes.FirstOrDefaultAsync(x => x.Usuario.Contains(Usuario));
            if(ExisteUsuario == null)
            {
                BadRequest("No existe el usuario");
            }
            return ExisteUsuario;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Post(Cliente cliente)
        {
            var ExisteUsuario = await context.Clientes.AnyAsync(x => x.Usuario == cliente.Usuario);

            if (ExisteUsuario)
            {
                return BadRequest($"El usuario: {cliente.Usuario} ya EXISTE en el sistema");
            }
            context.Add(cliente);
            await context.SaveChangesAsync();
            return Ok();
        }


        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(Cliente cliente, int id)
        {
            if (cliente.Id != id)
            {
                return BadRequest("El Id no existe en el sistema");
            }

            var existe = await context.Clientes.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }
            context.Update(cliente);
            await context.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Clientes.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Cliente(){Id = id});
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}