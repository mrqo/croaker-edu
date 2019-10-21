using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AspNetCore.Identity.LiteDB.Models;

using edu_croaker.Data.Entities;
using edu_croaker.Data.Dtos;

namespace edu_croaker.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Croak, CroakDto>()
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.Author))
                .ForMember(dest => dest.SharesCount, opt => opt.MapFrom(src => src.Shares))
                .ForMember(dest => dest.LikesCount, opt => opt.MapFrom(src => src.Likes));

            CreateMap<CroakDto, Croak>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.Shares, opt => opt.MapFrom(src => src.SharesCount))
                .ForMember(dest => dest.Likes, opt => opt.MapFrom(src => src.LikesCount));

            CreateMap<PublicUserData, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId));
            CreateMap<ApplicationUser, PublicUserData>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

            CreateMap<UserDetails, PublicUserData>();
            CreateMap<PublicUserData, UserDetails>();

            CreateMap<Follower, FollowerDto>();
            CreateMap<FollowerDto, Follower>();

            CreateMap<Like, LikeDto>();
            CreateMap<LikeDto, Like>();
        }
    }
}
