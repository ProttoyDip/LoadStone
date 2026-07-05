using AutoMapper;
using Lodestone.Application.DTOs.Booking;
using Lodestone.Application.DTOs.Forum;
using Lodestone.Application.DTOs.Journal;
using Lodestone.Domain.Entities;

namespace Lodestone.Application.Mappings;

/// <summary>Entity &lt;-&gt; DTO maps. Entities never leave the Application/Domain boundary as-is.</summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ForumPost, ForumPostDto>().ReverseMap();
        CreateMap<ForumComment, ForumCommentDto>().ReverseMap();
        CreateMap<MoodJournalEntry, JournalEntryDto>().ReverseMap();
        CreateMap<CounselorBooking, BookingDto>().ReverseMap();
    }
}
