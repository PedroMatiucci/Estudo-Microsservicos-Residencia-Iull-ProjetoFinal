using AutoMapper;
using scb_equipamentos.Data.TotemDto;
using scb_equipamentos.Models;

namespace scb_equipamentos.Profiles
{
    public class TotemProfile : Profile
    {
        public TotemProfile()
        {
            CreateMap<CreateTotemDto, Totem>();
            CreateMap<UpdateTotemDto, Totem>();
        }
    }
}
