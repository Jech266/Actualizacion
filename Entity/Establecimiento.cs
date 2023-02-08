using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yucom.Validaciones;

namespace Yucom.Entity
{
    public class Establecimiento
    {
        public int Id {get; set;}
        [PrimeraLetra]
        public string? Nombre { get; set; }
        public string? Imagenes { get; set; }
        public string Descripcion { get; set; }
        public int IdAsiento { get; set; }
        public Asiento asiento {get; set; }
        public List<Evento>? Eventos {get; set; }
    }
}