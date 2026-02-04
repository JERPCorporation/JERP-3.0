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
import { mockVendors } from '@/lib/finance/mock-data';
import { Vendor } from '@/lib/finance/types';
import { formatCurrency, formatDate } from '@/lib/finance/utils';

export default function VendorsPage() {
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedVendor, setSelectedVendor] = useState<Vendor | null>(null);
  
  const filteredVendors = mockVendors.filter(vendor =>
    vendor.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
    vendor.email.toLowerCase().includes(searchTerm.toLowerCase())
  );
  
  const totalBalance = mockVendors.reduce((sum, v) => sum + v.balance, 0);
  const totalPaid = mockVendors.reduce((sum, v) => sum + v.totalPaid, 0);
  
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
            🏢 Vendors
          </h1>
          <p style={{ color: "#94a3b8", fontSize: "16px" }}>
            Vendor management, payment terms, and compliance tracking
          </p>
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
            <div style={{ color: "#94a3b8", fontSize: "12px", marginBottom: "8px" }}>Total Vendors</div>
            <div style={{ color: "#f1f5f9", fontSize: "28px", fontWeight: "700" }}>
              {mockVendors.length}
            </div>
          </div>
          
          <div style={{
            background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
            borderRadius: "12px",
            padding: "20px",
            border: "1px solid rgba(71, 85, 105, 0.3)"
          }}>
            <div style={{ color: "#94a3b8", fontSize: "12px", marginBottom: "8px" }}>Outstanding Balance</div>
            <div style={{ color: "#ef4444", fontSize: "28px", fontWeight: "700", fontVariantNumeric: "tabular-nums" }}>
              {formatCurrency(totalBalance)}
            </div>
          </div>
          
          <div style={{
            background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
            borderRadius: "12px",
            padding: "20px",
            border: "1px solid rgba(71, 85, 105, 0.3)"
          }}>
            <div style={{ color: "#94a3b8", fontSize: "12px", marginBottom: "8px" }}>Total Paid YTD</div>
            <div style={{ color: "#10b981", fontSize: "28px", fontWeight: "700", fontVariantNumeric: "tabular-nums" }}>
              {formatCurrency(totalPaid)}
            </div>
          </div>
        </div>
        
        {/* Search */}
        <div style={{
          background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
          borderRadius: "12px",
          padding: "20px",
          border: "1px solid rgba(71, 85, 105, 0.3)",
          marginBottom: "24px"
        }}>
          <input
            type="text"
            placeholder="🔍 Search vendors by name or email..."
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
        
        {/* Vendor Cards Grid */}
        <div style={{
          display: "grid",
          gridTemplateColumns: "repeat(auto-fill, minmax(350px, 1fr))",
          gap: "20px"
        }}>
          {filteredVendors.map((vendor) => (
            <div
              key={vendor.id}
              onClick={() => setSelectedVendor(vendor)}
              style={{
                background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
                borderRadius: "12px",
                padding: "24px",
                border: "1px solid rgba(71, 85, 105, 0.3)",
                cursor: "pointer",
                transition: "all 0.2s"
              }}
              onMouseEnter={(e) => {
                e.currentTarget.style.borderColor = "rgba(139, 92, 246, 0.5)";
                e.currentTarget.style.transform = "translateY(-2px)";
              }}
              onMouseLeave={(e) => {
                e.currentTarget.style.borderColor = "rgba(71, 85, 105, 0.3)";
                e.currentTarget.style.transform = "translateY(0)";
              }}
            >
              <div style={{ display: "flex", justifyContent: "space-between", alignItems: "start", marginBottom: "16px" }}>
                <div>
                  <h3 style={{ color: "#f1f5f9", fontSize: "18px", fontWeight: "600", marginBottom: "4px" }}>
                    {vendor.name}
                  </h3>
                  <div style={{ color: "#94a3b8", fontSize: "13px" }}>
                    {vendor.email}
                  </div>
                </div>
                <div style={{
                  padding: "6px 12px",
                  borderRadius: "6px",
                  background: vendor.balance > 0 ? "#ef4444" : "#10b981",
                  fontSize: "11px",
                  fontWeight: "600",
                  color: "white"
                }}>
                  {vendor.balance > 0 ? 'BALANCE DUE' : 'PAID UP'}
                </div>
              </div>
              
              <div style={{ marginBottom: "16px" }}>
                <div style={{
                  padding: "16px",
                  background: "rgba(51, 65, 85, 0.5)",
                  borderRadius: "8px"
                }}>
                  <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "8px" }}>
                    <span style={{ color: "#94a3b8", fontSize: "12px" }}>Current Balance</span>
                    <span style={{ color: "#f1f5f9", fontSize: "16px", fontWeight: "700", fontVariantNumeric: "tabular-nums" }}>
                      {formatCurrency(vendor.balance)}
                    </span>
                  </div>
                  <div style={{ display: "flex", justifyContent: "space-between" }}>
                    <span style={{ color: "#94a3b8", fontSize: "12px" }}>Total Paid</span>
                    <span style={{ color: "#10b981", fontSize: "14px", fontWeight: "600", fontVariantNumeric: "tabular-nums" }}>
                      {formatCurrency(vendor.totalPaid)}
                    </span>
                  </div>
                </div>
              </div>
              
              <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "12px", marginBottom: "16px" }}>
                <div>
                  <div style={{ color: "#94a3b8", fontSize: "11px", marginBottom: "4px" }}>Payment Terms</div>
                  <div style={{ color: "#f1f5f9", fontSize: "13px", fontWeight: "500" }}>
                    {vendor.paymentTerms}
                  </div>
                </div>
                <div>
                  <div style={{ color: "#94a3b8", fontSize: "11px", marginBottom: "4px" }}>Last Payment</div>
                  <div style={{ color: "#f1f5f9", fontSize: "13px", fontWeight: "500" }}>
                    {vendor.lastPayment ? formatDate(vendor.lastPayment) : 'N/A'}
                  </div>
                </div>
              </div>
              
              <div style={{ display: "flex", gap: "8px" }}>
                <div style={{
                  flex: 1,
                  padding: "8px",
                  borderRadius: "6px",
                  background: vendor.w9OnFile ? "rgba(16, 185, 129, 0.2)" : "rgba(239, 68, 68, 0.2)",
                  border: `1px solid ${vendor.w9OnFile ? "rgba(16, 185, 129, 0.3)" : "rgba(239, 68, 68, 0.3)"}`,
                  textAlign: "center"
                }}>
                  <div style={{ fontSize: "16px", marginBottom: "2px" }}>
                    {vendor.w9OnFile ? '✅' : '❌'}
                  </div>
                  <div style={{ color: vendor.w9OnFile ? "#10b981" : "#ef4444", fontSize: "10px", fontWeight: "600" }}>
                    W-9
                  </div>
                </div>
                <div style={{
                  flex: 1,
                  padding: "8px",
                  borderRadius: "6px",
                  background: vendor.coiOnFile ? "rgba(16, 185, 129, 0.2)" : "rgba(239, 68, 68, 0.2)",
                  border: `1px solid ${vendor.coiOnFile ? "rgba(16, 185, 129, 0.3)" : "rgba(239, 68, 68, 0.3)"}`,
                  textAlign: "center"
                }}>
                  <div style={{ fontSize: "16px", marginBottom: "2px" }}>
                    {vendor.coiOnFile ? '✅' : '❌'}
                  </div>
                  <div style={{ color: vendor.coiOnFile ? "#10b981" : "#ef4444", fontSize: "10px", fontWeight: "600" }}>
                    COI
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>
      </div>
      
      {/* Vendor Detail Modal */}
      {selectedVendor && (
        <div 
          onClick={() => setSelectedVendor(null)}
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
                  {selectedVendor.name}
                </h2>
                <div style={{ color: "#94a3b8", fontSize: "14px" }}>
                  Vendor ID: {selectedVendor.id}
                </div>
              </div>
              <button
                onClick={() => setSelectedVendor(null)}
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
              <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "12px" }}>
                <span style={{ color: "#94a3b8", fontSize: "14px" }}>Current Balance</span>
                <span style={{ color: "#f1f5f9", fontSize: "24px", fontWeight: "700", fontVariantNumeric: "tabular-nums" }}>
                  {formatCurrency(selectedVendor.balance)}
                </span>
              </div>
              <div style={{ display: "flex", justifyContent: "space-between" }}>
                <span style={{ color: "#94a3b8", fontSize: "14px" }}>Total Paid YTD</span>
                <span style={{ color: "#10b981", fontSize: "16px", fontWeight: "600", fontVariantNumeric: "tabular-nums" }}>
                  {formatCurrency(selectedVendor.totalPaid)}
                </span>
              </div>
            </div>
            
            <div style={{ marginBottom: "24px" }}>
              <h4 style={{ color: "#f1f5f9", fontSize: "14px", fontWeight: "600", marginBottom: "12px" }}>
                📧 Contact Information
              </h4>
              <div style={{ display: "grid", gap: "12px" }}>
                <div>
                  <div style={{ color: "#94a3b8", fontSize: "11px", marginBottom: "4px" }}>Email</div>
                  <div style={{ color: "#f1f5f9", fontSize: "14px" }}>{selectedVendor.email}</div>
                </div>
                <div>
                  <div style={{ color: "#94a3b8", fontSize: "11px", marginBottom: "4px" }}>Phone</div>
                  <div style={{ color: "#f1f5f9", fontSize: "14px" }}>{selectedVendor.phone}</div>
                </div>
                <div>
                  <div style={{ color: "#94a3b8", fontSize: "11px", marginBottom: "4px" }}>Address</div>
                  <div style={{ color: "#f1f5f9", fontSize: "14px" }}>{selectedVendor.address}</div>
                </div>
              </div>
            </div>
            
            <div style={{ marginBottom: "24px" }}>
              <h4 style={{ color: "#f1f5f9", fontSize: "14px", fontWeight: "600", marginBottom: "12px" }}>
                📋 Payment Terms & Compliance
              </h4>
              <div style={{ display: "grid", gap: "12px" }}>
                <div>
                  <div style={{ color: "#94a3b8", fontSize: "11px", marginBottom: "4px" }}>Payment Terms</div>
                  <div style={{ color: "#f1f5f9", fontSize: "14px", fontWeight: "500" }}>{selectedVendor.paymentTerms}</div>
                </div>
                <div>
                  <div style={{ color: "#94a3b8", fontSize: "11px", marginBottom: "4px" }}>Last Payment Date</div>
                  <div style={{ color: "#f1f5f9", fontSize: "14px" }}>
                    {selectedVendor.lastPayment ? formatDate(selectedVendor.lastPayment) : 'No payments yet'}
                  </div>
                </div>
                <div style={{ display: "flex", gap: "12px" }}>
                  <div style={{
                    flex: 1,
                    padding: "12px",
                    borderRadius: "8px",
                    background: selectedVendor.w9OnFile ? "rgba(16, 185, 129, 0.1)" : "rgba(239, 68, 68, 0.1)",
                    border: `1px solid ${selectedVendor.w9OnFile ? "rgba(16, 185, 129, 0.3)" : "rgba(239, 68, 68, 0.3)"}`
                  }}>
                    <div style={{ fontSize: "24px", marginBottom: "8px" }}>
                      {selectedVendor.w9OnFile ? '✅' : '❌'}
                    </div>
                    <div style={{ color: selectedVendor.w9OnFile ? "#10b981" : "#ef4444", fontSize: "12px", fontWeight: "600" }}>
                      W-9 Form {selectedVendor.w9OnFile ? 'On File' : 'Missing'}
                    </div>
                  </div>
                  <div style={{
                    flex: 1,
                    padding: "12px",
                    borderRadius: "8px",
                    background: selectedVendor.coiOnFile ? "rgba(16, 185, 129, 0.1)" : "rgba(239, 68, 68, 0.1)",
                    border: `1px solid ${selectedVendor.coiOnFile ? "rgba(16, 185, 129, 0.3)" : "rgba(239, 68, 68, 0.3)"}`
                  }}>
                    <div style={{ fontSize: "24px", marginBottom: "8px" }}>
                      {selectedVendor.coiOnFile ? '✅' : '❌'}
                    </div>
                    <div style={{ color: selectedVendor.coiOnFile ? "#10b981" : "#ef4444", fontSize: "12px", fontWeight: "600" }}>
                      Insurance COI {selectedVendor.coiOnFile ? 'On File' : 'Missing'}
                    </div>
                  </div>
                </div>
              </div>
            </div>
            
            {selectedVendor.notes && (
              <div style={{ marginBottom: "24px" }}>
                <h4 style={{ color: "#f1f5f9", fontSize: "14px", fontWeight: "600", marginBottom: "8px" }}>
                  📝 Notes
                </h4>
                <div style={{
                  padding: "12px",
                  background: "rgba(51, 65, 85, 0.5)",
                  borderRadius: "8px",
                  color: "#94a3b8",
                  fontSize: "13px",
                  lineHeight: "1.6"
                }}>
                  {selectedVendor.notes}
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
                📋 View Bills
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
                ✏️ Edit Vendor
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
