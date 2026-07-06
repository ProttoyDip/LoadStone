# Lodestone

### A Behavioral Early-Warning & Peer Support Platform for Student Wellbeing

Lodestone is an **ASP.NET Core MVC** web application (C#, .NET 8) that helps educational
institutions spot early signs of student disengagement **before** they become crises — and then
routes those students toward gentle, human support.

Unlike ordinary mental-health journaling apps that depend entirely on a student *choosing* to log
how they feel, Lodestone reads **behavioral learning-analytics signals** the student is already
generating — login frequency, activity span, days since last access, forum/course participation,
and assignment-submission lateness. These signals feed an **ML.NET** risk model that flags
potential disengagement early, even when a student never writes a single journal entry.

> **Important:** Lodestone is **not** a diagnostic or clinical system. It does not detect, diagnose,
> or treat mental illness. Its risk scores exist only to help route students to peer support and
> qualified human counselors. All high-risk situations are escalated to people, never handled by the
> model alone.

---

## Table of Contents

- [Key Features](#key-features)
- [Tech Stack](#tech-stack)
- [Clean Architecture Overview](#clean-architecture-overview)
- [Dependency Direction](#dependency-direction)
- [Folder Structure](#folder-structure)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Environment Configuration](#environment-configuration)
- [Database Setup](#database-setup)
- [Authentication & Roles](#authentication--roles)
- [ML.NET Risk Prediction Setup](#mlnet-risk-prediction-setup)
- [Background Jobs](#background-jobs)
- [Real-Time Features](#real-time-features)
- [Reporting](#reporting)
- [Running Tests](#running-tests)
- [Deployment](#deployment)
- [Privacy & Ethical Design](#privacy--ethical-design)
- [Security Notes](#security-notes)
- [Recommended Development Workflow](#recommended-development-workflow)
- [Roadmap](#roadmap)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)
- [License](#license)
- [Academic Note](#academic-note)

---

## Key Features

- **Behavioral Risk Engine** — Scores disengagement risk from learning-activity signals using ML.NET, not self-report alone.
- **Low-pressure Nudge & Check-In Flow** — Sends gentle, optional prompts instead of alarming alerts.
- **Peer Support Forum** — Category-based discussion with moderation and flagging.
- **Counselor Review Queue** — Prioritized list of at-risk students for human triage.
- **Counselor Booking** — Students book counselor availability slots; status is tracked end to end.
- **Optional Mood Journal** — Private, opt-in journaling with protected storage.
- **Crisis Resource Page** — Always-accessible hotlines and support links (no login required).
- **Real-time SignalR Alerts** — Live queue and peer-chat updates without page refreshes.
- **Hangfire Background Scoring Jobs** — Recurring risk scoring, nudges, reminders, and escalation.
- **Admin Dashboard & Analytics** — Chart.js visualizations of engagement and risk trends.
- **QuestPDF Reporting** — Counselor session, risk-summary, and student-engagement PDFs.
- **Privacy-focused Design** — Opt-out monitoring, minimized data exposure, encrypted sensitive fields.

---

## Tech Stack

| Layer / Concern | Technology |
|---|---|
| Web framework | ASP.NET Core MVC |
| Language | C# |
| Runtime | .NET 8 (or later) |
| ORM / Data access | Entity Framework Core |
| Database | SQL Server (default) or PostgreSQL |
| Authentication | ASP.NET Core Identity |
| Machine learning | ML.NET |
| Real-time | SignalR |
| Background jobs | Hangfire |
| Charts & analytics | Chart.js |
| PDF reporting | QuestPDF |
| Testing | xUnit (+ Moq, FluentAssertions) |
| UI styling | Bootstrap / custom CSS |

---

## Clean Architecture Overview

Lodestone follows **Clean Architecture**: dependencies always point *inward* toward a stable core,
so business rules never depend on frameworks, databases, or the UI.

| Project | Responsibility |
|---|---|
| **Lodestone.Domain** | Enterprise core — entities, enums, base classes, and domain constants. Depends on nothing. |
| **Lodestone.Shared** | Cross-cutting primitives — `Result`/`Error` types, custom exceptions, helpers, and extensions. |
| **Lodestone.Application** | Use-case layer — service **interfaces**, DTOs, validators, mapping profiles, and business logic. |
| **Lodestone.Infrastructure** | Concrete technology — EF Core `DbContext`, repositories, Identity, email, and security. |
| **Lodestone.ML** | ML.NET data loading, feature engineering, training/evaluation, and the prediction service. |
| **Lodestone.Jobs** | Hangfire background jobs and their scheduling. |
| **Lodestone.Reporting** | QuestPDF templates and report generators. |
| **Lodestone.Web** | ASP.NET Core MVC UI, controllers, views, SignalR hubs, view models — and the composition root. |
| **tests/** | `Lodestone.UnitTests`, `Lodestone.IntegrationTests`, and `Lodestone.MLTests`. |

---

## Dependency Direction

```
Web
 ↓
Application
 ↓
Domain
```

A fuller view of how the outer projects compose the inner ones:

```
        Web  ──►  Application ──►  Domain  ◄── Shared
         │            ▲   ▲   ▲
         ├─► Infrastructure ─┘   │
         ├─► ML ────────────────┘
         ├─► Jobs ──► (Application + Infrastructure + ML)
         └─► Reporting ──► Application
```

**Rules that keep the architecture clean:**

- **Domain** has **no dependency** on any other project.
- **Application** contains **interfaces and business logic** — no database or UI code.
- **Infrastructure** contains **database and external implementations** of Application interfaces.
- **Web must not use `DbContext` directly** — it depends only on Application services.
- **Controllers call services, not repositories** — repositories live only in Infrastructure.
- **Web is the only composition root** — it wires every layer together in `Program.cs`.

---

## Folder Structure

```
Lodestone/
├── Lodestone.sln
├── src/
│   ├── Lodestone.Web/            # MVC UI: Controllers, Views, Hubs, ViewModels, Program.cs, wwwroot
│   ├── Lodestone.Application/    # Interfaces, Services, DTOs, Validators, Mappings
│   ├── Lodestone.Domain/         # Entities, Enums, Common base types, Constants
│   ├── Lodestone.Infrastructure/ # Data (DbContext, Configurations), Repositories, Identity, Email, Security
│   ├── Lodestone.ML/             # Models, Training, Prediction, SavedModels/
│   ├── Lodestone.Jobs/           # BackgroundJobs, Scheduling
│   ├── Lodestone.Reporting/      # Reports, Templates, Export
│   └── Lodestone.Shared/         # Helpers, Extensions, Exceptions, Result, Constants
├── tests/
│   ├── Lodestone.UnitTests/          # Services, Validators, ML helpers
│   ├── Lodestone.IntegrationTests/   # Controllers, Repositories, Auth, Database
│   └── Lodestone.MLTests/            # Feature engineering, evaluation, prediction
├── docs/            # proposal, ER diagram, wireframes, architecture, ML & final reports
├── database/        # reference DDL, seed data, sample activity logs
├── deployment/      # Docker assets, publish output, environment notes
├── README.md
├── .gitignore
└── LICENSE
```

---

## Prerequisites

- **.NET 8 SDK** or later
- **SQL Server** (LocalDB is fine for development) **or PostgreSQL**
- An IDE: **Visual Studio 2022**, **JetBrains Rider**, or **VS Code**
- **Git**
- *(Optional)* **Docker** — for containerized runs
- *(Optional)* The **OULAD dataset** — for ML model training and validation

---

## Getting Started

**Step 1 — Clone the repository**

```bash
git clone <repository-url>
cd Lodestone
```

**Step 2 — Open the solution**

Open `Lodestone.sln` in Visual Studio / Rider, or open the folder in VS Code.

**Step 3 — Configure `appsettings.json`**

Update the connection string and other settings in `src/Lodestone.Web/appsettings.json`
(see [Environment Configuration](#environment-configuration)).

**Step 4 — Restore NuGet packages and build**

```bash
dotnet restore
dotnet build
```

**Step 5 — Apply EF Core migrations**

```bash
dotnet ef database update \
  --project src/Lodestone.Infrastructure \
  --startup-project src/Lodestone.Web
```

**Step 6 — Seed roles and default data**

Roles (Student, Volunteer, Counselor, Admin) and baseline reference data are seeded automatically at
first startup by `DbInitializer` and `RoleSeeder`. No manual step is required.

**Step 7 — Run the application**

```bash
dotnet run --project src/Lodestone.Web
```

**Step 8 — Access the application**

Open your browser at the URL printed in the console, typically:

```
https://localhost:5001
```

---

## Environment Configuration

Below is an **example** structure using placeholders only — never commit real secrets.

```jsonc
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=<host>;Database=Lodestone;Trusted_Connection=True;MultipleActiveResultSets=true",
    "HangfireConnection": "Server=<host>;Database=LodestoneHangfire;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "MachineLearning": {
    "ModelPath": "src/Lodestone.ML/SavedModels/risk-model.zip"
  },
  "Hangfire": {
    "DashboardPath": "/hangfire",
    "WorkerCount": 4
  },
  "Email": {
    "SmtpHost": "<smtp-host>",
    "SmtpPort": 587,
    "FromAddress": "no-reply@lodestone.local",
    "UserName": "<smtp-username>",
    "Password": "<use-secrets-or-env-vars>"
  },
  "Encryption": {
    "KeyRingPath": "keys",
    "ApplicationName": "Lodestone"
  }
}
```

> Use **User Secrets** in development (`dotnet user-secrets`) and **environment variables** in
> production for anything sensitive.

---

## Database Setup

Lodestone uses **EF Core code-first migrations**. The database schema is owned by
`Lodestone.Infrastructure`.

**Create a migration** (when you change entities):

```bash
dotnet ef migrations add <MigrationName> \
  --project src/Lodestone.Infrastructure \
  --startup-project src/Lodestone.Web
```

**Apply migrations to the database:**

```bash
dotnet ef database update \
  --project src/Lodestone.Infrastructure \
  --startup-project src/Lodestone.Web
```

**Seed data** — On first run, the app seeds the four core roles and baseline reference data (forum
categories, crisis resources). Reference SQL for manual environments lives in `database/`.

**Roles seeded:**

- `Student`
- `Volunteer`
- `Counselor`
- `Admin`

---

## Authentication & Roles

Authentication is handled by **ASP.NET Core Identity** with **role-based authorization**.

| Role | Can do |
|---|---|
| **Student** | Write journal entries, use the peer forum, book counselors, respond to check-ins, view crisis resources. |
| **Volunteer** | Support and moderate the peer forum; help triage community discussions. |
| **Counselor** | Work the risk review queue, manage bookings and sessions, generate session reports. |
| **Admin** | Manage users, view analytics and the aggregate dashboard, configure system settings. |

---

## ML.NET Risk Prediction Setup

The risk model is intentionally built on **behavioral signals — never private emotional text or
journal contents.**

**Example features:**

- Login frequency
- Days since last access
- Activity span
- Submission-lateness trend
- Forum / course participation

**Dataset:** The **OULAD (Open University Learning Analytics Dataset)** can be used for training and
validation. Place raw/exported data under `src/Lodestone.ML/Data/`.

**Saved model location:**

```
src/Lodestone.ML/SavedModels/risk-model.zip
```

**Example training workflow** (via the `TrainingPipeline` in `Lodestone.ML`):

```bash
# Illustrative — expose training through a CLI or admin action, then:
# 1. Load OULAD / exported activity logs
# 2. Run feature engineering
# 3. Train + evaluate the binary classifier
# 4. Save the model to src/Lodestone.ML/SavedModels/risk-model.zip
```

At runtime the model is served through a pooled `PredictionEnginePool` behind the
`IMlRiskPredictionService` interface, keeping ML.NET out of the Application and Domain layers.

---

## Background Jobs

Scheduled work runs on **Hangfire** (jobs live only in `Lodestone.Jobs`, never in controllers):

| Job | Purpose |
|---|---|
| `WeeklyRiskScoringJob` | Recomputes risk scores for all students on a weekly cadence. |
| `NudgeNotificationJob` | Dispatches pending, low-pressure nudges to at-risk students. |
| `BookingReminderJob` | Sends upcoming counselor-session reminders. |
| `ForumModerationJob` | Sweeps flagged forum content for moderation triage. |
| `CrisisResourceEscalationJob` | Escalates critical-risk students to human support workflows. |

The Hangfire dashboard is available at `/hangfire` (secure it behind admin authorization in production).

---

## Real-Time Features

Live updates are delivered through **SignalR** hubs:

| Hub | Purpose |
|---|---|
| `CounselorQueueHub` | Pushes real-time updates to the counselor risk-review queue. |
| `PeerChatHub` | Powers real-time peer-support chat rooms. |

Client scripts live in `src/Lodestone.Web/wwwroot/js/` (`counselor-queue.js`, `peer-chat.js`).

---

## Reporting

PDF documents are produced with **QuestPDF** (generators live only in `Lodestone.Reporting`):

- **Counselor session reports** — Structured write-ups of a counseling session.
- **Risk summary reports** — Aggregate risk distribution over a date range.
- **Student engagement reports** — Individual engagement history and trends.

Reports are exposed to the Web layer through the `IReportService` interface.

---

## Running Tests

Run the full suite from the solution root:

```bash
dotnet test
```

Run a specific test project:

```bash
# Unit tests — services, validators, helpers
dotnet test tests/Lodestone.UnitTests

# Integration tests — controllers, repositories, auth, database
dotnet test tests/Lodestone.IntegrationTests

# ML tests — feature engineering, evaluation, prediction
dotnet test tests/Lodestone.MLTests
```

---

## Deployment

**1. Build a release version:**

```bash
dotnet publish src/Lodestone.Web -c Release -o ./deployment/publish
```

**2. Configure production settings** — Provide production `appsettings.Production.json` values via
**environment variables** (connection strings, SMTP, encryption keys).

**3. Apply migrations** against the production database:

```bash
dotnet ef database update \
  --project src/Lodestone.Infrastructure \
  --startup-project src/Lodestone.Web
```

**4. Set environment variables** — e.g. `ASPNETCORE_ENVIRONMENT=Production` and all secrets.

**5. Host it** — Run behind **IIS**, in **Docker** (see `deployment/docker/`), or on a cloud host
(Azure App Service, AWS, etc.).

```bash
# Docker (from repo root)
docker compose -f deployment/docker/docker-compose.yml up --build
```

---

## Privacy & Ethical Design

Lodestone is built support-first, with privacy and human oversight at its core:

- **Lodestone is not a diagnostic tool** and makes no clinical claims.
- **Risk scores are used only for support routing**, never for grading, discipline, or labeling.
- **Students can opt out** of behavioral monitoring.
- **Counselors see privacy-respecting queue data** — the minimum needed to help, not raw personal content.
- **The model never counsels students by itself.**
- **High-risk situations route to human support** and clearly presented crisis resources.

---

## Security Notes

- Use **ASP.NET Core Identity** for authentication.
- Enforce **role-based authorization** on every sensitive action.
- **Do not commit secrets** — use User Secrets and environment variables.
- Use **environment variables in production** for all credentials and keys.
- **Encrypt sensitive fields** (e.g. journal notes) using ASP.NET Data Protection.
- **Protect behavioral logs and journal entries** with least-privilege access.
- **Validate all user input** (FluentValidation on DTOs, model validation on view models).

---

## Recommended Development Workflow

- Create **feature branches** off `main` (e.g. `feature/counselor-queue`).
- Use **pull requests** with at least one teammate review.
- Keep **controllers thin** — delegate to Application services.
- Put **business logic in the Application layer**.
- Keep **database logic in Infrastructure**.
- **Add tests** for services and ML feature engineering as you build them.

---

## Roadmap

- [ ] MVP authentication and roles
- [ ] Forum and journal
- [ ] Activity log ingestion
- [ ] ML risk scoring
- [ ] Counselor review queue
- [ ] Booking system
- [ ] Dashboard analytics
- [ ] PDF reports
- [ ] Final testing and polishing

---

## Troubleshooting

| Issue | Likely cause & fix |
|---|---|
| **Database connection failed** | Check `ConnectionStrings:DefaultConnection`; confirm the DB server is running and reachable. |
| **EF migration not found** | Run `dotnet tool install --global dotnet-ef`; ensure you pass both `--project` and `--startup-project`. |
| **Identity roles not seeded** | Ensure the app completed first-run startup; verify `RoleSeeder`/`DbInitializer` ran without exceptions in the logs. |
| **ML model file missing** | Place `risk-model.zip` in `src/Lodestone.ML/SavedModels/`, or train a model first. |
| **Hangfire dashboard not loading** | Confirm the Hangfire connection string and that the Hangfire schema was created; check `/hangfire` authorization. |
| **SignalR connection issue** | Verify hub routes (`/hubs/counselor-queue`, `/hubs/peer-chat`) and that the SignalR client script is loaded. |

---

## Contributing

This project is developed by a **3-member academic team**. To keep collaboration smooth:

1. **Fork/branch** — create a feature branch per task.
2. **Small, focused commits** — write clear commit messages.
3. **Open a pull request** — describe what changed and why; request a review from a teammate.
4. **Keep the architecture clean** — respect layer boundaries (see [Dependency Direction](#dependency-direction)).
5. **Add or update tests** — for any new service or ML logic.
6. **Update docs** — reflect meaningful changes in `docs/` and this README.

---

## License

This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for details.

---

## Academic Note

Lodestone is designed for **educational / capstone  use**. It is a demonstration
of behavioral learning analytics and supportive-technology design — **not** a substitute for
professional mental health care. If you or someone you know is in crisis, please contact a qualified
professional or local emergency services.
