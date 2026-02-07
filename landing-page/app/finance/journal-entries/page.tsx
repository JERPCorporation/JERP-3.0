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
import { JournalEntryForm } from '@/components/finance/JournalEntryForm';
import { JournalEntryModal } from '@/components/finance/JournalEntryModal';
import { mockJournalEntries, mockAccounts } from '@/lib/finance/mock-data';
import { JournalEntry, JournalEntryStatus } from '@/lib/finance/types';
import { formatCurrency, formatDate, getStatusColor } from '@/lib/finance/utils';

export default function JournalEntriesPage() {
  const [selectedEntry, setSelectedEntry] = useState<JournalEntry | null>(null);
  const [showForm, setShowForm] = useState(false);
  const [statusFilter, setStatusFilter] = useState<JournalEntryStatus | 'All'>('All');
  const [searchTerm, setSearchTerm] = useState('');
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');
  
  const filteredEntries = mockJournalEntries.filter(entry => {
    const matchesStatus = statusFilter === 'All' || entry.status === statusFilter;
    const matchesSearch = entry.description.toLowerCase().includes(searchTerm.toLowerCase()) ||
                         entry.id.toLowerCase().includes(searchTerm.toLowerCase());
    const matchesStartDate = !startDate || entry.date >= startDate;
    const matchesEndDate = !endDate || entry.date <= endDate;
    
    return matchesStatus && matchesSearch && matchesStartDate && matchesEndDate;
  });
  
  const totalDebits = filteredEntries.reduce((sum, entry) => 
    sum + entry.lines.reduce((lineSum, line) => lineSum + line.debit, 0), 0
  );
  
  const totalCredits = filteredEntries.reduce((sum, entry) => 
    sum + entry.lines.reduce((lineSum, line) => lineSum + line.credit, 0), 0
  );
  
  const postedCount = filteredEntries.filter(e => e.status === 'Posted').length;
  const draftCount = filteredEntries.filter(e => e.status === 'Draft').length;
  
  const handleSaveEntry = (entry: any) => {
    console.log('Saving journal entry:', entry);
    setShowForm(false);
    // In production, this would call an API to save the entry
  };
  
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
          <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
            <div>
              <h1 style={{ 
                color: "#f1f5f9", 
                fontSize: "36px", 
                fontWeight: "700", 
                marginBottom: "8px",
                display: "flex",
                alignItems: "center",
                gap: "16px"
              }}>
                📝 Journal Entries
              </h1>
              <p style={{ color: "#94a3b8", fontSize: "16px" }}>
                Double-entry accounting transactions with GAAP compliance
              </p>
            </div>
            <button
              onClick={() => setShowForm(true)}
              style={{
                padding: "12px 24px",
                borderRadius: "8px",
                border: "none",
                background: "linear-gradient(135deg, #8b5cf6 0%, #7c3aed 100%)",
                color: "white",
                fontSize: "14px",
                fontWeight: "600",
                cursor: "pointer",
                display: "flex",
                alignItems: "center",
                gap: "8px"
              }}
            >
              ➕ New Journal Entry
            </button>
          </div>
        </div>
        
        {/* Stats Cards */}
        <div style={{
          display: "grid",
          gridTemplateColumns: "repeat(auto-fit, minmax(240px, 1fr))",
          gap: "20px",
          marginBottom: "32px"
        }}>
          <div style={{
            background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
            borderRadius: "12px",
            padding: "20px",
            border: "1px solid rgba(71, 85, 105, 0.3)"
          }}>
            <div style={{ color: "#94a3b8", fontSize: "12px", marginBottom: "8px" }}>Total Entries</div>
            <div style={{ color: "#f1f5f9", fontSize: "28px", fontWeight: "700" }}>
              {filteredEntries.length}
            </div>
          </div>
          
          <div style={{
            background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
            borderRadius: "12px",
            padding: "20px",
            border: "1px solid rgba(71, 85, 105, 0.3)"
          }}>
            <div style={{ color: "#94a3b8", fontSize: "12px", marginBottom: "8px" }}>Posted</div>
            <div style={{ color: "#10b981", fontSize: "28px", fontWeight: "700" }}>
              {postedCount}
            </div>
          </div>
          
          <div style={{
            background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
            borderRadius: "12px",
            padding: "20px",
            border: "1px solid rgba(71, 85, 105, 0.3)"
          }}>
            <div style={{ color: "#94a3b8", fontSize: "12px", marginBottom: "8px" }}>Draft</div>
            <div style={{ color: "#f59e0b", fontSize: "28px", fontWeight: "700" }}>
              {draftCount}
            </div>
          </div>
          
          <div style={{
            background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
            borderRadius: "12px",
            padding: "20px",
            border: "1px solid rgba(71, 85, 105, 0.3)"
          }}>
            <div style={{ color: "#94a3b8", fontSize: "12px", marginBottom: "8px" }}>Total Debits</div>
            <div style={{ color: "#f1f5f9", fontSize: "28px", fontWeight: "700", fontVariantNumeric: "tabular-nums" }}>
              {formatCurrency(totalDebits)}
            </div>
          </div>
        </div>
        
        {/* Filters */}
        <div style={{
          background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
          borderRadius: "12px",
          padding: "20px",
          border: "1px solid rgba(71, 85, 105, 0.3)",
          marginBottom: "24px"
        }}>
          <div style={{ display: "grid", gridTemplateColumns: "2fr 1fr 1fr 1fr", gap: "16px" }}>
            <div>
              <label style={{ display: "block", color: "#94a3b8", fontSize: "12px", marginBottom: "6px" }}>
                Search
              </label>
              <input
                type="text"
                placeholder="🔍 Search by JE # or description..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
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
              <label style={{ display: "block", color: "#94a3b8", fontSize: "12px", marginBottom: "6px" }}>
                Status
              </label>
              <select
                value={statusFilter}
                onChange={(e) => setStatusFilter(e.target.value as JournalEntryStatus | 'All')}
                style={{
                  width: "100%",
                  padding: "10px 12px",
                  borderRadius: "6px",
                  border: "1px solid rgba(71, 85, 105, 0.5)",
                  background: "rgba(15, 23, 42, 0.8)",
                  color: "#f1f5f9",
                  fontSize: "14px"
                }}
              >
                <option value="All">All Status</option>
                <option value="Posted">Posted</option>
                <option value="Draft">Draft</option>
                <option value="Void">Void</option>
              </select>
            </div>
            
            <div>
              <label style={{ display: "block", color: "#94a3b8", fontSize: "12px", marginBottom: "6px" }}>
                Start Date
              </label>
              <input
                type="date"
                value={startDate}
                onChange={(e) => setStartDate(e.target.value)}
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
              <label style={{ display: "block", color: "#94a3b8", fontSize: "12px", marginBottom: "6px" }}>
                End Date
              </label>
              <input
                type="date"
                value={endDate}
                onChange={(e) => setEndDate(e.target.value)}
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
        </div>
        
        {/* Journal Entries Table */}
        {!showForm && (
          <div style={{
            background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
            borderRadius: "12px",
            padding: "24px",
            border: "1px solid rgba(71, 85, 105, 0.3)",
          }}>
            <h3 style={{ 
              color: "#f1f5f9", 
              fontSize: "18px", 
              fontWeight: "600",
              marginBottom: "20px"
            }}>
              📋 All Journal Entries
            </h3>
            
            <div style={{ overflowX: "auto" }}>
              <table style={{ width: "100%", borderCollapse: "collapse" }}>
                <thead>
                  <tr style={{ borderBottom: "1px solid rgba(71, 85, 105, 0.3)" }}>
                    <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                      JE #
                    </th>
                    <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                      Date
                    </th>
                    <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                      Description
                    </th>
                    <th style={{ padding: "12px", textAlign: "center", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                      Status
                    </th>
                    <th style={{ padding: "12px", textAlign: "right", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                      Debits
                    </th>
                    <th style={{ padding: "12px", textAlign: "right", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                      Credits
                    </th>
                  </tr>
                </thead>
                <tbody>
                  {filteredEntries.map((entry) => {
                    const totalDebit = entry.lines.reduce((sum, line) => sum + line.debit, 0);
                    const totalCredit = entry.lines.reduce((sum, line) => sum + line.credit, 0);
                    return (
                      <tr 
                        key={entry.id}
                        onClick={() => setSelectedEntry(entry)}
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
                        <td style={{ padding: "12px", color: "#3b82f6", fontSize: "13px", fontWeight: "600" }}>
                          {entry.id}
                        </td>
                        <td style={{ padding: "12px", color: "#94a3b8", fontSize: "13px" }}>
                          {formatDate(entry.date)}
                        </td>
                        <td style={{ padding: "12px", color: "#f1f5f9", fontSize: "13px" }}>
                          {entry.description}
                        </td>
                        <td style={{ padding: "12px", textAlign: "center" }}>
                          <span style={{
                            padding: "4px 10px",
                            borderRadius: "6px",
                            background: getStatusColor(entry.status),
                            color: "white",
                            fontSize: "11px",
                            fontWeight: "500"
                          }}>
                            {entry.status}
                          </span>
                        </td>
                        <td style={{ 
                          padding: "12px",
                          color: "#10b981",
                          fontSize: "13px",
                          fontWeight: "600",
                          textAlign: "right",
                          fontVariantNumeric: "tabular-nums"
                        }}>
                          {formatCurrency(totalDebit)}
                        </td>
                        <td style={{ 
                          padding: "12px",
                          color: "#ef4444",
                          fontSize: "13px",
                          fontWeight: "600",
                          textAlign: "right",
                          fontVariantNumeric: "tabular-nums"
                        }}>
                          {formatCurrency(totalCredit)}
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            </div>
          </div>
        )}
        
        {/* Journal Entry Form */}
        {showForm && (
          <JournalEntryForm
            accounts={mockAccounts}
            onSave={handleSaveEntry}
            onCancel={() => setShowForm(false)}
          />
        )}
      </div>
      
      {/* Journal Entry Modal */}
      {selectedEntry && (
        <JournalEntryModal
          entry={selectedEntry}
          onClose={() => setSelectedEntry(null)}
        />
      )}
    </div>
  );
}
