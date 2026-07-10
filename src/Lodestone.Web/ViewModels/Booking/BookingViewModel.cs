using Lodestone.Application.DTOs.Booking;

namespace Lodestone.Web.ViewModels.Booking;

public class BookingViewModel
{
    public IReadOnlyList<BookingDto> Bookings { get; set; } = new List<BookingDto>();
    public IReadOnlyList<CounselorSummaryDto> CounselorList { get; set; } = new List<CounselorSummaryDto>();
    public CreateBookingDto NewBooking { get; set; } = new(0, null, DateTime.UtcNow.AddDays(1), null);
}
