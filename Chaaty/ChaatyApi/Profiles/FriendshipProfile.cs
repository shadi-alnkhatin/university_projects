using AutoMapper;
using ChaatyApi.DTOs;
using ChaatyApi.Entities;
using ChaatyApi.Helpers;

namespace ChaatyApi.Profiles
{
    public class FriendshipProfile:Profile
    {
        public FriendshipProfile()
        {
            CreateMap<FriendShip, FriendshipDto>()
                .ForMember(dest => dest.FriendshipId, Opt => Opt.MapFrom(i => i.Id))
                .ForMember(dest => dest.Id, Opt => Opt.MapFrom(i => i.Sender.Id ))
                .ForMember(dest => dest.FirstName, Opt => Opt.MapFrom(i => i.Sender.FirstName))
                .ForMember(dest => dest.LastName, Opt => Opt.MapFrom(i => i.Sender.LastName))
                .ForMember(dest => dest.PhotoUrl,
                Opt => Opt.MapFrom(i => i.Sender.Photos.FirstOrDefault(p => p.IsMain==true).Url));
        }
    }
}
