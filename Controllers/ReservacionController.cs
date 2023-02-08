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
    public class ReservacionController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ReservacionController(ApplicationDbContext context)
        {
            this.context = context;
        }
        
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrador")]
        public async Task<ActionResult<List<Reservacion>>> get()
        {
            return await context.Reservaciones.ToListAsync();
        }



        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
        public async Task<ActionResult> Post (ReservacionCreationDTO reservacionCreationDTO)
        {
            var ExisteBoleto = await context.Boletos.AnyAsync(x => x.Id == reservacionCreationDTO.IdBoleto);
            var ExisteCliente = await context.Clientes.AnyAsync(x => x.Id == reservacionCreationDTO.IdCliente);
            var ExisteReservacion = await context.Reservaciones.AnyAsync(X => X.Id == reservacionCreationDTO.Id);
            if(ExisteBoleto == null)
            {
                return BadRequest($"El Boleto: {reservacionCreationDTO.IdBoleto} ya esta reservado");
            }
            if(ExisteCliente == null)
            {
                return BadRequest($"El cliente: {reservacionCreationDTO.IdCliente} no se ha registrado");
            }
            if(ExisteReservacion)
            {
                return BadRequest($"La reservacion: {reservacionCreationDTO.IdCliente} ya existe.");
            }
            context.Add(reservacionCreationDTO);
            await context.SaveChangesAsync();
            return Ok();
        } 



        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
        public async Task<ActionResult> Put (Reservacion reservacion, int id)
        {
            if (reservacion.Id != id)
            {
                return BadRequest("El Id no existe en el sistema");
            }

            var existe = await context.Reservaciones.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }
            context.Update(reservacion);
            await context.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Reservaciones.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Reservacion(){Id = id});
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}