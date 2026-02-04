/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: licensing@jerp.io
 */

import React, { useState } from 'react';

interface CompanySettings {
  companyName: string;
  taxId: string;
  address: string;
  payrollFrequency: 'weekly' | 'biweekly' | 'monthly';
  fiscalYearStart: 'january' | 'april' | 'july' | 'october';
  timeZone: 'PT' | 'MT' | 'CT' | 'ET';
  currency: 'USD' | 'EUR' | 'GBP';
}

interface SystemSettingsProps {
  initialSettings: CompanySettings;
  onSave: (settings: CompanySettings) => void;
}

export function SystemSettings({ initialSettings, onSave }: SystemSettingsProps) {
  const [settings, setSettings] = useState<CompanySettings>(initialSettings);

  const handleChange = (field: keyof CompanySettings, value: string) => {
    setSettings((prev) => ({ ...prev, [field]: value }));
  };

  const handleSave = () => {
    onSave(settings);
  };

  const handleCancel = () => {
    setSettings(initialSettings);
  };

  return (
    <div className="bg-white border border-slate-200 rounded-2xl overflow-hidden shadow-sm">
      {/* Header */}
      <div className="px-6 py-4 border-b border-slate-200">
        <h3 className="text-lg font-semibold text-slate-900">System Settings</h3>
        <p className="text-sm text-slate-600 mt-1">
          Configure company information and system preferences
        </p>
      </div>

      {/* Form */}
      <div className="p-6 space-y-6">
        {/* Company Information */}
        <div>
          <h4 className="text-sm font-semibold text-slate-900 mb-4">Company Information</h4>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-slate-700 mb-2">
                Company Name
              </label>
              <input
                type="text"
                value={settings.companyName}
                onChange={(e) => handleChange('companyName', e.target.value)}
                className="w-full px-4 py-2 border border-slate-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Acme Corporation"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-slate-700 mb-2">
                Tax ID / EIN
              </label>
              <input
                type="text"
                value={settings.taxId}
                onChange={(e) => handleChange('taxId', e.target.value)}
                className="w-full px-4 py-2 border border-slate-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="12-3456789"
              />
            </div>
          </div>
          <div className="mt-4">
            <label className="block text-sm font-medium text-slate-700 mb-2">
              Physical Address
            </label>
            <textarea
              value={settings.address}
              onChange={(e) => handleChange('address', e.target.value)}
              rows={3}
              className="w-full px-4 py-2 border border-slate-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="123 Main Street, Suite 100, San Francisco, CA 94102"
            />
          </div>
        </div>

        {/* Payroll Configuration */}
        <div>
          <h4 className="text-sm font-semibold text-slate-900 mb-4">Payroll Configuration</h4>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-slate-700 mb-2">
                Payroll Frequency
              </label>
              <select
                value={settings.payrollFrequency}
                onChange={(e) => handleChange('payrollFrequency', e.target.value)}
                className="w-full px-4 py-2 border border-slate-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
              >
                <option value="weekly">Weekly</option>
                <option value="biweekly">Bi-weekly</option>
                <option value="monthly">Monthly</option>
              </select>
            </div>
            <div>
              <label className="block text-sm font-medium text-slate-700 mb-2">
                Fiscal Year Start
              </label>
              <select
                value={settings.fiscalYearStart}
                onChange={(e) => handleChange('fiscalYearStart', e.target.value)}
                className="w-full px-4 py-2 border border-slate-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
              >
                <option value="january">January</option>
                <option value="april">April</option>
                <option value="july">July</option>
                <option value="october">October</option>
              </select>
            </div>
          </div>
        </div>

        {/* Regional Settings */}
        <div>
          <h4 className="text-sm font-semibold text-slate-900 mb-4">Regional Settings</h4>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-slate-700 mb-2">
                Time Zone
              </label>
              <select
                value={settings.timeZone}
                onChange={(e) => handleChange('timeZone', e.target.value)}
                className="w-full px-4 py-2 border border-slate-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
              >
                <option value="PT">Pacific Time (PT)</option>
                <option value="MT">Mountain Time (MT)</option>
                <option value="CT">Central Time (CT)</option>
                <option value="ET">Eastern Time (ET)</option>
              </select>
            </div>
            <div>
              <label className="block text-sm font-medium text-slate-700 mb-2">
                Currency
              </label>
              <select
                value={settings.currency}
                onChange={(e) => handleChange('currency', e.target.value)}
                className="w-full px-4 py-2 border border-slate-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
              >
                <option value="USD">USD - US Dollar</option>
                <option value="EUR">EUR - Euro</option>
                <option value="GBP">GBP - British Pound</option>
              </select>
            </div>
          </div>
        </div>
      </div>

      {/* Actions */}
      <div className="px-6 py-4 bg-slate-50 border-t border-slate-200 flex items-center justify-end gap-3">
        <button
          onClick={handleCancel}
          className="px-5 py-2 border border-slate-300 text-slate-700 rounded-lg hover:bg-slate-100 transition-colors text-sm font-medium"
        >
          Cancel
        </button>
        <button
          onClick={handleSave}
          className="px-5 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors text-sm font-medium"
        >
          Save Changes
        </button>
      </div>
    </div>
  );
}
