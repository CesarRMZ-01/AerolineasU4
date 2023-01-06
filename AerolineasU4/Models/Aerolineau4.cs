using System;
using System.Collections.Generic;

namespace AerolineasU4.Models
{
    public partial class Aerolineau4
    {
        public int IdAerolineaU4 { get; set; }
        public DateTime Hora { get; set; }
        public string Vuelo { get; set; } = null!;
        public string Destino { get; set; } = null!;
        public string Puerta { get; set; } = null!;
        public string Estado { get; set; } = null!;
    }
}
