/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: licensing@jerp.io
 */

"use client";

import { LineChart, Line, PieChart, Pie, Cell, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, Legend } from 'recharts';
import { MetricCard } from '@/components/finance/MetricCard';
import { formatCurrency } from '@/lib/finance/utils';

const revenueExpensesData = [
  { month: 'Jan', revenue: 420000, expenses: 320000 },
  { month: 'Feb', revenue: 450000, expenses: 335000 },
  { month: 'Mar', revenue: 480000, expenses: 350000 },
  { month: 'Apr', revenue: 510000, expenses: 380000 },
  { month: 'May', revenue: 490000, expenses: 365000 },
  { month: 'Jun', revenue: 520000, expenses: 390000 },
];

const expenseBreakdownData = [
  { category: 'Payroll', value: 35, amount: 105000 },
  { category: 'Rent & Utilities', value: 20, amount: 60000 },
  { category: 'Marketing', value: 15, amount: 45000 },
  { category: 'COGS', value: 25, amount: 75000 },
  { category: 'Other', value: 5, amount: 15000 },
];

const EXPENSE_COLORS = ['#8b5cf6', '#3b82f6', '#10b981', '#f59e0b', '#64748b'];

export function FinanceDashboardTab() {
  return (
    <div>
      {/* Metric Cards Row */}
      <div style={{
        display: "grid",
        gridTemplateColumns: "repeat(auto-fit, minmax(260px, 1fr))",
        gap: "20px",
        marginBottom: "32px"
      }}>
        <MetricCard
          title="Total Assets"
          value={formatCurrency(1245000)}
          change="+8.5%"
          icon="💎"
          color="#10b981"
        />
        <MetricCard
          title="Net Income MTD"
          value={formatCurrency(185000)}
          change="+12.3%"
          icon="📈"
          color="#3b82f6"
        />
        <MetricCard
          title="Cash Balance"
          value={formatCurrency(450000)}
          change="-3.2%"
          icon="💵"
          color="#f59e0b"
        />
        <MetricCard
          title="Profit Margin"
          value="24.5%"
          change="+2.1%"
          icon="🎯"
          color="#8b5cf6"
        />
      </div>
      
      {/* Charts Row */}
      <div style={{
        display: "grid",
        gridTemplateColumns: "repeat(auto-fit, minmax(500px, 1fr))",
        gap: "20px",
        marginBottom: "32px"
      }}>
        {/* Revenue vs Expenses Chart */}
        <div style={{
          background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
          borderRadius: "12px",
          padding: "24px",
          border: "1px solid rgba(71, 85, 105, 0.3)",
        }}>
          <h3 style={{ 
            color: "#f1f5f9", 
            fontSize: "18px", 
            fontWeight: "600",
            marginBottom: "20px"
          }}>
            📊 Revenue vs Expenses (6 Months)
          </h3>
          <ResponsiveContainer width="100%" height={300}>
            <LineChart data={revenueExpensesData}>
              <CartesianGrid strokeDasharray="3 3" stroke="rgba(71, 85, 105, 0.3)" />
              <XAxis 
                dataKey="month" 
                stroke="#94a3b8"
                style={{ fontSize: '12px' }}
              />
              <YAxis 
                stroke="#94a3b8"
                style={{ fontSize: '12px' }}
                tickFormatter={(value) => `$${(value / 1000).toFixed(0)}k`}
              />
              <Tooltip
                contentStyle={{
                  background: "rgba(15, 23, 42, 0.95)",
                  border: "1px solid rgba(71, 85, 105, 0.5)",
                  borderRadius: "8px",
                  color: "#f1f5f9"
                }}
                formatter={(value: any) => [`$${value.toLocaleString()}`, '']}
              />
              <Legend />
              <Line 
                type="monotone" 
                dataKey="revenue" 
                stroke="#10b981"
                strokeWidth={3}
                name="Revenue"
              />
              <Line 
                type="monotone" 
                dataKey="expenses" 
                stroke="#ef4444"
                strokeWidth={3}
                name="Expenses"
              />
            </LineChart>
          </ResponsiveContainer>
        </div>
        
        {/* Expense Breakdown Chart */}
        <div style={{
          background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
          borderRadius: "12px",
          padding: "24px",
          border: "1px solid rgba(71, 85, 105, 0.3)",
        }}>
          <h3 style={{ 
            color: "#f1f5f9", 
            fontSize: "18px", 
            fontWeight: "600",
            marginBottom: "20px"
          }}>
            📉 Expense Breakdown
          </h3>
          <ResponsiveContainer width="100%" height={300}>
            <PieChart>
              <Pie
                data={expenseBreakdownData}
                cx="50%"
                cy="50%"
                labelLine={false}
                label={(entry) => `${entry.category}: ${entry.value}%`}
                outerRadius={100}
                fill="#8884d8"
                dataKey="value"
              >
                {expenseBreakdownData.map((entry, index) => (
                  <Cell key={`cell-${index}`} fill={EXPENSE_COLORS[index % EXPENSE_COLORS.length]} />
                ))}
              </Pie>
              <Tooltip
                contentStyle={{
                  background: "rgba(15, 23, 42, 0.95)",
                  border: "1px solid rgba(71, 85, 105, 0.5)",
                  borderRadius: "8px",
                  color: "#f1f5f9"
                }}
                formatter={(value: any, name: any, props: any) => [
                  `$${props.payload.amount.toLocaleString()} (${value}%)`,
                  props.payload.category
                ]}
              />
            </PieChart>
          </ResponsiveContainer>
        </div>
      </div>
      
      {/* Quick Action Buttons */}
      <div style={{
        background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
        borderRadius: "12px",
        padding: "24px",
        border: "1px solid rgba(71, 85, 105, 0.3)",
      }}>
        <h3 style={{ 
          color: "#f1f5f9", 
          fontSize: "18px", 
          fontWeight: "600",
          marginBottom: "20px"
        }}>
          ⚡ Quick Actions
        </h3>
        <div style={{
          display: "grid",
          gridTemplateColumns: "repeat(auto-fit, minmax(200px, 1fr))",
          gap: "16px"
        }}>
          {[
            { label: 'Create Journal Entry', icon: '📝', color: '#8b5cf6' },
            { label: 'Add Account', icon: '🏦', color: '#3b82f6' },
            { label: 'Generate P&L Report', icon: '📊', color: '#10b981' },
            { label: 'View Balance Sheet', icon: '📈', color: '#f59e0b' },
          ].map((action) => (
            <button
              key={action.label}
              onClick={() => alert(`${action.label} - Feature coming soon!`)}
              style={{
                padding: "16px",
                borderRadius: "8px",
                border: "1px solid rgba(71, 85, 105, 0.3)",
                background: "rgba(15, 23, 42, 0.5)",
                color: "#f1f5f9",
                fontSize: "14px",
                fontWeight: "500",
                cursor: "pointer",
                display: "flex",
                alignItems: "center",
                gap: "12px",
                transition: "all 0.2s"
              }}
              onMouseEnter={(e) => {
                e.currentTarget.style.background = action.color + "20";
                e.currentTarget.style.borderColor = action.color + "40";
                e.currentTarget.style.transform = "translateY(-2px)";
              }}
              onMouseLeave={(e) => {
                e.currentTarget.style.background = "rgba(15, 23, 42, 0.5)";
                e.currentTarget.style.borderColor = "rgba(71, 85, 105, 0.3)";
                e.currentTarget.style.transform = "translateY(0)";
              }}
            >
              <span style={{ fontSize: "20px" }}>{action.icon}</span>
              <span>{action.label}</span>
            </button>
          ))}
        </div>
      </div>
    </div>
  );
}
