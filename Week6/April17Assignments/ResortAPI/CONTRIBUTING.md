# Contributing to Online Resort Booking Portal

Thank you for contributing. This document defines coding standards, branch/PR workflow, and how to run & test the project.

## Guidelines

- Follow the `.editorconfig` rules. Use 4 spaces for indentation.
- Naming conventions: PascalCase for public types & methods; _camelCase for private fields.
- Keep methods small, single responsibility. Apply clean architecture separation: Models, DTOs, Repository, Service, Controller, Exception, Configuration.
- Add unit/integration tests where appropriate.

## Branching & PRs

- Use feature branches named `feature/<short-description>`.
- Open pull requests against `main`. PRs must include a description and any migrations.
- Require at least one approving review before merge.

## Migrations

- Use EF Core tools: `dotnet tool install --global dotnet-ef` (if needed)
- Add migration: `dotnet ef migrations add <Name> --project OnlineResortBooking`
- Apply migration: `dotnet ef database update --project OnlineResortBooking`

## Running the project

- Ensure .NET 8 SDK installed: `dotnet --info`
- Restore & run:
  - `dotnet restore`
  - `dotnet run`

Swagger UI available at `/swagger` while app is running.

## Testing

- Manual API testing can be done with Swagger or the provided frontend in `wwwroot`.

## Style & Linting

- Keep lines <= 120 characters.
- Document public methods with XML comments when non-trivial.

## Issues & Templates

- Create clear issue titles and reproduction steps.