using Clinica.Entidades.Dtos;
using Clinica.Entidades.Excepciones;
using Clinica.Negocio.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Clinica.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicosController : ControllerBase
{
    private readonly IMedicoServicio _medicoServicio;
    private readonly ICitaServicio _citaServicio;

    public MedicosController(IMedicoServicio medicoServicio, ICitaServicio citaServicio)
    {
        _medicoServicio = medicoServicio;
        _citaServicio = citaServicio;
    }

    // GET api/medicos?especialidad=Cardiología&activo=true
    [HttpGet]
    [ProducesResponseType(typeof(List<MedicoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<MedicoDto>>> ObtenerTodos(
        [FromQuery] string? especialidad, [FromQuery] bool? activo)
    {
        var medicos = await _medicoServicio.ObtenerTodosAsync(especialidad, activo);
        return Ok(medicos);
    }

    // GET api/medicos/1
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MedicoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MedicoDto>> ObtenerPorId(int id)
    {
        var medico = await _medicoServicio.ObtenerPorIdAsync(id);
        if (medico is null) return NotFound();
        return Ok(medico);
    }

    // GET api/medicos/1/citas  (ruta anidada)
    [HttpGet("{medicoId:int}/citas")]
    [ProducesResponseType(typeof(List<CitaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<CitaDto>>> ObtenerCitasDelMedico(int medicoId)
    {
        var citas = await _citaServicio.ObtenerPorMedicoAsync(medicoId);
        if (citas is null) return NotFound();
        return Ok(citas);
    }

    // POST api/medicos
    [HttpPost]
    [ProducesResponseType(typeof(MedicoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<MedicoDto>> Crear(MedicoCrearDto dto)
    {
        try
        {
            var creado = await _medicoServicio.CrearAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // PUT api/medicos/1
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Actualizar(int id, MedicoActualizarDto dto)
    {
        try
        {
            var existe = await _medicoServicio.ActualizarAsync(id, dto);
            if (!existe) return NotFound();
            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // DELETE api/medicos/1
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            var existe = await _medicoServicio.EliminarAsync(id);
            if (!existe) return NotFound();
            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }
}