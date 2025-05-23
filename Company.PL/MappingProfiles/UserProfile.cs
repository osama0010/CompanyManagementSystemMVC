﻿using AutoMapper;
using Company.DAL.Models;
using Company.PL.ViewModels;

namespace Company.PL.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
        }
    }
}
