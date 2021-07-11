using Microsoft.AspNetCore.Http;
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
    [Route("api/[controller]")]
    public class MorasController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Moras>> GetAll()
        {
            return MorasService.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<Moras> Get(int id)
        {
            var mora = MorasService.Get(id);

            if (mora == null)
                throw new IndexOutOfRangeException("Mora no encontrada");

            return mora;
        }

        [HttpPost]
        public IActionResult Create(Moras mora)
        {
            MorasService.Add(mora);
            return CreatedAtAction(nameof(Create), new { id = mora.MoraID }, mora);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Moras mora)
        {
            if (id != mora.MoraID)
                return BadRequest();

            var existingPizza = MorasService.Get(id);
            if (existingPizza is null)
                return NotFound();

            MorasService.Update(mora);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var mora = MorasService.Get(id);

            if (mora is null)
                return NotFound();

            MorasService.Delete(id);

            return NoContent();
        }

        [HttpPatch]
        public IActionResult Modify(string id, [FromBody] JsonPatchDocument<Moras> patchDoc)
        {
            if (int.TryParse(id, out _))
            {
                var mora = MorasService.Get(int.Parse(id));

                if (mora.MoraID == 0)
                    return NotFound();

                patchDoc.ApplyTo(mora);
                MorasService.Update(mora);

                return NoContent();
            }
            else
                return BadRequest();

        }
    }
}
