using FluentValidation;
using Lodestone.Application.DTOs.Journal;
using Lodestone.Application.Exceptions;
using Lodestone.Application.Interfaces;
using Lodestone.Domain.Constants;
using Lodestone.Web.ViewModels.Journal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lodestone.Web.Controllers;

[Authorize(Roles = RoleConstants.Student)]
public class JournalController : Controller
{
    private readonly IJournalService _journalService;
    private readonly IStudentProfileRepository _studentProfiles;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<CreateJournalEntryDto> _validator;

    public JournalController(
        IJournalService journalService,
        IStudentProfileRepository studentProfiles,
        ICurrentUserService currentUser,
        IValidator<CreateJournalEntryDto> validator)
    {
        _journalService = journalService;
        _studentProfiles = studentProfiles;
        _currentUser = currentUser;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var profileId = await GetStudentProfileIdAsync(cancellationToken);
        if (profileId is null)
            return Forbid();

        return View(await BuildViewModelAsync(profileId.Value, new CreateJournalEntryDto(3, null), cancellationToken));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddEntry(
        [Bind(Prefix = nameof(JournalViewModel.NewEntry))] CreateJournalEntryDto newEntry,
        CancellationToken cancellationToken)
    {
        var profileId = await GetStudentProfileIdAsync(cancellationToken);
        if (profileId is null)
            return Forbid();

        var validationResult = await _validator.ValidateAsync(newEntry, cancellationToken);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError($"NewEntry.{error.PropertyName}", error.ErrorMessage);

            return View("Index", await BuildViewModelAsync(profileId.Value, newEntry, cancellationToken));
        }

        try
        {
            await _journalService.AddEntryAsync(profileId.Value, newEntry, cancellationToken);
        }
        catch (DailyJournalEntryLimitException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return View("Index", await BuildViewModelAsync(profileId.Value, newEntry, cancellationToken));
        }

        TempData["JournalSuccess"] = "Your private check-in was saved.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<JournalViewModel> BuildViewModelAsync(
        int profileId,
        CreateJournalEntryDto newEntry,
        CancellationToken cancellationToken)
    {
        var entries = await _journalService.GetEntriesAsync(profileId, cancellationToken);
        return new JournalViewModel
        {
            Entries = entries,
            NewEntry = newEntry,
            HasEntryToday = entries.Any(entry => entry.EntryDateUtc.Date == DateTime.UtcNow.Date)
        };
    }

    private Task<int?> GetStudentProfileIdAsync(CancellationToken cancellationToken)
        => string.IsNullOrWhiteSpace(_currentUser.UserId)
            ? Task.FromResult<int?>(null)
            : _studentProfiles.GetIdByUserIdAsync(_currentUser.UserId, cancellationToken);
}
