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
import Link from 'next/link';
import { AgingReport } from '@/components/finance/AgingReport';
import { MetricCard } from '@/components/finance/MetricCard';
import { mockBills, mockAPAgingData } from '@/lib/finance/mock-data';
import { Bill, BillStatus } from '@/lib/finance/types';
import { formatCurrency, formatDate, getStatusColor, daysOverdue } from '@/lib/finance/utils';

export default function AccountsPayablePage() {
  const [statusFilter, setStatusFilter] = useState<BillStatus | 'All'>('All');
  const [selectedBill, setSelectedBill] = useState<Bill | null>(null);
  
  const filteredBills = statusFilter === 'All' 
    ? mockBills 
    : mockBills.filter(bill => bill.status === statusFilter);
  
  const totalAP = mockBills.filter(b => b.status !== 'Paid').reduce((sum, bill) => sum + (bill.amount - bill.paidAmount), 0);
  const overdue = mockBills.filter(b => b.status === 'Overdue').reduce((sum, bill) => sum + (bill.amount - bill.paidAmount), 0);
  const dueSoon = mockBills.filter(b => {
    if (b.status === 'Paid' || b.status === 'Overdue') return false;
    const days = daysOverdue(b.dueDate);
    return days > -8 && days <= 0;
  }).reduce((sum, bill) => sum + (bill.amount - bill.paidAmount), 0);
  const paidMTD = mockBills.filter(b => {
    if (b.status !== 'Paid') return false;
    const billDate = new Date(b.date);
    const now = new Date();
    return billDate.getMonth() === now.getMonth() && billDate.getFullYear() === now.getFullYear();
  }).reduce((sum, bill) => sum + bill.paidAmount, 0);
  
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
            📋 Accounts Payable
          </h1>
          <p style={{ color: "#94a3b8", fontSize: "16px" }}>
            Vendor bills, payments, and aging analysis
          </p>
        </div>
        
        {/* Metric Cards */}
        <div style={{
          display: "grid",
          gridTemplateColumns: "repeat(auto-fit, minmax(260px, 1fr))",
          gap: "20px",
          marginBottom: "32px"
        }}>
          <MetricCard
            title="Total AP Outstanding"
            value={formatCurrency(totalAP)}
            icon="💰"
            color="#ef4444"
          />
          <MetricCard
            title="Overdue"
            value={formatCurrency(overdue)}
            icon="⚠️"
            color="#dc2626"
          />
          <MetricCard
            title="Due Within 7 Days"
            value={formatCurrency(dueSoon)}
            icon="⏰"
            color="#f59e0b"
          />
          <MetricCard
            title="Paid MTD"
            value={formatCurrency(paidMTD)}
            icon="✅"
            color="#10b981"
          />
        </div>
        
        {/* AP Aging Chart */}
        <div style={{ marginBottom: "32px" }}>
          <AgingReport 
            data={mockAPAgingData}
            title="📊 AP Aging Report"
            type="AP"
          />
        </div>
        
        {/* Filters */}
        <div style={{
          background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
          borderRadius: "12px",
          padding: "20px",
          border: "1px solid rgba(71, 85, 105, 0.3)",
          marginBottom: "24px"
        }}>
          <div style={{ display: "flex", gap: "16px", alignItems: "center" }}>
            <label style={{ color: "#94a3b8", fontSize: "14px", fontWeight: "500" }}>
              Filter by Status:
            </label>
            <div style={{ display: "flex", gap: "8px", flexWrap: "wrap" }}>
              {['All', 'Overdue', 'Open', 'Paid'].map((status) => (
                <button
                  key={status}
                  onClick={() => setStatusFilter(status as BillStatus | 'All')}
                  style={{
                    padding: "8px 16px",
                    borderRadius: "6px",
                    border: statusFilter === status ? "none" : "1px solid rgba(71, 85, 105, 0.5)",
                    background: statusFilter === status 
                      ? "linear-gradient(135deg, #8b5cf6 0%, #7c3aed 100%)"
                      : "transparent",
                    color: statusFilter === status ? "white" : "#94a3b8",
                    fontSize: "13px",
                    fontWeight: "500",
                    cursor: "pointer"
                  }}
                >
                  {status}
                </button>
              ))}
            </div>
          </div>
        </div>
        
        {/* Bills Table */}
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
            <h3 style={{ 
              color: "#f1f5f9", 
              fontSize: "18px", 
              fontWeight: "600"
            }}>
              📄 Vendor Bills
            </h3>
            <div style={{ color: "#94a3b8", fontSize: "14px" }}>
              {filteredBills.length} bills
            </div>
          </div>
          
          <div style={{ overflowX: "auto" }}>
            <table style={{ width: "100%", borderCollapse: "collapse" }}>
              <thead>
                <tr style={{ borderBottom: "1px solid rgba(71, 85, 105, 0.3)" }}>
                  <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                    Bill #
                  </th>
                  <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                    Vendor
                  </th>
                  <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                    Description
                  </th>
                  <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                    Date
                  </th>
                  <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                    Due Date
                  </th>
                  <th style={{ padding: "12px", textAlign: "center", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                    Status
                  </th>
                  <th style={{ padding: "12px", textAlign: "right", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>
                    Amount
                  </th>
                </tr>
              </thead>
              <tbody>
                {filteredBills.map((bill) => {
                  const isOverdue = bill.status === 'Overdue';
                  const daysPastDue = isOverdue ? daysOverdue(bill.dueDate) : 0;
                  
                  return (
                    <tr 
                      key={bill.id}
                      onClick={() => setSelectedBill(bill)}
                      style={{ 
                        borderBottom: "1px solid rgba(71, 85, 105, 0.2)",
                        cursor: "pointer",
                        background: isOverdue ? "rgba(239, 68, 68, 0.05)" : "transparent"
                      }}
                      onMouseEnter={(e) => {
                        e.currentTarget.style.background = "rgba(51, 65, 85, 0.3)";
                      }}
                      onMouseLeave={(e) => {
                        e.currentTarget.style.background = isOverdue ? "rgba(239, 68, 68, 0.05)" : "transparent";
                      }}
                    >
                      <td style={{ padding: "12px", color: "#3b82f6", fontSize: "13px", fontWeight: "600" }}>
                        {bill.id}
                      </td>
                      <td style={{ padding: "12px", color: "#f1f5f9", fontSize: "13px", fontWeight: "500" }}>
                        {bill.vendorName}
                      </td>
                      <td style={{ padding: "12px", color: "#94a3b8", fontSize: "13px" }}>
                        {bill.description}
                      </td>
                      <td style={{ padding: "12px", color: "#94a3b8", fontSize: "13px" }}>
                        {formatDate(bill.date)}
                      </td>
                      <td style={{ padding: "12px", fontSize: "13px" }}>
                        <div style={{ color: isOverdue ? "#ef4444" : "#94a3b8" }}>
                          {formatDate(bill.dueDate)}
                        </div>
                        {isOverdue && (
                          <div style={{ color: "#ef4444", fontSize: "11px", marginTop: "2px" }}>
                            {daysPastDue} days overdue
                          </div>
                        )}
                      </td>
                      <td style={{ padding: "12px", textAlign: "center" }}>
                        <span style={{
                          padding: "4px 10px",
                          borderRadius: "6px",
                          background: getStatusColor(bill.status),
                          color: "white",
                          fontSize: "11px",
                          fontWeight: "500"
                        }}>
                          {bill.status}
                        </span>
                      </td>
                      <td style={{ 
                        padding: "12px",
                        color: bill.status === 'Paid' ? "#94a3b8" : "#f1f5f9",
                        fontSize: "13px",
                        fontWeight: "600",
                        textAlign: "right",
                        fontVariantNumeric: "tabular-nums"
                      }}>
                        {formatCurrency(bill.amount - bill.paidAmount)}
                      </td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          </div>
        </div>
      </div>
      
      {/* Bill Detail Modal */}
      {selectedBill && (
        <div 
          onClick={() => setSelectedBill(null)}
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
                  Bill {selectedBill.id}
                </h2>
                <div style={{ color: "#94a3b8", fontSize: "14px" }}>
                  Invoice: {selectedBill.invoiceNumber}
                </div>
              </div>
              <button
                onClick={() => setSelectedBill(null)}
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
            
            <div style={{ marginBottom: "24px" }}>
              <div style={{ color: "#94a3b8", fontSize: "12px", marginBottom: "8px" }}>Vendor</div>
              <div style={{ color: "#f1f5f9", fontSize: "16px", fontWeight: "600" }}>
                {selectedBill.vendorName}
              </div>
            </div>
            
            <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "16px", marginBottom: "24px" }}>
              <div>
                <div style={{ color: "#94a3b8", fontSize: "12px", marginBottom: "8px" }}>Bill Date</div>
                <div style={{ color: "#f1f5f9", fontSize: "14px" }}>{formatDate(selectedBill.date)}</div>
              </div>
              <div>
                <div style={{ color: "#94a3b8", fontSize: "12px", marginBottom: "8px" }}>Due Date</div>
                <div style={{ color: selectedBill.status === 'Overdue' ? "#ef4444" : "#f1f5f9", fontSize: "14px" }}>
                  {formatDate(selectedBill.dueDate)}
                </div>
              </div>
            </div>
            
            <div style={{ marginBottom: "24px" }}>
              <div style={{ color: "#94a3b8", fontSize: "12px", marginBottom: "8px" }}>Description</div>
              <div style={{ color: "#f1f5f9", fontSize: "14px" }}>{selectedBill.description}</div>
            </div>
            
            <div style={{
              padding: "20px",
              background: "rgba(51, 65, 85, 0.5)",
              borderRadius: "8px",
              marginBottom: "24px"
            }}>
              <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "8px" }}>
                <span style={{ color: "#94a3b8", fontSize: "14px" }}>Amount</span>
                <span style={{ color: "#f1f5f9", fontSize: "16px", fontWeight: "600", fontVariantNumeric: "tabular-nums" }}>
                  {formatCurrency(selectedBill.amount)}
                </span>
              </div>
              <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "8px" }}>
                <span style={{ color: "#94a3b8", fontSize: "14px" }}>Paid</span>
                <span style={{ color: "#10b981", fontSize: "16px", fontWeight: "600", fontVariantNumeric: "tabular-nums" }}>
                  {formatCurrency(selectedBill.paidAmount)}
                </span>
              </div>
              <div style={{ borderTop: "1px solid rgba(71, 85, 105, 0.3)", paddingTop: "8px", marginTop: "8px" }}>
                <div style={{ display: "flex", justifyContent: "space-between" }}>
                  <span style={{ color: "#f1f5f9", fontSize: "16px", fontWeight: "600" }}>Balance Due</span>
                  <span style={{ color: "#f1f5f9", fontSize: "20px", fontWeight: "700", fontVariantNumeric: "tabular-nums" }}>
                    {formatCurrency(selectedBill.amount - selectedBill.paidAmount)}
                  </span>
                </div>
              </div>
            </div>
            
            <div style={{ display: "flex", gap: "12px" }}>
              <button
                style={{
                  flex: 1,
                  padding: "10px 24px",
                  borderRadius: "8px",
                  border: "none",
                  background: selectedBill.status === 'Paid' 
                    ? "rgba(71, 85, 105, 0.3)"
                    : "linear-gradient(135deg, #10b981 0%, #059669 100%)",
                  color: "white",
                  fontSize: "14px",
                  fontWeight: "500",
                  cursor: selectedBill.status === 'Paid' ? "not-allowed" : "pointer"
                }}
                disabled={selectedBill.status === 'Paid'}
              >
                💳 Record Payment
              </button>
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
                📄 View Details
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
