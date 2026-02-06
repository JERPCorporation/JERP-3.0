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
import Link from 'next/link';
import { mockCustomers } from '@/lib/finance/mock-data';
import { Customer } from '@/lib/finance/types';
import { formatCurrency, formatDate } from '@/lib/finance/utils';

const LICENSE_EXPIRY_WARNING_DAYS = 90;

export default function CustomersPage() {
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedCustomer, setSelectedCustomer] = useState<Customer | null>(null);
  
  const filteredCustomers = mockCustomers.filter(customer =>
    customer.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
    customer.email.toLowerCase().includes(searchTerm.toLowerCase()) ||
    (customer.cannabisLicense && customer.cannabisLicense.toLowerCase().includes(searchTerm.toLowerCase()))
  );
  
  const totalBalance = mockCustomers.reduce((sum, c) => sum + c.balance, 0);
  const totalRevenue = mockCustomers.reduce((sum, c) => sum + c.totalRevenue, 0);
  const totalCreditLimit = mockCustomers.reduce((sum, c) => sum + c.creditLimit, 0);
  
  const isLicenseExpiringSoon = (expiryDate: string) => {
    const expiry = new Date(expiryDate);
    const now = new Date();
    const daysUntilExpiry = Math.floor((expiry.getTime() - now.getTime()) / (1000 * 60 * 60 * 24));
    return daysUntilExpiry <= LICENSE_EXPIRY_WARNING_DAYS && daysUntilExpiry >= 0;
  };
  
  const isLicenseExpired = (expiryDate: string) => {
    return new Date(expiryDate) < new Date();
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
          <h1 style={{ 
            color: "#f1f5f9", 
            fontSize: "36px", 
            fontWeight: "700", 
            marginBottom: "8px",
            display: "flex",
            alignItems: "center",
            gap: "16px"
          }}>
            👥 Customers
          </h1>
          <p style={{ color: "#94a3b8", fontSize: "16px" }}>
            Customer management with cannabis license tracking
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
            <div style={{ color: "#94a3b8", fontSize: "12px", marginBottom: "8px" }}>Total Customers</div>
            <div style={{ color: "#f1f5f9", fontSize: "28px", fontWeight: "700" }}>
              {mockCustomers.length}
            </div>
          </div>
          
          <div style={{
            background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
            borderRadius: "12px",
            padding: "20px",
            border: "1px solid rgba(71, 85, 105, 0.3)"
          }}>
            <div style={{ color: "#94a3b8", fontSize: "12px", marginBottom: "8px" }}>Outstanding AR</div>
            <div style={{ color: "#3b82f6", fontSize: "28px", fontWeight: "700", fontVariantNumeric: "tabular-nums" }}>
              {formatCurrency(totalBalance)}
            </div>
          </div>
          
          <div style={{
            background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
            borderRadius: "12px",
            padding: "20px",
            border: "1px solid rgba(71, 85, 105, 0.3)"
          }}>
            <div style={{ color: "#94a3b8", fontSize: "12px", marginBottom: "8px" }}>Total Revenue YTD</div>
            <div style={{ color: "#10b981", fontSize: "28px", fontWeight: "700", fontVariantNumeric: "tabular-nums" }}>
              {formatCurrency(totalRevenue)}
            </div>
          </div>
          
          <div style={{
            background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
            borderRadius: "12px",
            padding: "20px",
            border: "1px solid rgba(71, 85, 105, 0.3)"
          }}>
            <div style={{ color: "#94a3b8", fontSize: "12px", marginBottom: "8px" }}>Total Credit Extended</div>
            <div style={{ color: "#f59e0b", fontSize: "28px", fontWeight: "700", fontVariantNumeric: "tabular-nums" }}>
              {formatCurrency(totalCreditLimit)}
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
            placeholder="🔍 Search customers by name, email, or license number..."
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
        
        {/* Customer Cards Grid */}
        <div style={{
          display: "grid",
          gridTemplateColumns: "repeat(auto-fill, minmax(380px, 1fr))",
          gap: "20px"
        }}>
          {filteredCustomers.map((customer) => {
            const creditUsage = (customer.balance / customer.creditLimit) * 100;
            const licenseExpiring = customer.licenseExpiry && isLicenseExpiringSoon(customer.licenseExpiry);
            const licenseExpired = customer.licenseExpiry && isLicenseExpired(customer.licenseExpiry);
            
            return (
              <div
                key={customer.id}
                onClick={() => setSelectedCustomer(customer)}
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
                      {customer.name}
                    </h3>
                    <div style={{ color: "#94a3b8", fontSize: "13px" }}>
                      {customer.email}
                    </div>
                  </div>
                  <div style={{ display: "flex", flexDirection: "column", gap: "4px", alignItems: "flex-end" }}>
                    {licenseExpired && (
                      <div style={{
                        padding: "4px 8px",
                        borderRadius: "4px",
                        background: "#dc2626",
                        fontSize: "10px",
                        fontWeight: "600",
                        color: "white"
                      }}>
                        LICENSE EXPIRED
                      </div>
                    )}
                    {licenseExpiring && !licenseExpired && (
                      <div style={{
                        padding: "4px 8px",
                        borderRadius: "4px",
                        background: "#f59e0b",
                        fontSize: "10px",
                        fontWeight: "600",
                        color: "white"
                      }}>
                        EXPIRES SOON
                      </div>
                    )}
                    <div style={{
                      padding: "4px 8px",
                      borderRadius: "4px",
                      background: customer.balance > 0 ? "#3b82f6" : "#10b981",
                      fontSize: "10px",
                      fontWeight: "600",
                      color: "white"
                    }}>
                      {customer.balance > 0 ? 'BALANCE DUE' : 'CURRENT'}
                    </div>
                  </div>
                </div>
                
                <div style={{ marginBottom: "16px" }}>
                  <div style={{
                    padding: "16px",
                    background: "rgba(51, 65, 85, 0.5)",
                    borderRadius: "8px"
                  }}>
                    <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "8px" }}>
                      <span style={{ color: "#94a3b8", fontSize: "12px" }}>Balance Due</span>
                      <span style={{ color: "#f1f5f9", fontSize: "16px", fontWeight: "700", fontVariantNumeric: "tabular-nums" }}>
                        {formatCurrency(customer.balance)}
                      </span>
                    </div>
                    <div style={{ display: "flex", justifyContent: "space-between" }}>
                      <span style={{ color: "#94a3b8", fontSize: "12px" }}>Total Revenue</span>
                      <span style={{ color: "#10b981", fontSize: "14px", fontWeight: "600", fontVariantNumeric: "tabular-nums" }}>
                        {formatCurrency(customer.totalRevenue)}
                      </span>
                    </div>
                  </div>
                </div>
                
                <div style={{ marginBottom: "16px" }}>
                  <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "6px" }}>
                    <span style={{ color: "#94a3b8", fontSize: "11px" }}>Credit Usage</span>
                    <span style={{ color: "#f1f5f9", fontSize: "11px", fontWeight: "600" }}>
                      {formatCurrency(customer.balance)} / {formatCurrency(customer.creditLimit)}
                    </span>
                  </div>
                  <div style={{
                    height: "6px",
                    background: "rgba(51, 65, 85, 0.5)",
                    borderRadius: "3px",
                    overflow: "hidden"
                  }}>
                    <div style={{
                      width: `${Math.min(creditUsage, 100)}%`,
                      height: "100%",
                      background: creditUsage > 90 ? "#ef4444" : creditUsage > 70 ? "#f59e0b" : "#10b981",
                      borderRadius: "3px",
                      transition: "width 0.3s"
                    }} />
                  </div>
                </div>
                
                {customer.cannabisLicense && (
                  <div style={{
                    padding: "12px",
                    background: licenseExpired 
                      ? "rgba(220, 38, 38, 0.1)"
                      : licenseExpiring 
                        ? "rgba(245, 158, 11, 0.1)"
                        : "rgba(139, 92, 246, 0.1)",
                    border: licenseExpired
                      ? "1px solid rgba(220, 38, 38, 0.3)"
                      : licenseExpiring
                        ? "1px solid rgba(245, 158, 11, 0.3)"
                        : "1px solid rgba(139, 92, 246, 0.3)",
                    borderRadius: "8px",
                    marginBottom: "12px"
                  }}>
                    <div style={{ display: "flex", alignItems: "center", gap: "8px", marginBottom: "6px" }}>
                      <span style={{ fontSize: "16px" }}>🌿</span>
                      <span style={{ 
                        color: licenseExpired ? "#dc2626" : licenseExpiring ? "#f59e0b" : "#a78bfa",
                        fontSize: "11px",
                        fontWeight: "600"
                      }}>
                        CANNABIS LICENSE
                      </span>
                    </div>
                    <div style={{ 
                      color: "#f1f5f9",
                      fontSize: "13px",
                      fontWeight: "600",
                      marginBottom: "4px",
                      fontFamily: "monospace"
                    }}>
                      {customer.cannabisLicense}
                    </div>
                    <div style={{ 
                      color: licenseExpired ? "#ef4444" : licenseExpiring ? "#f59e0b" : "#94a3b8",
                      fontSize: "11px"
                    }}>
                      Expires: {customer.licenseExpiry ? formatDate(customer.licenseExpiry) : 'N/A'}
                    </div>
                  </div>
                )}
                
                <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "12px" }}>
                  <div>
                    <div style={{ color: "#94a3b8", fontSize: "11px", marginBottom: "4px" }}>Payment Terms</div>
                    <div style={{ color: "#f1f5f9", fontSize: "13px", fontWeight: "500" }}>
                      {customer.paymentTerms}
                    </div>
                  </div>
                  <div>
                    <div style={{ color: "#94a3b8", fontSize: "11px", marginBottom: "4px" }}>Last Invoice</div>
                    <div style={{ color: "#f1f5f9", fontSize: "13px", fontWeight: "500" }}>
                      {customer.lastInvoice ? formatDate(customer.lastInvoice) : 'N/A'}
                    </div>
                  </div>
                </div>
              </div>
            );
          })}
        </div>
      </div>
      
      {/* Customer Detail Modal */}
      {selectedCustomer && (
        <div 
          onClick={() => setSelectedCustomer(null)}
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
              maxWidth: "700px",
              width: "100%",
              maxHeight: "90vh",
              overflowY: "auto"
            }}
          >
            <div style={{ display: "flex", justifyContent: "space-between", alignItems: "start", marginBottom: "24px" }}>
              <div>
                <h2 style={{ color: "#f1f5f9", fontSize: "24px", fontWeight: "700", marginBottom: "8px" }}>
                  {selectedCustomer.name}
                </h2>
                <div style={{ color: "#94a3b8", fontSize: "14px" }}>
                  Customer ID: {selectedCustomer.id}
                </div>
              </div>
              <button
                onClick={() => setSelectedCustomer(null)}
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
                <span style={{ color: "#94a3b8", fontSize: "14px" }}>Balance Due</span>
                <span style={{ color: "#f1f5f9", fontSize: "24px", fontWeight: "700", fontVariantNumeric: "tabular-nums" }}>
                  {formatCurrency(selectedCustomer.balance)}
                </span>
              </div>
              <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "12px" }}>
                <span style={{ color: "#94a3b8", fontSize: "14px" }}>Credit Limit</span>
                <span style={{ color: "#f59e0b", fontSize: "16px", fontWeight: "600", fontVariantNumeric: "tabular-nums" }}>
                  {formatCurrency(selectedCustomer.creditLimit)}
                </span>
              </div>
              <div style={{ display: "flex", justifyContent: "space-between" }}>
                <span style={{ color: "#94a3b8", fontSize: "14px" }}>Total Revenue YTD</span>
                <span style={{ color: "#10b981", fontSize: "16px", fontWeight: "600", fontVariantNumeric: "tabular-nums" }}>
                  {formatCurrency(selectedCustomer.totalRevenue)}
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
                  <div style={{ color: "#f1f5f9", fontSize: "14px" }}>{selectedCustomer.email}</div>
                </div>
                <div>
                  <div style={{ color: "#94a3b8", fontSize: "11px", marginBottom: "4px" }}>Phone</div>
                  <div style={{ color: "#f1f5f9", fontSize: "14px" }}>{selectedCustomer.phone}</div>
                </div>
                <div>
                  <div style={{ color: "#94a3b8", fontSize: "11px", marginBottom: "4px" }}>Address</div>
                  <div style={{ color: "#f1f5f9", fontSize: "14px" }}>{selectedCustomer.address}</div>
                </div>
              </div>
            </div>
            
            {selectedCustomer.cannabisLicense && (
              <div style={{ marginBottom: "24px" }}>
                <h4 style={{ color: "#f1f5f9", fontSize: "14px", fontWeight: "600", marginBottom: "12px" }}>
                  🌿 Cannabis License Information
                </h4>
                <div style={{
                  padding: "16px",
                  background: selectedCustomer.licenseExpiry && isLicenseExpired(selectedCustomer.licenseExpiry)
                    ? "rgba(220, 38, 38, 0.1)"
                    : selectedCustomer.licenseExpiry && isLicenseExpiringSoon(selectedCustomer.licenseExpiry)
                      ? "rgba(245, 158, 11, 0.1)"
                      : "rgba(139, 92, 246, 0.1)",
                  border: selectedCustomer.licenseExpiry && isLicenseExpired(selectedCustomer.licenseExpiry)
                    ? "1px solid rgba(220, 38, 38, 0.3)"
                    : selectedCustomer.licenseExpiry && isLicenseExpiringSoon(selectedCustomer.licenseExpiry)
                      ? "1px solid rgba(245, 158, 11, 0.3)"
                      : "1px solid rgba(139, 92, 246, 0.3)",
                  borderRadius: "8px"
                }}>
                  <div style={{ marginBottom: "12px" }}>
                    <div style={{ color: "#94a3b8", fontSize: "11px", marginBottom: "4px" }}>License Number</div>
                    <div style={{ 
                      color: "#f1f5f9",
                      fontSize: "16px",
                      fontWeight: "600",
                      fontFamily: "monospace"
                    }}>
                      {selectedCustomer.cannabisLicense}
                    </div>
                  </div>
                  {selectedCustomer.licenseExpiry && (
                    <div>
                      <div style={{ color: "#94a3b8", fontSize: "11px", marginBottom: "4px" }}>Expiration Date</div>
                      <div style={{ 
                        color: isLicenseExpired(selectedCustomer.licenseExpiry)
                          ? "#ef4444"
                          : isLicenseExpiringSoon(selectedCustomer.licenseExpiry)
                            ? "#f59e0b"
                            : "#f1f5f9",
                        fontSize: "14px",
                        fontWeight: "600"
                      }}>
                        {formatDate(selectedCustomer.licenseExpiry)}
                        {isLicenseExpired(selectedCustomer.licenseExpiry) && ' ⚠️ EXPIRED'}
                        {isLicenseExpiringSoon(selectedCustomer.licenseExpiry) && !isLicenseExpired(selectedCustomer.licenseExpiry) && ' ⚠️ EXPIRES SOON'}
                      </div>
                    </div>
                  )}
                </div>
              </div>
            )}
            
            <div style={{ marginBottom: "24px" }}>
              <h4 style={{ color: "#f1f5f9", fontSize: "14px", fontWeight: "600", marginBottom: "12px" }}>
                💼 Account Details
              </h4>
              <div style={{ display: "grid", gap: "12px" }}>
                <div>
                  <div style={{ color: "#94a3b8", fontSize: "11px", marginBottom: "4px" }}>Payment Terms</div>
                  <div style={{ color: "#f1f5f9", fontSize: "14px", fontWeight: "500" }}>{selectedCustomer.paymentTerms}</div>
                </div>
                <div>
                  <div style={{ color: "#94a3b8", fontSize: "11px", marginBottom: "4px" }}>Last Invoice Date</div>
                  <div style={{ color: "#f1f5f9", fontSize: "14px" }}>
                    {selectedCustomer.lastInvoice ? formatDate(selectedCustomer.lastInvoice) : 'No invoices yet'}
                  </div>
                </div>
              </div>
            </div>
            
            {selectedCustomer.notes && (
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
                  {selectedCustomer.notes}
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
                📋 View Invoices
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
                ✏️ Edit Customer
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
