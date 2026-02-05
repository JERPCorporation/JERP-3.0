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
import { FinanceDashboardTab } from './tabs/FinanceDashboardTab';
import { ChartAccountsTab } from './tabs/ChartAccountsTab';
import { JournalEntriesTab } from './tabs/JournalEntriesTab';
import { VendorsTab } from './tabs/VendorsTab';
import { BillsTab } from './tabs/BillsTab';
import { CustomersTab } from './tabs/CustomersTab';
import { InvoicesTab } from './tabs/InvoicesTab';
import { AgingReportsTab } from './tabs/AgingReportsTab';
import { FinancialReportsTab } from './tabs/FinancialReportsTab';

type FinanceTabKey = 'dashboard' | 'accounts' | 'journal' | 'vendors' | 'bills' | 'customers' | 'invoices' | 'aging' | 'reports';

interface TabConfig {
  key: FinanceTabKey;
  label: string;
  icon: string;
}

const financeTabs: TabConfig[] = [
  { key: 'dashboard', label: 'Dashboard', icon: '📊' },
  { key: 'accounts', label: 'Chart of Accounts', icon: '🏦' },
  { key: 'journal', label: 'Journal Entries', icon: '📝' },
  { key: 'vendors', label: 'Vendors', icon: '🏢' },
  { key: 'bills', label: 'Bills', icon: '📋' },
  { key: 'customers', label: 'Customers', icon: '👥' },
  { key: 'invoices', label: 'Invoices', icon: '🧾' },
  { key: 'aging', label: 'Aging Reports', icon: '⏱️' },
  { key: 'reports', label: 'Financial Reports', icon: '📈' },
];

export default function FinanceModulePage() {
  const [selectedTab, setSelectedTab] = useState<FinanceTabKey>('dashboard');
  
  const renderTabContent = () => {
    switch (selectedTab) {
      case 'dashboard':
        return <FinanceDashboardTab />;
      case 'accounts':
        return <ChartAccountsTab />;
      case 'journal':
        return <JournalEntriesTab />;
      case 'vendors':
        return <VendorsTab />;
      case 'bills':
        return <BillsTab />;
      case 'customers':
        return <CustomersTab />;
      case 'invoices':
        return <InvoicesTab />;
      case 'aging':
        return <AgingReportsTab />;
      case 'reports':
        return <FinancialReportsTab />;
      default:
        return <FinanceDashboardTab />;
    }
  };
  
  return (
    <div style={{
      minHeight: "100vh",
      background: "linear-gradient(135deg, #1e293b 0%, #334155 100%)",
      padding: "20px",
      fontFamily: "'IBM Plex Sans', -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif"
    }}>
      <div style={{ maxWidth: "1600px", margin: "0 auto" }}>
        {/* Header Section */}
        <div style={{ marginBottom: "32px" }}>
          <h1 style={{ 
            color: "#f1f5f9", 
            fontSize: "32px", 
            fontWeight: "700", 
            marginBottom: "8px",
            display: "flex",
            alignItems: "center",
            gap: "12px"
          }}>
            💰 Finance Module
          </h1>
          <p style={{ color: "#94a3b8", fontSize: "15px" }}>
            Complete financial management with GAAP compliance and IRC §280E cannabis tax optimization
          </p>
        </div>
        
        {/* Tab Navigation */}
        <div style={{
          background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
          borderRadius: "12px",
          padding: "12px",
          border: "1px solid rgba(71, 85, 105, 0.3)",
          marginBottom: "24px",
          overflowX: "auto"
        }}>
          <div style={{
            display: "flex",
            gap: "8px",
            minWidth: "max-content"
          }}>
            {financeTabs.map((tab) => {
              const isActive = selectedTab === tab.key;
              return (
                <button
                  key={tab.key}
                  onClick={() => setSelectedTab(tab.key)}
                  style={{
                    padding: "10px 16px",
                    borderRadius: "8px",
                    border: isActive ? "1px solid rgba(139, 92, 246, 0.4)" : "1px solid transparent",
                    background: isActive 
                      ? "linear-gradient(135deg, rgba(139, 92, 246, 0.2) 0%, rgba(59, 130, 246, 0.2) 100%)"
                      : "transparent",
                    color: isActive ? "#ffffff" : "#94a3b8",
                    fontSize: "14px",
                    fontWeight: isActive ? "600" : "500",
                    cursor: "pointer",
                    display: "flex",
                    alignItems: "center",
                    gap: "8px",
                    transition: "all 0.2s",
                    whiteSpace: "nowrap"
                  }}
                  onMouseEnter={(e) => {
                    if (!isActive) {
                      e.currentTarget.style.background = "rgba(71, 85, 105, 0.2)";
                      e.currentTarget.style.color = "#f1f5f9";
                    }
                  }}
                  onMouseLeave={(e) => {
                    if (!isActive) {
                      e.currentTarget.style.background = "transparent";
                      e.currentTarget.style.color = "#94a3b8";
                    }
                  }}
                >
                  <span style={{ fontSize: "16px" }}>{tab.icon}</span>
                  <span>{tab.label}</span>
                </button>
              );
            })}
          </div>
        </div>
        
        {/* Tab Content Area */}
        <div>
          {renderTabContent()}
        </div>
        
        {/* Cannabis Compliance Footer */}
        <div style={{
          marginTop: "32px",
          padding: "16px 20px",
          background: "linear-gradient(135deg, rgba(139, 92, 246, 0.1) 0%, rgba(124, 58, 237, 0.1) 100%)",
          border: "1px solid rgba(139, 92, 246, 0.3)",
          borderRadius: "8px",
          display: "flex",
          alignItems: "center",
          gap: "12px"
        }}>
          <span style={{ fontSize: "20px" }}>🌿</span>
          <div>
            <div style={{ color: "#f1f5f9", fontSize: "13px", fontWeight: "600", marginBottom: "4px" }}>
              Cannabis Tax Compliance Active
            </div>
            <div style={{ color: "#c4b5fd", fontSize: "12px" }}>
              IRC §280E tracking enabled • COGS deductions optimized • Non-deductible expenses properly categorized
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
