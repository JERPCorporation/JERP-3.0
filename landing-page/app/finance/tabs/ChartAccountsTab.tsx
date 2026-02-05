/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, or use is strictly prohibited.
 * For licensing inquiries: licensing@jerp.io
 */

"use client";

import { useState, useMemo } from 'react';
import { mockAccounts } from '@/lib/finance/mock-data';
import { formatCurrency } from '@/lib/finance/utils';
import { StatusIndicator } from '@/components/finance/StatusIndicator';
import { NoResultsDisplay } from '@/components/finance/NoResultsDisplay';
import { Account } from '@/lib/finance/types';

const accountTypeColors: Record<string, { bg: string; text: string }> = {
  Asset: { bg: 'rgba(59, 130, 246, 0.15)', text: '#3b82f6' },
  Liability: { bg: 'rgba(239, 68, 68, 0.15)', text: '#ef4444' },
  Equity: { bg: 'rgba(139, 92, 246, 0.15)', text: '#8b5cf6' },
  Revenue: { bg: 'rgba(16, 185, 129, 0.15)', text: '#10b981' },
  COGS: { bg: 'rgba(245, 158, 11, 0.15)', text: '#f59e0b' },
  Expense: { bg: 'rgba(251, 146, 60, 0.15)', text: '#fb923c' },
};

export function ChartAccountsTab() {
  const [searchQuery, setSearchQuery] = useState('');
  const [selectedType, setSelectedType] = useState<string>('All');
  const [itemsPerPage, setItemsPerPage] = useState(25);
  const [currentPage, setCurrentPage] = useState(0);
  
  const accountTypes = ['All', 'Asset', 'Liability', 'Equity', 'Revenue', 'COGS', 'Expense'];
  
  const filteredAccounts = useMemo(() => {
    let results = [...mockAccounts];
    
    if (searchQuery) {
      const query = searchQuery.toLowerCase();
      results = results.filter(acc => 
        acc.name.toLowerCase().includes(query) || 
        acc.code.includes(query) ||
        acc.type.toLowerCase().includes(query)
      );
    }
    
    if (selectedType !== 'All') {
      results = results.filter(acc => acc.type === selectedType);
    }
    
    return results;
  }, [searchQuery, selectedType]);
  
  const paginatedAccounts = useMemo(() => {
    const startIdx = currentPage * itemsPerPage;
    return filteredAccounts.slice(startIdx, startIdx + itemsPerPage);
  }, [filteredAccounts, currentPage, itemsPerPage]);
  
  const totalPages = Math.ceil(filteredAccounts.length / itemsPerPage);
  
  return (
    <div>
      {/* Search and Filter Bar */}
      <div style={{
        background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
        borderRadius: "12px",
        padding: "20px",
        border: "1px solid rgba(71, 85, 105, 0.3)",
        marginBottom: "20px"
      }}>
        <div style={{ display: "flex", gap: "16px", flexWrap: "wrap", marginBottom: "16px" }}>
          <input
            type="text"
            value={searchQuery}
            onChange={(e) => {
              setSearchQuery(e.target.value);
              setCurrentPage(0);
            }}
            placeholder="🔍 Search by account number or name..."
            style={{
              flex: 1,
              minWidth: "300px",
              padding: "12px 16px",
              borderRadius: "8px",
              border: "1px solid rgba(71, 85, 105, 0.5)",
              background: "rgba(15, 23, 42, 0.8)",
              color: "#f1f5f9",
              fontSize: "14px",
              outline: "none"
            }}
          />
          <button
            onClick={() => alert('Add new account - Coming soon!')}
            style={{
              padding: "12px 24px",
              borderRadius: "8px",
              border: "none",
              background: "linear-gradient(135deg, #8b5cf6 0%, #7c3aed 100%)",
              color: "white",
              fontSize: "14px",
              fontWeight: "500",
              cursor: "pointer",
              whiteSpace: "nowrap"
            }}
          >
            + New Account
          </button>
        </div>
        
        {/* Type Filter Buttons */}
        <div style={{ display: "flex", gap: "8px", flexWrap: "wrap" }}>
          {accountTypes.map((type) => (
            <button
              key={type}
              onClick={() => {
                setSelectedType(type);
                setCurrentPage(0);
              }}
              style={{
                padding: "8px 16px",
                borderRadius: "6px",
                border: selectedType === type ? "1px solid rgba(139, 92, 246, 0.4)" : "1px solid rgba(71, 85, 105, 0.3)",
                background: selectedType === type ? "rgba(139, 92, 246, 0.2)" : "rgba(15, 23, 42, 0.5)",
                color: selectedType === type ? "#ffffff" : "#94a3b8",
                fontSize: "13px",
                fontWeight: "500",
                cursor: "pointer"
              }}
            >
              {type}
            </button>
          ))}
        </div>
      </div>
      
      {/* Accounts Table */}
      {paginatedAccounts.length === 0 ? (
        <div style={{
          background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
          borderRadius: "12px",
          padding: "24px",
          border: "1px solid rgba(71, 85, 105, 0.3)",
        }}>
          <NoResultsDisplay
            iconEmoji="🔍"
            headingText="No Accounts Found"
            descriptionText="Try adjusting your search or filter criteria"
            actionButton={{
              buttonText: "Clear Filters",
              onClickHandler: () => {
                setSearchQuery('');
                setSelectedType('All');
              }
            }}
          />
        </div>
      ) : (
        <div style={{
          background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
          borderRadius: "12px",
          padding: "24px",
          border: "1px solid rgba(71, 85, 105, 0.3)",
        }}>
          <div style={{ overflowX: "auto" }}>
            <table style={{ width: "100%", borderCollapse: "collapse" }}>
              <thead>
                <tr style={{ borderBottom: "2px solid rgba(71, 85, 105, 0.3)" }}>
                  <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                    Account #
                  </th>
                  <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                    Account Name
                  </th>
                  <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                    Type
                  </th>
                  <th style={{ padding: "12px", textAlign: "right", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                    Balance
                  </th>
                  <th style={{ padding: "12px", textAlign: "center", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                    Status
                  </th>
                  <th style={{ padding: "12px", textAlign: "center", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                    280E
                  </th>
                </tr>
              </thead>
              <tbody>
                {paginatedAccounts.map((account) => {
                  const typeStyle = accountTypeColors[account.type] || accountTypeColors.Asset;
                  return (
                    <tr 
                      key={account.id}
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
                      <td style={{ 
                        padding: "14px 12px", 
                        color: "#3b82f6", 
                        fontSize: "13px", 
                        fontWeight: "600",
                        fontFamily: "monospace"
                      }}>
                        {account.code}
                      </td>
                      <td style={{ padding: "14px 12px", color: "#f1f5f9", fontSize: "13px" }}>
                        <div style={{ display: "flex", alignItems: "center", gap: "8px" }}>
                          {account.icon && <span>{account.icon}</span>}
                          {account.name}
                        </div>
                      </td>
                      <td style={{ padding: "14px 12px" }}>
                        <span style={{
                          padding: "4px 10px",
                          borderRadius: "6px",
                          background: typeStyle.bg,
                          color: typeStyle.text,
                          fontSize: "11px",
                          fontWeight: "600",
                          textTransform: "uppercase"
                        }}>
                          {account.type}
                        </span>
                      </td>
                      <td style={{ 
                        padding: "14px 12px",
                        color: "#f1f5f9",
                        fontSize: "13px",
                        fontWeight: "600",
                        textAlign: "right",
                        fontVariantNumeric: "tabular-nums"
                      }}>
                        {formatCurrency(account.balance)}
                      </td>
                      <td style={{ padding: "14px 12px", textAlign: "center" }}>
                        <span style={{
                          width: "8px",
                          height: "8px",
                          borderRadius: "50%",
                          background: "#10b981",
                          display: "inline-block"
                        }} />
                      </td>
                      <td style={{ padding: "14px 12px", textAlign: "center", fontSize: "16px" }}>
                        {account.is280EDeductible !== undefined && (
                          <span title={account.is280EDeductible ? "280E Deductible" : "280E Non-Deductible"}>
                            {account.is280EDeductible ? '✅' : '❌'}
                          </span>
                        )}
                      </td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          </div>
          
          {/* Pagination Controls */}
          <div style={{
            display: "flex",
            alignItems: "center",
            justifyContent: "space-between",
            marginTop: "20px",
            paddingTop: "20px",
            borderTop: "1px solid rgba(71, 85, 105, 0.3)",
            flexWrap: "wrap",
            gap: "16px"
          }}>
            <div style={{ display: "flex", alignItems: "center", gap: "12px" }}>
              <span style={{ color: "#94a3b8", fontSize: "14px" }}>Rows per page:</span>
              <select
                value={itemsPerPage}
                onChange={(e) => {
                  setItemsPerPage(Number(e.target.value));
                  setCurrentPage(0);
                }}
                style={{
                  padding: "6px 12px",
                  borderRadius: "6px",
                  border: "1px solid rgba(71, 85, 105, 0.5)",
                  background: "rgba(15, 23, 42, 0.8)",
                  color: "#f1f5f9",
                  fontSize: "14px",
                  cursor: "pointer"
                }}
              >
                {[10, 25, 50, 100].map(size => (
                  <option key={size} value={size}>{size}</option>
                ))}
              </select>
            </div>
            
            <div style={{ color: "#94a3b8", fontSize: "14px" }}>
              {filteredAccounts.length > 0 
                ? `${currentPage * itemsPerPage + 1}-${Math.min((currentPage + 1) * itemsPerPage, filteredAccounts.length)} of ${filteredAccounts.length}`
                : '0 accounts'}
            </div>
            
            <div style={{ display: "flex", gap: "8px" }}>
              <button
                onClick={() => setCurrentPage(Math.max(0, currentPage - 1))}
                disabled={currentPage === 0}
                style={{
                  padding: "8px 12px",
                  borderRadius: "6px",
                  border: "1px solid rgba(71, 85, 105, 0.5)",
                  background: currentPage === 0 ? "rgba(15, 23, 42, 0.5)" : "rgba(15, 23, 42, 0.8)",
                  color: currentPage === 0 ? "#64748b" : "#f1f5f9",
                  fontSize: "14px",
                  cursor: currentPage === 0 ? "not-allowed" : "pointer"
                }}
              >
                Previous
              </button>
              <button
                onClick={() => setCurrentPage(Math.min(totalPages - 1, currentPage + 1))}
                disabled={currentPage >= totalPages - 1}
                style={{
                  padding: "8px 12px",
                  borderRadius: "6px",
                  border: "1px solid rgba(71, 85, 105, 0.5)",
                  background: currentPage >= totalPages - 1 ? "rgba(15, 23, 42, 0.5)" : "rgba(15, 23, 42, 0.8)",
                  color: currentPage >= totalPages - 1 ? "#64748b" : "#f1f5f9",
                  fontSize: "14px",
                  cursor: currentPage >= totalPages - 1 ? "not-allowed" : "pointer"
                }}
              >
                Next
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
