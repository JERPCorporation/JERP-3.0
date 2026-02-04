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
import { Calculator, DollarSign } from 'lucide-react';
import { useState } from 'react';

export default function Demo() {
  const [salary, setSalary] = useState(5000);
  const [hours, setHours] = useState(160);

  // Simple calculations for demo
  const federalTax = salary * 0.12;
  const stateTax = salary * 0.05;
  const fica = salary * 0.062;
  const medicare = salary * 0.0145;
  const totalDeductions = federalTax + stateTax + fica + medicare;
  const netPay = salary - totalDeductions;

  return (
    <section id="demo" className="py-24 px-4 relative">
      <div className="max-w-6xl mx-auto">
        <div className="text-center mb-16">
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            whileInView={{ opacity: 1, y: 0 }}
            viewport={{ once: true }}
            className="inline-block text-sm font-semibold text-red-500 mb-4 tracking-wider uppercase"
          >
            Demo Interactiva
          </motion.div>
          <motion.h2
            initial={{ opacity: 0, y: 20 }}
            whileInView={{ opacity: 1, y: 0 }}
            viewport={{ once: true }}
            className="text-4xl md:text-5xl font-bold text-white mb-4"
          >
            Calcula nómina en tiempo real
          </motion.h2>
          <motion.p
            initial={{ opacity: 0, y: 20 }}
            whileInView={{ opacity: 1, y: 0 }}
            viewport={{ once: true }}
            transition={{ delay: 0.1 }}
            className="text-xl text-slate-400 max-w-2xl mx-auto"
          >
            Prueba nuestro calculador de nómina con tasas de impuestos 2024
          </motion.p>
        </div>

        <motion.div
          initial={{ opacity: 0, y: 20 }}
          whileInView={{ opacity: 1, y: 0 }}
          viewport={{ once: true }}
          className="grid md:grid-cols-2 gap-8 bg-white/5 backdrop-blur-sm border border-white/10 rounded-2xl p-8"
        >
          {/* Input side */}
          <div className="space-y-6">
            <h3 className="text-2xl font-bold text-white mb-6 flex items-center gap-2">
              <Calculator className="w-6 h-6 text-red-500" />
              Datos de entrada
            </h3>

            <div>
              <label className="block text-slate-300 mb-2 font-semibold">
                Salario mensual: ${salary.toLocaleString()}
              </label>
              <input
                type="range"
                min="1000"
                max="20000"
                step="100"
                value={salary}
                onChange={(e) => setSalary(Number(e.target.value))}
                className="w-full h-2 bg-slate-700 rounded-lg appearance-none cursor-pointer accent-red-500"
              />
            </div>

            <div>
              <label className="block text-slate-300 mb-2 font-semibold">
                Horas trabajadas: {hours}h
              </label>
              <input
                type="range"
                min="80"
                max="240"
                step="1"
                value={hours}
                onChange={(e) => setHours(Number(e.target.value))}
                className="w-full h-2 bg-slate-700 rounded-lg appearance-none cursor-pointer accent-red-500"
              />
            </div>

            <div className="bg-slate-800/50 rounded-lg p-4 border border-slate-700">
              <div className="flex items-center gap-2 text-slate-400 mb-2">
                <DollarSign className="w-4 h-4" />
                <span className="text-sm">Tarifa por hora</span>
              </div>
              <p className="text-2xl font-bold text-white">
                ${(salary / hours).toFixed(2)}/hora
              </p>
            </div>
          </div>

          {/* Results side */}
          <div className="space-y-4">
            <h3 className="text-2xl font-bold text-white mb-6">
              Desglose de nómina
            </h3>

            <div className="space-y-3">
              <div className="flex justify-between items-center p-3 bg-slate-800/50 rounded-lg">
                <span className="text-slate-300">Salario bruto</span>
                <span className="text-white font-semibold">${salary.toLocaleString()}</span>
              </div>

              <div className="border-l-2 border-red-500 pl-3 space-y-2">
                <div className="flex justify-between items-center text-sm">
                  <span className="text-slate-400">Federal (12%)</span>
                  <span className="text-red-400">-${federalTax.toFixed(2)}</span>
                </div>
                <div className="flex justify-between items-center text-sm">
                  <span className="text-slate-400">Estatal CA (5%)</span>
                  <span className="text-red-400">-${stateTax.toFixed(2)}</span>
                </div>
                <div className="flex justify-between items-center text-sm">
                  <span className="text-slate-400">FICA (6.2%)</span>
                  <span className="text-red-400">-${fica.toFixed(2)}</span>
                </div>
                <div className="flex justify-between items-center text-sm">
                  <span className="text-slate-400">Medicare (1.45%)</span>
                  <span className="text-red-400">-${medicare.toFixed(2)}</span>
                </div>
              </div>

              <div className="flex justify-between items-center p-3 bg-slate-800/50 rounded-lg border-t-2 border-slate-700">
                <span className="text-slate-300">Total deducciones</span>
                <span className="text-red-400 font-semibold">-${totalDeductions.toFixed(2)}</span>
              </div>

              <div className="flex justify-between items-center p-4 bg-gradient-to-r from-green-500/20 to-emerald-500/20 border border-green-500/30 rounded-lg">
                <span className="text-white font-bold text-lg">Salario neto</span>
                <span className="text-green-400 font-bold text-2xl">${netPay.toFixed(2)}</span>
              </div>
            </div>

            <p className="text-xs text-slate-500 mt-4">
              * Cálculo simplificado para fines demostrativos. Los cálculos reales incluyen más factores.
            </p>
          </div>
        </motion.div>
      </div>
    </section>
  );
}
