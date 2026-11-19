using Clinica.Entidades.Dtos;
using Clinica.Entidades.Excepciones;
using Clinica.Negocio.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Clinica.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitasController : ControllerBase
{
    private readonly ICitaServicio _servicio;

    public CitasController(ICitaServicio servicio)
    {
        _servicio = servicio;
    }

    // GET api/citas?fecha=2026-11-20&estado=Programada&medicoId=1
    [HttpGet]
    [ProducesResponseType(typeof(List<CitaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CitaDto>>> ObtenerTodas(
        [FromQuery] DateTime? fecha, [FromQuery] string? estado, [FromQuery] int? medicoId)
    {
        var citas = await _servicio.ObtenerTodasAsync(fecha, estado, medicoId);
        return Ok(citas);
    }

    // GET api/citas/1
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CitaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CitaDto>> ObtenerPorId(int id)
    {
        var cita = await _servicio.ObtenerPorIdAsync(id);
        if (cita is null) return NotFound();
        return Ok(cita);
    }

    // POST api/citas
    [HttpPost]
    [ProducesResponseType(typeof(CitaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CitaDto>> Crear(CitaCrearDto dto)
    {
        try
        {
            var creada = await _servicio.CrearAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.Id }, creada);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // PUT api/citas/1
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Actualizar(int id, CitaActualizarDto dto)
    {
        try
        {
            var existe = await _servicio.ActualizarAsync(id, dto);
            if (!existe) return NotFound();
            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // DELETE api/citas/1
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id)
    {
        var existe = await _servicio.EliminarAsync(id);
        if (!existe) return NotFound();
        return NoContent();
    }
}