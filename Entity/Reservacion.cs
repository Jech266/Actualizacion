using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yucom.Entity
{
    public class Reservacion
    {
        public int Id {get; set; }
        public int IdCliente {get; set; }
        public int IdBoleto {get; set; }
        public Cliente cliente {get; set; }
        public Boleto boleto {get; set; }
    }
}