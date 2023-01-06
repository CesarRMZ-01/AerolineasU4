using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerolineasU4_WPF.Models
{
    public class Vuelum
    {
        public int IdAerolineaU4 { get; set; }
        public DateTime Hora { get; set; }
        public string Vuelo { get; set; } = null!;
        public string Destino { get; set; } = null!;
        public string Puerta { get; set; } = null!;
        public string Estado { get; set; } = null!;
    }
}
