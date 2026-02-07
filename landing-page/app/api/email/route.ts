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

// Simple email capture endpoint
// In production, integrate with a real email service like SendGrid or Resend
export async function POST(request: Request) {
  try {
    const { email } = await request.json();

    if (!email || !email.includes('@')) {
      return NextResponse.json(
        { error: 'Invalid email address' },
        { status: 400 }
      );
    }

    // TODO: Integrate with email service
    // Example with SendGrid:
    // const sgMail = require('@sendgrid/mail');
    // sgMail.setApiKey(process.env.SENDGRID_API_KEY);
    // await sgMail.send({
    //   to: email,
    //   from: 'noreply@jerp.com',
    //   subject: 'Welcome to JERP!',
    //   text: 'Thanks for subscribing!',
    // });

    // For now, just log the email
    console.log('📧 Email captured:', email);

    // Save to database or mailing list
    // await saveToMailingList(email);

    return NextResponse.json({ 
      success: true,
      message: 'Email captured successfully' 
    });
  } catch (error: any) {
    console.error('Email capture error:', error);
    return NextResponse.json(
      { error: error.message },
      { status: 500 }
    );
  }
}
