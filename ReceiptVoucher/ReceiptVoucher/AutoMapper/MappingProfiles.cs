using AutoMapper;
using ReceiptVoucher.Core.Enums;
using ReceiptVoucher.Core.Identity;
using ReceiptVoucher.Core.Models.ViewModels.UserModels;

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
            CreateMap<Receipt, GetReceiptDto>()
          .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))

                .ReverseMap();

            CreateMap<GetReceiptDto, ReceiptViewModel>().ReverseMap();


            CreateMap<PostReceiptDto, GetReceiptDto>().ReverseMap();


            CreateMap<Receipt, PostReceiptDto>()
                .ReverseMap();

            /// ReceiptWithFilter_VM
            CreateMap<ReceiptWithFilter_VM, FilterData>()
                .ReverseMap();

            /// ReceiptWithFilter_VM
            CreateMap<GrantDestination_VM, GetReceiptDto>()
                .ReverseMap();

            CreateMap<ReceiptWithRelatedDataDto, GetReceiptDto>()
              .ReverseMap();


            //========== User Mapping Config Strat =============

            CreateMap<ApplicationUser, UserViewModel>()
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => (src.Branch != null ? src.Branch.Name : "ليس مرتبط بمكتب")))
            //.ForMember(dest => dest.BranchAccountNumber, opt => opt.(src => (src.Branch != null ? src.Branch.AccountNumber : "No AccountNumber")))

            .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.PhoneNumber)).ReverseMap();

            //========== User Mapping Config End=============


        }
    }
}
