# Deployment Environment Notes

- Runtime: .NET 8, ASP.NET Core.
- Database: SQL Server (LocalDB for dev, container/managed instance for prod).
- Required environment variables / secrets:
  - `ConnectionStrings__DefaultConnection`
  - `ConnectionStrings__HangfireConnection`
  - `Email__SmtpHost`, `Email__UserName`, `Email__Password`
- The trained ML model (`risk-model.zip`) must be present under `Lodestone.ML/SavedModels/`.
- Hangfire dashboard is exposed at `/hangfire` (secure it behind admin auth in production).
