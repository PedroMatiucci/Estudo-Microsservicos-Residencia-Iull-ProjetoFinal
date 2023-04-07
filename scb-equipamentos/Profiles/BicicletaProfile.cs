using AutoMapper;
using scb_equipamentos.Data.BicicletaDto;
using scb_equipamentos.Models;

namespace scb_equipamentos.Profiles
{
    public class BicicletaProfile : Profile
    {
        public BicicletaProfile()
        {
            CreateMap<CreateBicicletaDto,Bicicleta>();
            CreateMap<UpdateBicicletaDto, Bicicleta>();
            CreateMap<Bicicleta, ReturnBicicletaDto>();
        }
    }
}
