/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: licensing@jerp.io
 */

'use client';

import { motion } from 'framer-motion';
import { Star, Quote } from 'lucide-react';

const testimonials = [
  {
    name: 'María González',
    role: 'CFO, TechStart Inc.',
    image: 'https://via.placeholder.com/100',
    rating: 5,
    text: 'JERP ha transformado completamente nuestra gestión de nómina. Lo que antes tomaba días ahora toma minutos. El compliance automático nos ha ahorrado miles en multas potenciales.',
  },
  {
    name: 'Carlos Ramírez',
    role: 'Founder, Digital Labs',
    image: 'https://via.placeholder.com/100',
    rating: 5,
    text: 'La integración API es excepcional. Conectamos JERP con nuestro ERP en menos de una semana. El soporte técnico es de primera clase.',
  },
  {
    name: 'Ana Martínez',
    role: 'HR Manager, Retail Plus',
    image: 'https://via.placeholder.com/100',
    rating: 5,
    text: 'Gestionar 200+ empleados nunca fue tan fácil. Los reportes automatizados y el cálculo de impuestos son increíblemente precisos. Altamente recomendado.',
  },
];

const companies = [
  { name: 'TechStart', logo: '🚀' },
  { name: 'Digital Labs', logo: '💻' },
  { name: 'Retail Plus', logo: '🏪' },
  { name: 'CloudCorp', logo: '☁️' },
  { name: 'Innovate', logo: '💡' },
  { name: 'GrowthCo', logo: '📈' },
];

export default function SocialProof() {
  return (
    <section className="py-24 px-4 relative">
      <div className="max-w-7xl mx-auto">
        {/* Companies section */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          whileInView={{ opacity: 1, y: 0 }}
          viewport={{ once: true }}
          className="text-center mb-20"
        >
          <p className="text-slate-400 mb-8">Empresas que confían en JERP</p>
          <div className="flex flex-wrap justify-center items-center gap-8 md:gap-12">
            {companies.map((company, i) => (
              <motion.div
                key={i}
                initial={{ opacity: 0, scale: 0.9 }}
                whileInView={{ opacity: 1, scale: 1 }}
                viewport={{ once: true }}
                transition={{ delay: i * 0.1 }}
                className="flex items-center gap-3 bg-white/5 backdrop-blur-sm border border-white/10 rounded-lg px-6 py-3 hover:bg-white/10 transition-all"
              >
                <span className="text-3xl">{company.logo}</span>
                <span className="text-white font-semibold">{company.name}</span>
              </motion.div>
            ))}
          </div>
        </motion.div>

        {/* Testimonials section */}
        <div className="text-center mb-16">
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            whileInView={{ opacity: 1, y: 0 }}
            viewport={{ once: true }}
            className="inline-block text-sm font-semibold text-red-500 mb-4 tracking-wider uppercase"
          >
            Testimonios
          </motion.div>
          <motion.h2
            initial={{ opacity: 0, y: 20 }}
            whileInView={{ opacity: 1, y: 0 }}
            viewport={{ once: true }}
            className="text-4xl md:text-5xl font-bold text-white mb-4"
          >
            Lo que dicen nuestros clientes
          </motion.h2>
        </div>

        <div className="grid md:grid-cols-3 gap-8">
          {testimonials.map((testimonial, i) => (
            <motion.div
              key={i}
              initial={{ opacity: 0, y: 20 }}
              whileInView={{ opacity: 1, y: 0 }}
              viewport={{ once: true }}
              transition={{ delay: i * 0.1 }}
              className="bg-white/5 backdrop-blur-sm border border-white/10 rounded-xl p-8 hover:bg-white/10 transition-all"
            >
              <Quote className="w-10 h-10 text-red-500 mb-4 opacity-50" />
              
              <div className="flex gap-1 mb-4">
                {[...Array(testimonial.rating)].map((_, j) => (
                  <Star key={j} className="w-5 h-5 fill-yellow-500 text-yellow-500" />
                ))}
              </div>

              <p className="text-slate-300 mb-6 leading-relaxed">
                &ldquo;{testimonial.text}&rdquo;
              </p>

              <div className="flex items-center gap-4">
                <div className="w-12 h-12 rounded-full bg-gradient-to-br from-red-500 to-orange-500 flex items-center justify-center text-white font-bold text-lg">
                  {testimonial.name.charAt(0)}
                </div>
                <div>
                  <div className="text-white font-semibold">{testimonial.name}</div>
                  <div className="text-slate-400 text-sm">{testimonial.role}</div>
                </div>
              </div>
            </motion.div>
          ))}
        </div>

        {/* Stats bar */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          whileInView={{ opacity: 1, y: 0 }}
          viewport={{ once: true }}
          className="mt-16 grid grid-cols-2 md:grid-cols-4 gap-8 bg-white/5 backdrop-blur-sm border border-white/10 rounded-xl p-8"
        >
          {[
            { value: '99.9%', label: 'Uptime' },
            { value: '< 2h', label: 'Tiempo de respuesta' },
            { value: '4.9/5', label: 'Rating promedio' },
            { value: '500+', label: 'Clientes activos' },
          ].map((stat, i) => (
            <div key={i} className="text-center">
              <div className="text-3xl md:text-4xl font-bold text-white mb-2">{stat.value}</div>
              <div className="text-slate-400">{stat.label}</div>
            </div>
          ))}
        </motion.div>
      </div>
    </section>
  );
}
