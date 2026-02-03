using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimelapseAPI.Models;
using TimelapseAPI.Services;

namespace TimelapseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContenidoController : ControllerBase
    {
        private readonly IContenidoService _contenidoService;

        public ContenidoController(IContenidoService contenidoService)
        {
            _contenidoService = contenidoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Contenido>>> GetAll()
        {
            var lista = await _contenidoService.GetAllAsync();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contenido>> GetById(int id)
        {
            var contenido = await _contenidoService.GetByIdAsync(id);
            if (contenido == null) return NotFound();
            return Ok(contenido);
        }

        [HttpPost]
        public async Task<ActionResult<Contenido>> Create([FromBody] Contenido contenido)
        {
            try
            {
                var nuevo = await _contenidoService.CreateAsync(contenido);
                return CreatedAtAction(nameof(GetById), new { id = nuevo.IdContenido }, nuevo);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Contenido>> Update(int id, [FromBody] Contenido contenido)
        {
            if (id != contenido.IdContenido)
                return BadRequest(new { mensaje = "El ID no coincide con el cuerpo de la solicitud." });

            try
            {
                var updated = await _contenidoService.UpdateAsync(contenido);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _contenidoService.DeleteAsync(id);
            if (!result) return NotFound(new { mensaje = "No se encontró el contenido con ese ID." });
            return NoContent();
        }
    }
}