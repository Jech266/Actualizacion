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
    [Route("api/[controller]")]
    public class EstablecimientoController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public readonly IMapper Mapper;

        public EstablecimientoController(ApplicationDbContext context, IMapper mapper)
        {
            this.Mapper = mapper;
            this.context = context;
        }

        [HttpGet("Paginación")]
        [AllowAnonymous]
        public async Task<ActionResult<List<EstablecimientoCreationDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Establecimientos.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var Establecimiento = await queryable.OrderBy(Establecimiento => Establecimiento.Nombre).Paginar(paginacionDTO).ToListAsync();
            return Mapper.Map<List<EstablecimientoCreationDTO>>(Establecimiento);
        }

        [HttpGet("{Nombre}")]
        [AllowAnonymous]
        public async Task<ActionResult<Establecimiento>> Get (string Nombre)
        {
            var NombreEstablecimiento = await context.Establecimientos.FirstOrDefaultAsync(x => x.Nombre.Contains(Nombre));
            if (NombreEstablecimiento == null)
            {
                return BadRequest($"El establecimento de nombre {Nombre} no se encuentra en los datos");
            }
            return NombreEstablecimiento;
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrador")]
        public async Task<ActionResult> Post([FromBody]EstablecimientoCreationDTO establecimientoCreationDTO)
        {
            var existeEstablecimiento = await context.Establecimientos.AnyAsync(x => x.Nombre == establecimientoCreationDTO.Nombre);

            if (existeEstablecimiento)
            {
                return BadRequest($"El establecimiento: {establecimientoCreationDTO.Nombre} ya EXISTE en el sistema");
            }
            var establecimiento = Mapper.Map<Establecimiento>(establecimientoCreationDTO);

            context.Add(establecimiento);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Adminitrador")]
        public async Task<ActionResult> Put(EstablecimientoCreationDTO establecimientoCreationDTO, int id)
        {
            if (establecimientoCreationDTO.Id != id)
            {
                return BadRequest("El Id no existe en el sistema");
            }

            var existe = await context.Establecimientos.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            var establecimiento =Mapper.Map<Establecimiento>(establecimientoCreationDTO);

            context.Update(establecimiento);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrador")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Establecimientos.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Establecimiento(){Id = id});
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}