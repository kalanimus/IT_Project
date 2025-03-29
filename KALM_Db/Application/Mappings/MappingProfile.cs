using AutoMapper;
using Core.Entities;
using Application.DTOs;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.SignalR;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ModelUser, UserDto>()
              .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
              .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));
            CreateMap<UserDto, ModelUser>()
              .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId));

            CreateMap<ModelSubject, SubjectDto>();
            CreateMap<SubjectDto, ModelSubject>();

            CreateMap<ModelRole, RoleDto>();
            CreateMap<RoleDto, ModelRole>();

            CreateMap<ModelPermission, PermissionDto>();
            CreateMap<PermissionDto, ModelPermission>();

            CreateMap<ModelGroup, GroupDto>();
            CreateMap<GroupDto, ModelGroup>();
        }
    }
}