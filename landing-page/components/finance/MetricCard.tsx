/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: ichbincesartobar@yahoo.com
 */

interface MetricCardProps {
  title: string;
  value: string | number;
  change?: string;
  icon: string;
  color: string;
}

export function MetricCard({ title, value, change, icon, color }: MetricCardProps) {
  const isPositive = change && change.startsWith('+');
  const isNegative = change && change.startsWith('-');
  
  return (
    <div className="glass-effect rounded-xl p-6 metric-card-glow card-hover-effect">
      <div className="flex items-center justify-between mb-4">
        <span className="text-sm text-slate-400 font-medium uppercase tracking-wider">
          {title}
        </span>
        <div className="text-2xl">{icon}</div>
      </div>
      <div className="text-3xl font-bold text-slate-50 mb-2 tabular-nums">
        {value}
      </div>
      {change && (
        <div className={`text-xs font-medium ${
          isPositive ? 'text-emerald-400' : isNegative ? 'text-red-400' : 'text-slate-400'
        }`}>
          {change}
        </div>
      )}
    </div>
  );
}
