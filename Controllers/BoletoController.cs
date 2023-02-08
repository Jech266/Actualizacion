using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yucom.DTOs;
using Yucom.Entity;

namespace Yucom.Controllers
{
    [ApiController]
    [Route("api/Boleto")]
    public class BoletoController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public BoletoController(ApplicationDbContext context)
        {
            this.context = context;
        }
        
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrador")]
        public async Task<ActionResult<List<Boleto>>> get()
        {
            return await context.Boletos.ToListAsync();
        }



        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post (BoletoCreationDTO boletoCreationDTO)
        {
            var ExisteBoleto = await context.Boletos.AnyAsync(x => x.Id == boletoCreationDTO.Id);
            if(ExisteBoleto)
            {
                return BadRequest($"El boleto: {boletoCreationDTO.Id} ya Existe");
            }
            context.Add(boletoCreationDTO);
            await context.SaveChangesAsync();
            return Ok();
        } 



        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put (Boleto boleto, int id)
        {
            if (boleto.Id != id)
            {
                return BadRequest("El Id no existe en el sistema");
            }

            var existe = await context.Boletos.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }
            context.Update(boleto);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id:int}")]
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrador")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Boletos.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Boleto(){Id = id});
            await context.SaveChangesAsync();
            return Ok();
        }        
    }
}