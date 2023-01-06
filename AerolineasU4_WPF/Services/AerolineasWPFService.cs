using AerolineasU4_WPF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AerolineasU4_WPF.Services
{
    internal class AerolineasWPFService
    {
        HttpClient cliente = new HttpClient
        {
            BaseAddress = new Uri("https://AerolineasU4.sistemas19.com/")
        };

        public async Task<List<Vuelum>> Get()
        {
            List<Vuelum>? vuelos = null;
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

        public async Task<List<Vuelum>> GetDespegoCancelado()
        {
            List<Vuelum>? vuelos = null;
            var resp = await cliente.GetAsync("api/Aerolinea/CanceladosyAterrizados");

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

        //Borrar Datos
        public async Task<bool> Delete(Vuelum vuelo)
        {
            var response = await cliente.DeleteAsync("api/Aerolinea/" + vuelo.IdAerolineaU4);


            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errores = await response.Content.ReadAsStringAsync();
                LanzarErrorJson(errores);
                return false;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                LanzarError("Este vuelo ya fue cancelado.");
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
