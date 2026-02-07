/**
 * JERP 3.0 - Finance Module - Invoices Tab Component
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: licensing@jerp.io
 *
 * Displays accounts receivable invoices with aging and status management
 */

'use client';

import { useState } from 'react';
import { mockInvoices } from '@/lib/finance/mock-data';
import { formatCurrency, formatDate } from '@/lib/finance/utils';


type Invoice = typeof mockInvoices[0];
type FilterStatus = 'All' | 'Draft' | 'Sent' | 'Paid' | 'Overdue' | 'Partial';

export function InvoicesTab() {
  const [selectedFilter, setSelectedFilter] = useState<FilterStatus>('All');

  const computeDaysOverdue = (dueDate: string, status: string): number => {
    if (status === 'Paid') return 0;
    const millisecondsPast = new Date().getTime() - new Date(dueDate).getTime();
    const daysOverdue = Math.max(0, Math.floor(millisecondsPast / (1000 * 60 * 60 * 24)));
    return daysOverdue;
  };

  const determineIfOverdue = (dueDate: string, status: string): boolean => {
    return status !== 'Paid' && new Date(dueDate) < new Date();
  };

  const applyFilter = (invoice: Invoice): boolean => {
    if (selectedFilter === 'All') return true;
    if (selectedFilter === 'Overdue') return determineIfOverdue(invoice.dueDate, invoice.status);
    if (selectedFilter === 'Partial') return (invoice.amount - invoice.paidAmount) > 0 && (invoice.amount - invoice.paidAmount) < invoice.amount;
    return invoice.status === selectedFilter;
  };

  const displayedInvoices = mockInvoices.filter(applyFilter);

  const statusCategories: FilterStatus[] = ['All', 'Draft', 'Sent', 'Partial', 'Paid', 'Overdue'];

  const countByStatus = (status: FilterStatus): number => {
    if (status === 'All') return mockInvoices.length;
    if (status === 'Overdue') return mockInvoices.filter(inv => determineIfOverdue(inv.dueDate, inv.status)).length;
    if (status === 'Partial') return mockInvoices.filter(inv => (inv.amount - inv.paidAmount) > 0 && (inv.amount - inv.paidAmount) < inv.amount).length;
    return mockInvoices.filter(inv => inv.status === status).length;
  };

  return (
    <div style={{ padding: '24px' }}>
      {/* Header Section */}
      <div style={{ marginBottom: '28px' }}>
        <h2 style={{ 
          fontSize: '26px', 
          fontWeight: '700', 
          margin: '0 0 8px 0',
          color: '#0f172a'
        }}>
          Invoices & Receivables
        </h2>
        <p style={{ 
          fontSize: '14px', 
          color: '#64748b',
          margin: 0,
          lineHeight: '1.5'
        }}>
          Track outstanding invoices, payment status, and aging receivables
        </p>
      </div>

      {/* Filter Buttons */}
      <div style={{ 
        display: 'flex', 
        gap: '10px', 
        marginBottom: '28px',
        flexWrap: 'wrap',
        padding: '16px',
        background: 'linear-gradient(135deg, #f8fafc 0%, #f1f5f9 100%)',
        borderRadius: '12px',
        border: '1px solid #e2e8f0'
      }}>
        {statusCategories.map(status => {
          const active = selectedFilter === status;
          const itemCount = countByStatus(status);
          
          return (
            <button
              key={status}
              onClick={() => setSelectedFilter(status)}
              style={{
                padding: '12px 20px',
                border: 'none',
                background: active 
                  ? 'linear-gradient(135deg, #3b82f6 0%, #2563eb 100%)' 
                  : 'white',
                color: active ? 'white' : '#475569',
                borderRadius: '10px',
                fontSize: '14px',
                fontWeight: active ? '700' : '500',
                cursor: 'pointer',
                transition: 'all 0.2s',
                boxShadow: active 
                  ? '0 4px 6px rgba(37, 99, 235, 0.3)' 
                  : '0 1px 2px rgba(0, 0, 0, 0.05)',
                display: 'flex',
                alignItems: 'center',
                gap: '10px'
              }}
              onMouseOver={(e) => {
                if (!active) {
                  e.currentTarget.style.transform = 'translateY(-2px)';
                  e.currentTarget.style.boxShadow = '0 4px 6px rgba(0, 0, 0, 0.1)';
                }
              }}
              onMouseOut={(e) => {
                if (!active) {
                  e.currentTarget.style.transform = 'translateY(0)';
                  e.currentTarget.style.boxShadow = '0 1px 2px rgba(0, 0, 0, 0.05)';
                }
              }}
            >
              <span>{status}</span>
              <span style={{
                background: active ? 'rgba(255, 255, 255, 0.3)' : '#e2e8f0',
                color: active ? 'white' : '#475569',
                padding: '2px 10px',
                borderRadius: '12px',
                fontSize: '12px',
                fontWeight: '700',
                minWidth: '28px',
                textAlign: 'center'
              }}>
                {itemCount}
              </span>
            </button>
          );
        })}
      </div>

      {/* Invoices Table */}
      <div style={{ 
        background: 'white',
        borderRadius: '16px',
        boxShadow: '0 4px 6px rgba(0, 0, 0, 0.07), 0 1px 3px rgba(0, 0, 0, 0.06)',
        overflow: 'hidden',
        border: '1px solid #f1f5f9'
      }}>
        <div style={{ overflowX: 'auto' }}>
          <table style={{ 
            width: '100%', 
            borderCollapse: 'collapse',
            minWidth: '1200px'
          }}>
            <thead>
              <tr style={{ 
                background: 'linear-gradient(to right, #0f172a 0%, #1e293b 100%)',
                color: 'white'
              }}>
                <th style={thStyle}>Invoice #</th>
                <th style={thStyle}>Customer Name</th>
                <th style={thStyle}>Invoice Date</th>
                <th style={thStyle}>Due Date</th>
                <th style={{ ...thStyle, textAlign: 'right' }}>Total Amount</th>
                <th style={{ ...thStyle, textAlign: 'right' }}>Amount Due</th>
                <th style={thStyle}>Status</th>
                <th style={{ ...thStyle, textAlign: 'center' }}>Days Past Due</th>
              </tr>
            </thead>
            <tbody>
              {displayedInvoices.map((invoice) => {
                const overdueDays = computeDaysOverdue(invoice.dueDate, invoice.status);
                const pastDue = determineIfOverdue(invoice.dueDate, invoice.status);
                const isPartial = (invoice.amount - invoice.paidAmount) > 0 && (invoice.amount - invoice.paidAmount) < invoice.amount;
                
                return (
                  <tr 
                    key={invoice.id}
                    style={{ 
                      borderBottom: '1px solid #f1f5f9',
                      background: pastDue ? '#fef2f2' : 'transparent',
                      transition: 'all 0.15s'
                    }}
                    onMouseOver={(e) => {
                      e.currentTarget.style.backgroundColor = pastDue ? '#fee2e2' : '#f8fafc';
                    }}
                    onMouseOut={(e) => {
                      e.currentTarget.style.backgroundColor = pastDue ? '#fef2f2' : 'transparent';
                    }}
                  >
                    <td style={tdStyle}>
                      <div style={{ 
                        fontFamily: 'monospace',
                        fontSize: '13px',
                        fontWeight: '700',
                        color: '#2563eb',
                        background: '#eff6ff',
                        padding: '6px 12px',
                        borderRadius: '6px',
                        display: 'inline-block',
                        border: '1px solid #bfdbfe'
                      }}>
                        {invoice.invoiceNumber}
                      </div>
                    </td>
                    <td style={tdStyle}>
                      <span style={{ fontWeight: '600', color: '#0f172a', fontSize: '14px' }}>
                        {invoice.customerName}
                      </span>
                    </td>
                    <td style={tdStyle}>
                      <span style={{ color: '#64748b', fontSize: '14px' }}>
                        {formatDate(invoice.date)}
                      </span>
                    </td>
                    <td style={tdStyle}>
                      <span style={{ 
                        color: pastDue ? '#dc2626' : '#64748b',
                        fontWeight: pastDue ? '700' : '400',
                        fontSize: '14px'
                      }}>
                        {formatDate(invoice.dueDate)}
                      </span>
                    </td>
                    <td style={{ ...tdStyle, textAlign: 'right' }}>
                      <span style={{ fontWeight: '600', color: '#1e293b', fontSize: '14px' }}>
                        {formatCurrency(invoice.amount)}
                      </span>
                    </td>
                    <td style={{ ...tdStyle, textAlign: 'right' }}>
                      <span style={{ 
                        fontWeight: '700',
                        color: (invoice.amount - invoice.paidAmount) > 0 ? '#059669' : '#94a3b8',
                        fontSize: '15px'
                      }}>
                        {formatCurrency((invoice.amount - invoice.paidAmount))}
                      </span>
                    </td>
                    <td style={tdStyle}>
                      {(() => {
                        const displayStatus = pastDue ? 'Overdue' : isPartial ? 'Partial' : invoice.status;
                        const getStatusColor = (status: string) => {
                          if (status === 'Overdue') return { bg: 'rgba(239, 68, 68, 0.15)', text: '#ef4444' };
                          if (status === 'Paid') return { bg: 'rgba(16, 185, 129, 0.15)', text: '#10b981' };
                          if (status === 'Sent') return { bg: 'rgba(59, 130, 246, 0.15)', text: '#3b82f6' };
                          if (status === 'Partial') return { bg: 'rgba(245, 158, 11, 0.15)', text: '#f59e0b' };
                          return { bg: 'rgba(100, 116, 139, 0.15)', text: '#94a3b8' };
                        };
                        const colors = getStatusColor(displayStatus);
                        return (
                          <span style={{
                            display: 'inline-block',
                            padding: '4px 12px',
                            borderRadius: '12px',
                            fontSize: '11px',
                            fontWeight: '600',
                            textTransform: 'uppercase',
                            letterSpacing: '0.05em',
                            background: colors.bg,
                            color: colors.text
                          }}>
                            {displayStatus}
                          </span>
                        );
                      })()}
                    </td>
                    <td style={{ ...tdStyle, textAlign: 'center' }}>
                      {overdueDays > 0 ? (
                        <div style={{
                          display: 'inline-flex',
                          alignItems: 'center',
                          padding: '6px 14px',
                          background: 'linear-gradient(135deg, #fee2e2 0%, #fecaca 100%)',
                          color: '#991b1b',
                          borderRadius: '20px',
                          fontSize: '13px',
                          fontWeight: '800',
                          border: '2px solid #fca5a5',
                          boxShadow: '0 2px 4px rgba(220, 38, 38, 0.2)'
                        }}>
                          {overdueDays} {overdueDays === 1 ? 'day' : 'days'}
                        </div>
                      ) : (
                        <span style={{ color: '#cbd5e1', fontSize: '14px', fontWeight: '500' }}>
                          —
                        </span>
                      )}
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>

        {/* Empty State */}
        {displayedInvoices.length === 0 && (
          <div style={{ 
            padding: '60px 24px',
            textAlign: 'center',
            color: '#94a3b8'
          }}>
            <div style={{ fontSize: '48px', marginBottom: '16px' }}>📄</div>
            <p style={{ fontSize: '16px', margin: 0, fontWeight: '600' }}>
              No invoices match the selected filter
            </p>
          </div>
        )}

        {/* Summary Footer */}
        <div style={{ 
          padding: '24px',
          background: 'linear-gradient(135deg, #f0fdfa 0%, #ccfbf1 100%)',
          borderTop: '3px solid #5eead4',
          display: 'flex',
          justifyContent: 'space-between',
          alignItems: 'center',
          flexWrap: 'wrap',
          gap: '20px'
        }}>
          <div style={{ flex: '1', minWidth: '180px' }}>
            <div style={{ fontSize: '12px', color: '#115e59', fontWeight: '600', marginBottom: '6px' }}>
              Filtered Invoices
            </div>
            <div style={{ fontSize: '24px', fontWeight: '800', color: '#134e4a' }}>
              {displayedInvoices.length}
            </div>
          </div>
          <div style={{ flex: '1', minWidth: '180px' }}>
            <div style={{ fontSize: '12px', color: '#115e59', fontWeight: '600', marginBottom: '6px' }}>
              Total Outstanding
            </div>
            <div style={{ fontSize: '24px', fontWeight: '800', color: '#059669' }}>
              {formatCurrency(displayedInvoices.reduce((total, inv) => total + (inv.amount - inv.paidAmount), 0))}
            </div>
          </div>
          <div style={{ flex: '1', minWidth: '180px' }}>
            <div style={{ fontSize: '12px', color: '#115e59', fontWeight: '600', marginBottom: '6px' }}>
              Past Due Count
            </div>
            <div style={{ fontSize: '24px', fontWeight: '800', color: '#dc2626' }}>
              {displayedInvoices.filter(inv => determineIfOverdue(inv.dueDate, inv.status)).length}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

const thStyle: React.CSSProperties = {
  padding: '18px 20px',
  textAlign: 'left',
  fontSize: '12px',
  fontWeight: '800',
  textTransform: 'uppercase',
  letterSpacing: '1px'
};

const tdStyle: React.CSSProperties = {
  padding: '18px 20px',
  fontSize: '14px'
};
