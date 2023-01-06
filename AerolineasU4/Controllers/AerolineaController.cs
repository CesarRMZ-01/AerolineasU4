using AerolineasU4.DTOs;
using AerolineasU4.Models;
using AerolineasU4.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AerolineasU4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AerolineaController : ControllerBase
    {
        private sistem21_aerolineau4Context context;
        Repository<Aerolineau4> repository;

        public AerolineaController(sistem21_aerolineau4Context sistem21_aerolineau4Context)
        {
            this.context = sistem21_aerolineau4Context;
            repository = new Repository<Aerolineau4>(context);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var vuelos = repository.Get().OrderBy(x => x.Hora);

            return Ok(vuelos.Select(x => new AerolineaDTO()
            {
                IdAerolineaU4 = x.IdAerolineaU4,
                Hora = x.Hora,
                Vuelo = x.Vuelo,
                Destino = x.Destino,
                Puerta = x.Puerta,
                Estado = x.Estado
            }));
        }

        [HttpGet("CanceladosyAterrizados")]
        public IActionResult GetAterrCancel()
        {
            var vuelos = repository.Get().Where(x=> x.Estado == "CANCELADO" || x.Estado == "DESPEGO");

            return Ok(vuelos.Select(x => new AerolineaDTO()
            {
                IdAerolineaU4 = x.IdAerolineaU4,
                Hora = x.Hora,
                Vuelo = x.Vuelo,
                Destino = x.Destino,
                Puerta = x.Puerta,
                Estado = x.Estado
            }));
        }

        [HttpPost]
        public IActionResult Post(AerolineaDTO vuelo)
        {
            if (vuelo == null)
            {
                return BadRequest("Llene los datos del vuelo, porfavor.");
            }
            if (repository.Get().Any(x => x.Vuelo == vuelo.Vuelo))
            {
                return BadRequest("Este vuelo ya fue registrado.");
            }


            //buscar excepcion para DATETIME
            //-------------

            if (string.IsNullOrWhiteSpace(vuelo.Vuelo))
            {
                return BadRequest("Especifique el codigo de vuelo.");
            }
            if (vuelo.Vuelo.Length > 45)
            {
                return BadRequest("El codigo de vuelo debe constar de entre 1 y 45 caracteres.");
            }
            if (string.IsNullOrWhiteSpace(vuelo.Destino))
            {
                return BadRequest("Especifique el destino de vuelo.");
            }
            if (vuelo.Destino.Length > 45)
            {
                return BadRequest("El destino del vuelo debe constar de entre 1 y 45 caracteres.");
            }
            if (string.IsNullOrWhiteSpace(vuelo.Puerta))
            {
                return BadRequest("Especifique la puerta por la cual subir al vuelo.");
            }
            if (vuelo.Puerta.Length > 3)
            {
                return BadRequest("La puerta a utilizar debe constar de entre 1 y 3 caracteres.");
            }


            Aerolineau4 NvoVuelo = new()
            {
                Hora = vuelo.Hora,
                Vuelo = vuelo.Vuelo,
                Destino = vuelo.Destino,
                Puerta = vuelo.Puerta,
                Estado = vuelo.Estado
            };

            repository.Insert(NvoVuelo);

            return Ok();
        }

        [HttpPut]
        public IActionResult Put(AerolineaDTO vuelo)
        {
            //escepciones
            if (vuelo == null)
            {
                return BadRequest("Llene los datos del vuelo, porfavor.");
            }

            //buscar excepcion para DATETIME
            //-------------

            if (string.IsNullOrWhiteSpace(vuelo.Vuelo))
            {
                return BadRequest("Especifique el codigo de vuelo.");
            }
            if (vuelo.Vuelo.Length > 45)
            {
                return BadRequest("El codigo de vuelo debe constar de entre 1 y 45 caracteres.");
            }
            if (string.IsNullOrWhiteSpace(vuelo.Destino))
            {
                return BadRequest("Especifique el destino de vuelo.");
            }
            if (vuelo.Destino.Length > 45)
            {
                return BadRequest("El destino del vuelo debe constar de entre 1 y 45 caracteres.");
            }
            if (string.IsNullOrWhiteSpace(vuelo.Puerta))
            {
                return BadRequest("Especifique la puerta por la cual subir al vuelo.");
            }
            if (vuelo.Puerta.Length > 3)
            {
                return BadRequest("La puerta a utilizar debe constar de entre 1 y 3 caracteres.");
            }

            var entidad = repository.Get(vuelo.IdAerolineaU4);

            if (entidad == null)
            {
                NotFound("No existe");
            }

            if (entidad != null)
            {
                entidad.Hora = vuelo.Hora;
                entidad.Vuelo = vuelo.Vuelo;
                entidad.Destino = vuelo.Destino;
                entidad.Puerta = vuelo.Puerta;
                entidad.Estado = vuelo.Estado;


                repository.Update(entidad);
            }
            return Ok();


        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            var entidad = repository.Get(id);

            if (entidad == null)
            {
                return NotFound();
            }

            repository.Delete(entidad);
            return Ok();
        }

    }
}
