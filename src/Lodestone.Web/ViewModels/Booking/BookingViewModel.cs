using Lodestone.Application.DTOs.Booking;

namespace Lodestone.Web.ViewModels.Booking;

public class BookingViewModel
{
    public IReadOnlyList<BookingDto> Bookings { get; set; } = new List<BookingDto>();
    public CreateBookingDto NewBooking { get; set; } = new(0, null, DateTime.UtcNow, null);
}
