﻿using AutoMapper;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Company.PL.MappingProfiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole, RoleViewModel>().ForMember(d => d.RoleName,o=>o.MapFrom(s=>s.Name))
                                                    .ReverseMap();
        }
    }
}
