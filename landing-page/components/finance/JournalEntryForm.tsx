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
import { Account, JournalEntryLine } from '@/lib/finance/types';
import { validateJournalEntry, formatCurrencyDetailed } from '@/lib/finance/utils';

interface JournalEntryFormProps {
  accounts: Account[];
  onSave: (entry: {
    date: string;
    description: string;
    lines: JournalEntryLine[];
    status: 'Draft' | 'Posted';
  }) => void;
  onCancel: () => void;
}

export function JournalEntryForm({ accounts, onSave, onCancel }: JournalEntryFormProps) {
  const [date, setDate] = useState(new Date().toISOString().split('T')[0]);
  const [description, setDescription] = useState('');
  const [lines, setLines] = useState<Partial<JournalEntryLine>[]>([
    { accountId: '', description: '', debit: 0, credit: 0 },
    { accountId: '', description: '', debit: 0, credit: 0 }
  ]);
  
  const addLine = () => {
    setLines([...lines, { accountId: '', description: '', debit: 0, credit: 0 }]);
  };
  
  const removeLine = (index: number) => {
    if (lines.length > 2) {
      setLines(lines.filter((_, i) => i !== index));
    }
  };
  
  const updateLine = (index: number, field: string, value: any) => {
    const newLines = [...lines];
    newLines[index] = { ...newLines[index], [field]: value };
    setLines(newLines);
  };
  
  const totalDebits = lines.reduce((sum, line) => sum + (Number(line.debit) || 0), 0);
  const totalCredits = lines.reduce((sum, line) => sum + (Number(line.credit) || 0), 0);
  const isBalanced = validateJournalEntry(totalDebits, totalCredits);
  
  const handleSave = (status: 'Draft' | 'Posted') => {
    if (!isBalanced) {
      alert('Journal entry must be balanced (debits = credits)');
      return;
    }
    
    if (!date || !description) {
      alert('Date and description are required');
      return;
    }
    
    const validLines = lines.filter(line => line.accountId && (line.debit || line.credit));
    if (validLines.length < 2) {
      alert('At least 2 lines are required');
      return;
    }
    
    const completeLines: JournalEntryLine[] = validLines.map((line, index) => {
      const account = accounts.find(a => a.id === line.accountId)!;
      return {
        id: `L${index + 1}`,
        accountId: line.accountId!,
        accountCode: account.code,
        accountName: account.name,
        description: line.description || description,
        debit: Number(line.debit) || 0,
        credit: Number(line.credit) || 0
      };
    });
    
    onSave({ date, description, lines: completeLines, status });
  };
  
  return (
    <div style={{
      background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
      borderRadius: "12px",
      padding: "24px",
      border: "1px solid rgba(71, 85, 105, 0.3)",
      maxWidth: "1200px",
      margin: "0 auto"
    }}>
      <h2 style={{ color: "#f1f5f9", fontSize: "24px", fontWeight: "700", marginBottom: "24px" }}>
        📝 New Journal Entry
      </h2>
      
      <div style={{ marginBottom: "24px", display: "grid", gridTemplateColumns: "1fr 2fr", gap: "16px" }}>
        <div>
          <label style={{ display: "block", color: "#94a3b8", fontSize: "12px", fontWeight: "500", marginBottom: "6px" }}>
            Date *
          </label>
          <input
            type="date"
            value={date}
            onChange={(e) => setDate(e.target.value)}
            style={{
              width: "100%",
              padding: "10px 12px",
              borderRadius: "6px",
              border: "1px solid rgba(71, 85, 105, 0.5)",
              background: "rgba(15, 23, 42, 0.8)",
              color: "#f1f5f9",
              fontSize: "14px"
            }}
          />
        </div>
        <div>
          <label style={{ display: "block", color: "#94a3b8", fontSize: "12px", fontWeight: "500", marginBottom: "6px" }}>
            Description *
          </label>
          <input
            type="text"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            placeholder="e.g., Monthly rent payment"
            style={{
              width: "100%",
              padding: "10px 12px",
              borderRadius: "6px",
              border: "1px solid rgba(71, 85, 105, 0.5)",
              background: "rgba(15, 23, 42, 0.8)",
              color: "#f1f5f9",
              fontSize: "14px"
            }}
          />
        </div>
      </div>
      
      <div style={{ marginBottom: "16px" }}>
        <div style={{ 
          display: "grid", 
          gridTemplateColumns: "2fr 2fr 1fr 1fr auto",
          gap: "12px",
          padding: "12px",
          background: "rgba(51, 65, 85, 0.5)",
          borderRadius: "6px",
          marginBottom: "8px"
        }}>
          <div style={{ color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>Account</div>
          <div style={{ color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>Description</div>
          <div style={{ color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase", textAlign: "right" }}>Debit</div>
          <div style={{ color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase", textAlign: "right" }}>Credit</div>
          <div style={{ width: "32px" }}></div>
        </div>
        
        {lines.map((line, index) => (
          <div key={index} style={{ 
            display: "grid", 
            gridTemplateColumns: "2fr 2fr 1fr 1fr auto",
            gap: "12px",
            marginBottom: "8px"
          }}>
            <select
              value={line.accountId || ''}
              onChange={(e) => updateLine(index, 'accountId', e.target.value)}
              style={{
                padding: "10px 12px",
                borderRadius: "6px",
                border: "1px solid rgba(71, 85, 105, 0.5)",
                background: "rgba(15, 23, 42, 0.8)",
                color: "#f1f5f9",
                fontSize: "14px"
              }}
            >
              <option value="">Select Account</option>
              {accounts.map(account => (
                <option key={account.id} value={account.id}>
                  {account.code} - {account.name}
                </option>
              ))}
            </select>
            
            <input
              type="text"
              value={line.description || ''}
              onChange={(e) => updateLine(index, 'description', e.target.value)}
              placeholder="Line description (optional)"
              style={{
                padding: "10px 12px",
                borderRadius: "6px",
                border: "1px solid rgba(71, 85, 105, 0.5)",
                background: "rgba(15, 23, 42, 0.8)",
                color: "#f1f5f9",
                fontSize: "14px"
              }}
            />
            
            <input
              type="number"
              value={line.debit || ''}
              onChange={(e) => {
                updateLine(index, 'debit', e.target.value);
                if (e.target.value) updateLine(index, 'credit', 0);
              }}
              placeholder="0.00"
              step="0.01"
              style={{
                padding: "10px 12px",
                borderRadius: "6px",
                border: "1px solid rgba(71, 85, 105, 0.5)",
                background: "rgba(15, 23, 42, 0.8)",
                color: "#f1f5f9",
                fontSize: "14px",
                textAlign: "right",
                fontVariantNumeric: "tabular-nums"
              }}
            />
            
            <input
              type="number"
              value={line.credit || ''}
              onChange={(e) => {
                updateLine(index, 'credit', e.target.value);
                if (e.target.value) updateLine(index, 'debit', 0);
              }}
              placeholder="0.00"
              step="0.01"
              style={{
                padding: "10px 12px",
                borderRadius: "6px",
                border: "1px solid rgba(71, 85, 105, 0.5)",
                background: "rgba(15, 23, 42, 0.8)",
                color: "#f1f5f9",
                fontSize: "14px",
                textAlign: "right",
                fontVariantNumeric: "tabular-nums"
              }}
            />
            
            <button
              onClick={() => removeLine(index)}
              disabled={lines.length <= 2}
              style={{
                padding: "8px",
                borderRadius: "6px",
                border: "none",
                background: lines.length > 2 ? "#ef4444" : "rgba(71, 85, 105, 0.3)",
                color: "white",
                cursor: lines.length > 2 ? "pointer" : "not-allowed",
                fontSize: "16px"
              }}
            >
              🗑️
            </button>
          </div>
        ))}
      </div>
      
      <button
        onClick={addLine}
        style={{
          padding: "8px 16px",
          borderRadius: "6px",
          border: "1px dashed rgba(71, 85, 105, 0.5)",
          background: "transparent",
          color: "#94a3b8",
          fontSize: "14px",
          cursor: "pointer",
          marginBottom: "24px"
        }}
      >
        + Add Line
      </button>
      
      <div style={{ 
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
        padding: "16px",
        background: "rgba(51, 65, 85, 0.5)",
        borderRadius: "8px",
        marginBottom: "24px"
      }}>
        <div style={{ display: "flex", gap: "32px" }}>
          <div>
            <div style={{ color: "#94a3b8", fontSize: "12px", marginBottom: "4px" }}>Total Debits</div>
            <div style={{ color: "#f1f5f9", fontSize: "18px", fontWeight: "600", fontVariantNumeric: "tabular-nums" }}>
              {formatCurrencyDetailed(totalDebits)}
            </div>
          </div>
          <div>
            <div style={{ color: "#94a3b8", fontSize: "12px", marginBottom: "4px" }}>Total Credits</div>
            <div style={{ color: "#f1f5f9", fontSize: "18px", fontWeight: "600", fontVariantNumeric: "tabular-nums" }}>
              {formatCurrencyDetailed(totalCredits)}
            </div>
          </div>
          <div>
            <div style={{ color: "#94a3b8", fontSize: "12px", marginBottom: "4px" }}>Difference</div>
            <div style={{ 
              color: isBalanced ? "#10b981" : "#ef4444",
              fontSize: "18px", 
              fontWeight: "600",
              fontVariantNumeric: "tabular-nums"
            }}>
              {formatCurrencyDetailed(Math.abs(totalDebits - totalCredits))}
            </div>
          </div>
        </div>
        <div style={{ 
          padding: "8px 16px",
          borderRadius: "6px",
          background: isBalanced ? "#10b981" : "#ef4444",
          color: "white",
          fontSize: "14px",
          fontWeight: "600"
        }}>
          {isBalanced ? '✅ Balanced' : '⚠️ Not Balanced'}
        </div>
      </div>
      
      <div style={{ display: "flex", gap: "12px", justifyContent: "flex-end" }}>
        <button
          onClick={onCancel}
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
          Cancel
        </button>
        <button
          onClick={() => handleSave('Draft')}
          disabled={!isBalanced}
          style={{
            padding: "10px 24px",
            borderRadius: "8px",
            border: "none",
            background: isBalanced ? "linear-gradient(135deg, #f59e0b 0%, #d97706 100%)" : "rgba(71, 85, 105, 0.3)",
            color: "white",
            fontSize: "14px",
            fontWeight: "500",
            cursor: isBalanced ? "pointer" : "not-allowed"
          }}
        >
          💾 Save as Draft
        </button>
        <button
          onClick={() => handleSave('Posted')}
          disabled={!isBalanced}
          style={{
            padding: "10px 24px",
            borderRadius: "8px",
            border: "none",
            background: isBalanced ? "linear-gradient(135deg, #10b981 0%, #059669 100%)" : "rgba(71, 85, 105, 0.3)",
            color: "white",
            fontSize: "14px",
            fontWeight: "500",
            cursor: isBalanced ? "pointer" : "not-allowed"
          }}
        >
          ✅ Post Entry
        </button>
      </div>
    </div>
  );
}
