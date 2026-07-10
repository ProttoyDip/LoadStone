using Lodestone.Application.DTOs.Booking;
using Lodestone.Application.Interfaces;
using Lodestone.Domain.Entities;
using Lodestone.Infrastructure.Data;
using Lodestone.Web.ViewModels.Booking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lodestone.Web.Controllers;

[Authorize]
public class BookingController : Controller
{
    private readonly IBookingService _bookingService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _db;

    public BookingController(
        IBookingService bookingService,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext db)
    {
        _bookingService = bookingService;
        _userManager    = userManager;
        _db             = db;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var profileId = await GetStudentProfileIdAsync();
        if (profileId is null) return Challenge();

        var bookings   = await _bookingService.GetStudentBookingsAsync(profileId.Value, cancellationToken);
        var counselors = await _bookingService.GetCounselorsAsync(cancellationToken);

        var vm = new BookingViewModel
        {
            Bookings      = bookings,
            CounselorList = counselors,
            NewBooking    = new CreateBookingDto(0, null, DateTime.UtcNow.AddDays(1), null),
        };
        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var counselors = await _bookingService.GetCounselorsAsync(cancellationToken);
        var vm = new BookingViewModel
        {
            CounselorList = counselors,
            NewBooking    = new CreateBookingDto(0, null, DateTime.UtcNow.AddDays(1), null),
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBookingDto model, CancellationToken cancellationToken)
    {
        var profileId = await GetStudentProfileIdAsync();
        if (profileId is null) return Challenge();

        if (!ModelState.IsValid || model.CounselorProfileId == 0)
        {
            ModelState.AddModelError(string.Empty, "Please select a counselor.");
            var counselors = await _bookingService.GetCounselorsAsync(cancellationToken);
            return View(new BookingViewModel { CounselorList = counselors, NewBooking = model });
        }

        await _bookingService.CreateBookingAsync(profileId.Value, model, cancellationToken);
        TempData["Success"] = "Booking requested successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id, CancellationToken cancellationToken)
    {
        await _bookingService.CancelAsync(id, cancellationToken);
        TempData["Success"] = "Booking cancelled.";
        return RedirectToAction(nameof(Index));
    }

    // --- Helpers ---

    private async Task<int?> GetStudentProfileIdAsync()
    {
        var userId = _userManager.GetUserId(User);
        if (userId is null) return null;
        var profile = await _db.StudentProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == userId);
        return profile?.Id;
    }
}
