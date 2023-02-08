using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yucom.DTOs
{
    public class ReservacionCreationDTO
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int IdBoleto { get; set; }
    }
}