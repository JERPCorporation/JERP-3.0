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
import { ChevronDown } from 'lucide-react';
import { useState } from 'react';

const faqs = [
  {
    question: '¿Cómo funciona la prueba gratuita de 14 días?',
    answer: 'Puedes usar todas las funcionalidades del plan Pro durante 14 días sin necesidad de tarjeta de crédito. Si decides continuar, simplemente añade tu método de pago. Si no, tu cuenta se convertirá automáticamente en el plan gratuito.',
  },
  {
    question: '¿Qué bases de datos son compatibles?',
    answer: 'JERP 3.0 soporta PostgreSQL, MySQL y SQL Server. Puedes elegir tu base de datos preferida durante la configuración inicial y cambiarla en cualquier momento sin pérdida de datos.',
  },
  {
    question: '¿Cómo se calculan los impuestos?',
    answer: 'Nuestro sistema usa las tasas de impuestos federales y estatales (California) más recientes de 2024. Incluye Federal Income Tax, State Tax, FICA (Social Security) y Medicare. Los cálculos se actualizan automáticamente cuando cambian las tasas.',
  },
  {
    question: '¿Puedo generar reportes personalizados?',
    answer: 'Sí, el sistema incluye un generador de reportes flexible que permite crear reportes de nómina, compliance, costos laborales, y más. Los reportes se pueden exportar en PDF, Excel o CSV.',
  },
  {
    question: '¿El sistema verifica compliance automáticamente?',
    answer: 'Sí, JERP incluye 7+ reglas de compliance (California Labor Code y Federal FLSA) que se verifican en tiempo real. Si se detecta una violación, el sistema te alertará inmediatamente con recomendaciones para corregirla.',
  },
  {
    question: '¿Cómo funciona la integración con mi stack actual?',
    answer: 'JERP ofrece una API REST completa con 50+ endpoints documentados en Swagger. También incluye webhooks para eventos importantes como nuevos pagos, cambios de empleados, etc. La documentación completa está disponible en /api/docs.',
  },
  {
    question: '¿Qué tipo de soporte ofrecen?',
    answer: 'El plan gratuito incluye soporte por email. Los planes Pro y Enterprise tienen soporte prioritario con tiempos de respuesta garantizados. Enterprise incluye un account manager dedicado y soporte 24/7.',
  },
  {
    question: '¿Puedo cancelar en cualquier momento?',
    answer: 'Sí, puedes cancelar tu suscripción en cualquier momento desde el panel de control. No hay contratos ni penalizaciones. Si cancelas, mantendrás acceso hasta el final del periodo de facturación actual.',
  },
];

export default function FAQ() {
  const [openIndex, setOpenIndex] = useState<number | null>(null);

  return (
    <section id="faq" className="py-24 px-4 relative">
      <div className="max-w-4xl mx-auto">
        <div className="text-center mb-16">
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            whileInView={{ opacity: 1, y: 0 }}
            viewport={{ once: true }}
            className="inline-block text-sm font-semibold text-red-500 mb-4 tracking-wider uppercase"
          >
            Preguntas Frecuentes
          </motion.div>
          <motion.h2
            initial={{ opacity: 0, y: 20 }}
            whileInView={{ opacity: 1, y: 0 }}
            viewport={{ once: true }}
            className="text-4xl md:text-5xl font-bold text-white mb-4"
          >
            ¿Tienes dudas?
          </motion.h2>
          <motion.p
            initial={{ opacity: 0, y: 20 }}
            whileInView={{ opacity: 1, y: 0 }}
            viewport={{ once: true }}
            transition={{ delay: 0.1 }}
            className="text-xl text-slate-400"
          >
            Encuentra respuestas a las preguntas más comunes
          </motion.p>
        </div>

        <div className="space-y-4">
          {faqs.map((faq, i) => (
            <motion.div
              key={i}
              initial={{ opacity: 0, y: 20 }}
              whileInView={{ opacity: 1, y: 0 }}
              viewport={{ once: true }}
              transition={{ delay: i * 0.05 }}
              className="bg-white/5 backdrop-blur-sm border border-white/10 rounded-xl overflow-hidden"
            >
              <button
                onClick={() => setOpenIndex(openIndex === i ? null : i)}
                className="w-full flex items-center justify-between p-6 text-left hover:bg-white/5 transition-colors"
              >
                <span className="text-lg font-semibold text-white pr-8">
                  {faq.question}
                </span>
                <ChevronDown
                  className={`w-5 h-5 text-slate-400 flex-shrink-0 transition-transform ${
                    openIndex === i ? 'rotate-180' : ''
                  }`}
                />
              </button>
              
              {openIndex === i && (
                <motion.div
                  initial={{ height: 0, opacity: 0 }}
                  animate={{ height: 'auto', opacity: 1 }}
                  exit={{ height: 0, opacity: 0 }}
                  transition={{ duration: 0.3 }}
                  className="px-6 pb-6"
                >
                  <p className="text-slate-300 leading-relaxed">
                    {faq.answer}
                  </p>
                </motion.div>
              )}
            </motion.div>
          ))}
        </div>

        <motion.div
          initial={{ opacity: 0, y: 20 }}
          whileInView={{ opacity: 1, y: 0 }}
          viewport={{ once: true }}
          className="mt-12 text-center"
        >
          <p className="text-slate-400 mb-4">
            ¿No encuentras lo que buscas?
          </p>
          <a
            href="mailto:support@jerp.com"
            className="inline-block px-6 py-3 bg-white/10 border border-white/20 text-white rounded-lg hover:bg-white/20 transition-all"
          >
            Contacta soporte
          </a>
        </motion.div>
      </div>
    </section>
  );
}
