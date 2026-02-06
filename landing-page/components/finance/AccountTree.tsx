/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: licensing@jerp.io
 */

"use client";

import { useState } from 'react';
import { Account } from '@/lib/finance/types';
import { formatCurrency } from '@/lib/finance/utils';

interface AccountTreeProps {
  accounts: Account[];
  onAccountClick?: (account: Account) => void;
}

export function AccountTree({ accounts, onAccountClick }: AccountTreeProps) {
  const [expandedTypes, setExpandedTypes] = useState<Set<string>>(new Set(['Asset', 'Liability', 'Revenue']));
  
  const toggleType = (type: string) => {
    const newExpanded = new Set(expandedTypes);
    if (newExpanded.has(type)) {
      newExpanded.delete(type);
    } else {
      newExpanded.add(type);
    }
    setExpandedTypes(newExpanded);
  };
  
  const accountsByType = accounts.reduce((acc, account) => {
    if (!acc[account.type]) {
      acc[account.type] = [];
    }
    acc[account.type].push(account);
    return acc;
  }, {} as Record<string, Account[]>);
  
  const typeOrder = ['Asset', 'Liability', 'Equity', 'Revenue', 'COGS', 'Expense'];
  const typeIcons: Record<string, string> = {
    'Asset': '💰',
    'Liability': '📋',
    'Equity': '🏦',
    'Revenue': '💵',
    'COGS': '📦',
    'Expense': '💳'
  };
  
  const typeRanges: Record<string, string> = {
    'Asset': '1000-1999',
    'Liability': '2000-2999',
    'Equity': '3000-3999',
    'Revenue': '4000-4999',
    'COGS': '5000-5099',
    'Expense': '5100-5999'
  };
  
  return (
    <div style={{ 
      background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
      borderRadius: "12px",
      padding: "24px",
      border: "1px solid rgba(71, 85, 105, 0.3)",
    }}>
      {typeOrder.map((type) => {
        const typeAccounts = accountsByType[type] || [];
        if (typeAccounts.length === 0) return null;
        
        const isExpanded = expandedTypes.has(type);
        const totalBalance = typeAccounts.reduce((sum, acc) => sum + acc.balance, 0);
        
        return (
          <div key={type} style={{ marginBottom: "20px" }}>
            <div
              onClick={() => toggleType(type)}
              style={{
                display: "flex",
                alignItems: "center",
                justifyContent: "space-between",
                padding: "12px 16px",
                background: "rgba(51, 65, 85, 0.5)",
                borderRadius: "8px",
                cursor: "pointer",
                marginBottom: isExpanded ? "12px" : "0"
              }}
            >
              <div style={{ display: "flex", alignItems: "center", gap: "12px" }}>
                <span style={{ fontSize: "20px" }}>{typeIcons[type]}</span>
                <div>
                  <div style={{ 
                    color: "#f1f5f9", 
                    fontSize: "16px", 
                    fontWeight: "600" 
                  }}>
                    {type.toUpperCase()} ({typeRanges[type]})
                  </div>
                  <div style={{ 
                    color: "#94a3b8", 
                    fontSize: "12px" 
                  }}>
                    {typeAccounts.length} accounts
                  </div>
                </div>
              </div>
              <div style={{ display: "flex", alignItems: "center", gap: "16px" }}>
                <div style={{ 
                  color: "#f1f5f9", 
                  fontSize: "18px", 
                  fontWeight: "600",
                  fontVariantNumeric: "tabular-nums"
                }}>
                  {formatCurrency(totalBalance)}
                </div>
                <span style={{ 
                  color: "#94a3b8", 
                  fontSize: "18px",
                  transform: isExpanded ? "rotate(90deg)" : "rotate(0deg)",
                  transition: "transform 0.2s"
                }}>
                  ▶
                </span>
              </div>
            </div>
            
            {isExpanded && (
              <div style={{ 
                marginLeft: "40px",
                borderLeft: "2px solid rgba(71, 85, 105, 0.3)",
                paddingLeft: "16px"
              }}>
                {typeAccounts.map((account) => (
                  <div
                    key={account.id}
                    onClick={() => onAccountClick && onAccountClick(account)}
                    style={{
                      display: "flex",
                      alignItems: "center",
                      justifyContent: "space-between",
                      padding: "12px 16px",
                      borderRadius: "6px",
                      cursor: "pointer",
                      transition: "background 0.2s",
                      marginBottom: "4px"
                    }}
                    onMouseEnter={(e) => {
                      e.currentTarget.style.background = "rgba(51, 65, 85, 0.3)";
                    }}
                    onMouseLeave={(e) => {
                      e.currentTarget.style.background = "transparent";
                    }}
                  >
                    <div style={{ display: "flex", alignItems: "center", gap: "12px" }}>
                      {account.icon && <span style={{ fontSize: "16px" }}>{account.icon}</span>}
                      <div>
                        <div style={{ 
                          color: "#f1f5f9", 
                          fontSize: "14px", 
                          fontWeight: "500" 
                        }}>
                          {account.code} - {account.name}
                        </div>
                        {account.description && (
                          <div style={{ 
                            color: "#94a3b8", 
                            fontSize: "12px" 
                          }}>
                            {account.description}
                          </div>
                        )}
                      </div>
                    </div>
                    <div style={{ display: "flex", alignItems: "center", gap: "12px" }}>
                      {account.is280EDeductible !== undefined && (
                        <span style={{ fontSize: "14px" }}>
                          {account.is280EDeductible ? '✅' : '❌'}
                        </span>
                      )}
                      <div style={{ 
                        color: "#f1f5f9", 
                        fontSize: "14px", 
                        fontWeight: "600",
                        fontVariantNumeric: "tabular-nums",
                        minWidth: "100px",
                        textAlign: "right"
                      }}>
                        {formatCurrency(account.balance)}
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </div>
        );
      })}
    </div>
  );
}
