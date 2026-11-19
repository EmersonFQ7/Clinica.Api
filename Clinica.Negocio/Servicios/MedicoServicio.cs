using AutoMapper;
using Clinica.Datos.Repositorios;
using Clinica.Entidades.Dtos;
using Clinica.Entidades.Excepciones;
using Clinica.Entidades.Models;

namespace Clinica.Negocio.Servicios;

public class MedicoServicio : IMedicoServicio
{
    private readonly IMedicoRepositorio _repositorio;
    private readonly IMapper _mapper;

    public MedicoServicio(IMedicoRepositorio repositorio, IMapper mapper)
    {
        _repositorio = repositorio;
        _mapper = mapper;
    }

    public async Task<List<MedicoDto>> ObtenerTodosAsync(string? especialidad, bool? activo)
    {
        var medicos = await _repositorio.ObtenerTodosAsync(especialidad, activo);
        return _mapper.Map<List<MedicoDto>>(medicos);
    }

    public async Task<MedicoDto?> ObtenerPorIdAsync(int id)
    {
        var medico = await _repositorio.ObtenerPorIdAsync(id);
        return medico is null ? null : _mapper.Map<MedicoDto>(medico);
    }

    public async Task<MedicoDto> CrearAsync(MedicoCrearDto dto)
    {
        // Regla 5: el email debe ser único
        if (await _repositorio.ExisteEmailAsync(dto.Email))
            throw new ReglaNegocioException($"Ya existe un médico con el email '{dto.Email}'.");

        var medico = _mapper.Map<Medico>(dto);
        medico.Activo = true;

        await _repositorio.AgregarAsync(medico);
        return _mapper.Map<MedicoDto>(medico);
    }

    public async Task<bool> ActualizarAsync(int id, MedicoActualizarDto dto)
    {
        var medico = await _repositorio.ObtenerPorIdAsync(id);
        if (medico is null) return false;

        // Regla 5: el email debe ser único (excluyendo al propio médico)
        if (await _repositorio.ExisteEmailAsync(dto.Email, id))
            throw new ReglaNegocioException($"Ya existe otro médico con el email '{dto.Email}'.");

        _mapper.Map(dto, medico);
        await _repositorio.ActualizarAsync(medico);
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var medico = await _repositorio.ObtenerPorIdAsync(id);
        if (medico is null) return false;

        // Regla 4: no se elimina un médico con citas
        if (await _repositorio.TieneCitasAsync(id))
            throw new ReglaNegocioException("No se puede eliminar un médico que tiene citas registradas.");

        await _repositorio.EliminarAsync(medico);
        return true;
    }
}