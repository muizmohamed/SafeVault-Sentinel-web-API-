# SafeVault Security Test Plan

| Test | Input / condition | Expected result |
|---|---|---|
| Username allow-list | `alice_123` | accepted |
| SQLi input validation | `admin' OR '1'='1` | rejected as username |
| SQL parameterization | same SQLi string in query parameter | treated as literal data; no injected match |
| XSS script | `<script>alert('xss')</script>` | encoded, not executable |
| XSS event | `<img src=x onerror=alert(1)>` | encoded, not executable |
| Password hashing | correct password vs BCrypt hash | verification succeeds |
| Wrong password | incorrect password | verification fails |
| RBAC admin | role `Admin` | authorized |
| RBAC normal user | role `User` | denied |
