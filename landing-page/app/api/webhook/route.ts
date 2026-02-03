import { NextResponse } from 'next/server';
import Stripe from 'stripe';
import { headers } from 'next/headers';

const stripe = new Stripe(process.env.STRIPE_SECRET_KEY!, {
  apiVersion: '2023-10-16',
});

const webhookSecret = process.env.STRIPE_WEBHOOK_SECRET!;

export async function POST(request: Request) {
  const body = await request.text();
  const headersList = await headers();
  const signature = headersList.get('stripe-signature')!;

  let event: Stripe.Event;

  try {
    event = stripe.webhooks.constructEvent(body, signature, webhookSecret);
  } catch (error: any) {
    console.error('Webhook signature verification failed:', error.message);
    return NextResponse.json({ error: 'Invalid signature' }, { status: 400 });
  }

  // Handle the event
  switch (event.type) {
    case 'checkout.session.completed':
      const session = event.data.object as Stripe.Checkout.Session;
      console.log('💰 Payment successful:', session.id);
      
      // TODO: Provision user account, send welcome email
      // await createUserAccount(session);
      // await sendWelcomeEmail(session.customer_email);
      break;

    case 'customer.subscription.created':
      const subscription = event.data.object as Stripe.Subscription;
      console.log('✅ Subscription created:', subscription.id);
      break;

    case 'customer.subscription.updated':
      console.log('🔄 Subscription updated');
      break;

    case 'customer.subscription.deleted':
      console.log('❌ Subscription cancelled');
      // Handle cancellation
      break;

    case 'invoice.payment_succeeded':
      console.log('💳 Payment succeeded');
      break;

    case 'invoice.payment_failed':
      console.log('⚠️ Payment failed');
      // Send dunning email
      break;

    default:
      console.log(`Unhandled event type ${event.type}`);
  }

  return NextResponse.json({ received: true });
}
