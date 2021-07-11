using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PrestamosAPI.Models;
using PrestamosAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrestamosAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrestamosController : Controller
    {
        [HttpGet]
        public ActionResult<List<Prestamos>> GetAll()
        {
            return PrestamosService.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<Prestamos> Get(int id)
        {
            var prestamos = PrestamosService.Get(id);

            if (prestamos == null)
                throw new IndexOutOfRangeException("Prestamo no encontrado");

            return prestamos;
        }

        [HttpPost]
        public IActionResult Create(Prestamos prestamos)
        {
            PrestamosService.Add(prestamos);
            return CreatedAtAction(nameof(Create), new { id = prestamos.PrestamoID }, prestamos);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Prestamos prestamos)
        {
            if (id != prestamos.PrestamoID)
                return BadRequest();

            var existingPizza = PrestamosService.Get(id);
            if (existingPizza is null)
                return NotFound();

            PrestamosService.Update(prestamos);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var prestamos = PrestamosService.Get(id);

            if (prestamos is null)
                return NotFound();

            PrestamosService.Delete(id);

            return NoContent();
        }

        [HttpPatch]
        public IActionResult Modify(string id, [FromBody] JsonPatchDocument<Prestamos> patchDoc)
        {
            if (int.TryParse(id, out _))
            {
                var prestamo = PrestamosService.Get(int.Parse(id));

                if (prestamo.PrestamoID == 0)
                    return NotFound();

                patchDoc.ApplyTo(prestamo);
                PrestamosService.Update(prestamo);

                return NoContent();
            }
            else
                return BadRequest();

        }
    }
}
