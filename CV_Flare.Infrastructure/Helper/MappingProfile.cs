using AutoMapper;
using CV_Flare.Application.DTOs;
using CV_FLare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV_Flare.Infrastructure.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, AccountDTO>()
           .ForMember(dest => dest.Email, otp => otp.MapFrom(src => src.UserEmail))
           .ReverseMap()
           .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.Email))
           .ForMember(dest => dest.UserPassword, opt => opt.MapFrom(src => src.Password))
           .ReverseMap()
           .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.UserPassword));
        }
    }
}
