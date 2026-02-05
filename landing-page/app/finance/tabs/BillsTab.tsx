/**
 * JERP 3.0 - Finance Module - Bills Tab Component
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: licensing@jerp.io
 *
 * Displays bills payable with status filtering and overdue tracking
 */

'use client';

import { useState } from 'react';
import { mockBills } from '@/lib/finance/mock-data';
import { formatCurrency, formatDate } from '@/lib/finance/utils';


type Bill = typeof mockBills[0];
type BillStatus = 'All' | 'Draft' | 'Approved' | 'Paid' | 'Overdue';

export function BillsTab() {
  const [activeFilter, setActiveFilter] = useState<BillStatus>('All');

  const calculateDaysPastDue = (dueDate: string, status: string): number => {
    if (status === 'Paid') return 0;
    const daysPast = Math.max(0, Math.floor((new Date().getTime() - new Date(dueDate).getTime()) / (1000 * 60 * 60 * 24)));
    return daysPast;
  };

  const isOverdue = (dueDate: string, status: string): boolean => {
    return status !== 'Paid' && new Date(dueDate) < new Date();
  };

  const filteredBills = mockBills.filter(bill => {
    if (activeFilter === 'All') return true;
    if (activeFilter === 'Overdue') return isOverdue(bill.dueDate, bill.status);
    return bill.status === activeFilter;
  });

  const statusFilters: BillStatus[] = ['All', 'Draft', 'Approved', 'Paid', 'Overdue'];

  const getFilterCount = (filter: BillStatus): number => {
    if (filter === 'All') return mockBills.length;
    if (filter === 'Overdue') return mockBills.filter(b => isOverdue(b.dueDate, b.status)).length;
    return mockBills.filter(b => b.status === filter).length;
  };

  return (
    <div style={{ padding: '24px' }}>
      {/* Header */}
      <div style={{ marginBottom: '24px' }}>
        <h2 style={{ 
          fontSize: '24px', 
          fontWeight: '600', 
          margin: '0 0 8px 0',
          color: '#1a1a1a'
        }}>
          Bills Payable
        </h2>
        <p style={{ 
          fontSize: '14px', 
          color: '#666',
          margin: 0
        }}>
          Track and manage vendor bills and payment schedules
        </p>
      </div>

      {/* Status Filter Pills */}
      <div style={{ 
        display: 'flex', 
        gap: '12px', 
        marginBottom: '24px',
        flexWrap: 'wrap'
      }}>
        {statusFilters.map(filter => {
          const isActive = activeFilter === filter;
          const count = getFilterCount(filter);
          
          return (
            <button
              key={filter}
              onClick={() => setActiveFilter(filter)}
              style={{
                padding: '10px 20px',
                border: isActive ? '2px solid #2563eb' : '2px solid #e5e7eb',
                background: isActive 
                  ? 'linear-gradient(135deg, #dbeafe 0%, #bfdbfe 100%)' 
                  : 'white',
                color: isActive ? '#1e40af' : '#6b7280',
                borderRadius: '20px',
                fontSize: '14px',
                fontWeight: isActive ? '600' : '500',
                cursor: 'pointer',
                transition: 'all 0.2s',
                display: 'flex',
                alignItems: 'center',
                gap: '8px'
              }}
              onMouseOver={(e) => {
                if (!isActive) {
                  e.currentTarget.style.borderColor = '#2563eb';
                  e.currentTarget.style.background = '#f9fafb';
                }
              }}
              onMouseOut={(e) => {
                if (!isActive) {
                  e.currentTarget.style.borderColor = '#e5e7eb';
                  e.currentTarget.style.background = 'white';
                }
              }}
            >
              {filter}
              <span style={{
                background: isActive ? '#2563eb' : '#d1d5db',
                color: 'white',
                padding: '2px 8px',
                borderRadius: '10px',
                fontSize: '12px',
                fontWeight: '600',
                minWidth: '24px',
                textAlign: 'center'
              }}>
                {count}
              </span>
            </button>
          );
        })}
      </div>

      {/* Bills Table */}
      <div style={{ 
        background: 'white',
        borderRadius: '12px',
        boxShadow: '0 1px 3px rgba(0, 0, 0, 0.1)',
        overflow: 'hidden'
      }}>
        <div style={{ overflowX: 'auto' }}>
          <table style={{ 
            width: '100%', 
            borderCollapse: 'collapse',
            minWidth: '1100px'
          }}>
            <thead>
              <tr style={{ 
                background: 'linear-gradient(to right, #1e3a8a 0%, #1e40af 100%)',
                color: 'white'
              }}>
                <th style={headerStyle}>Bill #</th>
                <th style={headerStyle}>Vendor Name</th>
                <th style={headerStyle}>Bill Date</th>
                <th style={headerStyle}>Due Date</th>
                <th style={{ ...headerStyle, textAlign: 'right' }}>Total Amount</th>
                <th style={{ ...headerStyle, textAlign: 'right' }}>Amount Due</th>
                <th style={headerStyle}>Status</th>
                <th style={{ ...headerStyle, textAlign: 'center' }}>Days Past Due</th>
              </tr>
            </thead>
            <tbody>
              {filteredBills.map((bill) => {
                const daysPast = calculateDaysPastDue(bill.dueDate, bill.status);
                const overdue = isOverdue(bill.dueDate, bill.status);
                
                return (
                  <tr 
                    key={bill.id}
                    style={{ 
                      borderBottom: '1px solid #f3f4f6',
                      background: overdue ? '#fef2f2' : 'transparent',
                      transition: 'background-color 0.2s'
                    }}
                    onMouseOver={(e) => {
                      e.currentTarget.style.backgroundColor = overdue ? '#fee2e2' : '#f9fafb';
                    }}
                    onMouseOut={(e) => {
                      e.currentTarget.style.backgroundColor = overdue ? '#fef2f2' : 'transparent';
                    }}
                  >
                    <td style={dataStyle}>
                      <span style={{ 
                        fontFamily: 'monospace',
                        fontSize: '13px',
                        fontWeight: '600',
                        color: '#1e40af'
                      }}>
                        {bill.id}
                      </span>
                    </td>
                    <td style={dataStyle}>
                      <span style={{ fontWeight: '500', color: '#1a1a1a' }}>
                        {bill.vendorName}
                      </span>
                    </td>
                    <td style={dataStyle}>
                      <span style={{ color: '#4b5563' }}>
                        {formatDate(bill.date)}
                      </span>
                    </td>
                    <td style={dataStyle}>
                      <span style={{ 
                        color: overdue ? '#dc2626' : '#4b5563',
                        fontWeight: overdue ? '600' : '400'
                      }}>
                        {formatDate(bill.dueDate)}
                      </span>
                    </td>
                    <td style={{ ...dataStyle, textAlign: 'right' }}>
                      <span style={{ fontWeight: '500', color: '#1a1a1a' }}>
                        {formatCurrency(bill.amount)}
                      </span>
                    </td>
                    <td style={{ ...dataStyle, textAlign: 'right' }}>
                      <span style={{ 
                        fontWeight: '600',
                        color: (bill.amount - bill.paidAmount) > 0 ? '#dc2626' : '#059669',
                        fontSize: '14px'
                      }}>
                        {formatCurrency((bill.amount - bill.paidAmount))}
                      </span>
                    </td>
                    <td style={dataStyle}>
                      {(() => {
                        const displayStatus = overdue ? 'Overdue' : bill.status;
                        const getStatusColor = (status: string) => {
                          if (status === 'Overdue') return { bg: 'rgba(239, 68, 68, 0.15)', text: '#ef4444' };
                          if (status === 'Paid') return { bg: 'rgba(16, 185, 129, 0.15)', text: '#10b981' };
                          if (status === 'Approved') return { bg: 'rgba(59, 130, 246, 0.15)', text: '#3b82f6' };
                          return { bg: 'rgba(245, 158, 11, 0.15)', text: '#f59e0b' };
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
                    <td style={{ ...dataStyle, textAlign: 'center' }}>
                      {daysPast > 0 ? (
                        <span style={{
                          display: 'inline-block',
                          padding: '4px 12px',
                          background: 'linear-gradient(135deg, #fee2e2 0%, #fecaca 100%)',
                          color: '#991b1b',
                          borderRadius: '12px',
                          fontSize: '13px',
                          fontWeight: '700',
                          border: '1px solid #fca5a5'
                        }}>
                          {daysPast} days
                        </span>
                      ) : (
                        <span style={{ color: '#9ca3af', fontSize: '13px' }}>—</span>
                      )}
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>

        {/* Empty State */}
        {filteredBills.length === 0 && (
          <div style={{ 
            padding: '48px 24px',
            textAlign: 'center',
            color: '#9ca3af'
          }}>
            <p style={{ fontSize: '16px', margin: 0 }}>
              No bills found for the selected filter
            </p>
          </div>
        )}

        {/* Summary Footer */}
        <div style={{ 
          padding: '20px 24px',
          background: 'linear-gradient(135deg, #f0f9ff 0%, #e0f2fe 100%)',
          borderTop: '2px solid #bae6fd',
          display: 'grid',
          gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))',
          gap: '16px'
        }}>
          <div>
            <div style={{ fontSize: '12px', color: '#6b7280', marginBottom: '4px' }}>
              Total Bills
            </div>
            <div style={{ fontSize: '18px', fontWeight: '700', color: '#1e40af' }}>
              {filteredBills.length}
            </div>
          </div>
          <div>
            <div style={{ fontSize: '12px', color: '#6b7280', marginBottom: '4px' }}>
              Total Amount Due
            </div>
            <div style={{ fontSize: '18px', fontWeight: '700', color: '#dc2626' }}>
              {formatCurrency(filteredBills.reduce((sum, b) => sum + (b.amount - b.paidAmount), 0))}
            </div>
          </div>
          <div>
            <div style={{ fontSize: '12px', color: '#6b7280', marginBottom: '4px' }}>
              Overdue Bills
            </div>
            <div style={{ fontSize: '18px', fontWeight: '700', color: '#ea580c' }}>
              {filteredBills.filter(b => isOverdue(b.dueDate, b.status)).length}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

const headerStyle: React.CSSProperties = {
  padding: '16px',
  textAlign: 'left',
  fontSize: '12px',
  fontWeight: '700',
  textTransform: 'uppercase',
  letterSpacing: '0.8px'
};

const dataStyle: React.CSSProperties = {
  padding: '16px',
  fontSize: '14px'
};
