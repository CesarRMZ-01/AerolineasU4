using AerolineasU4_Xamarin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AerolineasU4_Xamarin.Services
{
    public class AerolineasXamarinService
    {
        HttpClient cliente = new HttpClient
        {
            BaseAddress = new Uri("https://AerolineasU4.sistemas19.com/")
        };


        //Recibir Datos
        public async Task<List<Vuelum>> Get()
        {
            List<Vuelum> vuelos = null;
            var resp = await cliente.GetAsync("api/Aerolinea/");

            if (resp.IsSuccessStatusCode)
            {
                var json = await resp.Content.ReadAsStringAsync();
                vuelos = JsonConvert.DeserializeObject<List<Vuelum>>(json);
            }

            if (vuelos == null)
            {
                return new List<Vuelum>();
            }
            else
            {
                return vuelos;
            }
        }

        //Enviar Datos
        public async Task<bool> Insert(Vuelum vuelo)
        {

            var json = JsonConvert.SerializeObject(vuelo);
            var response = await cliente.PostAsync("api/Aerolinea", new StringContent(json, Encoding.UTF8,
                "application/json"));

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errores = await response.Content.ReadAsStringAsync();
                LanzarErrorJson(errores);
                return false;
            }
            return true;
        }


        //Actualizar Datos
        public async Task<bool> Update(Vuelum vuelo)
        {

            var json = JsonConvert.SerializeObject(vuelo);
            var response = await cliente.PutAsync("api/Aerolinea", new StringContent(json, Encoding.UTF8,
                "application/json"));


            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errores = await response.Content.ReadAsStringAsync();
                LanzarErrorJson(errores);
                return false;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                LanzarError("No se encontro el vuelo.");
            }
            return true;
        }

        public event Action<List<string>> Error;

        //Met Errores
        void LanzarError(string mensaje)
        {
            Error?.Invoke(new List<string> { mensaje });
        }

        void LanzarErrorJson(string json)
        {
            List<string> obj = JsonConvert.DeserializeObject<List<string>>(json);
            if (obj != null)
            {
                Error?.Invoke(obj);
            }
        }


    }
}
