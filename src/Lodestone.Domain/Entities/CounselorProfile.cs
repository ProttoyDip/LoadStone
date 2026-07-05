using Lodestone.Domain.Common;

namespace Lodestone.Domain.Entities;

public class CounselorProfile : AuditableEntity
{
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }

    public string? Specialization { get; set; }
    public bool IsAcceptingBookings { get; set; } = true;

    public ICollection<CounselorAvailabilitySlot> AvailabilitySlots { get; set; } = new List<CounselorAvailabilitySlot>();
    public ICollection<CounselorBooking> Bookings { get; set; } = new List<CounselorBooking>();
}
