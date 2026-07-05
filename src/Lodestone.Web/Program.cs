using Hangfire;
using Lodestone.Application;
using Lodestone.Application.Interfaces;
using Lodestone.Infrastructure;
using Lodestone.Infrastructure.Data;
using Lodestone.Infrastructure.Identity;
using Lodestone.Jobs;
using Lodestone.Jobs.Scheduling;
using Lodestone.ML;
using Lodestone.Reporting;
using Lodestone.Web;
using Lodestone.Web.Hubs;

var builder = WebApplication.CreateBuilder(args);

// QuestPDF community licence (report generation lives in Lodestone.Reporting).
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

// ---- MVC + Web-only services ----
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Identity UI area
builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// ---- Authorization policies (defined in Infrastructure/Identity) ----
builder.Services.AddAuthorization(IdentityPolicySeeder.AddPolicies);

// ---- Clean Architecture layer registrations ----
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddMachineLearning(
    Path.Combine(builder.Environment.ContentRootPath, "..", "Lodestone.ML", "SavedModels", "risk-model.zip"));
builder.Services.AddJobs(builder.Configuration);
builder.Services.AddReporting();

var app = builder.Build();

// ---- Middleware pipeline ----
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// ---- Endpoints ----
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapHub<CounselorQueueHub>(CounselorQueueHub.Route);
app.MapHub<PeerChatHub>(PeerChatHub.Route);
app.MapHangfireDashboard();

// ---- Startup work: migrate DB, seed roles, schedule recurring jobs ----
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    await DbInitializer.InitializeAsync(context);

    var roleManager = services.GetRequiredService<Microsoft.AspNetCore.Identity.RoleManager<Microsoft.AspNetCore.Identity.IdentityRole>>();
    await RoleSeeder.SeedRolesAsync(roleManager);

    var recurringJobs = services.GetRequiredService<IRecurringJobManager>();
    RecurringJobScheduler.RegisterRecurringJobs(recurringJobs);
}

app.Run();

// Exposed for WebApplicationFactory in integration tests.
public partial class Program { }
