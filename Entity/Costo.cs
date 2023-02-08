using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yucom.Entity
{
    public class Costo
    {
        public int Id { get; set; }
        public int IdEvento { get; set; }
        public double Precio {get; set; }
        public Evento evento {get; set; }
    }
}