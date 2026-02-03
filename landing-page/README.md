# JERP 3.0 - SaaS Landing Page

A complete, conversion-optimized SaaS landing page for JERP 3.0 Payroll Module with Stripe payment integration, email capture, and interactive demo.

## 🚀 Features

- ✅ **Next.js 14** with App Router
- ✅ **TypeScript** for type safety
- ✅ **Tailwind CSS** for styling
- ✅ **Framer Motion** for animations
- ✅ **Stripe Integration** for payments
- ✅ **Email Capture** functionality
- ✅ **Interactive Demo** calculator
- ✅ **Responsive Design** (mobile-first)
- ✅ **SEO Optimized** with meta tags
- ✅ **14-day Free Trial** flow

## 📦 Installation

### Prerequisites

- Node.js 18+ 
- npm or yarn
- Stripe account (for payment integration)

### Setup

1. **Navigate to the landing page directory:**

```bash
cd landing-page
```

2. **Install dependencies:**

```bash
npm install
# or
yarn install
```

3. **Create environment file:**

```bash
cp .env.example .env.local
```

4. **Configure environment variables:**

Edit `.env.local` with your actual values:

```env
# Stripe Keys (get from https://dashboard.stripe.com/apikeys)
NEXT_PUBLIC_STRIPE_PUBLISHABLE_KEY=pk_test_your_key_here
STRIPE_SECRET_KEY=sk_test_your_key_here
STRIPE_WEBHOOK_SECRET=whsec_your_webhook_secret_here

# App URL
NEXT_PUBLIC_URL=http://localhost:3000

# Optional: Email Service
SENDGRID_API_KEY=your_sendgrid_key
RESEND_API_KEY=your_resend_key

# Optional: Analytics
NEXT_PUBLIC_GA_ID=G-your_ga_id
```

## 🏃‍♂️ Running the Application

### Development Mode

```bash
npm run dev
```

Open [http://localhost:3000](http://localhost:3000) to view the landing page.

### Production Build

```bash
npm run build
npm start
```

### Linting

```bash
npm run lint
```

## 💳 Stripe Configuration

### 1. Get Your Stripe Keys

1. Create a Stripe account at [stripe.com](https://stripe.com)
2. Navigate to [Dashboard → API Keys](https://dashboard.stripe.com/apikeys)
3. Copy your **Publishable key** and **Secret key**
4. Add them to your `.env.local` file

### 2. Create Products and Prices

1. Go to [Dashboard → Products](https://dashboard.stripe.com/products)
2. Create products for each plan:
   - **Pro Plan** - $49/month
   - **Enterprise Plan** - $149/month
3. Copy the **Price ID** for each product
4. Update the `priceId` values in `components/Pricing.tsx`

### 3. Set Up Webhooks

1. Install Stripe CLI: [stripe.com/docs/stripe-cli](https://stripe.com/docs/stripe-cli)
2. Login to Stripe CLI:
   ```bash
   stripe login
   ```
3. Forward webhooks to your local server:
   ```bash
   stripe listen --forward-to localhost:3000/api/webhook
   ```
4. Copy the webhook signing secret and add to `.env.local`

For production webhooks:
1. Go to [Dashboard → Webhooks](https://dashboard.stripe.com/webhooks)
2. Add endpoint: `https://yourdomain.com/api/webhook`
3. Select events to listen for:
   - `checkout.session.completed`
   - `customer.subscription.created`
   - `customer.subscription.updated`
   - `customer.subscription.deleted`
   - `invoice.payment_succeeded`
   - `invoice.payment_failed`

### 4. Test with Stripe Test Cards

Use these test cards in test mode:

- **Success:** `4242 4242 4242 4242`
- **Decline:** `4000 0000 0000 0002`
- **3D Secure:** `4000 0025 0000 3155`

Any future expiration date and any 3-digit CVC will work.

## 📧 Email Service Integration

The landing page includes an email capture component. To enable it:

### Option 1: SendGrid

```bash
npm install @sendgrid/mail
```

Update `app/api/email/route.ts`:

```typescript
import sgMail from '@sendgrid/mail';

sgMail.setApiKey(process.env.SENDGRID_API_KEY!);

await sgMail.send({
  to: email,
  from: 'noreply@yourdomain.com',
  subject: 'Welcome to JERP!',
  html: '<strong>Thanks for subscribing!</strong>',
});
```

### Option 2: Resend

```bash
npm install resend
```

Update `app/api/email/route.ts`:

```typescript
import { Resend } from 'resend';

const resend = new Resend(process.env.RESEND_API_KEY);

await resend.emails.send({
  from: 'JERP <noreply@yourdomain.com>',
  to: email,
  subject: 'Welcome to JERP!',
  html: '<strong>Thanks for subscribing!</strong>',
});
```

## 🚢 Deployment

### Vercel (Recommended)

1. Push your code to GitHub
2. Import project in [Vercel](https://vercel.com)
3. Configure environment variables in project settings
4. Deploy!

[![Deploy with Vercel](https://vercel.com/button)](https://vercel.com/new/clone?repository-url=https://github.com/yourusername/jerp-landing)

### Other Platforms

The app can be deployed to any platform that supports Next.js:

- **Netlify:** Follow [Next.js on Netlify guide](https://docs.netlify.com/integrations/frameworks/next-js/)
- **Railway:** One-click deploy with Railway
- **AWS Amplify:** Follow AWS Amplify Next.js deployment guide
- **Docker:** Use the included Dockerfile

## 📁 Project Structure

```
landing-page/
├── app/
│   ├── layout.tsx          # Root layout with metadata
│   ├── page.tsx            # Home page
│   ├── api/
│   │   ├── subscribe/      # Stripe checkout API
│   │   ├── email/          # Email capture API
│   │   └── webhook/        # Stripe webhook handler
│   ├── pricing/            # Pricing page (optional)
│   └── demo/               # Demo page (optional)
├── components/
│   ├── Hero.tsx            # Hero section with CTA
│   ├── Features.tsx        # Features grid
│   ├── Pricing.tsx         # Pricing plans with Stripe
│   ├── Demo.tsx            # Interactive calculator
│   ├── FAQ.tsx             # FAQ accordion
│   ├── EmailCapture.tsx    # Email subscription form
│   ├── SocialProof.tsx     # Testimonials & logos
│   └── Footer.tsx          # Footer with links
├── lib/                    # Utility functions
├── public/                 # Static assets
├── styles/
│   └── globals.css         # Global styles
├── .env.example            # Environment variables template
├── .gitignore              # Git ignore rules
├── next.config.js          # Next.js configuration
├── tailwind.config.ts      # Tailwind CSS configuration
├── tsconfig.json           # TypeScript configuration
└── package.json            # Dependencies
```

## 🎨 Customization

### Colors

Edit `tailwind.config.ts` to customize the color scheme:

```typescript
colors: {
  brand: {
    red: '#e8533f',      // Primary brand color
    orange: '#f0a500',   // Secondary brand color
    dark: '#0f1923',     // Dark background
  },
}
```

### Content

- **Hero Section:** Edit `components/Hero.tsx`
- **Features:** Update the `features` array in `components/Features.tsx`
- **Pricing Plans:** Modify the `plans` array in `components/Pricing.tsx`
- **Testimonials:** Update `components/SocialProof.tsx`
- **FAQ:** Edit the `faqs` array in `components/FAQ.tsx`

### Metadata & SEO

Update metadata in `app/layout.tsx`:

```typescript
export const metadata: Metadata = {
  title: 'Your Title',
  description: 'Your Description',
  keywords: 'your, keywords',
  // ... more metadata
};
```

## 📊 Analytics Integration

### Google Analytics

1. Get your GA4 Measurement ID
2. Add to `.env.local`:
   ```
   NEXT_PUBLIC_GA_ID=G-XXXXXXXXXX
   ```
3. Add to `app/layout.tsx`:

```typescript
import Script from 'next/script';

// In layout component
<Script
  src={`https://www.googletagmanager.com/gtag/js?id=${process.env.NEXT_PUBLIC_GA_ID}`}
  strategy="afterInteractive"
/>
<Script id="google-analytics" strategy="afterInteractive">
  {`
    window.dataLayer = window.dataLayer || [];
    function gtag(){dataLayer.push(arguments);}
    gtag('js', new Date());
    gtag('config', '${process.env.NEXT_PUBLIC_GA_ID}');
  `}
</Script>
```

## 🧪 Testing

### Manual Testing Checklist

- [ ] Hero section displays correctly
- [ ] All animations work smoothly
- [ ] Pricing toggle (monthly/yearly) works
- [ ] Stripe checkout redirects properly
- [ ] Email capture form validates input
- [ ] Interactive demo calculator works
- [ ] FAQ accordion expands/collapses
- [ ] All links work correctly
- [ ] Responsive on mobile devices
- [ ] Page loads in < 3 seconds

### Test Stripe Integration

1. Use test card: `4242 4242 4242 4242`
2. Enter any future expiration date
3. Enter any 3-digit CVC
4. Complete checkout
5. Verify webhook receives events

## 🐛 Troubleshooting

### Stripe Errors

**Error:** "No such price"
- **Solution:** Make sure you've created products in Stripe Dashboard and updated the `priceId` in `Pricing.tsx`

**Error:** "Invalid webhook signature"
- **Solution:** Check that your `STRIPE_WEBHOOK_SECRET` matches the webhook endpoint

### Build Errors

**Error:** Module not found
- **Solution:** Run `npm install` to ensure all dependencies are installed

**Error:** TypeScript errors
- **Solution:** Run `npm run build` to see detailed type errors

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🤝 Support

- **Email:** support@jerp.com
- **Documentation:** [docs.jerp.com](https://docs.jerp.com)
- **Community:** [Discord](https://discord.gg/jerp)

## 🎯 Next Steps

1. [ ] Customize colors and branding
2. [ ] Add your Stripe keys
3. [ ] Create Stripe products and prices
4. [ ] Set up email service
5. [ ] Configure analytics
6. [ ] Test the complete flow
7. [ ] Deploy to production
8. [ ] Set up production webhooks
9. [ ] Add custom domain
10. [ ] Launch! 🚀

---

Made with ❤️ for PYMEs
