/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: ichbincesartobar@yahoo.com
 */

'use client';

import { useState } from 'react';
import Link from 'next/link';
import { AreaChart, Area, PieChart, Pie, Cell, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, Legend } from 'recharts';
import { logout } from '@/lib/auth-mock';

// Mock data
const payrollTrendData = [
  { month: 'Jan', amount: 265000 },
  { month: 'Feb', amount: 272000 },
  { month: 'Mar', amount: 268000 },
  { month: 'Apr', amount: 278000 },
  { month: 'May', amount: 282000 },
  { month: 'Jun', amount: 285420 },
];

const departmentData = [
  { name: 'Engineering', value: 18, color: '#8b5cf6' },
  { name: 'Sales', value: 12, color: '#3b82f6' },
  { name: 'Marketing', value: 8, color: '#06b6d4' },
  { name: 'HR', value: 6, color: '#10b981' },
  { name: 'Finance', value: 4, color: '#f59e0b' },
];

const recentActivity = [
  { id: 1, action: 'Payroll processed for June 2025', user: 'System', time: '2 hours ago', type: 'success' },
  { id: 2, action: 'New employee Sarah Johnson added', user: 'Admin', time: '5 hours ago', type: 'info' },
  { id: 3, action: 'Tax compliance report generated', user: 'System', time: '1 day ago', type: 'success' },
  { id: 4, action: 'Benefits enrollment updated', user: 'HR Manager', time: '2 days ago', type: 'info' },
  { id: 5, action: 'Salary adjustment approved', user: 'Finance Director', time: '3 days ago', type: 'warning' },
];

const employees = [
  { id: 1, name: 'John Smith', department: 'Engineering', position: 'Senior Developer', salary: 95000, status: 'active', avatar: 'JS' },
  { id: 2, name: 'Sarah Johnson', department: 'Sales', position: 'Sales Manager', salary: 82000, status: 'active', avatar: 'SJ' },
  { id: 3, name: 'Michael Chen', department: 'Engineering', position: 'Team Lead', salary: 105000, status: 'active', avatar: 'MC' },
  { id: 4, name: 'Emily Davis', department: 'Marketing', position: 'Marketing Director', salary: 98000, status: 'active', avatar: 'ED' },
  { id: 5, name: 'Robert Wilson', department: 'Finance', position: 'Financial Analyst', salary: 72000, status: 'leave', avatar: 'RW' },
  { id: 6, name: 'Lisa Anderson', department: 'HR', position: 'HR Manager', salary: 78000, status: 'active', avatar: 'LA' },
  { id: 7, name: 'David Martinez', department: 'Engineering', position: 'Developer', salary: 85000, status: 'active', avatar: 'DM' },
  { id: 8, name: 'Jennifer Lee', department: 'Sales', position: 'Account Executive', salary: 68000, status: 'active', avatar: 'JL' },
  { id: 9, name: 'James Brown', department: 'Marketing', position: 'Content Manager', salary: 65000, status: 'active', avatar: 'JB' },
  { id: 10, name: 'Patricia Taylor', department: 'Finance', position: 'Accountant', salary: 62000, status: 'terminated', avatar: 'PT' },
];

const payrollRecords = [
  { id: 1, period: 'June 2025', employees: 48, amount: 285420, status: 'completed', date: '2025-06-30' },
  { id: 2, period: 'May 2025', employees: 47, amount: 282000, status: 'completed', date: '2025-05-31' },
  { id: 3, period: 'April 2025', employees: 46, amount: 278000, status: 'completed', date: '2025-04-30' },
  { id: 4, period: 'March 2025', employees: 45, amount: 268000, status: 'completed', date: '2025-03-31' },
  { id: 5, period: 'July 2025', employees: 48, amount: 104920, status: 'pending', date: '2025-07-15' },
];

const complianceCategories = [
  { name: 'Tax Withholding', score: 100, description: 'All tax withholdings up to date' },
  { name: 'Benefits', score: 95, description: 'Minor updates needed for Q3' },
  { name: 'Time Tracking', score: 98, description: 'Excellent time reporting compliance' },
  { name: 'Document Retention', score: 97, description: 'Records properly archived' },
];

const auditLog = [
  { id: 1, event: 'Tax form 941 submitted', date: '2025-06-28', user: 'System', result: 'Success' },
  { id: 2, event: 'Benefits audit completed', date: '2025-06-25', user: 'HR Manager', result: 'Passed' },
  { id: 3, event: 'Payroll compliance check', date: '2025-06-20', user: 'System', result: 'Success' },
];

export default function DashboardPage() {
  const [activeTab, setActiveTab] = useState('dashboard');
  const [showUserMenu, setShowUserMenu] = useState(false);

  const handleLogout = () => {
    logout();
    window.location.href = '/';
  };

  const styles = {
    container: {
      minHeight: '100vh',
      background: 'linear-gradient(135deg, #0f0f23 0%, #1a1a2e 50%, #16213e 100%)',
      fontFamily: '"IBM Plex Sans", -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif',
      color: '#ffffff',
    },
    header: {
      background: 'rgba(15, 15, 35, 0.8)',
      backdropFilter: 'blur(10px)',
      borderBottom: '1px solid rgba(139, 92, 246, 0.2)',
      padding: '1rem 2rem',
      position: 'sticky' as const,
      top: 0,
      zIndex: 1000,
    },
    headerContent: {
      maxWidth: '1400px',
      margin: '0 auto',
      display: 'flex',
      justifyContent: 'space-between',
      alignItems: 'center',
    },
    logo: {
      background: 'linear-gradient(135deg, #8b5cf6 0%, #3b82f6 100%)',
      WebkitBackgroundClip: 'text',
      WebkitTextFillColor: 'transparent',
      backgroundClip: 'text',
      fontSize: '1.75rem',
      fontWeight: 700,
      letterSpacing: '-0.02em',
    },
    userSection: {
      display: 'flex',
      alignItems: 'center',
      gap: '1rem',
      position: 'relative' as const,
    },
    userButton: {
      display: 'flex',
      alignItems: 'center',
      gap: '0.75rem',
      padding: '0.5rem 1rem',
      background: 'rgba(139, 92, 246, 0.1)',
      border: '1px solid rgba(139, 92, 246, 0.3)',
      borderRadius: '12px',
      color: '#ffffff',
      cursor: 'pointer',
      transition: 'all 0.3s ease',
    },
    avatar: {
      width: '36px',
      height: '36px',
      borderRadius: '50%',
      background: 'linear-gradient(135deg, #8b5cf6 0%, #3b82f6 100%)',
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center',
      fontWeight: 600,
      fontSize: '0.875rem',
    },
    dropdown: {
      position: 'absolute' as const,
      top: '100%',
      right: 0,
      marginTop: '0.5rem',
      background: 'rgba(15, 15, 35, 0.95)',
      backdropFilter: 'blur(10px)',
      border: '1px solid rgba(139, 92, 246, 0.3)',
      borderRadius: '12px',
      minWidth: '200px',
      boxShadow: '0 8px 32px rgba(0, 0, 0, 0.4)',
      overflow: 'hidden',
    },
    dropdownItem: {
      padding: '0.75rem 1rem',
      cursor: 'pointer',
      borderBottom: '1px solid rgba(139, 92, 246, 0.1)',
      transition: 'background 0.2s ease',
    },
    main: {
      maxWidth: '1400px',
      margin: '0 auto',
      padding: '2rem',
    },
    tabs: {
      display: 'flex',
      gap: '1rem',
      marginBottom: '2rem',
      borderBottom: '1px solid rgba(139, 92, 246, 0.2)',
      paddingBottom: '1rem',
    },
    tab: {
      padding: '0.75rem 1.5rem',
      background: 'transparent',
      border: 'none',
      color: '#9ca3af',
      cursor: 'pointer',
      fontSize: '1rem',
      fontWeight: 500,
      borderRadius: '8px',
      transition: 'all 0.3s ease',
    },
    tabActive: {
      background: 'linear-gradient(135deg, rgba(139, 92, 246, 0.2) 0%, rgba(59, 130, 246, 0.2) 100%)',
      color: '#ffffff',
      border: '1px solid rgba(139, 92, 246, 0.4)',
    },
    grid: {
      display: 'grid',
      gridTemplateColumns: 'repeat(auto-fit, minmax(250px, 1fr))',
      gap: '1.5rem',
      marginBottom: '2rem',
    },
    card: {
      background: 'linear-gradient(135deg, rgba(139, 92, 246, 0.1) 0%, rgba(59, 130, 246, 0.1) 100%)',
      border: '1px solid rgba(139, 92, 246, 0.3)',
      borderRadius: '16px',
      padding: '1.5rem',
      boxShadow: '0 8px 32px rgba(0, 0, 0, 0.2)',
    },
    metricLabel: {
      color: '#9ca3af',
      fontSize: '0.875rem',
      marginBottom: '0.5rem',
      fontWeight: 500,
    },
    metricValue: {
      fontSize: '2rem',
      fontWeight: 700,
      background: 'linear-gradient(135deg, #8b5cf6 0%, #3b82f6 100%)',
      WebkitBackgroundClip: 'text',
      WebkitTextFillColor: 'transparent',
      backgroundClip: 'text',
    },
    chartContainer: {
      background: 'linear-gradient(135deg, rgba(139, 92, 246, 0.1) 0%, rgba(59, 130, 246, 0.1) 100%)',
      border: '1px solid rgba(139, 92, 246, 0.3)',
      borderRadius: '16px',
      padding: '1.5rem',
      marginBottom: '1.5rem',
      boxShadow: '0 8px 32px rgba(0, 0, 0, 0.2)',
    },
    chartTitle: {
      fontSize: '1.25rem',
      fontWeight: 600,
      marginBottom: '1.5rem',
      color: '#ffffff',
    },
    table: {
      width: '100%',
      borderCollapse: 'collapse' as const,
    },
    th: {
      textAlign: 'left' as const,
      padding: '1rem',
      borderBottom: '1px solid rgba(139, 92, 246, 0.3)',
      color: '#9ca3af',
      fontSize: '0.875rem',
      fontWeight: 600,
      textTransform: 'uppercase' as const,
      letterSpacing: '0.05em',
    },
    td: {
      padding: '1rem',
      borderBottom: '1px solid rgba(139, 92, 246, 0.1)',
    },
    badge: {
      padding: '0.375rem 0.75rem',
      borderRadius: '12px',
      fontSize: '0.75rem',
      fontWeight: 600,
      display: 'inline-block',
    },
    badgeActive: {
      background: 'rgba(16, 185, 129, 0.2)',
      color: '#10b981',
      border: '1px solid rgba(16, 185, 129, 0.4)',
    },
    badgeLeave: {
      background: 'rgba(245, 158, 11, 0.2)',
      color: '#f59e0b',
      border: '1px solid rgba(245, 158, 11, 0.4)',
    },
    badgeTerminated: {
      background: 'rgba(239, 68, 68, 0.2)',
      color: '#ef4444',
      border: '1px solid rgba(239, 68, 68, 0.4)',
    },
    badgeCompleted: {
      background: 'rgba(16, 185, 129, 0.2)',
      color: '#10b981',
      border: '1px solid rgba(16, 185, 129, 0.4)',
    },
    badgePending: {
      background: 'rgba(245, 158, 11, 0.2)',
      color: '#f59e0b',
      border: '1px solid rgba(245, 158, 11, 0.4)',
    },
    badgeProcessing: {
      background: 'rgba(59, 130, 246, 0.2)',
      color: '#3b82f6',
      border: '1px solid rgba(59, 130, 246, 0.4)',
    },
    button: {
      padding: '0.75rem 1.5rem',
      background: 'linear-gradient(135deg, #8b5cf6 0%, #3b82f6 100%)',
      border: 'none',
      borderRadius: '12px',
      color: '#ffffff',
      fontSize: '0.875rem',
      fontWeight: 600,
      cursor: 'pointer',
      transition: 'all 0.3s ease',
      boxShadow: '0 4px 16px rgba(139, 92, 246, 0.3)',
    },
    activityItem: {
      padding: '1rem',
      borderBottom: '1px solid rgba(139, 92, 246, 0.1)',
      display: 'flex',
      justifyContent: 'space-between',
      alignItems: 'flex-start',
    },
    activityIcon: {
      width: '8px',
      height: '8px',
      borderRadius: '50%',
      marginRight: '0.75rem',
      marginTop: '0.375rem',
    },
    progressBar: {
      height: '8px',
      background: 'rgba(139, 92, 246, 0.2)',
      borderRadius: '4px',
      overflow: 'hidden',
      marginTop: '0.5rem',
    },
    progressFill: {
      height: '100%',
      background: 'linear-gradient(90deg, #8b5cf6 0%, #3b82f6 100%)',
      borderRadius: '4px',
      transition: 'width 0.3s ease',
    },
  };

  const getStatusBadge = (status: string) => {
    const badgeStyles = {
      active: { ...styles.badge, ...styles.badgeActive },
      leave: { ...styles.badge, ...styles.badgeLeave },
      terminated: { ...styles.badge, ...styles.badgeTerminated },
    };
    return <span style={badgeStyles[status as keyof typeof badgeStyles]}>{status.toUpperCase()}</span>;
  };

  const getPayrollStatusBadge = (status: string) => {
    const badgeStyles = {
      completed: { ...styles.badge, ...styles.badgeCompleted },
      pending: { ...styles.badge, ...styles.badgePending },
      processing: { ...styles.badge, ...styles.badgeProcessing },
    };
    return <span style={badgeStyles[status as keyof typeof badgeStyles]}>{status.toUpperCase()}</span>;
  };

  return (
    <div style={styles.container}>
      <header style={styles.header}>
        <div style={styles.headerContent}>
          <h1 style={styles.logo}>JERP Payroll</h1>
          <div style={styles.userSection}>
            <Link href="/dashboard/calculator" style={{ textDecoration: 'none' }}>
              <button style={styles.button}>💰 Salary Calculator</button>
            </Link>
            <div
              style={styles.userButton}
              onClick={() => setShowUserMenu(!showUserMenu)}
              onMouseEnter={(e) => {
                e.currentTarget.style.background = 'rgba(139, 92, 246, 0.2)';
                e.currentTarget.style.borderColor = 'rgba(139, 92, 246, 0.5)';
              }}
              onMouseLeave={(e) => {
                e.currentTarget.style.background = 'rgba(139, 92, 246, 0.1)';
                e.currentTarget.style.borderColor = 'rgba(139, 92, 246, 0.3)';
              }}
            >
              <div style={styles.avatar}>JM</div>
              <div>
                <div style={{ fontSize: '0.875rem', fontWeight: 600 }}>Julio Mendez</div>
                <div style={{ fontSize: '0.75rem', color: '#9ca3af' }}>ichbincesartobar@yahoo.com</div>
              </div>
              <svg width="16" height="16" viewBox="0 0 16 16" fill="currentColor">
                <path d="M4 6l4 4 4-4" stroke="currentColor" strokeWidth="2" fill="none" strokeLinecap="round" />
              </svg>
            </div>
            {showUserMenu && (
              <div style={styles.dropdown}>
                <div
                  style={styles.dropdownItem}
                  onClick={() => alert('Profile settings')}
                  onMouseEnter={(e) => (e.currentTarget.style.background = 'rgba(139, 92, 246, 0.1)')}
                  onMouseLeave={(e) => (e.currentTarget.style.background = 'transparent')}
                >
                  👤 Profile Settings
                </div>
                <div
                  style={styles.dropdownItem}
                  onClick={() => alert('Preferences')}
                  onMouseEnter={(e) => (e.currentTarget.style.background = 'rgba(139, 92, 246, 0.1)')}
                  onMouseLeave={(e) => (e.currentTarget.style.background = 'transparent')}
                >
                  ⚙️ Preferences
                </div>
                <div
                  style={{ ...styles.dropdownItem, borderBottom: 'none', color: '#ef4444' }}
                  onClick={handleLogout}
                  onMouseEnter={(e) => (e.currentTarget.style.background = 'rgba(239, 68, 68, 0.1)')}
                  onMouseLeave={(e) => (e.currentTarget.style.background = 'transparent')}
                >
                  🚪 Logout
                </div>
              </div>
            )}
          </div>
        </div>
      </header>

      <main style={styles.main}>
        <div style={styles.tabs}>
          {['dashboard', 'employees', 'payroll', 'compliance'].map((tab) => (
            <button
              key={tab}
              style={activeTab === tab ? { ...styles.tab, ...styles.tabActive } : styles.tab}
              onClick={() => setActiveTab(tab)}
              onMouseEnter={(e) => {
                if (activeTab !== tab) {
                  e.currentTarget.style.background = 'rgba(139, 92, 246, 0.1)';
                }
              }}
              onMouseLeave={(e) => {
                if (activeTab !== tab) {
                  e.currentTarget.style.background = 'transparent';
                }
              }}
            >
              {tab.charAt(0).toUpperCase() + tab.slice(1)}
            </button>
          ))}
        </div>

        {activeTab === 'dashboard' && (
          <>
            <div style={styles.grid}>
              <div style={styles.card}>
                <div style={styles.metricLabel}>Total Employees</div>
                <div style={styles.metricValue}>48</div>
                <div style={{ color: '#10b981', fontSize: '0.875rem', marginTop: '0.5rem' }}>+2 this month</div>
              </div>
              <div style={styles.card}>
                <div style={styles.metricLabel}>Payroll Total</div>
                <div style={styles.metricValue}>$285,420</div>
                <div style={{ color: '#10b981', fontSize: '0.875rem', marginTop: '0.5rem' }}>June 2025</div>
              </div>
              <div style={styles.card}>
                <div style={styles.metricLabel}>Average Salary</div>
                <div style={styles.metricValue}>$5,946</div>
                <div style={{ color: '#9ca3af', fontSize: '0.875rem', marginTop: '0.5rem' }}>Per employee/month</div>
              </div>
              <div style={styles.card}>
                <div style={styles.metricLabel}>Compliance Score</div>
                <div style={styles.metricValue}>98%</div>
                <div style={{ color: '#10b981', fontSize: '0.875rem', marginTop: '0.5rem' }}>Excellent</div>
              </div>
            </div>

            <div style={{ display: 'grid', gridTemplateColumns: '2fr 1fr', gap: '1.5rem', marginBottom: '1.5rem' }}>
              <div style={styles.chartContainer}>
                <h3 style={styles.chartTitle}>Payroll Trend (Jan - Jun 2025)</h3>
                <ResponsiveContainer width="100%" height={300}>
                  <AreaChart data={payrollTrendData}>
                    <defs>
                      <linearGradient id="colorAmount" x1="0" y1="0" x2="0" y2="1">
                        <stop offset="5%" stopColor="#8b5cf6" stopOpacity={0.8} />
                        <stop offset="95%" stopColor="#3b82f6" stopOpacity={0.1} />
                      </linearGradient>
                    </defs>
                    <CartesianGrid strokeDasharray="3 3" stroke="rgba(139, 92, 246, 0.1)" />
                    <XAxis dataKey="month" stroke="#9ca3af" />
                    <YAxis stroke="#9ca3af" />
                    <Tooltip
                      contentStyle={{
                        background: 'rgba(15, 15, 35, 0.95)',
                        border: '1px solid rgba(139, 92, 246, 0.3)',
                        borderRadius: '8px',
                        color: '#ffffff',
                      }}
                      formatter={(value: number) => `$${value.toLocaleString()}`}
                    />
                    <Area type="monotone" dataKey="amount" stroke="#8b5cf6" fillOpacity={1} fill="url(#colorAmount)" />
                  </AreaChart>
                </ResponsiveContainer>
              </div>

              <div style={styles.chartContainer}>
                <h3 style={styles.chartTitle}>Department Distribution</h3>
                <ResponsiveContainer width="100%" height={300}>
                  <PieChart>
                    <Pie
                      data={departmentData}
                      cx="50%"
                      cy="50%"
                      labelLine={false}
                      label={({ name, percent }) => `${name} ${(percent * 100).toFixed(0)}%`}
                      outerRadius={80}
                      fill="#8884d8"
                      dataKey="value"
                    >
                      {departmentData.map((entry, index) => (
                        <Cell key={`cell-${index}`} fill={entry.color} />
                      ))}
                    </Pie>
                    <Tooltip
                      contentStyle={{
                        background: 'rgba(15, 15, 35, 0.95)',
                        border: '1px solid rgba(139, 92, 246, 0.3)',
                        borderRadius: '8px',
                        color: '#ffffff',
                      }}
                    />
                  </PieChart>
                </ResponsiveContainer>
              </div>
            </div>

            <div style={styles.chartContainer}>
              <h3 style={styles.chartTitle}>Recent Activity</h3>
              {recentActivity.map((activity, index) => (
                <div key={activity.id} style={styles.activityItem}>
                  <div style={{ display: 'flex', flex: 1 }}>
                    <div
                      style={{
                        ...styles.activityIcon,
                        background:
                          activity.type === 'success'
                            ? '#10b981'
                            : activity.type === 'warning'
                            ? '#f59e0b'
                            : '#3b82f6',
                      }}
                    />
                    <div style={{ flex: 1 }}>
                      <div style={{ fontSize: '0.875rem', marginBottom: '0.25rem' }}>{activity.action}</div>
                      <div style={{ fontSize: '0.75rem', color: '#9ca3af' }}>
                        {activity.user} · {activity.time}
                      </div>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          </>
        )}

        {activeTab === 'employees' && (
          <div style={styles.chartContainer}>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '1.5rem' }}>
              <h3 style={styles.chartTitle}>Employee Management</h3>
              <button
                style={styles.button}
                onMouseEnter={(e) => {
                  e.currentTarget.style.transform = 'translateY(-2px)';
                  e.currentTarget.style.boxShadow = '0 6px 20px rgba(139, 92, 246, 0.4)';
                }}
                onMouseLeave={(e) => {
                  e.currentTarget.style.transform = 'translateY(0)';
                  e.currentTarget.style.boxShadow = '0 4px 16px rgba(139, 92, 246, 0.3)';
                }}
                onClick={() => alert('Add Employee form would open here')}
              >
                + Add Employee
              </button>
            </div>
            <table style={styles.table}>
              <thead>
                <tr>
                  <th style={styles.th}>Employee</th>
                  <th style={styles.th}>Department</th>
                  <th style={styles.th}>Position</th>
                  <th style={styles.th}>Salary</th>
                  <th style={styles.th}>Status</th>
                </tr>
              </thead>
              <tbody>
                {employees.map((employee) => (
                  <tr key={employee.id}>
                    <td style={styles.td}>
                      <div style={{ display: 'flex', alignItems: 'center', gap: '0.75rem' }}>
                        <div
                          style={{
                            ...styles.avatar,
                            width: '32px',
                            height: '32px',
                            fontSize: '0.75rem',
                          }}
                        >
                          {employee.avatar}
                        </div>
                        <span>{employee.name}</span>
                      </div>
                    </td>
                    <td style={styles.td}>{employee.department}</td>
                    <td style={styles.td}>{employee.position}</td>
                    <td style={styles.td}>${employee.salary.toLocaleString()}</td>
                    <td style={styles.td}>{getStatusBadge(employee.status)}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}

        {activeTab === 'payroll' && (
          <>
            <div style={styles.grid}>
              <div style={styles.card}>
                <div style={styles.metricLabel}>Paid</div>
                <div style={styles.metricValue}>$180,500</div>
                <div style={{ color: '#10b981', fontSize: '0.875rem', marginTop: '0.5rem' }}>4 periods</div>
              </div>
              <div style={styles.card}>
                <div style={styles.metricLabel}>Pending</div>
                <div style={styles.metricValue}>$104,920</div>
                <div style={{ color: '#f59e0b', fontSize: '0.875rem', marginTop: '0.5rem' }}>1 period</div>
              </div>
              <div style={styles.card}>
                <div style={styles.metricLabel}>Total</div>
                <div style={styles.metricValue}>$285,420</div>
                <div style={{ color: '#9ca3af', fontSize: '0.875rem', marginTop: '0.5rem' }}>YTD 2025</div>
              </div>
            </div>

            <div style={styles.chartContainer}>
              <h3 style={styles.chartTitle}>Recent Payroll Periods</h3>
              <table style={styles.table}>
                <thead>
                  <tr>
                    <th style={styles.th}>Period</th>
                    <th style={styles.th}>Employees</th>
                    <th style={styles.th}>Amount</th>
                    <th style={styles.th}>Date</th>
                    <th style={styles.th}>Status</th>
                  </tr>
                </thead>
                <tbody>
                  {payrollRecords.map((record) => (
                    <tr key={record.id}>
                      <td style={styles.td}>{record.period}</td>
                      <td style={styles.td}>{record.employees}</td>
                      <td style={styles.td}>${record.amount.toLocaleString()}</td>
                      <td style={styles.td}>{record.date}</td>
                      <td style={styles.td}>{getPayrollStatusBadge(record.status)}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </>
        )}

        {activeTab === 'compliance' && (
          <>
            <div style={{ ...styles.chartContainer, marginBottom: '1.5rem' }}>
              <div style={{ textAlign: 'center', padding: '2rem' }}>
                <div style={styles.metricLabel}>Overall Compliance Score</div>
                <div style={{ ...styles.metricValue, fontSize: '4rem', marginTop: '1rem', marginBottom: '1rem' }}>
                  98%
                </div>
                <div style={{ color: '#10b981', fontSize: '1rem' }}>Excellent Compliance Status</div>
              </div>
            </div>

            <div style={styles.chartContainer}>
              <h3 style={styles.chartTitle}>Compliance Categories</h3>
              {complianceCategories.map((category, index) => (
                <div key={index} style={{ marginBottom: '1.5rem' }}>
                  <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '0.5rem' }}>
                    <div>
                      <div style={{ fontSize: '0.875rem', fontWeight: 600, marginBottom: '0.25rem' }}>
                        {category.name}
                      </div>
                      <div style={{ fontSize: '0.75rem', color: '#9ca3af' }}>{category.description}</div>
                    </div>
                    <div style={{ fontSize: '1.25rem', fontWeight: 700, color: '#8b5cf6' }}>{category.score}%</div>
                  </div>
                  <div style={styles.progressBar}>
                    <div style={{ ...styles.progressFill, width: `${category.score}%` }} />
                  </div>
                </div>
              ))}
            </div>

            <div style={styles.chartContainer}>
              <h3 style={styles.chartTitle}>Recent Audit Log</h3>
              <table style={styles.table}>
                <thead>
                  <tr>
                    <th style={styles.th}>Event</th>
                    <th style={styles.th}>Date</th>
                    <th style={styles.th}>User</th>
                    <th style={styles.th}>Result</th>
                  </tr>
                </thead>
                <tbody>
                  {auditLog.map((entry) => (
                    <tr key={entry.id}>
                      <td style={styles.td}>{entry.event}</td>
                      <td style={styles.td}>{entry.date}</td>
                      <td style={styles.td}>{entry.user}</td>
                      <td style={styles.td}>
                        <span style={{ ...styles.badge, ...styles.badgeCompleted }}>{entry.result}</span>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </>
        )}
      </main>
    </div>
  );
}
