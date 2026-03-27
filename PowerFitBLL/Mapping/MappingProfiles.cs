using AutoMapper;
using PowerFitBLL.ViewModels.SessionViewModel;
using PowerFitDAL.Entities;
using PowerFitDAL.Repositories.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerFitBLL.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.CategoryName, options => options.MapFrom(Src => Src.Category.CategoryName))
                .ForMember(dest => dest.TrainerName, options => options.MapFrom(Src => Src.Trainer.Name))
                .ForMember(dest => dest.AvailableSlots, options => options.Ignore());

            CreateMap<CreateSessionViewModel, Session>();

            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
        }    
    }
}
                
