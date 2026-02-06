/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: licensing@jerp.io
 */

'use client';

import { motion } from 'framer-motion';
import { Check, Zap } from 'lucide-react';
import { useState } from 'react';

const plans = [
  {
    name: 'Gratis',
    price: 0,
    priceId: '', // No Stripe price for free tier
    description: 'Perfecto para empezar',
    features: [
      'Hasta 5 empleados',
      'Cálculo básico de impuestos',
      'Recibos PDF',
      'Soporte por email',
      'API REST básica',
    ],
    cta: 'Comenzar gratis',
    popular: false,
  },
  {
    name: 'Pro',
    price: 49,
    priceId: 'price_xxx', // Replace with actual Stripe price ID
    description: 'Para empresas en crecimiento',
    features: [
      'Hasta 50 empleados',
      'Todos los cálculos de impuestos',
      'Compliance engine completo',
      'Reportes avanzados',
      'Soporte prioritario',
      'API REST completa',
      'Webhooks',
      'Multi-base de datos',
    ],
    cta: 'Prueba 14 días gratis',
    popular: true,
  },
  {
    name: 'Enterprise',
    price: 149,
    priceId: 'price_yyy', // Replace with actual Stripe price ID
    description: 'Para organizaciones grandes',
    features: [
      'Empleados ilimitados',
      'Todo de Pro +',
      'SLA garantizado',
      'Account manager dedicado',
      'Onboarding personalizado',
      'Integraciones custom',
      'Auditoría y compliance avanzado',
      'Soporte 24/7',
    ],
    cta: 'Contactar ventas',
    popular: false,
  },
];

export default function Pricing() {
  const [billingCycle, setBillingCycle] = useState<'monthly' | 'yearly'>('monthly');
  const [loading, setLoading] = useState<string | null>(null);

  const handleSubscribe = async (priceId: string, planName: string) => {
    if (!priceId) {
      // Free plan - redirect to signup
      window.location.href = '/signup?plan=free';
      return;
    }

    if (planName === 'Enterprise') {
      // Enterprise - redirect to contact
      window.location.href = '/contact?plan=enterprise';
      return;
    }

    setLoading(priceId);

    try {
      const response = await fetch('/api/subscribe', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ priceId, billingCycle }),
      });

      const { sessionId } = await response.json();

      // Redirect to Stripe Checkout
      const stripe = await import('@stripe/stripe-js').then(m => m.loadStripe(process.env.NEXT_PUBLIC_STRIPE_PUBLISHABLE_KEY!));
      if (stripe) {
        await stripe.redirectToCheckout({ sessionId });
      }
    } catch (error) {
      console.error('Subscription error:', error);
      alert('Error al procesar el pago. Intenta nuevamente.');
    } finally {
      setLoading(null);
    }
  };

  return (
    <section id="pricing" className="py-24 px-4 relative">
      <div className="max-w-7xl mx-auto">
        {/* Section header */}
        <div className="text-center mb-16">
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            whileInView={{ opacity: 1, y: 0 }}
            viewport={{ once: true }}
            className="inline-block text-sm font-semibold text-red-500 mb-4 tracking-wider uppercase"
          >
            Precios
          </motion.div>
          <motion.h2
            initial={{ opacity: 0, y: 20 }}
            whileInView={{ opacity: 1, y: 0 }}
            viewport={{ once: true }}
            className="text-4xl md:text-5xl font-bold text-white mb-4"
          >
            Planes para cada etapa
            <br />
            de tu empresa
          </motion.h2>
          <motion.p
            initial={{ opacity: 0, y: 20 }}
            whileInView={{ opacity: 1, y: 0 }}
            viewport={{ once: true }}
            transition={{ delay: 0.1 }}
            className="text-xl text-slate-400 max-w-2xl mx-auto mb-8"
          >
            Comienza gratis. Escala cuando crezcas. Sin contratos ni compromiso.
          </motion.p>

          {/* Billing toggle */}
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            whileInView={{ opacity: 1, y: 0 }}
            viewport={{ once: true }}
            transition={{ delay: 0.2 }}
            className="inline-flex items-center gap-4 bg-white/5 backdrop-blur-sm border border-white/10 rounded-full p-2"
          >
            <button
              onClick={() => setBillingCycle('monthly')}
              className={`px-6 py-2 rounded-full transition-all ${
                billingCycle === 'monthly'
                  ? 'bg-gradient-to-r from-red-500 to-orange-500 text-white'
                  : 'text-slate-400 hover:text-white'
              }`}
            >
              Mensual
            </button>
            <button
              onClick={() => setBillingCycle('yearly')}
              className={`px-6 py-2 rounded-full transition-all flex items-center gap-2 ${
                billingCycle === 'yearly'
                  ? 'bg-gradient-to-r from-red-500 to-orange-500 text-white'
                  : 'text-slate-400 hover:text-white'
              }`}
            >
              Anual
              <span className="text-xs bg-green-500 text-white px-2 py-1 rounded-full">Ahorra 20%</span>
            </button>
          </motion.div>
        </div>

        {/* Pricing cards */}
        <div className="grid md:grid-cols-3 gap-8 max-w-6xl mx-auto">
          {plans.map((plan, i) => (
            <motion.div
              key={i}
              initial={{ opacity: 0, y: 20 }}
              whileInView={{ opacity: 1, y: 0 }}
              viewport={{ once: true }}
              transition={{ delay: i * 0.1 }}
              className={`relative bg-white/5 backdrop-blur-sm border rounded-2xl p-8 ${
                plan.popular
                  ? 'border-red-500 shadow-2xl shadow-red-500/20 scale-105'
                  : 'border-white/10'
              }`}
            >
              {plan.popular && (
                <div className="absolute -top-4 left-1/2 -translate-x-1/2 bg-gradient-to-r from-red-500 to-orange-500 text-white px-4 py-1 rounded-full text-sm font-semibold flex items-center gap-1">
                  <Zap className="w-4 h-4" />
                  Más popular
                </div>
              )}

              <div className="text-center mb-8">
                <h3 className="text-2xl font-bold text-white mb-2">{plan.name}</h3>
                <p className="text-slate-400 mb-6">{plan.description}</p>
                <div className="flex items-baseline justify-center gap-2 mb-2">
                  <span className="text-5xl font-bold text-white">
                    ${billingCycle === 'yearly' ? Math.round(plan.price * 0.8) : plan.price}
                  </span>
                  <span className="text-slate-400">
                    /{billingCycle === 'yearly' ? 'año' : 'mes'}
                  </span>
                </div>
                {billingCycle === 'yearly' && plan.price > 0 && (
                  <p className="text-sm text-green-500">
                    Ahorras ${Math.round(plan.price * 12 * 0.2)} al año
                  </p>
                )}
              </div>

              <ul className="space-y-4 mb-8">
                {plan.features.map((feature, j) => (
                  <li key={j} className="flex items-start gap-3">
                    <Check className="w-5 h-5 text-green-500 flex-shrink-0 mt-0.5" />
                    <span className="text-slate-300">{feature}</span>
                  </li>
                ))}
              </ul>

              <button
                onClick={() => handleSubscribe(plan.priceId, plan.name)}
                disabled={loading === plan.priceId}
                className={`w-full py-4 rounded-lg font-semibold transition-all ${
                  plan.popular
                    ? 'bg-gradient-to-r from-red-500 to-orange-500 text-white hover:shadow-2xl hover:shadow-red-500/50'
                    : 'bg-white/10 text-white border border-white/20 hover:bg-white/20'
                }`}
              >
                {loading === plan.priceId ? 'Procesando...' : plan.cta}
              </button>
            </motion.div>
          ))}
        </div>
      </div>
    </section>
  );
}
