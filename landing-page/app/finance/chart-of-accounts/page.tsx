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

import { useState } from 'react';
import Link from 'next/link';
import { AccountTree } from '@/components/finance/AccountTree';
import { mockAccounts } from '@/lib/finance/mock-data';
import { Account } from '@/lib/finance/types';
import { formatCurrency } from '@/lib/finance/utils';

export default function ChartOfAccountsPage() {
  const [selectedAccount, setSelectedAccount] = useState<Account | null>(null);
  const [searchTerm, setSearchTerm] = useState('');
  
  const filteredAccounts = searchTerm
    ? mockAccounts.filter(acc => 
        acc.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
        acc.code.includes(searchTerm)
      )
    : mockAccounts;
  
  return (
    <div style={{
      minHeight: "100vh",
      background: "linear-gradient(135deg, #1e293b 0%, #334155 100%)",
      padding: "40px 20px",
      fontFamily: "'IBM Plex Sans', -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif"
    }}>
      <div style={{ maxWidth: "1400px", margin: "0 auto" }}>
        {/* Header */}
        <div style={{ marginBottom: "32px" }}>
          <Link 
            href="/finance"
            style={{
              color: "#94a3b8",
              fontSize: "14px",
              textDecoration: "none",
              display: "inline-flex",
              alignItems: "center",
              gap: "6px",
              marginBottom: "16px"
            }}
          >
            ← Back to Finance Dashboard
          </Link>
          <h1 style={{ 
            color: "#f1f5f9", 
            fontSize: "36px", 
            fontWeight: "700", 
            marginBottom: "8px",
            display: "flex",
            alignItems: "center",
            gap: "16px"
          }}>
            📊 Chart of Accounts
          </h1>
          <p style={{ color: "#94a3b8", fontSize: "16px" }}>
            Cannabis-optimized accounts with IRC §280E deductibility indicators
          </p>
        </div>
        
        {/* Search and Stats */}
        <div style={{
          display: "grid",
          gridTemplateColumns: "1fr auto",
          gap: "20px",
          marginBottom: "32px"
        }}>
          <div style={{
            background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
            borderRadius: "12px",
            padding: "20px",
            border: "1px solid rgba(71, 85, 105, 0.3)",
          }}>
            <input
              type="text"
              placeholder="🔍 Search accounts by name or code..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              style={{
                width: "100%",
                padding: "12px 16px",
                borderRadius: "8px",
                border: "1px solid rgba(71, 85, 105, 0.5)",
                background: "rgba(15, 23, 42, 0.8)",
                color: "#f1f5f9",
                fontSize: "14px"
              }}
            />
          </div>
          
          <div style={{
            background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
            borderRadius: "12px",
            padding: "20px",
            border: "1px solid rgba(71, 85, 105, 0.3)",
            minWidth: "200px"
          }}>
            <div style={{ color: "#94a3b8", fontSize: "12px", marginBottom: "6px" }}>
              Total Accounts
            </div>
            <div style={{ color: "#f1f5f9", fontSize: "28px", fontWeight: "700" }}>
              {filteredAccounts.length}
            </div>
          </div>
        </div>
        
        {/* Account Tree */}
        <AccountTree 
          accounts={filteredAccounts}
          onAccountClick={setSelectedAccount}
        />
        
        {/* 280E Legend */}
        <div style={{
          marginTop: "32px",
          background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
          borderRadius: "12px",
          padding: "24px",
          border: "1px solid rgba(71, 85, 105, 0.3)",
        }}>
          <h3 style={{ 
            color: "#f1f5f9", 
            fontSize: "18px", 
            fontWeight: "600",
            marginBottom: "16px"
          }}>
            🌿 IRC §280E Cannabis Tax Legend
          </h3>
          <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(300px, 1fr))", gap: "16px" }}>
            <div style={{ display: "flex", alignItems: "start", gap: "12px" }}>
              <span style={{ fontSize: "20px" }}>✅</span>
              <div>
                <div style={{ color: "#f1f5f9", fontSize: "14px", fontWeight: "600", marginBottom: "4px" }}>
                  280E Deductible (COGS)
                </div>
                <div style={{ color: "#94a3b8", fontSize: "12px" }}>
                  Direct costs of producing cannabis products. Fully deductible under IRC §280E.
                </div>
              </div>
            </div>
            <div style={{ display: "flex", alignItems: "start", gap: "12px" }}>
              <span style={{ fontSize: "20px" }}>❌</span>
              <div>
                <div style={{ color: "#f1f5f9", fontSize: "14px", fontWeight: "600", marginBottom: "4px" }}>
                  280E Non-Deductible (Expenses)
                </div>
                <div style={{ color: "#94a3b8", fontSize: "12px" }}>
                  Operating expenses that cannot be deducted under IRC §280E. Results in higher tax burden.
                </div>
              </div>
            </div>
            <div style={{ display: "flex", alignItems: "start", gap: "12px" }}>
              <span style={{ fontSize: "20px" }}>🌿</span>
              <div>
                <div style={{ color: "#f1f5f9", fontSize: "14px", fontWeight: "600", marginBottom: "4px" }}>
                  Cannabis-Related
                </div>
                <div style={{ color: "#94a3b8", fontSize: "12px" }}>
                  Accounts specifically for cannabis operations and inventory tracking.
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      
      {/* Account Detail Modal */}
      {selectedAccount && (
        <div 
          onClick={() => setSelectedAccount(null)}
          style={{
            position: "fixed",
            top: 0,
            left: 0,
            right: 0,
            bottom: 0,
            background: "rgba(0, 0, 0, 0.7)",
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
            zIndex: 1000,
            padding: "20px"
          }}
        >
          <div 
            onClick={(e) => e.stopPropagation()}
            style={{
              background: "linear-gradient(135deg, rgba(30, 41, 59, 0.95) 0%, rgba(15, 23, 42, 0.95) 100%)",
              borderRadius: "12px",
              padding: "32px",
              border: "1px solid rgba(71, 85, 105, 0.3)",
              maxWidth: "600px",
              width: "100%"
            }}
          >
            <div style={{ display: "flex", justifyContent: "space-between", alignItems: "start", marginBottom: "24px" }}>
              <div>
                <h2 style={{ color: "#f1f5f9", fontSize: "24px", fontWeight: "700", marginBottom: "8px" }}>
                  {selectedAccount.code} - {selectedAccount.name}
                </h2>
                <div style={{ display: "flex", gap: "8px", alignItems: "center" }}>
                  <span style={{
                    padding: "4px 12px",
                    borderRadius: "6px",
                    background: "#3b82f6",
                    color: "white",
                    fontSize: "12px",
                    fontWeight: "500"
                  }}>
                    {selectedAccount.type}
                  </span>
                  {selectedAccount.cannabisRelated && (
                    <span style={{ fontSize: "16px" }}>🌿</span>
                  )}
                  {selectedAccount.is280EDeductible !== undefined && (
                    <span style={{ fontSize: "16px" }}>
                      {selectedAccount.is280EDeductible ? '✅' : '❌'}
                    </span>
                  )}
                </div>
              </div>
              <button
                onClick={() => setSelectedAccount(null)}
                style={{
                  padding: "8px 12px",
                  borderRadius: "6px",
                  border: "none",
                  background: "rgba(71, 85, 105, 0.5)",
                  color: "#f1f5f9",
                  fontSize: "18px",
                  cursor: "pointer"
                }}
              >
                ✕
              </button>
            </div>
            
            <div style={{ 
              padding: "20px",
              background: "rgba(51, 65, 85, 0.5)",
              borderRadius: "8px",
              marginBottom: "24px"
            }}>
              <div style={{ color: "#94a3b8", fontSize: "12px", marginBottom: "6px" }}>
                Current Balance
              </div>
              <div style={{ 
                color: "#f1f5f9", 
                fontSize: "32px", 
                fontWeight: "700",
                fontVariantNumeric: "tabular-nums"
              }}>
                {formatCurrency(selectedAccount.balance)}
              </div>
            </div>
            
            {selectedAccount.description && (
              <div style={{ marginBottom: "24px" }}>
                <div style={{ color: "#94a3b8", fontSize: "12px", marginBottom: "8px", fontWeight: "600" }}>
                  Description
                </div>
                <div style={{ color: "#f1f5f9", fontSize: "14px" }}>
                  {selectedAccount.description}
                </div>
              </div>
            )}
            
            {selectedAccount.is280EDeductible !== undefined && (
              <div style={{
                padding: "16px",
                background: selectedAccount.is280EDeductible 
                  ? "rgba(16, 185, 129, 0.1)" 
                  : "rgba(239, 68, 68, 0.1)",
                border: selectedAccount.is280EDeductible
                  ? "1px solid rgba(16, 185, 129, 0.3)"
                  : "1px solid rgba(239, 68, 68, 0.3)",
                borderRadius: "8px",
                marginBottom: "24px"
              }}>
                <div style={{ 
                  color: selectedAccount.is280EDeductible ? "#10b981" : "#ef4444",
                  fontSize: "14px",
                  fontWeight: "600",
                  marginBottom: "4px"
                }}>
                  {selectedAccount.is280EDeductible ? '✅ IRC §280E Deductible' : '❌ IRC §280E Non-Deductible'}
                </div>
                <div style={{ 
                  color: selectedAccount.is280EDeductible ? "#6ee7b7" : "#fca5a5",
                  fontSize: "12px"
                }}>
                  {selectedAccount.is280EDeductible 
                    ? 'This account qualifies as Cost of Goods Sold and is deductible for cannabis businesses.'
                    : 'This account is an operating expense and cannot be deducted under IRC §280E for cannabis businesses.'}
                </div>
              </div>
            )}
            
            <div style={{ display: "flex", gap: "12px" }}>
              <button
                style={{
                  flex: 1,
                  padding: "10px 24px",
                  borderRadius: "8px",
                  border: "1px solid rgba(71, 85, 105, 0.5)",
                  background: "transparent",
                  color: "#94a3b8",
                  fontSize: "14px",
                  fontWeight: "500",
                  cursor: "pointer"
                }}
              >
                📋 View Transactions
              </button>
              <button
                style={{
                  flex: 1,
                  padding: "10px 24px",
                  borderRadius: "8px",
                  border: "none",
                  background: "linear-gradient(135deg, #8b5cf6 0%, #7c3aed 100%)",
                  color: "white",
                  fontSize: "14px",
                  fontWeight: "500",
                  cursor: "pointer"
                }}
              >
                ✏️ Edit Account
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
