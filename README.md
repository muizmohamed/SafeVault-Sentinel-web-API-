# SafeVault Capstone

A secure ASP.NET Core 10 demonstration that covers input validation, parameterized SQL, authentication, RBAC, XSS-safe output encoding, and automated security tests.

## Security design

- **Input validation:** usernames use a strict allow-list (`A-Z`, `a-z`, `0-9`, `_`); email is validated and normalized.
- **SQL Injection defense:** all user-controlled SQL values use parameters (`$username`, `$email`, etc.).
- **Authentication:** passwords are hashed with BCrypt; plaintext passwords are never persisted.
- **Authorization:** the admin area requires the `Admin` role through `[Authorize(Roles = "Admin")]`.
- **XSS defense:** untrusted content is rendered through normal Razor expressions. `Html.Raw` is intentionally not used.
- **CSRF:** state-changing form posts include antiforgery tokens.
- **Cookies:** the authentication cookie is HttpOnly, Secure, SameSite=Lax and has a finite lifetime.

## Run

```bash
dotnet restore
dotnet run
```

The application creates `App_Data/safevault.db` automatically.

## Create an admin safely

For a classroom demo, create a normal user, then promote that user directly in a local development database with a controlled administrative procedure. Do not put a real admin password in source control.

Example local SQL:

```sql
UPDATE Users SET Role = 'Admin' WHERE Username = 'your-test-user';
```

## Test

```bash
dotnet test
```

The test suite includes:

1. Username/email validation tests.
2. SQL injection payload tests against parameterized queries.
3. XSS encoding tests.
4. BCrypt password verification tests.
5. Admin RBAC allow/deny tests.

## Attack strings used in tests

- SQLi-style username: `admin' OR '1'='1`
- XSS script: `<script>alert('xss')</script>`
- XSS event handler: `<img src=x onerror=alert(1)>`

These are tests of defensive behavior in a local application, not production attack instructions.

## Rubric mapping

| Rubric item | Evidence |
|---|---|
| GitHub repository | This repository structure + `.github/workflows/ci.yml` |
| Secure input validation / SQLi prevention | `Services/InputValidator.cs`, `Data/UserRepository.cs`, `Tests/SqlInjectionTests.cs` |
| Authentication + RBAC | `Services/AuthenticationService.cs`, `Controllers/AccountController.cs`, `Controllers/AdminController.cs`, `Tests/AuthorizationTests.cs` |
| Fix SQLi + XSS | Parameterized SQL + Razor encoding, `Tests/SqlInjectionTests.cs`, `Tests/XssTests.cs` |
| Security tests | `Tests/` project |
| Vulnerability summary | `SECURITY.md` |
