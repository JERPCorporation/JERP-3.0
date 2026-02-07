/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: ichbincesartobar@yahoo.com
 */

"use client";

import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, Cell } from 'recharts';

interface AgingData {
  name: string;
  value: number;
  count: number;
}

interface AgingReportProps {
  data: AgingData[];
  title: string;
  type: 'AP' | 'AR';
}

const COLORS = {
  current: '#10b981',
  '1-30': '#3b82f6',
  '31-60': '#f59e0b',
  '61-90': '#ef4444',
  '90+': '#991b1b'
};

export function AgingReport({ data, title, type }: AgingReportProps) {
  const getBarColor = (name: string) => {
    if (name.includes('Current')) return COLORS.current;
    if (name.includes('1-30')) return COLORS['1-30'];
    if (name.includes('31-60')) return COLORS['31-60'];
    if (name.includes('61-90')) return COLORS['61-90'];
    if (name.includes('90+')) return COLORS['90+'];
    return '#64748b';
  };
  
  const totalValue = data.reduce((sum, item) => sum + item.value, 0);
  const totalCount = data.reduce((sum, item) => sum + item.count, 0);
  
  const CustomTooltip = ({ active, payload }: any) => {
    if (active && payload && payload.length) {
      const item = payload[0].payload;
      return (
        <div style={{
          background: "rgba(15, 23, 42, 0.95)",
          border: "1px solid rgba(71, 85, 105, 0.5)",
          borderRadius: "8px",
          padding: "12px",
          boxShadow: "0 4px 6px rgba(0, 0, 0, 0.3)"
        }}>
          <div style={{ color: "#f1f5f9", fontSize: "14px", fontWeight: "600", marginBottom: "6px" }}>
            {item.name}
          </div>
          <div style={{ color: "#94a3b8", fontSize: "12px" }}>
            Amount: <span style={{ color: "#f1f5f9", fontWeight: "600" }}>
              ${item.value.toLocaleString()}
            </span>
          </div>
          <div style={{ color: "#94a3b8", fontSize: "12px" }}>
            Count: <span style={{ color: "#f1f5f9", fontWeight: "600" }}>
              {item.count} {type === 'AP' ? 'bills' : 'invoices'}
            </span>
          </div>
        </div>
      );
    }
    return null;
  };
  
  return (
    <div style={{
      background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
      borderRadius: "12px",
      padding: "24px",
      border: "1px solid rgba(71, 85, 105, 0.3)",
    }}>
      <div style={{ marginBottom: "20px" }}>
        <h3 style={{ color: "#f1f5f9", fontSize: "18px", fontWeight: "600", marginBottom: "8px" }}>
          {title}
        </h3>
        <div style={{ display: "flex", gap: "24px" }}>
          <div>
            <span style={{ color: "#94a3b8", fontSize: "12px" }}>Total: </span>
            <span style={{ color: "#f1f5f9", fontSize: "14px", fontWeight: "600" }}>
              ${totalValue.toLocaleString()}
            </span>
          </div>
          <div>
            <span style={{ color: "#94a3b8", fontSize: "12px" }}>Count: </span>
            <span style={{ color: "#f1f5f9", fontSize: "14px", fontWeight: "600" }}>
              {totalCount} {type === 'AP' ? 'bills' : 'invoices'}
            </span>
          </div>
        </div>
      </div>
      
      <ResponsiveContainer width="100%" height={300}>
        <BarChart data={data}>
          <CartesianGrid strokeDasharray="3 3" stroke="rgba(71, 85, 105, 0.3)" />
          <XAxis 
            dataKey="name" 
            stroke="#94a3b8"
            style={{ fontSize: '12px' }}
          />
          <YAxis 
            stroke="#94a3b8"
            style={{ fontSize: '12px' }}
            tickFormatter={(value) => `$${(value / 1000).toFixed(0)}k`}
          />
          <Tooltip content={<CustomTooltip />} />
          <Bar dataKey="value" radius={[8, 8, 0, 0]}>
            {data.map((entry, index) => (
              <Cell key={`cell-${index}`} fill={getBarColor(entry.name)} />
            ))}
          </Bar>
        </BarChart>
      </ResponsiveContainer>
      
      <div style={{ 
        display: "flex",
        flexWrap: "wrap",
        gap: "16px",
        marginTop: "16px",
        paddingTop: "16px",
        borderTop: "1px solid rgba(71, 85, 105, 0.3)"
      }}>
        {data.map((item, index) => {
          const percentage = totalValue > 0 ? (item.value / totalValue * 100).toFixed(1) : '0.0';
          return (
            <div key={index} style={{ display: "flex", alignItems: "center", gap: "8px" }}>
              <div style={{ 
                width: "12px",
                height: "12px",
                borderRadius: "3px",
                background: getBarColor(item.name)
              }}></div>
              <span style={{ color: "#94a3b8", fontSize: "12px" }}>
                {item.name}: {percentage}%
              </span>
            </div>
          );
        })}
      </div>
    </div>
  );
}
