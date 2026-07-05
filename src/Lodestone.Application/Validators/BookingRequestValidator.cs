using FluentValidation;
using Lodestone.Application.DTOs.Booking;

namespace Lodestone.Application.Validators;

public class BookingRequestValidator : AbstractValidator<CreateBookingDto>
{
    public BookingRequestValidator()
    {
        RuleFor(x => x.CounselorProfileId).GreaterThan(0);
        RuleFor(x => x.ScheduledForUtc).GreaterThan(DateTime.UtcNow);
    }
}
