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

import { useState } from 'react';
import Link from 'next/link';
import { AreaChart, Area, BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';
import { MetricCard } from '@/components/finance/MetricCard';
import { JournalEntryModal } from '@/components/finance/JournalEntryModal';
import { mockCashFlowData, mockPLData, mockJournalEntries } from '@/lib/finance/mock-data';
import { JournalEntry } from '@/lib/finance/types';
import { formatCurrency, formatDate, getStatusColor } from '@/lib/finance/utils';

export default function FinanceDashboard() {
  const [selectedEntry, setSelectedEntry] = useState<JournalEntry | null>(null);
  
  // Recent transactions (last 10)
  const recentTransactions = mockJournalEntries.slice(0, 10);
  
  return (
    <div style={{
      minHeight: "100vh",
      background: "linear-gradient(135deg, #1e293b 0%, #334155 100%)",
      padding: "40px 20px",
      fontFamily: "'IBM Plex Sans', -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif"
    }}>
      <div style={{ maxWidth: "1400px", margin: "0 auto" }}>
        {/* Header */}
        <div style={{ marginBottom: "40px" }}>
          <h1 style={{ 
            color: "#f1f5f9", 
            fontSize: "36px", 
            fontWeight: "700", 
            marginBottom: "8px",
            display: "flex",
            alignItems: "center",
            gap: "16px"
          }}>
            💰 Finance Dashboard
          </h1>
          <p style={{ color: "#94a3b8", fontSize: "16px" }}>
            GAAP-compliant accounting with IRC §280E cannabis tax optimization
          </p>
        </div>
        
        {/* Quick Navigation */}
        <div style={{ 
          display: "grid",
          gridTemplateColumns: "repeat(auto-fit, minmax(200px, 1fr))",
          gap: "16px",
          marginBottom: "32px"
        }}>
          {[
            { href: '/finance/chart-of-accounts', icon: '📊', label: 'Chart of Accounts' },
            { href: '/finance/journal-entries', icon: '📝', label: 'Journal Entries' },
            { href: '/finance/accounts-payable', icon: '📋', label: 'Accounts Payable' },
            { href: '/finance/accounts-receivable', icon: '💵', label: 'Accounts Receivable' },
            { href: '/finance/reports', icon: '📈', label: 'Reports' },
            { href: '/finance/settings', icon: '⚙️', label: 'Settings' }
          ].map((link) => (
            <Link
              key={link.href}
              href={link.href}
              style={{
                padding: "16px",
                background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
                border: "1px solid rgba(71, 85, 105, 0.3)",
                borderRadius: "8px",
                color: "#f1f5f9",
                textDecoration: "none",
                display: "flex",
                alignItems: "center",
                gap: "12px",
                fontSize: "14px",
                fontWeight: "500",
                transition: "all 0.2s"
              }}
              onMouseEnter={(e) => {
                e.currentTarget.style.background = "linear-gradient(135deg, rgba(51, 65, 85, 0.9) 0%, rgba(30, 41, 59, 0.9) 100%)";
                e.currentTarget.style.borderColor = "rgba(139, 92, 246, 0.5)";
              }}
              onMouseLeave={(e) => {
                e.currentTarget.style.background = "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)";
                e.currentTarget.style.borderColor = "rgba(71, 85, 105, 0.3)";
              }}
            >
              <span style={{ fontSize: "20px" }}>{link.icon}</span>
              {link.label}
            </Link>
          ))}
        </div>
        
        {/* Metric Cards */}
        <div style={{
          display: "grid",
          gridTemplateColumns: "repeat(auto-fit, minmax(280px, 1fr))",
          gap: "20px",
          marginBottom: "32px"
        }}>
          <MetricCard
            title="Total Cash"
            value={formatCurrency(429472)}
            change="+12.5%"
            icon="💵"
            color="#10b981"
          />
          <MetricCard
            title="Revenue MTD"
            value={formatCurrency(245890)}
            change="+8.3%"
            icon="📈"
            color="#3b82f6"
          />
          <MetricCard
            title="Expenses MTD"
            value={formatCurrency(187458)}
            change="+15.2%"
            icon="📉"
            color="#f59e0b"
          />
          <MetricCard
            title="Net Income"
            value={formatCurrency(58432)}
            change="-3.8%"
            icon="💰"
            color="#8b5cf6"
          />
        </div>
        
        {/* Charts */}
        <div style={{
          display: "grid",
          gridTemplateColumns: "repeat(auto-fit, minmax(500px, 1fr))",
          gap: "20px",
          marginBottom: "32px"
        }}>
          {/* Cash Flow Chart */}
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
              💸 Cash Flow (30 Days)
            </h3>
            <ResponsiveContainer width="100%" height={300}>
              <AreaChart data={mockCashFlowData.slice(-30)}>
                <defs>
                  <linearGradient id="colorNet" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="5%" stopColor="#10b981" stopOpacity={0.3}/>
                    <stop offset="95%" stopColor="#10b981" stopOpacity={0}/>
                  </linearGradient>
                </defs>
                <CartesianGrid strokeDasharray="3 3" stroke="rgba(71, 85, 105, 0.3)" />
                <XAxis 
                  dataKey="date" 
                  stroke="#94a3b8"
                  style={{ fontSize: '11px' }}
                />
                <YAxis 
                  stroke="#94a3b8"
                  style={{ fontSize: '11px' }}
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
                <Area 
                  type="monotone" 
                  dataKey="net" 
                  stroke="#10b981"
                  strokeWidth={2}
                  fillOpacity={1} 
                  fill="url(#colorNet)" 
                />
              </AreaChart>
            </ResponsiveContainer>
          </div>
          
          {/* P&L Summary Chart */}
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
              📊 P&L Summary (6 Months)
            </h3>
            <ResponsiveContainer width="100%" height={300}>
              <BarChart data={mockPLData}>
                <CartesianGrid strokeDasharray="3 3" stroke="rgba(71, 85, 105, 0.3)" />
                <XAxis 
                  dataKey="month" 
                  stroke="#94a3b8"
                  style={{ fontSize: '11px' }}
                />
                <YAxis 
                  stroke="#94a3b8"
                  style={{ fontSize: '11px' }}
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
                <Bar dataKey="revenue" fill="#3b82f6" radius={[8, 8, 0, 0]} />
                <Bar dataKey="cogs" fill="#f59e0b" radius={[8, 8, 0, 0]} />
                <Bar dataKey="expenses" fill="#ef4444" radius={[8, 8, 0, 0]} />
                <Bar dataKey="netIncome" fill="#10b981" radius={[8, 8, 0, 0]} />
              </BarChart>
            </ResponsiveContainer>
            <div style={{ 
              display: "flex",
              gap: "16px",
              marginTop: "16px",
              justifyContent: "center"
            }}>
              {[
                { label: 'Revenue', color: '#3b82f6' },
                { label: 'COGS', color: '#f59e0b' },
                { label: 'Expenses', color: '#ef4444' },
                { label: 'Net Income', color: '#10b981' }
              ].map((item) => (
                <div key={item.label} style={{ display: "flex", alignItems: "center", gap: "6px" }}>
                  <div style={{ 
                    width: "12px",
                    height: "12px",
                    borderRadius: "3px",
                    background: item.color
                  }}></div>
                  <span style={{ color: "#94a3b8", fontSize: "12px" }}>
                    {item.label}
                  </span>
                </div>
              ))}
            </div>
          </div>
        </div>
        
        {/* Recent Transactions Table */}
        <div style={{
          background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
          borderRadius: "12px",
          padding: "24px",
          border: "1px solid rgba(71, 85, 105, 0.3)",
        }}>
          <div style={{ 
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
            marginBottom: "20px"
          }}>
            <h3 style={{ 
              color: "#f1f5f9", 
              fontSize: "18px", 
              fontWeight: "600"
            }}>
              📋 Recent Transactions
            </h3>
            <Link
              href="/finance/general-ledger"
              style={{
                padding: "8px 16px",
                borderRadius: "8px",
                background: "linear-gradient(135deg, #8b5cf6 0%, #7c3aed 100%)",
                border: "none",
                color: "white",
                fontSize: "14px",
                fontWeight: "500",
                textDecoration: "none",
                display: "inline-block"
              }}
            >
              View All →
            </Link>
          </div>
          
          <div style={{ overflowX: "auto" }}>
            <table style={{ width: "100%", borderCollapse: "collapse" }}>
              <thead>
                <tr style={{ borderBottom: "1px solid rgba(71, 85, 105, 0.3)" }}>
                  <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                    JE #
                  </th>
                  <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                    Date
                  </th>
                  <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                    Description
                  </th>
                  <th style={{ padding: "12px", textAlign: "center", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                    Status
                  </th>
                  <th style={{ padding: "12px", textAlign: "right", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                    Amount
                  </th>
                </tr>
              </thead>
              <tbody>
                {recentTransactions.map((entry) => {
                  const totalAmount = entry.lines.reduce((sum, line) => sum + line.debit, 0);
                  return (
                    <tr 
                      key={entry.id}
                      onClick={() => setSelectedEntry(entry)}
                      style={{ 
                        borderBottom: "1px solid rgba(71, 85, 105, 0.2)",
                        cursor: "pointer"
                      }}
                      onMouseEnter={(e) => {
                        e.currentTarget.style.background = "rgba(51, 65, 85, 0.3)";
                      }}
                      onMouseLeave={(e) => {
                        e.currentTarget.style.background = "transparent";
                      }}
                    >
                      <td style={{ padding: "12px", color: "#3b82f6", fontSize: "13px", fontWeight: "600" }}>
                        {entry.id}
                      </td>
                      <td style={{ padding: "12px", color: "#94a3b8", fontSize: "13px" }}>
                        {formatDate(entry.date)}
                      </td>
                      <td style={{ padding: "12px", color: "#f1f5f9", fontSize: "13px" }}>
                        {entry.description}
                      </td>
                      <td style={{ padding: "12px", textAlign: "center" }}>
                        <span style={{
                          padding: "4px 10px",
                          borderRadius: "6px",
                          background: getStatusColor(entry.status),
                          color: "white",
                          fontSize: "11px",
                          fontWeight: "500"
                        }}>
                          {entry.status}
                        </span>
                      </td>
                      <td style={{ 
                        padding: "12px",
                        color: "#f1f5f9",
                        fontSize: "13px",
                        fontWeight: "600",
                        textAlign: "right",
                        fontVariantNumeric: "tabular-nums"
                      }}>
                        {formatCurrency(totalAmount)}
                      </td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          </div>
        </div>
        
        {/* Cannabis 280E Notice */}
        <div style={{
          marginTop: "32px",
          padding: "16px 20px",
          background: "linear-gradient(135deg, rgba(139, 92, 246, 0.1) 0%, rgba(124, 58, 237, 0.1) 100%)",
          border: "1px solid rgba(139, 92, 246, 0.3)",
          borderRadius: "8px",
          display: "flex",
          alignItems: "center",
          gap: "12px"
        }}>
          <span style={{ fontSize: "24px" }}>🌿</span>
          <div>
            <div style={{ color: "#f1f5f9", fontSize: "14px", fontWeight: "600", marginBottom: "4px" }}>
              Cannabis Tax Compliance Active
            </div>
            <div style={{ color: "#c4b5fd", fontSize: "12px" }}>
              IRC §280E tracking enabled. COGS deductions optimized. Non-deductible expenses properly categorized.{' '}
              <Link 
                href="/finance/reports/280e-report"
                style={{ color: "#a78bfa", textDecoration: "underline" }}
              >
                View 280E Report →
              </Link>
            </div>
          </div>
        </div>
      </div>
      
      {/* Journal Entry Modal */}
      {selectedEntry && (
        <JournalEntryModal
          entry={selectedEntry}
          onClose={() => setSelectedEntry(null)}
        />
      )}
    </div>
  );
}
