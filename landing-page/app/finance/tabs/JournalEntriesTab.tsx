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
import { mockJournalEntries } from '@/lib/finance/mock-data';
import { formatCurrency, formatDate, getStatusColor } from '@/lib/finance/utils';
import { StatusIndicator } from '@/components/finance/StatusIndicator';

export function JournalEntriesTab() {
  const [expandedEntryId, setExpandedEntryId] = useState<string | null>(null);
  
  const getStatusVariant = (status: string): any => {
    switch (status) {
      case 'Posted': return 'success';
      case 'Draft': return 'warning';
      case 'Void': return 'danger';
      default: return 'neutral';
    }
  };
  
  return (
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
        <h3 style={{ color: "#f1f5f9", fontSize: "18px", fontWeight: "600" }}>
          Journal Entries
        </h3>
        <button
          onClick={() => alert('Create new journal entry - Coming soon!')}
          style={{
            padding: "10px 20px",
            borderRadius: "8px",
            border: "none",
            background: "linear-gradient(135deg, #8b5cf6 0%, #7c3aed 100%)",
            color: "white",
            fontSize: "14px",
            fontWeight: "500",
            cursor: "pointer"
          }}
        >
          + New Entry
        </button>
      </div>
      
      <div style={{ overflowX: "auto" }}>
        <table style={{ width: "100%", borderCollapse: "collapse" }}>
          <thead>
            <tr style={{ borderBottom: "2px solid rgba(71, 85, 105, 0.3)" }}>
              <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase", width: "40px" }}></th>
              <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                JE Number
              </th>
              <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                Date
              </th>
              <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                Description
              </th>
              <th style={{ padding: "12px", textAlign: "right", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                Total Debit
              </th>
              <th style={{ padding: "12px", textAlign: "right", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                Total Credit
              </th>
              <th style={{ padding: "12px", textAlign: "center", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                Status
              </th>
            </tr>
          </thead>
          <tbody>
            {mockJournalEntries.map((entry) => {
              const totalDebit = entry.lines.reduce((sum, line) => sum + line.debit, 0);
              const totalCredit = entry.lines.reduce((sum, line) => sum + line.credit, 0);
              const isExpanded = expandedEntryId === entry.id;
              
              return (
                <>
                  <tr 
                    key={entry.id}
                    style={{ 
                      borderBottom: "1px solid rgba(71, 85, 105, 0.2)",
                      cursor: "pointer"
                    }}
                    onClick={() => setExpandedEntryId(isExpanded ? null : entry.id)}
                    onMouseEnter={(e) => {
                      e.currentTarget.style.background = "rgba(51, 65, 85, 0.3)";
                    }}
                    onMouseLeave={(e) => {
                      e.currentTarget.style.background = "transparent";
                    }}
                  >
                    <td style={{ padding: "14px 12px", color: "#94a3b8", fontSize: "16px" }}>
                      {isExpanded ? '▼' : '▶'}
                    </td>
                    <td style={{ padding: "14px 12px", color: "#3b82f6", fontSize: "13px", fontWeight: "600" }}>
                      {entry.id}
                    </td>
                    <td style={{ padding: "14px 12px", color: "#94a3b8", fontSize: "13px" }}>
                      {formatDate(entry.date)}
                    </td>
                    <td style={{ padding: "14px 12px", color: "#f1f5f9", fontSize: "13px" }}>
                      {entry.description}
                    </td>
                    <td style={{ 
                      padding: "14px 12px",
                      color: "#f1f5f9",
                      fontSize: "13px",
                      fontWeight: "600",
                      textAlign: "right",
                      fontVariantNumeric: "tabular-nums"
                    }}>
                      {formatCurrency(totalDebit)}
                    </td>
                    <td style={{ 
                      padding: "14px 12px",
                      color: "#f1f5f9",
                      fontSize: "13px",
                      fontWeight: "600",
                      textAlign: "right",
                      fontVariantNumeric: "tabular-nums"
                    }}>
                      {formatCurrency(totalCredit)}
                    </td>
                    <td style={{ padding: "14px 12px", textAlign: "center" }}>
                      <StatusIndicator 
                        label={entry.status}
                        variant={getStatusVariant(entry.status)}
                      />
                    </td>
                  </tr>
                  {isExpanded && (
                    <tr>
                      <td colSpan={7} style={{ padding: "0 12px 20px 52px", background: "rgba(51, 65, 85, 0.2)" }}>
                        <table style={{ width: "100%", borderCollapse: "collapse", marginTop: "12px" }}>
                          <thead>
                            <tr style={{ borderBottom: "1px solid rgba(71, 85, 105, 0.3)" }}>
                              <th style={{ padding: "8px", textAlign: "left", color: "#94a3b8", fontSize: "11px", fontWeight: "600", textTransform: "uppercase" }}>
                                Account
                              </th>
                              <th style={{ padding: "8px", textAlign: "right", color: "#94a3b8", fontSize: "11px", fontWeight: "600", textTransform: "uppercase" }}>
                                Debit
                              </th>
                              <th style={{ padding: "8px", textAlign: "right", color: "#94a3b8", fontSize: "11px", fontWeight: "600", textTransform: "uppercase" }}>
                                Credit
                              </th>
                            </tr>
                          </thead>
                          <tbody>
                            {entry.lines.map((line) => (
                              <tr key={line.id}>
                                <td style={{ padding: "10px 8px", color: "#f1f5f9", fontSize: "12px" }}>
                                  {line.accountCode} - {line.accountName}
                                </td>
                                <td style={{ 
                                  padding: "10px 8px",
                                  color: line.debit > 0 ? "#10b981" : "#64748b",
                                  fontSize: "12px",
                                  fontWeight: "600",
                                  textAlign: "right",
                                  fontVariantNumeric: "tabular-nums"
                                }}>
                                  {line.debit > 0 ? formatCurrency(line.debit) : '-'}
                                </td>
                                <td style={{ 
                                  padding: "10px 8px",
                                  color: line.credit > 0 ? "#ef4444" : "#64748b",
                                  fontSize: "12px",
                                  fontWeight: "600",
                                  textAlign: "right",
                                  fontVariantNumeric: "tabular-nums"
                                }}>
                                  {line.credit > 0 ? formatCurrency(line.credit) : '-'}
                                </td>
                              </tr>
                            ))}
                          </tbody>
                        </table>
                      </td>
                    </tr>
                  )}
                </>
              );
            })}
          </tbody>
        </table>
      </div>
    </div>
  );
}
