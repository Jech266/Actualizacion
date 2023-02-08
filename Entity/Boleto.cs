using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yucom.Entity
{
    public class Boleto
    {
        public int Id {get; set; }
        public int IdPrecio { get; set; }
        public DateTime Fecha { get; set; }
        public Costo costo {get; set; }
    }
}