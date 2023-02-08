using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yucom.Validaciones;

namespace Yucom.Entity
{
    public class Cliente
    {
        public int Id {get; set;}
        [PrimeraLetra]
        public string Nombre { get; set; }
        [PrimeraLetra]
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno {get; set;}
        public string Usuario { get; set; }
        public string Email { get; set; }
        public string Contrasenia { get; set; }  
        public string Telefono { get; set; }
    }
}