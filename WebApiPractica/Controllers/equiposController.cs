using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using WebApiPractica.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class equiposController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public equiposController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;   
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {//equipos =  tabla
            List<equipos>mi_lista = (from e in _equiposContexto.equipo where e.estado =="A" select e).ToList();
            if (mi_lista.Count == 0 )
            {
                return NotFound();
            }
            else
            {
                return Ok(mi_lista);
            }
        }
        // localhost:4455/api/equipos/getbyid?id=23&nombre=pwa
        [HttpGet]
            [Route("getbyid/{id}")]

            public IActionResult get(int id)
            {
                equipos? unequipo = (from e in _equiposContexto.equipo
                                     where e.id_equipos == id && e.estado =="A"
                                     select e).FirstOrDefault();
                if (unequipo == null)
                    return NotFound();

                return Ok(unequipo);

            }

            [HttpGet]
            [Route("find")]
            public IActionResult buscar(string filtro)
            {
                List<equipos> equiposList = (from e in _equiposContexto.equipo
                                             where e.nombre.Contains(filtro)
                                             || e.descripcion.Contains(filtro)
                                             && e.estado =="A" select e).ToList();

                if (equiposList.Any()) //saber si hay registros  
                {
                    return Ok(equiposList);
                }

                return NotFound();

            }
            [HttpPost]
            [Route("add")]

            public IActionResult Crear([FromBody] equipos equipoNuevo)
            {
                try
                {
                    equipoNuevo.estado = "A";
                    _equiposContexto.equipo.Add(equipoNuevo);
                    _equiposContexto.SaveChanges();

                    return Ok(equipoNuevo);

                }
                catch (Exception ex)
                {

                    return BadRequest(ex.Message);
                }
            }
            [HttpPut]
            [Route("actualizar")]
            public IActionResult actualizarEquipo(int id, [FromBody] equipos equipoModificar)
            {
                equipos? equipoExiste = (from e in _equiposContexto.equipo
                                         where e.id_equipos == id
                                         select e).FirstOrDefault();
                if (equipoExiste == null)
                    return NotFound();

                equipoExiste.nombre = equipoModificar.nombre;
                equipoExiste.descripcion = equipoModificar.descripcion;

                _equiposContexto.Entry(equipoExiste).State = EntityState.Modified;
                _equiposContexto.SaveChanges();

                return Ok(equipoExiste);
            }
            [HttpDelete]
            [Route("delete/{id}")]

            public IActionResult eliminarEquipo(int id)
            {
                equipos? equipoExiste = (from e in _equiposContexto.equipo
                                         where e.id_equipos == id
                                         select e).FirstOrDefault();
                if (equipoExiste == null)
                    return NotFound();
                equipoExiste.estado = "A";

                _equiposContexto.Entry(equipoExiste).State = EntityState.Modified;
                _equiposContexto.SaveChanges();

                return Ok(equipoExiste);
            }
        }

    }

