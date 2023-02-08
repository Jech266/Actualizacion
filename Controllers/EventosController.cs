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
    [Route("api/Eventos")]
    public class EventosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public EventosController(ApplicationDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }
        [HttpGet("Paginación")]
        [AllowAnonymous]
        public async Task<ActionResult<List<EventosCreationDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Eventos.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var Evento = await queryable.OrderBy(Evento => Evento.Nombre).Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<EventosCreationDTO>>(Evento);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Evento>> Get(int id)
        {
            return await context.Eventos.Include(x => x.presentador).Include(c => c.establecimiento).FirstOrDefaultAsync(a => a.Id == id);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrador")]
        public async Task<ActionResult> Post (EventosCreationDTO eventosCreationDTO)
        {
            var existeComediante = await context.Presentadors.AnyAsync(x => x.Id == eventosCreationDTO.IdPresentador);
            var existeEstablecimiento = await context.Establecimientos.AnyAsync(x => x.Id == eventosCreationDTO.IdEstablecimiento);
            var existeEvento = await context.Eventos.AnyAsync(x => x.Nombre == eventosCreationDTO.Nombre);
            if (existeComediante == null)
            {
                return BadRequest($"No existe el presentador {eventosCreationDTO.IdPresentador} asignado al evento");
            }
            if (existeEstablecimiento == null)
            {
                return BadRequest($"No existe el establecimiento {eventosCreationDTO.IdEstablecimiento} asignado al evento");
            }
            if(existeEvento)
            {
                return BadRequest("Ya existe el evento");
            }
            var eventos = mapper.Map<Evento>(eventosCreationDTO);

            context.Add(eventos);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}