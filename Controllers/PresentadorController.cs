using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yucom.DTOs;
using Yucom.Entity;
using Yucom.Utilidades;

namespace Yucom.Controllers
{
    [ApiController]
    [Route("api/presentador")]
    public class PresentadorController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public readonly IMapper mapper;

        public PresentadorController(ApplicationDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }
        

        [HttpGet("Paginación")]
        [AllowAnonymous]
        public async Task<ActionResult<List<PresentadorCreationDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Presentadors.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var presentador = await queryable.OrderBy(presentador => presentador.Nombre).Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<PresentadorCreationDTO>>(presentador);
        }



        [HttpGet("{Nombre}")]
        [AllowAnonymous]
        public async Task<ActionResult<Presentador>> Get (string Nombre)
        {
            var nombre = await context.Presentadors.FirstOrDefaultAsync(x => x.Nombre.Contains(Nombre));
            if (nombre == null)
            {
                return BadRequest($"El comediante de nombre {Nombre} no se encuentra en los datos");
            }
            return nombre;
        }



        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrador")]
        public async Task<ActionResult> Post([FromBody] PresentadorCreationDTO presentadorCreationDTO)
        {
            var existePresentador = await context.Presentadors.AnyAsync(x => x.Nombre == presentadorCreationDTO.Nombre);

            if (existePresentador)
            {
                return BadRequest($"El presentador {presentadorCreationDTO.Nombre} ya EXISTE en el sistema");
            }
            var presentador = mapper.Map<Presentador>(presentadorCreationDTO);
            
            context.Add(presentador);
            await context.SaveChangesAsync();
            return Ok();
        }



        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrador")] 
        public async Task<ActionResult> Put(PresentadorCreationDTO presentadorCreationDTO, int id)
        {
            if (presentadorCreationDTO.Id != id)
            {
                return BadRequest("El Id no existe en el sistema");
            }

            var existe = await context.Presentadors.AnyAsync(x => x.Id == presentadorCreationDTO.Id);

            if (!existe)
            {
                return NotFound();
            }
            var presentador = mapper.Map<Presentador>(presentadorCreationDTO);
            context.Update(presentador);
            await context.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrador")] 
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Presentadors.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Presentador(){Id = id});
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}