/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: ichbincesartobar@yahoo.com
 */

import { NextResponse } from 'next/server';
import Stripe from 'stripe';
import { headers, cookies } from 'next/headers';
import { prisma } from '@/lib/prisma';

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
      
      // Check for referral cookie and create referral record
      try {
        const cookieStore = await cookies();
        const referralCode = cookieStore.get('jerp_ref')?.value;
        
        if (referralCode) {
          const partner = await prisma.partner.findUnique({
            where: { referralCode, status: 'ACTIVE' },
          });
          
          if (partner) {
            // Get customer info from Stripe
            const customer = await stripe.customers.retrieve(subscription.customer as string) as Stripe.Customer;
            
            // Calculate MRR from subscription
            const mrr = subscription.items.data.reduce((sum, item) => {
              return sum + (item.price.unit_amount || 0) / 100;
            }, 0);
            
            // Create referral record
            await prisma.referral.create({
              data: {
                partnerId: partner.id,
                customerEmail: customer.email || '',
                status: 'TRIAL',
                plan: subscription.items.data[0]?.price?.lookup_key || 'Pro',
                trialStartDate: new Date(),
                mrr,
              },
            });
            
            // Update partner stats
            await prisma.partner.update({
              where: { id: partner.id },
              data: {
                totalReferrals: { increment: 1 },
              },
            });
            
            console.log('✨ Referral created for partner:', partner.email);
          }
        }
      } catch (error) {
        console.error('Error creating referral:', error);
      }
      break;

    case 'customer.subscription.updated':
      console.log('🔄 Subscription updated');
      break;

    case 'customer.subscription.deleted':
      console.log('❌ Subscription cancelled');
      
      // Update referral status to cancelled
      try {
        const cancelledSub = event.data.object as Stripe.Subscription;
        const customer = await stripe.customers.retrieve(cancelledSub.customer as string) as Stripe.Customer;
        
        const referral = await prisma.referral.findFirst({
          where: {
            customerEmail: customer.email || '',
            status: { in: ['TRIAL', 'ACTIVE'] },
          },
        });
        
        if (referral) {
          await prisma.referral.update({
            where: { id: referral.id },
            data: {
              status: 'CANCELLED',
              cancellationDate: new Date(),
            },
          });
          
          // Update partner active customer count
          await prisma.partner.update({
            where: { id: referral.partnerId },
            data: {
              activeCustomers: { decrement: 1 },
            },
          });
        }
      } catch (error) {
        console.error('Error handling cancellation:', error);
      }
      break;

    case 'invoice.payment_succeeded':
      console.log('💳 Payment succeeded');
      
      // Handle first payment = conversion, create signup bonus
      try {
        const invoice = event.data.object as Stripe.Invoice;
        const customer = await stripe.customers.retrieve(invoice.customer as string) as Stripe.Customer;
        
        const referral = await prisma.referral.findFirst({
          where: {
            customerEmail: customer.email || '',
            status: { in: ['TRIAL', 'ACTIVE'] },
          },
          include: { partner: true },
        });
        
        if (referral) {
          // First payment = conversion
          if (invoice.billing_reason === 'subscription_create' && referral.status === 'TRIAL') {
            await prisma.referral.update({
              where: { id: referral.id },
              data: {
                status: 'ACTIVE',
                conversionDate: new Date(),
              },
            });
            
            // Create signup bonus commission
            await prisma.commission.create({
              data: {
                partnerId: referral.partnerId,
                referralId: referral.id,
                type: 'SIGNUP_BONUS',
                amount: referral.partner.signupBonus,
                rate: 1.0,
                status: 'APPROVED',
                periodStart: new Date(),
                periodEnd: new Date(),
              },
            });
            
            // Update partner stats
            await prisma.partner.update({
              where: { id: referral.partnerId },
              data: {
                activeCustomers: { increment: 1 },
                pendingEarnings: { increment: referral.partner.signupBonus },
                totalEarnings: { increment: referral.partner.signupBonus },
              },
            });
            
            console.log('🎉 Signup bonus created for partner:', referral.partner.email);
          }
          
          // Create recurring commission for active referral
          if (referral.status === 'ACTIVE') {
            const commissionAmount = (invoice.amount_paid / 100) * referral.partner.commissionRate;
            
            await prisma.commission.create({
              data: {
                partnerId: referral.partnerId,
                referralId: referral.id,
                type: 'RECURRING',
                amount: commissionAmount,
                rate: referral.partner.commissionRate,
                status: 'APPROVED',
                periodStart: new Date(invoice.period_start * 1000),
                periodEnd: new Date(invoice.period_end * 1000),
              },
            });
            
            // Update partner earnings
            await prisma.partner.update({
              where: { id: referral.partnerId },
              data: {
                pendingEarnings: { increment: commissionAmount },
                totalEarnings: { increment: commissionAmount },
              },
            });
            
            console.log('💰 Recurring commission created:', commissionAmount);
          }
        }
      } catch (error) {
        console.error('Error creating commission:', error);
      }
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
