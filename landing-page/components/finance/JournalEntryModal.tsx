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

import { JournalEntry } from '@/lib/finance/types';
import { formatCurrencyDetailed, formatDate, getStatusColor } from '@/lib/finance/utils';

interface JournalEntryModalProps {
  entry: JournalEntry;
  onClose: () => void;
  onVoid?: (id: string) => void;
}

export function JournalEntryModal({ entry, onClose, onVoid }: JournalEntryModalProps) {
  const totalDebits = entry.lines.reduce((sum, line) => sum + line.debit, 0);
  const totalCredits = entry.lines.reduce((sum, line) => sum + line.credit, 0);
  
  const handlePrint = () => {
    window.print();
  };
  
  return (
    <div 
      onClick={onClose}
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
          maxWidth: "900px",
          width: "100%",
          maxHeight: "90vh",
          overflowY: "auto"
        }}
      >
        <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "24px" }}>
          <h2 style={{ color: "#f1f5f9", fontSize: "24px", fontWeight: "700", display: "flex", alignItems: "center", gap: "12px" }}>
            📝 Journal Entry #{entry.id}
            <span style={{
              padding: "4px 12px",
              borderRadius: "6px",
              background: getStatusColor(entry.status),
              color: "white",
              fontSize: "12px",
              fontWeight: "500"
            }}>
              {entry.status}
            </span>
          </h2>
          <button
            onClick={onClose}
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
          display: "grid",
          gridTemplateColumns: "repeat(2, 1fr)",
          gap: "16px",
          marginBottom: "24px",
          padding: "16px",
          background: "rgba(51, 65, 85, 0.5)",
          borderRadius: "8px"
        }}>
          <div>
            <div style={{ color: "#94a3b8", fontSize: "12px", marginBottom: "4px" }}>Date</div>
            <div style={{ color: "#f1f5f9", fontSize: "14px", fontWeight: "500" }}>
              {formatDate(entry.date)}
            </div>
          </div>
          <div>
            <div style={{ color: "#94a3b8", fontSize: "12px", marginBottom: "4px" }}>Created By</div>
            <div style={{ color: "#f1f5f9", fontSize: "14px", fontWeight: "500" }}>
              {entry.createdBy}
            </div>
          </div>
          <div style={{ gridColumn: "1 / -1" }}>
            <div style={{ color: "#94a3b8", fontSize: "12px", marginBottom: "4px" }}>Description</div>
            <div style={{ color: "#f1f5f9", fontSize: "14px", fontWeight: "500" }}>
              {entry.description}
            </div>
          </div>
        </div>
        
        <div style={{ marginBottom: "24px" }}>
          <div style={{ 
            display: "grid",
            gridTemplateColumns: "80px 2fr 2fr 1fr 1fr",
            gap: "12px",
            padding: "12px",
            background: "rgba(51, 65, 85, 0.5)",
            borderRadius: "6px",
            marginBottom: "8px"
          }}>
            <div style={{ color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>Code</div>
            <div style={{ color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>Account</div>
            <div style={{ color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>Description</div>
            <div style={{ color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase", textAlign: "right" }}>Debit</div>
            <div style={{ color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase", textAlign: "right" }}>Credit</div>
          </div>
          
          {entry.lines.map((line) => (
            <div 
              key={line.id}
              style={{ 
                display: "grid",
                gridTemplateColumns: "80px 2fr 2fr 1fr 1fr",
                gap: "12px",
                padding: "12px",
                borderBottom: "1px solid rgba(71, 85, 105, 0.3)"
              }}
            >
              <div style={{ color: "#94a3b8", fontSize: "13px", fontFamily: "monospace" }}>
                {line.accountCode}
              </div>
              <div style={{ color: "#f1f5f9", fontSize: "13px", fontWeight: "500" }}>
                {line.accountName}
              </div>
              <div style={{ color: "#94a3b8", fontSize: "13px" }}>
                {line.description}
              </div>
              <div style={{ 
                color: line.debit > 0 ? "#f1f5f9" : "#64748b",
                fontSize: "13px",
                fontWeight: line.debit > 0 ? "600" : "400",
                textAlign: "right",
                fontVariantNumeric: "tabular-nums"
              }}>
                {line.debit > 0 ? formatCurrencyDetailed(line.debit) : '-'}
              </div>
              <div style={{ 
                color: line.credit > 0 ? "#f1f5f9" : "#64748b",
                fontSize: "13px",
                fontWeight: line.credit > 0 ? "600" : "400",
                textAlign: "right",
                fontVariantNumeric: "tabular-nums"
              }}>
                {line.credit > 0 ? formatCurrencyDetailed(line.credit) : '-'}
              </div>
            </div>
          ))}
          
          <div style={{ 
            display: "grid",
            gridTemplateColumns: "80px 2fr 2fr 1fr 1fr",
            gap: "12px",
            padding: "12px",
            background: "rgba(51, 65, 85, 0.5)",
            borderRadius: "6px",
            marginTop: "8px"
          }}>
            <div></div>
            <div></div>
            <div style={{ color: "#f1f5f9", fontSize: "14px", fontWeight: "600" }}>
              TOTALS
            </div>
            <div style={{ 
              color: "#f1f5f9",
              fontSize: "14px",
              fontWeight: "700",
              textAlign: "right",
              fontVariantNumeric: "tabular-nums"
            }}>
              {formatCurrencyDetailed(totalDebits)}
            </div>
            <div style={{ 
              color: "#f1f5f9",
              fontSize: "14px",
              fontWeight: "700",
              textAlign: "right",
              fontVariantNumeric: "tabular-nums"
            }}>
              {formatCurrencyDetailed(totalCredits)}
            </div>
          </div>
        </div>
        
        {entry.postedAt && (
          <div style={{ 
            padding: "12px 16px",
            background: "rgba(16, 185, 129, 0.1)",
            border: "1px solid rgba(16, 185, 129, 0.3)",
            borderRadius: "8px",
            color: "#10b981",
            fontSize: "13px",
            marginBottom: "24px"
          }}>
            ✅ Posted on {formatDate(entry.postedAt)} at {new Date(entry.postedAt).toLocaleTimeString()}
          </div>
        )}
        
        <div style={{ display: "flex", gap: "12px", justifyContent: "flex-end" }}>
          <button
            onClick={handlePrint}
            style={{
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
            🖨️ Print
          </button>
          {entry.status === 'Posted' && onVoid && (
            <button
              onClick={() => {
                if (confirm(`Are you sure you want to void journal entry ${entry.id}?`)) {
                  onVoid(entry.id);
                  onClose();
                }
              }}
              style={{
                padding: "10px 24px",
                borderRadius: "8px",
                border: "none",
                background: "linear-gradient(135deg, #ef4444 0%, #dc2626 100%)",
                color: "white",
                fontSize: "14px",
                fontWeight: "500",
                cursor: "pointer"
              }}
            >
              🚫 Void Entry
            </button>
          )}
        </div>
      </div>
    </div>
  );
}
