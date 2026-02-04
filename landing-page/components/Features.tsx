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
import { Calculator, Shield, FileText, Clock, Users, TrendingUp, Database, Zap } from 'lucide-react';

const features = [
  {
    icon: Calculator,
    title: 'Cálculo Automático de Impuestos',
    description: 'Tasas 2024 actualizadas: Federal, Estatal (CA), FICA, Medicare. Cálculo automático de deducciones.',
    color: 'from-blue-500 to-cyan-500',
  },
  {
    icon: Shield,
    title: 'Compliance Integrado',
    description: '7+ reglas laborales (CA & Federal FLSA). Detecta violaciones en tiempo real y protege tu empresa.',
    color: 'from-green-500 to-emerald-500',
  },
  {
    icon: FileText,
    title: 'Recibos PDF Profesionales',
    description: 'Genera recibos de pago en PDF con desglose completo. Envío automático por email.',
    color: 'from-purple-500 to-pink-500',
  },
  {
    icon: Clock,
    title: 'Gestión de Horas',
    description: 'Timesheet integrado con cálculo automático de overtime. Aprobación por workflow.',
    color: 'from-orange-500 to-red-500',
  },
  {
    icon: Users,
    title: 'Multi-Empleados',
    description: 'Gestiona ilimitados empleados. Perfiles completos, documentos, historial de pagos.',
    color: 'from-yellow-500 to-orange-500',
  },
  {
    icon: TrendingUp,
    title: 'Reportes & Analytics',
    description: 'Dashboard con métricas clave. Reportes de nómina, compliance y costos laborales.',
    color: 'from-indigo-500 to-purple-500',
  },
  {
    icon: Database,
    title: 'Multi-Base de Datos',
    description: 'Soporta PostgreSQL, MySQL y SQL Server. Cambia de proveedor sin reescribir código.',
    color: 'from-teal-500 to-green-500',
  },
  {
    icon: Zap,
    title: 'API REST Completa',
    description: 'Integra con tu stack. 50+ endpoints documentados en Swagger. Webhooks incluidos.',
    color: 'from-pink-500 to-rose-500',
  },
];

export default function Features() {
  return (
    <section id="features" className="py-24 px-4 relative">
      <div className="max-w-7xl mx-auto">
        {/* Section header */}
        <div className="text-center mb-16">
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            whileInView={{ opacity: 1, y: 0 }}
            viewport={{ once: true }}
            className="inline-block text-sm font-semibold text-red-500 mb-4 tracking-wider uppercase"
          >
            Características
          </motion.div>
          <motion.h2
            initial={{ opacity: 0, y: 20 }}
            whileInView={{ opacity: 1, y: 0 }}
            viewport={{ once: true }}
            className="text-4xl md:text-5xl font-bold text-white mb-4"
          >
            Todo lo que necesitas para
            <br />
            gestionar tu nómina
          </motion.h2>
          <motion.p
            initial={{ opacity: 0, y: 20 }}
            whileInView={{ opacity: 1, y: 0 }}
            viewport={{ once: true }}
            transition={{ delay: 0.1 }}
            className="text-xl text-slate-400 max-w-2xl mx-auto"
          >
            Solución completa de payroll con compliance, reportes y API REST.
          </motion.p>
        </div>

        {/* Features grid */}
        <div className="grid md:grid-cols-2 lg:grid-cols-4 gap-6">
          {features.map((feature, i) => (
            <motion.div
              key={i}
              initial={{ opacity: 0, y: 20 }}
              whileInView={{ opacity: 1, y: 0 }}
              viewport={{ once: true }}
              transition={{ delay: i * 0.1 }}
              className="group relative bg-white/5 backdrop-blur-sm border border-white/10 rounded-xl p-6 hover:bg-white/10 transition-all duration-300"
            >
              <div className={`w-12 h-12 rounded-lg bg-gradient-to-br ${feature.color} flex items-center justify-center mb-4 group-hover:scale-110 transition-transform`}>
                <feature.icon className="w-6 h-6 text-white" />
              </div>
              <h3 className="text-xl font-semibold text-white mb-2">{feature.title}</h3>
              <p className="text-slate-400 leading-relaxed">{feature.description}</p>
            </motion.div>
          ))}
        </div>
      </div>
    </section>
  );
}
