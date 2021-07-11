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
    public class PersonasController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Personas>> GetAll()
        {
            return PersonasService.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<Personas> Get(int id)
        {
            var persona = PersonasService.Get(id);

            if (persona == null)
                throw new IndexOutOfRangeException("Persona no encontrada");

            return persona;
        }

        [HttpPost]
        public IActionResult Create(Personas persona)
        {
            PersonasService.Add(persona);
            return CreatedAtAction(nameof(Create), new { id = persona.PersonaID }, persona);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Personas persona)
        {
            if (id != persona.PersonaID)
                return BadRequest();

            var existingPizza = PersonasService.Get(id);
            if (existingPizza is null)
                return NotFound();

            PersonasService.Update(persona);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var persona = PersonasService.Get(id);

            if (persona is null)
                return NotFound();

            PersonasService.Delete(id);

            return NoContent();
        }

        [HttpPatch]
        public IActionResult Modify(string id, [FromBody] JsonPatchDocument<Personas> patchDoc)
        {
            if (int.TryParse(id, out _))
            {
                var persona = PersonasService.Get(int.Parse(id));

                if (persona.PersonaID == 0)
                    return NotFound();

                patchDoc.ApplyTo(persona);
                PersonasService.Update(persona);

                return NoContent();
            }
            else
                return BadRequest();

        }
    }
}
