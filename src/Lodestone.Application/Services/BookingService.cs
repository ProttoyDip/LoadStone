using Lodestone.Application.DTOs.Booking;
using Lodestone.Application.Interfaces;

namespace Lodestone.Application.Services;

public class BookingService : IBookingService
{
    private readonly IUnitOfWork _unitOfWork;

    public BookingService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public Task<BookingDto> CreateBookingAsync(int studentProfileId, CreateBookingDto dto, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task<IReadOnlyList<BookingDto>> GetUpcomingAsync(int counselorProfileId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task CancelAsync(int bookingId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
