# Security Policy

## Reporting a Vulnerability

**DO NOT** create a public GitHub issue for security vulnerabilities.

Instead, please report security issues via email to: **ichbincesartobar@yahoo.com**

Include:
- Description of the vulnerability
- Steps to reproduce
- Potential impact
- Suggested fix (if any)

We will respond within 48 hours and work with you to address the issue.

## Security Response Process

1. **Acknowledgment** - We confirm receipt within 48 hours
2. **Investigation** - We assess severity and impact (1-7 days)
3. **Fix Development** - We develop and test a fix (1-14 days)
4. **Release** - Security patch deployed to production
5. **Disclosure** - Public disclosure after customers are protected (30 days)

## Supported Versions

| Version | Supported          |
| ------- | ------------------ |
| 3.x     | ✅ Yes             |
| 2.x     | ❌ No longer supported |
| 1.x     | ❌ No longer supported |

## Security Best Practices

For users:
- Use strong passwords (12+ characters)
- Enable two-factor authentication (when available)
- Keep your system updated
- Don't share credentials
- Report suspicious activity

For self-hosted deployments:
- Keep all dependencies updated
- Use HTTPS only
- Configure firewall rules
- Regular database backups
- Monitor access logs
- Use environment variables for secrets

## Known Security Features

✅ BCrypt password hashing (cost factor 11)  
✅ JWT token authentication  
✅ HTTPS/TLS encryption in transit  
✅ SQL injection protection (parameterized queries)  
✅ CSRF protection  
✅ XSS protection  
✅ Rate limiting  
✅ Audit logging with hash chains (tamper-proof)  

## Bug Bounty

We do not currently have a formal bug bounty program, but we appreciate responsible disclosure and will acknowledge security researchers in our release notes (with permission).

## Contact

- **Security issues:** ichbincesartobar@yahoo.com
- **General support:** ichbincesartobar@yahoo.com
- **Alternative contact:** ichbincesartobar@gmail.com
- **Urgent issues:** +1 (555) JERP-360

---

*Last updated: February 4, 2026*
