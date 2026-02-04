/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: licensing@jerp.io
 */

'use client';

import React, { useState } from 'react';
import { Users, Activity, Clock, AlertTriangle, BarChart3, Server, Database, Lock, Mail } from 'lucide-react';
import { AreaChart, Area, BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';
import { MetricCard } from '@/components/admin/MetricCard';
import { UserManagementTable } from '@/components/admin/UserManagementTable';
import { AuditLogPanel } from '@/components/admin/AuditLogPanel';
import { TaxRateConfiguration } from '@/components/admin/TaxRateConfiguration';
import { SystemSettings } from '@/components/admin/SystemSettings';

// Mock Data
const mockUsers = [
  {
    id: '1',
    name: 'John Administrator',
    email: 'john@jerp.io',
    role: 'super_admin' as const,
    status: 'active' as const,
    lastLogin: '2025-02-03 14:23',
  },
  {
    id: '2',
    name: 'Sarah Manager',
    email: 'sarah@jerp.io',
    role: 'admin' as const,
    status: 'active' as const,
    lastLogin: '2025-02-03 09:15',
  },
  {
    id: '3',
    name: 'Mike Payroll',
    email: 'mike@jerp.io',
    role: 'payroll_manager' as const,
    status: 'active' as const,
    lastLogin: '2025-02-02 16:45',
  },
  {
    id: '4',
    name: 'Lisa HR',
    email: 'lisa@jerp.io',
    role: 'hr_manager' as const,
    status: 'active' as const,
    lastLogin: '2025-02-01 11:30',
  },
  {
    id: '5',
    name: 'Tom Finance',
    email: 'tom@jerp.io',
    role: 'accountant' as const,
    status: 'suspended' as const,
    lastLogin: '2025-01-28 10:00',
  },
];

const mockAuditLogs = [
  {
    id: '1',
    timestamp: '2025-02-03 14:23:15',
    action: 'payroll_processed' as const,
    resource: 'payroll_run_2025_02',
    user: 'mike@jerp.io',
    ipAddress: '192.168.1.100',
    hash: 'a7f9c2d8e1b3f4c5a8d9e0f1b2c3d4e5f6a7b8c9d0e1f2a3b4c5d6e7f8a9b0c1',
  },
  {
    id: '2',
    timestamp: '2025-02-03 11:45:32',
    action: 'employee_updated' as const,
    resource: 'employee_12345',
    user: 'lisa@jerp.io',
    ipAddress: '192.168.1.105',
    hash: 'b8c0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f7a8b9c0',
  },
  {
    id: '3',
    timestamp: '2025-02-02 16:30:00',
    action: 'compliance_check' as const,
    resource: 'tax_compliance_q1_2025',
    user: 'system',
    ipAddress: '127.0.0.1',
    hash: 'c9d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a6b7c8d9e0f1a2b3c4d5e6f7a8b9c0d1',
  },
  {
    id: '4',
    timestamp: '2025-02-02 09:15:20',
    action: 'user_created' as const,
    resource: 'user_67890',
    user: 'john@jerp.io',
    ipAddress: '192.168.1.95',
    hash: 'd0e1f2a3b4c5d6e7f8a9b0c1d2e3f4a5b6c7d8e9f0a1b2c3d4e5f6a7b8c9d0e1',
  },
  {
    id: '5',
    timestamp: '2025-02-01 13:00:45',
    action: 'deduction_modified' as const,
    resource: 'deduction_health_insurance',
    user: 'tom@jerp.io',
    ipAddress: '192.168.1.110',
    hash: 'e1f2a3b4c5d6e7f8a9b0c1d2e3f4a5b6c7d8e9f0a1b2c3d4e5f6a7b8c9d0e1f2',
  },
  {
    id: '6',
    timestamp: '2025-02-01 02:00:00',
    action: 'system_backup' as const,
    resource: 'database_backup_daily',
    user: 'system',
    ipAddress: '127.0.0.1',
    hash: 'f2a3b4c5d6e7f8a9b0c1d2e3f4a5b6c7d8e9f0a1b2c3d4e5f6a7b8c9d0e1f2a3',
  },
];

const mockMetrics = [
  { date: 'Jan 28', apiCalls: 2340, users: 45, errors: 2, responseTime: 145 },
  { date: 'Jan 29', apiCalls: 2780, users: 52, errors: 1, responseTime: 138 },
  { date: 'Jan 30', apiCalls: 2950, users: 56, errors: 0, responseTime: 128 },
  { date: 'Jan 31', apiCalls: 3120, users: 58, errors: 3, responseTime: 152 },
  { date: 'Feb 1', apiCalls: 2890, users: 54, errors: 1, responseTime: 143 },
  { date: 'Feb 2', apiCalls: 3340, users: 61, errors: 4, responseTime: 167 },
  { date: 'Feb 3', apiCalls: 3150, users: 59, errors: 2, responseTime: 149 },
];

const mockTaxRates = [
  {
    id: '1',
    name: 'Social Security (OASDI)',
    type: 'federal' as const,
    rate: 6.2,
    wageBase: '$168,600',
    appliesTo: 'Employee & Employer',
  },
  {
    id: '2',
    name: 'Medicare (HI)',
    type: 'federal' as const,
    rate: 1.45,
    wageBase: 'Unlimited',
    appliesTo: 'Employee & Employer',
  },
  {
    id: '3',
    name: 'Additional Medicare',
    type: 'federal' as const,
    rate: 0.9,
    wageBase: 'Above $200,000',
    appliesTo: 'Employee only',
  },
  {
    id: '4',
    name: 'FUTA',
    type: 'federal' as const,
    rate: 0.6,
    wageBase: '$7,000',
    appliesTo: 'Employer only',
  },
  {
    id: '5',
    name: 'CA SDI',
    type: 'state' as const,
    rate: 0.9,
    wageBase: 'Unlimited',
    appliesTo: 'Employee only',
  },
  {
    id: '6',
    name: 'CA State Income Tax',
    type: 'state' as const,
    rate: 9.3,
    wageBase: 'Varies by bracket',
    appliesTo: 'Employee only',
  },
];

const mockCompanySettings = {
  companyName: 'JERP Enterprises Inc.',
  taxId: '12-3456789',
  address: '123 Market Street, Suite 500\nSan Francisco, CA 94103',
  payrollFrequency: 'biweekly' as const,
  fiscalYearStart: 'january' as const,
  timeZone: 'PT' as const,
  currency: 'USD' as const,
};

export default function AdminPortal() {
  const [activeTab, setActiveTab] = useState<'overview' | 'users' | 'audit' | 'taxes' | 'settings'>('overview');

  const tabs = [
    { id: 'overview' as const, label: 'Overview', icon: BarChart3 },
    { id: 'users' as const, label: 'Users', icon: Users },
    { id: 'audit' as const, label: 'Audit', icon: Activity },
    { id: 'taxes' as const, label: 'Tax Rates', icon: Database },
    { id: 'settings' as const, label: 'Settings', icon: Server },
  ];

  // Handlers (mock implementations)
  const handleEditUser = (user: any) => {
    console.log('Edit user:', user);
    alert(`Edit user: ${user.name}`);
  };

  const handleDeleteUser = (user: any) => {
    console.log('Delete user:', user);
    if (confirm(`Delete user ${user.name}?`)) {
      alert('User deleted (mock)');
    }
  };

  const handleExportLog = () => {
    alert('Exporting audit log... (mock)');
  };

  const handleEditTaxRate = (rate: any) => {
    console.log('Edit tax rate:', rate);
    alert(`Edit tax rate: ${rate.name}`);
  };

  const handleUpdateRates = () => {
    alert('Updating tax rates... (mock)');
  };

  const handleSaveSettings = (settings: any) => {
    console.log('Save settings:', settings);
    alert('Settings saved successfully! (mock)');
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-900 via-slate-800 to-slate-900">
      {/* Header */}
      <header className="bg-slate-900/50 border-b border-slate-700">
        <div className="max-w-7xl mx-auto px-6 py-6">
          <div className="flex items-center justify-between">
            <div>
              <h1 className="text-3xl font-bold text-white">Admin Portal</h1>
              <p className="text-slate-400 mt-1">JERP 3.0 System Administration</p>
            </div>
            <div className="flex items-center gap-3">
              <span className="px-4 py-2 bg-red-500/20 text-red-400 rounded-lg text-sm font-semibold border border-red-500/30">
                ⚡ SUPER ADMIN
              </span>
            </div>
          </div>
        </div>
      </header>

      {/* Main Content */}
      <main className="max-w-7xl mx-auto px-6 py-8">
        {/* Tab Navigation */}
        <div className="flex items-center gap-2 mb-8 bg-slate-800/50 p-2 rounded-xl border border-slate-700">
          {tabs.map((tab) => {
            const Icon = tab.icon;
            return (
              <button
                key={tab.id}
                onClick={() => setActiveTab(tab.id)}
                className={`flex items-center gap-2 px-6 py-3 rounded-lg transition-all font-medium ${
                  activeTab === tab.id
                    ? 'bg-blue-600 text-white shadow-lg'
                    : 'text-slate-400 hover:text-white hover:bg-slate-700'
                }`}
              >
                <Icon className="w-5 h-5" />
                <span>{tab.label}</span>
              </button>
            );
          })}
        </div>

        {/* Tab Content */}
        {activeTab === 'overview' && (
          <div className="space-y-8">
            {/* Metrics */}
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
              <MetricCard
                title="Active Users"
                value={59}
                change={12.5}
                icon={Users}
                color="#2563eb"
              />
              <MetricCard
                title="API Calls (Today)"
                value="3,150"
                change={8.3}
                icon={Activity}
                color="#10b981"
              />
              <MetricCard
                title="Avg Response Time"
                value="149ms"
                change={-3.2}
                icon={Clock}
                color="#f59e0b"
              />
              <MetricCard
                title="Errors (24h)"
                value={2}
                change={-50}
                icon={AlertTriangle}
                color="#ef4444"
              />
            </div>

            {/* Charts */}
            <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
              {/* API Usage Chart */}
              <div className="bg-white border border-slate-200 rounded-2xl p-6 shadow-sm">
                <h3 className="text-lg font-semibold text-slate-900 mb-4">API Usage (Last 7 Days)</h3>
                <ResponsiveContainer width="100%" height={300}>
                  <AreaChart data={mockMetrics}>
                    <defs>
                      <linearGradient id="colorApiCalls" x1="0" y1="0" x2="0" y2="1">
                        <stop offset="5%" stopColor="#2563eb" stopOpacity={0.3} />
                        <stop offset="95%" stopColor="#2563eb" stopOpacity={0} />
                      </linearGradient>
                    </defs>
                    <CartesianGrid strokeDasharray="3 3" stroke="#e2e8f0" />
                    <XAxis dataKey="date" stroke="#64748b" fontSize={12} />
                    <YAxis stroke="#64748b" fontSize={12} />
                    <Tooltip
                      contentStyle={{
                        backgroundColor: '#fff',
                        border: '1px solid #e2e8f0',
                        borderRadius: '8px',
                      }}
                    />
                    <Area
                      type="monotone"
                      dataKey="apiCalls"
                      stroke="#2563eb"
                      strokeWidth={2}
                      fillOpacity={1}
                      fill="url(#colorApiCalls)"
                    />
                  </AreaChart>
                </ResponsiveContainer>
              </div>

              {/* Errors Chart */}
              <div className="bg-white border border-slate-200 rounded-2xl p-6 shadow-sm">
                <h3 className="text-lg font-semibold text-slate-900 mb-4">Errors by Day</h3>
                <ResponsiveContainer width="100%" height={300}>
                  <BarChart data={mockMetrics}>
                    <CartesianGrid strokeDasharray="3 3" stroke="#e2e8f0" />
                    <XAxis dataKey="date" stroke="#64748b" fontSize={12} />
                    <YAxis stroke="#64748b" fontSize={12} />
                    <Tooltip
                      contentStyle={{
                        backgroundColor: '#fff',
                        border: '1px solid #e2e8f0',
                        borderRadius: '8px',
                      }}
                    />
                    <Bar dataKey="errors" fill="#ef4444" radius={[8, 8, 0, 0]} />
                  </BarChart>
                </ResponsiveContainer>
              </div>
            </div>

            {/* System Status */}
            <div className="bg-white border border-slate-200 rounded-2xl p-6 shadow-sm">
              <h3 className="text-lg font-semibold text-slate-900 mb-4">System Status</h3>
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
                {[
                  { name: 'API Server', status: 'operational', uptime: '99.98%', icon: Server },
                  { name: 'Database', status: 'operational', uptime: '99.99%', icon: Database },
                  { name: 'Authentication', status: 'operational', uptime: '100%', icon: Lock },
                  { name: 'Email Service', status: 'degraded', uptime: '98.50%', icon: Mail },
                ].map((service) => {
                  const Icon = service.icon;
                  return (
                    <div key={service.name} className="p-4 bg-slate-50 rounded-xl border border-slate-200">
                      <div className="flex items-center gap-3 mb-2">
                        <Icon className="w-5 h-5 text-slate-600" />
                        <span className="font-semibold text-slate-900">{service.name}</span>
                      </div>
                      <div className="flex items-center justify-between">
                        <span
                          className={`text-xs font-semibold px-2 py-1 rounded-full ${
                            service.status === 'operational'
                              ? 'bg-green-100 text-green-700'
                              : 'bg-yellow-100 text-yellow-700'
                          }`}
                        >
                          {service.status === 'operational' ? '● Operational' : '● Degraded'}
                        </span>
                        <span className="text-sm text-slate-600 font-mono">{service.uptime}</span>
                      </div>
                    </div>
                  );
                })}
              </div>
            </div>
          </div>
        )}

        {activeTab === 'users' && (
          <UserManagementTable
            users={mockUsers}
            onEdit={handleEditUser}
            onDelete={handleDeleteUser}
          />
        )}

        {activeTab === 'audit' && (
          <AuditLogPanel logs={mockAuditLogs} onExport={handleExportLog} />
        )}

        {activeTab === 'taxes' && (
          <TaxRateConfiguration
            taxRates={mockTaxRates}
            onEdit={handleEditTaxRate}
            onUpdateRates={handleUpdateRates}
          />
        )}

        {activeTab === 'settings' && (
          <SystemSettings
            initialSettings={mockCompanySettings}
            onSave={handleSaveSettings}
          />
        )}
      </main>
    </div>
  );
}
