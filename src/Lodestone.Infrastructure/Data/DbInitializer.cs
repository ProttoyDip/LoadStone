using Lodestone.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lodestone.Infrastructure.Data;

/// <summary>Applies migrations and seeds baseline reference data at startup.</summary>
public static class DbInitializer
{
    public static async Task InitializeAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
    {
        await context.Database.MigrateAsync(cancellationToken);
        await SeedCrisisResourcesAsync(context, cancellationToken);
        await SeedForumCategoriesAsync(context, cancellationToken);
    }

    private static async Task SeedCrisisResourcesAsync(ApplicationDbContext context, CancellationToken ct)
    {
        if (await context.CrisisResources.AnyAsync(ct)) return;

        var resources = new List<CrisisResource>
        {
            new()
            {
                Title        = "National Suicide & Crisis Lifeline",
                Description  = "Free, confidential support for people in distress. Available 24/7.",
                PhoneNumber  = "988",
                Url          = "https://988lifeline.org",
                IsEmergency  = true,
                DisplayOrder = 1,
                CreatedAtUtc = DateTime.UtcNow,
            },
            new()
            {
                Title        = "Crisis Text Line",
                Description  = "Text HOME to 741741 to connect with a trained Crisis Counselor. Free, 24/7.",
                PhoneNumber  = "741741",
                Url          = "https://www.crisistextline.org",
                IsEmergency  = true,
                DisplayOrder = 2,
                CreatedAtUtc = DateTime.UtcNow,
            },
            new()
            {
                Title        = "Emergency Services",
                Description  = "For immediate danger to yourself or others, call emergency services.",
                PhoneNumber  = "911",
                IsEmergency  = true,
                DisplayOrder = 3,
                CreatedAtUtc = DateTime.UtcNow,
            },
            new()
            {
                Title        = "SAMHSA National Helpline",
                Description  = "Free, confidential mental health and substance use treatment referral. 24/7, 365-day-a-year.",
                PhoneNumber  = "1-800-662-4357",
                Url          = "https://www.samhsa.gov/find-help/national-helpline",
                IsEmergency  = false,
                DisplayOrder = 4,
                CreatedAtUtc = DateTime.UtcNow,
            },
            new()
            {
                Title        = "Trevor Project (LGBTQ+)",
                Description  = "Crisis intervention and suicide prevention for LGBTQ+ youth.",
                PhoneNumber  = "1-866-488-7386",
                Url          = "https://www.thetrevorproject.org",
                IsEmergency  = false,
                DisplayOrder = 5,
                CreatedAtUtc = DateTime.UtcNow,
            },
            new()
            {
                Title        = "NAMI HelpLine",
                Description  = "Mental health information, support, and local resources from the National Alliance on Mental Illness.",
                PhoneNumber  = "1-800-950-6264",
                Url          = "https://www.nami.org/help",
                IsEmergency  = false,
                DisplayOrder = 6,
                CreatedAtUtc = DateTime.UtcNow,
            },
            new()
            {
                Title        = "Campus Counseling Centre",
                Description  = "Book a session with a Lodestone counselor. Available Mon–Fri 9am–5pm.",
                Url          = "/Booking",
                IsEmergency  = false,
                DisplayOrder = 7,
                CreatedAtUtc = DateTime.UtcNow,
            },
        };

        await context.CrisisResources.AddRangeAsync(resources, ct);
        await context.SaveChangesAsync(ct);
    }

    private static async Task SeedForumCategoriesAsync(ApplicationDbContext context, CancellationToken ct)
    {
        if (await context.ForumCategories.AnyAsync(ct)) return;

        var categories = new List<ForumCategory>
        {
            new()
            {
                Name         = "General Support",
                Description  = "A place to share how you're doing, ask for advice, or just vent.",
                CreatedAtUtc = DateTime.UtcNow,
            },
            new()
            {
                Name         = "Academic Stress",
                Description  = "Struggling with coursework, exams, or time management? You're not alone.",
                CreatedAtUtc = DateTime.UtcNow,
            },
            new()
            {
                Name         = "Wellbeing & Self-Care",
                Description  = "Tips, habits, and discussions around mental and physical wellness.",
                CreatedAtUtc = DateTime.UtcNow,
            },
            new()
            {
                Name         = "Social & Relationships",
                Description  = "Navigating friendships, homesickness, loneliness, and belonging on campus.",
                CreatedAtUtc = DateTime.UtcNow,
            },
        };

        await context.ForumCategories.AddRangeAsync(categories, ct);
        await context.SaveChangesAsync(ct);
    }
}
