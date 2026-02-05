/**
 * JERP 3.0 - Finance Module - Vendors Tab Component
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: licensing@jerp.io
 *
 * Displays vendor management interface with search and filtering capabilities
 */

'use client';

import { useState } from 'react';
import { mockVendors } from '@/lib/finance/mock-data';
import { formatCurrency } from '@/lib/finance/utils';


type Vendor = typeof mockVendors[0];

export function VendorsTab() {
  const [searchQuery, setSearchQuery] = useState('');

  const filteredVendors = mockVendors.filter(vendor => 
    vendor.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
    vendor.id.toLowerCase().includes(searchQuery.toLowerCase())
  );

  const handleNewVendor = () => {
    alert('New Vendor form would open here');
  };

  return (
    <div style={{ padding: '24px' }}>
      {/* Header Section */}
      <div style={{ 
        display: 'flex', 
        justifyContent: 'space-between', 
        alignItems: 'center',
        marginBottom: '24px',
        flexWrap: 'wrap',
        gap: '16px'
      }}>
        <div>
          <h2 style={{ 
            fontSize: '24px', 
            fontWeight: '600', 
            margin: '0 0 8px 0',
            color: '#1a1a1a'
          }}>
            Vendor Management
          </h2>
          <p style={{ 
            fontSize: '14px', 
            color: '#666',
            margin: 0
          }}>
            Manage your vendor relationships and payment obligations
          </p>
        </div>
        <button
          onClick={handleNewVendor}
          style={{
            padding: '12px 24px',
            background: 'linear-gradient(135deg, #2563eb 0%, #1d4ed8 100%)',
            color: 'white',
            border: 'none',
            borderRadius: '8px',
            fontSize: '14px',
            fontWeight: '500',
            cursor: 'pointer',
            boxShadow: '0 2px 4px rgba(37, 99, 235, 0.2)',
            transition: 'all 0.2s'
          }}
          onMouseOver={(e) => {
            e.currentTarget.style.transform = 'translateY(-1px)';
            e.currentTarget.style.boxShadow = '0 4px 8px rgba(37, 99, 235, 0.3)';
          }}
          onMouseOut={(e) => {
            e.currentTarget.style.transform = 'translateY(0)';
            e.currentTarget.style.boxShadow = '0 2px 4px rgba(37, 99, 235, 0.2)';
          }}
        >
          + New Vendor
        </button>
      </div>

      {/* Search Bar */}
      <div style={{ marginBottom: '20px' }}>
        <input
          type="text"
          placeholder="Search vendors by name, contact, or vendor number..."
          value={searchQuery}
          onChange={(e) => setSearchQuery(e.target.value)}
          style={{
            width: '100%',
            maxWidth: '500px',
            padding: '12px 16px',
            border: '2px solid #e5e7eb',
            borderRadius: '8px',
            fontSize: '14px',
            outline: 'none',
            transition: 'border-color 0.2s'
          }}
          onFocus={(e) => e.currentTarget.style.borderColor = '#2563eb'}
          onBlur={(e) => e.currentTarget.style.borderColor = '#e5e7eb'}
        />
      </div>

      {/* Vendors Table */}
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
            minWidth: '900px'
          }}>
            <thead>
              <tr style={{ 
                background: 'linear-gradient(135deg, #f9fafb 0%, #f3f4f6 100%)',
                borderBottom: '2px solid #e5e7eb'
              }}>
                <th style={headerCellStyle}>Vendor #</th>
                <th style={headerCellStyle}>Company Name</th>
                <th style={headerCellStyle}>Contact Person</th>
                <th style={headerCellStyle}>Email</th>
                <th style={{ ...headerCellStyle, textAlign: 'right' }}>Current Balance</th>
                <th style={headerCellStyle}>Status</th>
              </tr>
            </thead>
            <tbody>
              {filteredVendors.map((vendor, index) => (
                <tr 
                  key={vendor.id}
                  style={{ 
                    borderBottom: '1px solid #f3f4f6',
                    transition: 'background-color 0.2s',
                    cursor: 'pointer'
                  }}
                  onMouseOver={(e) => e.currentTarget.style.backgroundColor = '#f9fafb'}
                  onMouseOut={(e) => e.currentTarget.style.backgroundColor = 'transparent'}
                >
                  <td style={cellStyle}>
                    <span style={{ 
                      fontFamily: 'monospace',
                      fontSize: '13px',
                      color: '#4b5563',
                      fontWeight: '500'
                    }}>
                      {vendor.id}
                    </span>
                  </td>
                  <td style={cellStyle}>
                    <span style={{ fontWeight: '500', color: '#1a1a1a' }}>
                      {vendor.name}
                    </span>
                  </td>
                  <td style={cellStyle}>
                    <span style={{ color: '#4b5563' }}>
                    </span>
                  </td>
                  <td style={cellStyle}>
                    <a 
                      href={`mailto:${vendor.email}`}
                      style={{ 
                        color: '#2563eb',
                        textDecoration: 'none',
                        fontSize: '14px'
                      }}
                      onMouseOver={(e) => e.currentTarget.style.textDecoration = 'underline'}
                      onMouseOut={(e) => e.currentTarget.style.textDecoration = 'none'}
                    >
                      {vendor.email}
                    </a>
                  </td>
                  <td style={{ ...cellStyle, textAlign: 'right' }}>
                    <span style={{ 
                      fontWeight: '600',
                      color: vendor.balance > 0 ? '#dc2626' : '#059669',
                      fontSize: '14px'
                    }}>
                      {formatCurrency(vendor.balance)}
                    </span>
                  </td>
                  <td style={cellStyle}>
                    <span style={{
                      display: 'inline-block',
                      padding: '4px 12px',
                      borderRadius: '12px',
                      fontSize: '11px',
                      fontWeight: '600',
                      textTransform: 'uppercase',
                      letterSpacing: '0.05em',
                      background: 'rgba(16, 185, 129, 0.15)',
                      color: '#10b981'
                    }}>
                      Active
                    </span>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        {/* Empty State */}
        {filteredVendors.length === 0 && (
          <div style={{ 
            padding: '48px 24px',
            textAlign: 'center',
            color: '#9ca3af'
          }}>
            <p style={{ fontSize: '16px', margin: 0 }}>
              No vendors found matching your search criteria
            </p>
          </div>
        )}

        {/* Summary Footer */}
        <div style={{ 
          padding: '16px 24px',
          background: 'linear-gradient(135deg, #f9fafb 0%, #f3f4f6 100%)',
          borderTop: '2px solid #e5e7eb',
          display: 'flex',
          justifyContent: 'space-between',
          alignItems: 'center',
          flexWrap: 'wrap',
          gap: '12px'
        }}>
          <span style={{ fontSize: '14px', color: '#6b7280', fontWeight: '500' }}>
            Total Vendors: {filteredVendors.length}
          </span>
          <span style={{ fontSize: '14px', color: '#1a1a1a', fontWeight: '600' }}>
            Total Outstanding: {formatCurrency(
              filteredVendors.reduce((sum, v) => sum + v.balance, 0)
            )}
          </span>
        </div>
      </div>
    </div>
  );
}

const headerCellStyle: React.CSSProperties = {
  padding: '16px',
  textAlign: 'left',
  fontSize: '13px',
  fontWeight: '600',
  color: '#374151',
  textTransform: 'uppercase',
  letterSpacing: '0.5px'
};

const cellStyle: React.CSSProperties = {
  padding: '16px',
  fontSize: '14px',
  color: '#1f2937'
};
