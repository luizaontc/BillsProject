using AutoMapper;
using Bills.Domain.Dto.Bills;
using Bills.Domain.Dto.Users;
using Bills.Domain.Entities;

namespace Bills.Domain.Mappings
{
    public class BillMappingProfile : Profile
    {
        public BillMappingProfile()
        {
            CreateMap<Bill, BillDto>();
            CreateMap<BillDto, Bill>();
        }
    }
}
