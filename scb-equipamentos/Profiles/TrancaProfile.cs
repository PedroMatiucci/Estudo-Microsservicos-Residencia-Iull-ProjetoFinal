using AutoMapper;
using scb_equipamentos.Data.TrancaDto;
using scb_equipamentos.Models;

namespace scb_equipamentos.Profiles
{
    public class TrancaProfile : Profile
    {
        public TrancaProfile()
        {
            CreateMap<CreateTrancaDto, Tranca>();
            CreateMap<UpdateTrancaDto, Tranca>();
            CreateMap<Tranca, ReturnTrancaDto>();
        }
    }
}
