using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yucom.DTOs
{
    public class ClienteCreationDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Usuario { get; set; }
        public string Email { get; set;}
        public string Contrasenia { get; set; }
        public string Telefono { get; set; }
    }
}