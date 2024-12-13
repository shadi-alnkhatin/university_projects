using AutoMapper;
using ChaatyApi.DTOs;
using ChaatyApi.Entities;

namespace ChaatyApi.Profiles
{
    public class MessageProfile:Profile
    {
        public MessageProfile()
        {
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderPhotoUrl,
                opt => opt.MapFrom(src => src.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
                 .ForMember(dest => dest.RecipientPhotoUrl,
                opt => opt.MapFrom(src => src.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url));
        }
    }
}
