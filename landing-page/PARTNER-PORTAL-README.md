# JERP Partner Portal

Complete partner/reseller portal with referral tracking and commission management.

## Features

### Partner Management
- Partner application and approval workflow
- Tiered commission structure (Bronze 25%, Silver 30%, Gold 35%)
- Automatic tier upgrades based on active customer count
- Referral code generation and tracking

### Dashboard
- Real-time performance metrics
- Active customer tracking
- Earnings overview (total, pending, paid)
- Recent referrals list
- Tier progression tracking

### Referral System
- Cookie-based attribution (30-day window)
- UTM parameter tracking
- Click analytics
- Referral status tracking (Lead → Trial → Active → Cancelled)

### Commission System
- Signup bonuses ($75-$125 based on tier)
- Recurring monthly commissions (25-35% of MRR)
- Automatic commission calculation
- Commission status workflow (Pending → Approved → Paid)
- Tier bonus support

### Portal Pages
1. **Public Program Page** (`/partners`) - Partner program landing page
2. **Signup** (`/partners/signup`) - Partner application form
3. **Login** (`/partners/login`) - Partner authentication
4. **Dashboard** (`/partners/dashboard`) - Overview and metrics
5. **Referrals** (`/partners/dashboard/referrals`) - Referral management
6. **Commissions** (`/partners/dashboard/commissions`) - Commission tracking with CSV export
7. **Links** (`/partners/dashboard/links`) - Referral link generator with QR codes
8. **Resources** (`/partners/dashboard/resources`) - Marketing materials
9. **Academy** (`/partners/dashboard/academy`) - Training and FAQs
10. **Settings** (`/partners/dashboard/settings`) - Account management

## Setup

### 1. Environment Variables

Add to your `.env` file:

```bash
# Database
DATABASE_URL=postgresql://user:password@localhost:5432/jerp_partners

# NextAuth
NEXTAUTH_URL=http://localhost:3000
NEXTAUTH_SECRET=your-secret-key-here-min-32-chars

# Stripe (existing)
STRIPE_SECRET_KEY=sk_test_xxx
STRIPE_WEBHOOK_SECRET=whsec_xxx

# Email (optional)
RESEND_API_KEY=re_xxx
```

### 2. Database Migration

```bash
cd landing-page
npx prisma migrate dev --name init
npx prisma generate
```

### 3. Install Dependencies

Already included in package.json:
- prisma / @prisma/client
- next-auth
- bcrypt
- qrcode
- zod

### 4. Run Development Server

```bash
npm run dev
```

Visit:
- Partner program: http://localhost:3000/partners
- Partner signup: http://localhost:3000/partners/signup
- Partner login: http://localhost:3000/partners/login
- Partner dashboard: http://localhost:3000/partners/dashboard

## API Endpoints

### Partner Routes

- `POST /api/partners/apply` - Submit partner application
- `POST /api/partners/approve` - Admin: Approve/reject applications
- `GET /api/partners/referrals` - List partner's referrals (authenticated)
- `GET /api/partners/commissions` - List partner's commissions (authenticated)
- `POST /api/partners/track-click` - Track referral link clicks
- `POST /api/partners/payouts/request` - Request payout

### Authentication

- `POST /api/auth/[...nextauth]` - NextAuth authentication

### Webhook Integration

Stripe webhook at `/api/webhook` now includes:
- Referral tracking on subscription creation
- Commission calculation on invoice payment
- Referral status updates on cancellation

## Commission Structure

### Tiers

| Tier | Active Customers | Commission Rate | Signup Bonus |
|------|-----------------|-----------------|--------------|
| 🥉 Bronze | 0-5 | 25% | $75 |
| 🥈 Silver | 6-15 | 30% | $100 |
| 🥇 Gold | 16+ | 35% | $125 |

### Pricing Reference

| Plan | Base | Per Employee | Example (20 emp) |
|------|------|-------------|------------------|
| Free | $0 | $0 | $0 |
| Starter | $19 | $2.50 | $69 |
| Pro | $39 | $3.50 | $109 |
| Enterprise | $99 | $4.50 | $189 |

### Example Earnings

Silver partner with 10 Pro plan customers ($109/mo each):
- Monthly recurring: $109 × 30% × 10 = $327/mo
- Annual recurring: $3,924/year
- Plus signup bonuses: $100 × 10 = $1,000 one-time

## Referral Tracking Flow

1. Partner shares referral link with unique code
2. Customer clicks link → Cookie set (30 days)
3. Customer signs up for trial → Referral record created (TRIAL status)
4. Customer converts to paid → Referral updated (ACTIVE status)
   - Signup bonus commission created
5. Monthly payments → Recurring commission created
6. Customer cancels → Referral updated (CANCELLED status)

## Database Schema

See `prisma/schema.prisma` for complete schema including:
- Partner (user accounts)
- Referral (tracked customers)
- Commission (earnings records)
- Payout (payment batches)
- ReferralClick (analytics)

## Security

- Password hashing with bcrypt (10 rounds)
- JWT-based sessions via NextAuth
- Protected routes with middleware
- HTTPS-only cookies in production
- Input validation with Zod

## Future Enhancements

- [ ] Admin panel for partner management
- [ ] Email notifications (application, approval, new referral, etc.)
- [ ] Automated payout processing
- [ ] Performance charts and analytics
- [ ] Partner API access
- [ ] Webhook for partner events
- [ ] Multi-language support
- [ ] Mobile app

## Support

For questions or issues:
- Email: partners@jerp.com
- Documentation: https://jerp.com/partners/docs
