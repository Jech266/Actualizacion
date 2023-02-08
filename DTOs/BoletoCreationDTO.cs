using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yucom.DTOs
{
    public class BoletoCreationDTO
    {
        public int Id { get; set; }
        public int IdPrecio { get; set; }
        public DateTime Fecha { get; set; }
    }
}