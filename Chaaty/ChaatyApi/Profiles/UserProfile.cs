using AutoMapper;
using ChaatyApi.DTOs;
using ChaatyApi.Entities;

namespace ChaatyApi.Profiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {

            CreateMap<RigesterDto, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt
                .MapFrom(src => src.Email));
            CreateMap<PhotoDto, Photo>();
            CreateMap<Photo, PhotoDto>();
            CreateMap<Photo, PhotoToRreturnDto>();
            CreateMap<AppUser, UserDto>()
                .ForMember(dest=>dest.Photos ,opt=>opt.MapFrom(src=>src.Photos))
                .ForMember(dest => dest.PhotoUrl,
                opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url));
            CreateMap<AppUser, AuthResponse>();
            CreateMap<UserToUpdateDto, AppUser>();
            CreateMap<AppUser,UserDetailsDto>()
                .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos))
                .ForMember(dest => dest.PhotoUrl,
                opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url)); 
        }
    }
}
