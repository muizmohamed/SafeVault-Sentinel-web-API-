# SafeVault Security Review

## 1. SQL Injection

**Finding:** The initial threat model assumes a vulnerable pattern such as string concatenation:

```csharp
var sql = "SELECT * FROM Users WHERE Username = '" + username + "'";
```

**Risk:** user input could alter SQL syntax.

**Fix:** every user-controlled value is supplied as a SQL parameter:

```csharp
command.CommandText = "SELECT UserID, Username, Email, PasswordHash, Role FROM Users WHERE Username = $username LIMIT 1;";
command.Parameters.AddWithValue("$username", username);
```

## 2. Cross-Site Scripting (XSS)

**Finding:** rendering untrusted form content as raw HTML would permit markup/script execution.

**Fix:** the Razor view uses a normal Razor expression:

```cshtml
<p id="feedback-output">@Model.Feedback</p>
```

It intentionally does **not** use `Html.Raw(Model.Feedback)`.

## 3. Authentication

**Finding:** storing passwords as plaintext would make a database disclosure immediately expose credentials.

**Fix:** passwords are hashed with BCrypt before persistence and verified with `BCrypt.Verify` during login.

## 4. Authorization / RBAC

**Finding:** authentication alone does not stop a normal user from reaching an administrative endpoint.

**Fix:** the admin controller requires the `Admin` role:

```csharp
[Authorize(Roles = "Admin")]
public sealed class AdminController : Controller
```

## 5. Input validation

**Finding:** accepting arbitrary usernames creates ambiguity and increases attack surface.

**Fix:** usernames use an allow-list. Importantly, the application does not pretend that deleting “bad characters” is a replacement for parameterized SQL or output encoding.

## 6. Testing evidence

The test project verifies:

- malicious username rejection;
- parameterized SQL treats injection strings as literal data;
- XSS payloads become encoded text;
- BCrypt rejects wrong passwords;
- Admin is allowed by RBAC and User is denied.

## How AI-assisted debugging helped

AI assistance was used as a code-review aid to identify unsafe string-concatenated SQL patterns, recommend parameterized queries, flag raw HTML rendering as an XSS risk, and suggest focused regression tests. Every recommendation was translated into explicit source-code changes and automated tests rather than being accepted without verification.
