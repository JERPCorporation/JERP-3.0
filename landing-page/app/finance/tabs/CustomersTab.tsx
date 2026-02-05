/**
 * JERP 3.0 - Finance Module - Customers Tab Component
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: licensing@jerp.io
 *
 * Displays customer accounts with credit management and balance tracking
 */

'use client';

import { useState } from 'react';
import { mockCustomers } from '@/lib/finance/mock-data';
import { formatCurrency } from '@/lib/finance/utils';

type Customer = typeof mockCustomers[0];

export function CustomersTab() {
  const [searchTerm, setSearchTerm] = useState('');

  const matchingCustomers = mockCustomers.filter(customer => 
    customer.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
    customer.id.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const calculateAvailableCredit = (creditLimit: number, currentBalance: number): number => {
    return creditLimit - currentBalance;
  };

  const getCreditUtilization = (creditLimit: number, currentBalance: number): number => {
    if (creditLimit === 0) return 0;
    return (currentBalance / creditLimit) * 100;
  };

  const totalReceivables = matchingCustomers.reduce((acc, cust) => acc + cust.balance, 0);
  const totalCreditLimit = matchingCustomers.reduce((acc, cust) => acc + cust.creditLimit, 0);

  return (
    <div style={{ padding: '24px' }}>
      {/* Header Area */}
      <div style={{ marginBottom: '28px' }}>
        <h2 style={{ 
          fontSize: '26px', 
          fontWeight: '700', 
          margin: '0 0 6px 0',
          background: 'linear-gradient(135deg, #1e3a8a 0%, #3b82f6 100%)',
          WebkitBackgroundClip: 'text',
          WebkitTextFillColor: 'transparent',
          backgroundClip: 'text'
        }}>
          Customer Accounts
        </h2>
        <p style={{ 
          fontSize: '14px', 
          color: '#6b7280',
          margin: 0
        }}>
          Monitor customer balances, credit limits, and available credit
        </p>
      </div>

      {/* Summary Cards */}
      <div style={{ 
        display: 'grid',
        gridTemplateColumns: 'repeat(auto-fit, minmax(240px, 1fr))',
        gap: '16px',
        marginBottom: '24px'
      }}>
        <div style={{
          background: 'linear-gradient(135deg, #dbeafe 0%, #bfdbfe 100%)',
          padding: '20px',
          borderRadius: '12px',
          border: '2px solid #93c5fd'
        }}>
          <div style={{ fontSize: '13px', color: '#1e40af', fontWeight: '600', marginBottom: '8px' }}>
            Total Customers
          </div>
          <div style={{ fontSize: '28px', fontWeight: '700', color: '#1e3a8a' }}>
            {matchingCustomers.length}
          </div>
        </div>
        <div style={{
          background: 'linear-gradient(135deg, #dcfce7 0%, #bbf7d0 100%)',
          padding: '20px',
          borderRadius: '12px',
          border: '2px solid #86efac'
        }}>
          <div style={{ fontSize: '13px', color: '#15803d', fontWeight: '600', marginBottom: '8px' }}>
            Total Receivables
          </div>
          <div style={{ fontSize: '28px', fontWeight: '700', color: '#166534' }}>
            {formatCurrency(totalReceivables)}
          </div>
        </div>
        <div style={{
          background: 'linear-gradient(135deg, #fef3c7 0%, #fde68a 100%)',
          padding: '20px',
          borderRadius: '12px',
          border: '2px solid #fcd34d'
        }}>
          <div style={{ fontSize: '13px', color: '#92400e', fontWeight: '600', marginBottom: '8px' }}>
            Total Credit Extended
          </div>
          <div style={{ fontSize: '28px', fontWeight: '700', color: '#78350f' }}>
            {formatCurrency(totalCreditLimit)}
          </div>
        </div>
      </div>

      {/* Search Input */}
      <div style={{ marginBottom: '20px' }}>
        <input
          type="text"
          placeholder="Search by customer name, contact, or customer number..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          style={{
            width: '100%',
            maxWidth: '600px',
            padding: '14px 20px',
            border: '2px solid #e5e7eb',
            borderRadius: '10px',
            fontSize: '15px',
            outline: 'none',
            transition: 'all 0.2s',
            boxShadow: '0 1px 2px rgba(0, 0, 0, 0.05)'
          }}
          onFocus={(e) => {
            e.currentTarget.style.borderColor = '#3b82f6';
            e.currentTarget.style.boxShadow = '0 0 0 3px rgba(59, 130, 246, 0.1)';
          }}
          onBlur={(e) => {
            e.currentTarget.style.borderColor = '#e5e7eb';
            e.currentTarget.style.boxShadow = '0 1px 2px rgba(0, 0, 0, 0.05)';
          }}
        />
      </div>

      {/* Customers Table */}
      <div style={{ 
        background: 'white',
        borderRadius: '16px',
        boxShadow: '0 4px 6px rgba(0, 0, 0, 0.07)',
        overflow: 'hidden',
        border: '1px solid #f3f4f6'
      }}>
        <div style={{ overflowX: 'auto' }}>
          <table style={{ 
            width: '100%', 
            borderCollapse: 'collapse',
            minWidth: '1000px'
          }}>
            <thead>
              <tr style={{ 
                background: 'linear-gradient(to right, #f9fafb 0%, #f3f4f6 100%)',
                borderBottom: '3px solid #e5e7eb'
              }}>
                <th style={tableHeaderStyle}>Customer #</th>
                <th style={tableHeaderStyle}>Company Name</th>
                <th style={tableHeaderStyle}>Contact Person</th>
                <th style={{ ...tableHeaderStyle, textAlign: 'right' }}>Current Balance</th>
                <th style={{ ...tableHeaderStyle, textAlign: 'right' }}>Credit Limit</th>
                <th style={{ ...tableHeaderStyle, textAlign: 'right' }}>Available Credit</th>
                <th style={{ ...tableHeaderStyle, textAlign: 'center' }}>Utilization</th>
              </tr>
            </thead>
            <tbody>
              {matchingCustomers.map((customer) => {
                const availableCredit = calculateAvailableCredit(customer.creditLimit, customer.balance);
                const utilization = getCreditUtilization(customer.creditLimit, customer.balance);
                const isHighUtilization = utilization > 80;
                
                return (
                  <tr 
                    key={customer.id}
                    style={{ 
                      borderBottom: '1px solid #f3f4f6',
                      transition: 'all 0.2s',
                      cursor: 'pointer'
                    }}
                    onMouseOver={(e) => {
                      e.currentTarget.style.backgroundColor = '#fafbfc';
                      e.currentTarget.style.transform = 'scale(1.005)';
                    }}
                    onMouseOut={(e) => {
                      e.currentTarget.style.backgroundColor = 'transparent';
                      e.currentTarget.style.transform = 'scale(1)';
                    }}
                  >
                    <td style={tableCellStyle}>
                      <div style={{ 
                        fontFamily: 'monospace',
                        fontSize: '13px',
                        color: '#3b82f6',
                        fontWeight: '700',
                        padding: '6px 12px',
                        background: '#eff6ff',
                        borderRadius: '6px',
                        display: 'inline-block'
                      }}>
                        {customer.id}
                      </div>
                    </td>
                    <td style={tableCellStyle}>
                      <span style={{ fontWeight: '600', color: '#111827', fontSize: '15px' }}>
                        {customer.name}
                      </span>
                    </td>
                    <td style={tableCellStyle}>
                      <span style={{ color: '#6b7280', fontSize: '14px' }}>
                      </span>
                    </td>
                    <td style={{ ...tableCellStyle, textAlign: 'right' }}>
                      <span style={{ 
                        fontWeight: '700',
                        color: customer.balance > 0 ? '#059669' : '#9ca3af',
                        fontSize: '15px'
                      }}>
                        {formatCurrency(customer.balance)}
                      </span>
                    </td>
                    <td style={{ ...tableCellStyle, textAlign: 'right' }}>
                      <span style={{ 
                        fontWeight: '600',
                        color: '#4b5563',
                        fontSize: '14px'
                      }}>
                        {formatCurrency(customer.creditLimit)}
                      </span>
                    </td>
                    <td style={{ ...tableCellStyle, textAlign: 'right' }}>
                      <span style={{ 
                        fontWeight: '700',
                        color: availableCredit < customer.creditLimit * 0.2 ? '#dc2626' : '#059669',
                        fontSize: '15px'
                      }}>
                        {formatCurrency(availableCredit)}
                      </span>
                    </td>
                    <td style={{ ...tableCellStyle, textAlign: 'center' }}>
                      <div style={{ 
                        display: 'inline-flex',
                        alignItems: 'center',
                        gap: '8px',
                        padding: '6px 14px',
                        background: isHighUtilization 
                          ? 'linear-gradient(135deg, #fee2e2 0%, #fecaca 100%)' 
                          : 'linear-gradient(135deg, #d1fae5 0%, #a7f3d0 100%)',
                        borderRadius: '20px',
                        border: isHighUtilization ? '2px solid #fca5a5' : '2px solid #6ee7b7'
                      }}>
                        <span style={{ 
                          fontWeight: '700',
                          color: isHighUtilization ? '#991b1b' : '#065f46',
                          fontSize: '13px'
                        }}>
                          {utilization.toFixed(1)}%
                        </span>
                      </div>
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>

        {/* No Results State */}
        {matchingCustomers.length === 0 && (
          <div style={{ 
            padding: '60px 24px',
            textAlign: 'center',
            color: '#9ca3af'
          }}>
            <div style={{ fontSize: '48px', marginBottom: '16px' }}>🔍</div>
            <p style={{ fontSize: '16px', margin: 0, fontWeight: '500' }}>
              No customers found matching your search
            </p>
          </div>
        )}
      </div>
    </div>
  );
}

const tableHeaderStyle: React.CSSProperties = {
  padding: '18px 20px',
  textAlign: 'left',
  fontSize: '12px',
  fontWeight: '700',
  color: '#374151',
  textTransform: 'uppercase',
  letterSpacing: '0.8px'
};

const tableCellStyle: React.CSSProperties = {
  padding: '18px 20px',
  fontSize: '14px',
  color: '#1f2937'
};
