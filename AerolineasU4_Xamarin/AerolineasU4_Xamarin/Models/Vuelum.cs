using System;
using System.Collections.Generic;
using System.Text;

namespace AerolineasU4_Xamarin.Models
{
    public class Vuelum
    {
        public int IdAerolineaU4 { get; set; }
        public DateTime Hora { get; set; }
        public string Vuelo { get; set; }
        public string Destino { get; set; }
        public string Puerta { get; set; }
        public string Estado { get; set; } 
    }
}
