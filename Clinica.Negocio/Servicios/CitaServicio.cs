using AutoMapper;
using Clinica.Datos.Repositorios;
using Clinica.Entidades.Dtos;
using Clinica.Entidades.Excepciones;
using Clinica.Entidades.Models;

namespace Clinica.Negocio.Servicios;

public class CitaServicio : ICitaServicio
{
    private readonly ICitaRepositorio _citaRepositorio;
    private readonly IMedicoRepositorio _medicoRepositorio;
    private readonly IMapper _mapper;

    public CitaServicio(
        ICitaRepositorio citaRepositorio,
        IMedicoRepositorio medicoRepositorio,
        IMapper mapper)
    {
        _citaRepositorio = citaRepositorio;
        _medicoRepositorio = medicoRepositorio;
        _mapper = mapper;
    }

    public async Task<List<CitaDto>> ObtenerTodasAsync(DateTime? fecha, string? estado, int? medicoId)
    {
        var citas = await _citaRepositorio.ObtenerTodasAsync(fecha, estado, medicoId);
        return _mapper.Map<List<CitaDto>>(citas);
    }

    public async Task<List<CitaDto>?> ObtenerPorMedicoAsync(int medicoId)
    {
        var medico = await _medicoRepositorio.ObtenerPorIdAsync(medicoId);
        if (medico is null) return null;

        var citas = await _citaRepositorio.ObtenerPorMedicoAsync(medicoId);
        return _mapper.Map<List<CitaDto>>(citas);
    }

    public async Task<CitaDto?> ObtenerPorIdAsync(int id)
    {
        var cita = await _citaRepositorio.ObtenerPorIdAsync(id);
        return cita is null ? null : _mapper.Map<CitaDto>(cita);
    }

    public async Task<CitaDto> CrearAsync(CitaCrearDto dto)
    {
        var medico = await _medicoRepositorio.ObtenerPorIdAsync(dto.MedicoId)
            ?? throw new ReglaNegocioException($"El médico con id {dto.MedicoId} no existe.");

        // Regla 3: médico inactivo
        if (!medico.Activo)
            throw new ReglaNegocioException("No se puede agendar una cita con un médico inactivo.");

        // Regla 2: fecha pasada
        if (dto.FechaHora <= DateTime.Now)
            throw new ReglaNegocioException("No se puede agendar una cita en una fecha pasada.");

        // Regla 1: cruce de horario
        if (await _citaRepositorio.ExisteCitaEnHorarioAsync(dto.MedicoId, dto.FechaHora))
            throw new ReglaNegocioException("El médico ya tiene una cita en ese horario.");

        var cita = _mapper.Map<Cita>(dto);
        cita.Estado = "Programada";

        await _citaRepositorio.AgregarAsync(cita);

        // Recargar con el médico incluido para armar el DTO
        var creada = await _citaRepositorio.ObtenerPorIdAsync(cita.Id);
        return _mapper.Map<CitaDto>(creada);
    }

    public async Task<bool> ActualizarAsync(int id, CitaActualizarDto dto)
    {
        var cita = await _citaRepositorio.ObtenerPorIdAsync(id);
        if (cita is null) return false;

        var medico = await _medicoRepositorio.ObtenerPorIdAsync(dto.MedicoId)
            ?? throw new ReglaNegocioException($"El médico con id {dto.MedicoId} no existe.");

        if (!medico.Activo)
            throw new ReglaNegocioException("No se puede asignar una cita a un médico inactivo.");

        // Solo validamos fecha pasada si cambió la fecha
        if (dto.FechaHora != cita.FechaHora && dto.FechaHora <= DateTime.Now)
            throw new ReglaNegocioException("No se puede reprogramar una cita a una fecha pasada.");

        if (dto.Estado != "Cancelada" &&
            await _citaRepositorio.ExisteCitaEnHorarioAsync(dto.MedicoId, dto.FechaHora, id))
            throw new ReglaNegocioException("El médico ya tiene una cita en ese horario.");

        _mapper.Map(dto, cita);
        await _citaRepositorio.ActualizarAsync(cita);
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var cita = await _citaRepositorio.ObtenerPorIdAsync(id);
        if (cita is null) return false;

        await _citaRepositorio.EliminarAsync(cita);
        return true;
    }
}