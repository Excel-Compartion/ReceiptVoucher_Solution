using AutoMapper;
using ReceiptVoucher.Core.Enums;

namespace ReceiptVoucher.Server.AutoMapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //========= ShowTime Mapping ===============
            CreateMap<Branch_ViewModel, Branch>().ReverseMap();
            //.ForMember(dest => dest.Id, opt => opt.Ignore()); // ignore Id when Mapp From ViewModel To DataBase


            CreateMap<Receipt, ReceiptWithRelatedDataDto>()
          .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
          .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))
          .ForMember(dest => dest.SubProjectName, opt => opt.MapFrom(src => src.SubProject.Name))
          .ForMember(dest => dest.GrantDestinationName, opt => opt.MapFrom(src => src.GrantDestinations.GetDisplayName()))
          .ForMember(dest => dest.GenderName, opt => opt.MapFrom(src => src.Gender.GetDisplayName()))
          .ForMember(dest => dest.PaymentTypeName, opt => opt.MapFrom(src => src.PaymentType.GetDisplayName()))
          .ReverseMap();


            CreateMap<Receipt, GrantDestination_VM>().ReverseMap();


            /// Receipt
            CreateMap<Receipt, GetReceiptDto>().ReverseMap();

            CreateMap<GetReceiptDto, ReceiptViewModel>().ReverseMap();


            CreateMap<PostReceiptDto, GetReceiptDto>().ReverseMap();


            CreateMap<Receipt, PostReceiptDto>()
                .ReverseMap();

        }
    }
}
