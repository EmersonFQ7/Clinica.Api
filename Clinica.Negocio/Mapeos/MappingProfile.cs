using AutoMapper;
using Clinica.Entidades.Dtos;
using Clinica.Entidades.Models;

namespace Clinica.Negocio.Mapeos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Entidad -> DTO de lectura
        CreateMap<Medico, MedicoDto>();

        CreateMap<Cita, CitaDto>()
            .ForMember(d => d.Medico,
                o => o.MapFrom(s => s.Medico.Nombre + " " + s.Medico.Apellido));

        // DTO de entrada -> Entidad (crear)
        CreateMap<MedicoCrearDto, Medico>();
        CreateMap<CitaCrearDto, Cita>();

        // DTO de entrada -> Entidad (actualizar sobre objeto existente)
        CreateMap<MedicoActualizarDto, Medico>();
        CreateMap<CitaActualizarDto, Cita>();
    }
}