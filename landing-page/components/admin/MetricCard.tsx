/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: licensing@jerp.io
 */

import React from 'react';
import { LucideIcon } from 'lucide-react';

interface MetricCardProps {
  title: string;
  value: string | number;
  change?: number;
  icon: LucideIcon;
  color: string;
}

export function MetricCard({ title, value, change, icon: Icon, color }: MetricCardProps) {
  const isPositive = change && change > 0;
  const isNegative = change && change < 0;

  return (
    <div className="relative overflow-hidden bg-white border border-slate-200 rounded-2xl p-6 shadow-sm">
      {/* Icon background */}
      <div className="absolute top-4 right-4 opacity-10">
        <Icon className="w-20 h-20" style={{ color }} />
      </div>

      {/* Content */}
      <div className="relative z-10">
        <div className="flex items-center justify-between mb-4">
          <div
            className="p-3 rounded-xl"
            style={{ backgroundColor: `${color}20` }}
          >
            <Icon className="w-6 h-6" style={{ color }} />
          </div>
          {change !== undefined && (
            <span
              className={`text-sm font-semibold ${
                isPositive ? 'text-green-600' : isNegative ? 'text-red-600' : 'text-slate-500'
              }`}
            >
              {isPositive ? '↑' : isNegative ? '↓' : ''} {Math.abs(change)}%
            </span>
          )}
        </div>

        <div className="mb-2">
          <div className="text-3xl font-bold text-slate-900">{value}</div>
        </div>

        <div className="text-sm text-slate-600 font-medium">{title}</div>
      </div>
    </div>
  );
}
