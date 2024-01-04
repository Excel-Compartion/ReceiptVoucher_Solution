using AutoMapper;

namespace ReceiptVoucher.Server.AutoMapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //========= ShowTime Mapping ===============
            CreateMap<Branch_ViewModel, Branch>().ReverseMap();
            //.ForMember(dest => dest.Id, opt => opt.Ignore()); // ignore Id when Mapp From ViewModel To DataBase


           
        }
    }
}
