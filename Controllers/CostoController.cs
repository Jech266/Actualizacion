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
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrador")]
    public class CostoController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public CostoController(ApplicationDbContext context)
        {
            this.context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<Costo>>> get()
        {
            return await context.Costos.ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult> Post (CostoCreationDTO costoCreationDTO)
        {
            var ExisteCosto = await context.Eventos.AnyAsync(x => x.Id == costoCreationDTO.Id);
            if(ExisteCosto)
            {
                return BadRequest($"El asiento: {costoCreationDTO.Id} ya Existe en el sistema");
            }
            context.Add(costoCreationDTO);
            await context.SaveChangesAsync();
            return Ok();
        } 
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put (Costo costo, int id)
        {
            if (costo.Id != id)
            {
                return BadRequest("El Id no existe en el sistema");
            }

            var existe = await context.Costos.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }
            context.Update(costo);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Costos.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Costo(){Id = id});
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}