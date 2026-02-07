# JERP 3.0 Stripe Integration Guide

## Overview

JERP 3.0 uses **Stripe** for payment processing and subscription management. This guide covers setup, integration, and testing.

---

## Why Stripe?

- ✅ **Industry Standard:** Trusted by millions of businesses
- ✅ **PCI Compliance:** No need to handle sensitive card data
- ✅ **Subscription Management:** Built-in recurring billing
- ✅ **Global Support:** 135+ currencies, 45+ countries
- ✅ **Developer-Friendly:** Excellent API and documentation
- ✅ **Test Mode:** Full testing environment included

---

## Getting Started

### 1. Create Stripe Account

1. Go to https://stripe.com
2. Sign up for a free account
3. Complete business verification (required for live mode)
4. Get API keys from Dashboard → Developers → API keys

### 2. API Keys

You'll need three keys:

| Key Type | Purpose | Example |
|----------|---------|---------|
| **Publishable Key** | Client-side, safe to expose | `pk_test_...` |
| **Secret Key** | Server-side, keep secure | `sk_test_...` |
| **Webhook Secret** | Verify webhook events | `whsec_...` |

---

## Configuration

### appsettings.json

Add Stripe configuration:

```json
{
  "Stripe": {
    "PublishableKey": "pk_test_51H...",
    "SecretKey": "sk_test_51H...",
    "WebhookSecret": "whsec_...",
    "EnablePayments": true
  }
}
```

### Environment Variables (Production)

**NEVER commit secret keys to source control!**

Use environment variables:

```bash
export STRIPE_SECRET_KEY="sk_live_..."
export STRIPE_WEBHOOK_SECRET="whsec_..."
```

Or in Azure/AWS:
```
STRIPE__SECRETKEY=sk_live_...
STRIPE__WEBHOOKSECRET=whsec_...
```

---

## Setup Products & Prices in Stripe

### 1. Create Products

In Stripe Dashboard → Products, create products for each plan:

| Product Name | Description |
|--------------|-------------|
| JERP Starter | Up to 3 employees - Basic accounting |
| JERP Small Business | Up to 10 employees - Core accounting & payroll |
| JERP Professional | Up to 50 employees - Advanced features |
| JERP Enterprise | Up to 150 employees - Multi-company |
| JERP Ultimate | Unlimited employees - Custom development |

### 2. Create Prices

For each product, create **two prices**:

**Monthly Price:**
- Billing period: Monthly
- Price: $79, $149, $299, $599, or $999

**Annual Price:**
- Billing period: Yearly
- Price: $854, $1,609, $3,229, $6,469, or $10,789

### 3. Note Price IDs

Save the price IDs (e.g., `price_1234...`) - you'll need them for checkout.

---

## Implementation

### 1. Install Stripe SDK

Add to `JERP.Application.csproj`:

```xml
<PackageReference Include="Stripe.net" Version="43.0.0" />
```

### 2. Register Stripe Services

In `Program.cs` or `Startup.cs`:

```csharp
using Stripe;

// Configure Stripe
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

// Register subscription service
builder.Services.AddScoped<ISubscriptionService, StripeSubscriptionService>();
```

### 3. Implement StripeSubscriptionService

See full implementation in `JERP.Application/Services/Licensing/StripeSubscriptionService.cs`

Key methods:
- `CreateCheckoutSessionAsync()` - Start subscription flow
- `HandleWebhookAsync()` - Process payment events
- `UpgradePlanAsync()` - Change subscription
- `CancelSubscriptionAsync()` - End subscription

---

## Checkout Flow

### 1. User Selects Plan

User chooses a subscription plan (e.g., Professional, Annual).

### 2. Create Checkout Session

```csharp
var checkoutUrl = await _subscriptionService.CreateCheckoutSessionAsync(
    plan: SubscriptionPlan.Professional,
    isAnnual: true
);

// Redirect user to Stripe Checkout
Process.Start(new ProcessStartInfo
{
    FileName = checkoutUrl,
    UseShellExecute = true
});
```

### 3. Stripe Checkout Page

Stripe handles:
- Card information collection
- Payment processing
- PCI compliance
- Error handling

### 4. Redirect Back

After payment:
- **Success:** User redirected to `https://app.jerp3.com/subscription-success`
- **Cancel:** User redirected to `https://app.jerp3.com/subscription-cancel`

### 5. Webhook Notification

Stripe sends webhook to your server:
- Event: `checkout.session.completed`
- Contains: Customer ID, Subscription ID, Payment status

### 6. Activate License

Your webhook handler:
1. Verifies webhook signature
2. Creates/updates license in database
3. Sends confirmation email to customer

---

## Webhook Setup

### 1. Create Webhook Endpoint

In `LicenseController.cs`:

```csharp
[HttpPost("webhook")]
[AllowAnonymous]
public async Task<IActionResult> StripeWebhook()
{
    var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
    var signature = Request.Headers["Stripe-Signature"];

    try
    {
        var stripeEvent = EventUtility.ConstructEvent(
            json,
            signature,
            _configuration["Stripe:WebhookSecret"]
        );

        await _subscriptionService.HandleWebhookAsync(stripeEvent);
        
        return Ok();
    }
    catch (StripeException ex)
    {
        _logger.LogError(ex, "Webhook signature verification failed");
        return BadRequest();
    }
}
```

### 2. Register Webhook in Stripe

1. Go to Stripe Dashboard → Developers → Webhooks
2. Click "Add endpoint"
3. Enter URL: `https://api.jerp3.com/api/license/webhook`
4. Select events to listen for:
   - `checkout.session.completed`
   - `customer.subscription.created`
   - `customer.subscription.updated`
   - `customer.subscription.deleted`
   - `invoice.payment_succeeded`
   - `invoice.payment_failed`

5. Save webhook secret for configuration

---

## Webhook Events

### checkout.session.completed
**Triggers when:** User completes checkout

**Action:**
1. Retrieve customer email and subscription details
2. Create new License record
3. Set plan and expiration date
4. Enable features for plan
5. Send welcome email

### customer.subscription.created
**Triggers when:** New subscription starts

**Action:**
1. Update license status to Active
2. Record subscription start in SubscriptionHistory

### customer.subscription.updated
**Triggers when:** Subscription changes (upgrade/downgrade)

**Action:**
1. Update license plan
2. Update max employees/companies
3. Update enabled features
4. Record change in SubscriptionHistory

### customer.subscription.deleted
**Triggers when:** Subscription cancelled

**Action:**
1. Set license status to Expired
2. Keep access until end of billing period
3. Send cancellation email

### invoice.payment_succeeded
**Triggers when:** Renewal payment succeeds

**Action:**
1. Extend license expiration date
2. Send receipt email
3. Record payment in history

### invoice.payment_failed
**Triggers when:** Renewal payment fails

**Action:**
1. Set license status to Suspended
2. Send payment failure email
3. Retry payment (Stripe handles this)
4. After 3 failures, cancel subscription

---

## Testing

### Test Mode

Use Stripe test keys for development:
- `pk_test_...` (publishable)
- `sk_test_...` (secret)

### Test Cards

Stripe provides test cards:

| Card Number | Result | Notes |
|------------|--------|-------|
| `4242 4242 4242 4242` | Success | Any future date, any CVC |
| `4000 0000 0000 0002` | Declined | Card declined |
| `4000 0000 0000 9995` | Insufficient funds | |
| `4000 0000 0000 0069` | Expired card | |

### Test Webhooks

#### Local Testing with Stripe CLI

1. Install Stripe CLI:
```bash
# macOS
brew install stripe/stripe-cli/stripe

# Windows
scoop install stripe
```

2. Login:
```bash
stripe login
```

3. Forward webhooks to local:
```bash
stripe listen --forward-to localhost:5000/api/license/webhook
```

4. Trigger test events:
```bash
stripe trigger checkout.session.completed
stripe trigger customer.subscription.created
stripe trigger invoice.payment_succeeded
```

#### Test in Stripe Dashboard

1. Go to Webhooks → Your endpoint
2. Click "Send test webhook"
3. Select event type
4. View response and logs

---

## Security Best Practices

### 1. Verify Webhook Signatures

**ALWAYS verify webhook signatures:**

```csharp
var stripeEvent = EventUtility.ConstructEvent(
    json,
    signature,
    webhookSecret
);
```

This prevents attackers from sending fake events.

### 2. Use HTTPS

Stripe requires HTTPS for production webhooks.

### 3. Don't Expose Secret Keys

- Never commit to source control
- Use environment variables
- Rotate keys if compromised

### 4. Idempotency

Stripe may send duplicate webhooks. Use idempotency keys:

```csharp
if (await _db.WebhookEvents.AnyAsync(e => e.StripeEventId == stripeEvent.Id))
{
    return; // Already processed
}

// Process event
await ProcessEventAsync(stripeEvent);

// Mark as processed
await _db.WebhookEvents.AddAsync(new WebhookEvent
{
    StripeEventId = stripeEvent.Id,
    ProcessedAt = DateTime.UtcNow
});
await _db.SaveChangesAsync();
```

---

## Price Updates

### Changing Prices

To update prices:

1. **Create new price** in Stripe Dashboard
2. **Update price IDs** in configuration
3. **Don't delete old prices** (existing customers still use them)

### Grandfathering

Existing customers keep their old pricing until they upgrade/downgrade.

---

## Subscription Management

### Upgrading

```csharp
public async Task<bool> UpgradePlanAsync(SubscriptionPlan newPlan)
{
    var license = await GetCurrentLicenseAsync();
    var subscription = await _stripeClient.Subscriptions.GetAsync(
        license.StripeSubscriptionId
    );

    // Get new price ID
    var newPriceId = GetStripePriceId(newPlan, license.IsAnnualBilling);

    // Update subscription
    var options = new SubscriptionUpdateOptions
    {
        Items = new List<SubscriptionItemOptions>
        {
            new SubscriptionItemOptions
            {
                Id = subscription.Items.Data[0].Id,
                Price = newPriceId,
            },
        },
        ProrationBehavior = "always_invoice", // Charge prorated amount now
    };

    await _stripeClient.Subscriptions.UpdateAsync(subscription.Id, options);
    
    // Update license in database
    license.Plan = newPlan;
    await _db.SaveChangesAsync();
    
    return true;
}
```

### Downgrading

```csharp
public async Task<bool> DowngradePlanAsync(SubscriptionPlan newPlan)
{
    // Similar to upgrade, but:
    // ProrationBehavior = "none" - Apply at next billing cycle
    
    var options = new SubscriptionUpdateOptions
    {
        Items = new List<SubscriptionItemOptions> { /* ... */ },
        ProrationBehavior = "none", // Wait until next billing
    };
    
    // Schedule downgrade
    await _stripeClient.Subscriptions.UpdateAsync(subscription.Id, options);
    
    return true;
}
```

### Cancelling

```csharp
public async Task<bool> CancelSubscriptionAsync()
{
    var license = await GetCurrentLicenseAsync();
    
    // Cancel at period end (don't refund current period)
    var options = new SubscriptionCancelOptions
    {
        CancelAtPeriodEnd = true,
    };
    
    await _stripeClient.Subscriptions.CancelAsync(
        license.StripeSubscriptionId,
        options
    );
    
    return true;
}
```

---

## Monitoring

### Stripe Dashboard

Monitor:
- Successful/failed payments
- Active subscriptions
- Churn rate
- Revenue (MRR/ARR)
- Customer lifetime value

### Custom Analytics

Track in your database:
```sql
-- Monthly Recurring Revenue
SELECT 
    SUM(CASE 
        WHEN l.Plan = 1 AND NOT l.IsAnnualBilling THEN 79
        WHEN l.Plan = 2 AND NOT l.IsAnnualBilling THEN 149
        WHEN l.Plan = 3 AND NOT l.IsAnnualBilling THEN 299
        WHEN l.Plan = 4 AND NOT l.IsAnnualBilling THEN 599
        WHEN l.Plan = 5 AND NOT l.IsAnnualBilling THEN 999
        WHEN l.IsAnnualBilling THEN AnnualPrice / 12
        ELSE 0
    END) as MRR
FROM Licenses l
WHERE l.Status = 0; -- Active

-- Churn Rate
SELECT 
    COUNT(CASE WHEN Status = 2 THEN 1 END) * 100.0 / COUNT(*) as ChurnRate
FROM Licenses
WHERE MONTH(CreatedAt) = MONTH(GETDATE());
```

---

## Troubleshooting

### Payment Failed
**Problem:** Customer's card declined

**Solution:**
1. Stripe automatically retries failed payments
2. Email customer about payment failure
3. After 3 failures, subscription cancelled

### Webhook Not Received
**Problem:** Payment succeeded but license not activated

**Solutions:**
1. Check webhook endpoint is accessible (HTTPS)
2. Verify webhook secret is correct
3. Check webhook logs in Stripe Dashboard
4. Manually trigger test webhook

### Proration Issues
**Problem:** Customer charged wrong amount on upgrade

**Solutions:**
1. Check proration behavior setting
2. Verify price IDs are correct
3. Use Stripe's proration preview before upgrading

---

## Going Live

### Pre-Launch Checklist

- [ ] Activate Stripe account (complete verification)
- [ ] Switch from test keys to live keys
- [ ] Update webhook endpoint URL to production
- [ ] Test full checkout flow with real card
- [ ] Set up webhook monitoring/alerts
- [ ] Configure Stripe Radar (fraud prevention)
- [ ] Enable 3D Secure for added security
- [ ] Set up email notifications for payment events

### Live Mode Considerations

1. **Test thoroughly** in test mode first
2. **Start with small amount** of customers
3. **Monitor webhooks** closely for first week
4. **Have rollback plan** if issues arise

---

## Resources

### Stripe Documentation
- API Reference: https://stripe.com/docs/api
- Webhooks Guide: https://stripe.com/docs/webhooks
- Testing: https://stripe.com/docs/testing
- Subscriptions: https://stripe.com/docs/billing/subscriptions/overview

### Stripe.NET Library
- GitHub: https://github.com/stripe/stripe-dotnet
- NuGet: https://www.nuget.org/packages/Stripe.net/

---

## Support

For payment integration issues:

- **Stripe Support:** https://support.stripe.com
- **JERP Developer:** ichbincesartobar@yahoo.com
- **Support Email:** support@jerp3.com

---

© 2026 Julio Cesar Mendez Tobar Jr. All Rights Reserved.
