using AutoMapper;
using Yucom.DTOs;
using Yucom.Entity;

namespace Yucom.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AsientoCreationDTO, Asiento>();
            CreateMap<BoletoCreationDTO, Boleto>();
            CreateMap<ClienteCreationDTO, Cliente>();
            CreateMap<CostoCreationDTO, Costo>();
            CreateMap<EstablecimientoCreationDTO, Establecimiento>();
            CreateMap<EventosCreationDTO, Evento>();
            CreateMap<PresentadorCreationDTO, Presentador>();
            CreateMap<ReservacionCreationDTO, Reservacion>();

            CreateMap<Asiento, AsientoCreationDTO>();
            CreateMap<Boleto, BoletoCreationDTO>();
            CreateMap<Cliente, ClienteCreationDTO>();
            CreateMap<Costo, CostoCreationDTO>();
            CreateMap<Establecimiento, EstablecimientoCreationDTO>();
            CreateMap<Evento, EventosCreationDTO>();            
            CreateMap<Presentador, PresentadorCreationDTO>();
            CreateMap<Reservacion, ReservacionCreationDTO>();
        }
    }
}