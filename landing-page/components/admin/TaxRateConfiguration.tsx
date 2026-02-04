/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: licensing@jerp.io
 */

import React from 'react';
import { Edit } from 'lucide-react';

interface TaxRate {
  id: string;
  name: string;
  type: 'federal' | 'state';
  rate: number;
  wageBase: string;
  appliesTo: string;
}

interface TaxRateConfigurationProps {
  taxRates: TaxRate[];
  onEdit: (rate: TaxRate) => void;
  onUpdateRates: () => void;
}

export function TaxRateConfiguration({ taxRates, onEdit, onUpdateRates }: TaxRateConfigurationProps) {
  return (
    <div className="bg-white border border-slate-200 rounded-2xl overflow-hidden shadow-sm">
      {/* Header */}
      <div className="px-6 py-4 border-b border-slate-200 flex items-center justify-between">
        <div>
          <h3 className="text-lg font-semibold text-slate-900">Tax Rate Configuration</h3>
          <p className="text-sm text-slate-600 mt-1">
            Manage federal and state tax rates for payroll calculations
          </p>
        </div>
        <button
          onClick={onUpdateRates}
          className="px-4 py-2 bg-orange-600 text-white rounded-lg hover:bg-orange-700 transition-colors text-sm font-medium"
        >
          Update Rates
        </button>
      </div>

      {/* Table */}
      <div className="overflow-x-auto">
        <table className="w-full">
          <thead className="bg-slate-50 border-b border-slate-200">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-semibold text-slate-600 uppercase tracking-wider">
                Tax Name
              </th>
              <th className="px-6 py-3 text-left text-xs font-semibold text-slate-600 uppercase tracking-wider">
                Type
              </th>
              <th className="px-6 py-3 text-left text-xs font-semibold text-slate-600 uppercase tracking-wider">
                Rate
              </th>
              <th className="px-6 py-3 text-left text-xs font-semibold text-slate-600 uppercase tracking-wider">
                Wage Base
              </th>
              <th className="px-6 py-3 text-left text-xs font-semibold text-slate-600 uppercase tracking-wider">
                Applies To
              </th>
              <th className="px-6 py-3 text-right text-xs font-semibold text-slate-600 uppercase tracking-wider">
                Actions
              </th>
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-200">
            {taxRates.map((rate) => (
              <tr key={rate.id} className="hover:bg-slate-50 transition-colors">
                <td className="px-6 py-4">
                  <div className="font-medium text-slate-900">{rate.name}</div>
                </td>
                <td className="px-6 py-4">
                  <span
                    className={`inline-flex items-center px-3 py-1 rounded-full text-xs font-semibold border ${
                      rate.type === 'federal'
                        ? 'bg-blue-100 text-blue-700 border-blue-200'
                        : 'bg-purple-100 text-purple-700 border-purple-200'
                    }`}
                  >
                    {rate.type === 'federal' ? 'Federal' : 'State'}
                  </span>
                </td>
                <td className="px-6 py-4">
                  <span className="font-mono text-slate-900 font-semibold">
                    {rate.rate}%
                  </span>
                </td>
                <td className="px-6 py-4 text-sm text-slate-600">{rate.wageBase}</td>
                <td className="px-6 py-4 text-sm text-slate-600">{rate.appliesTo}</td>
                <td className="px-6 py-4">
                  <div className="flex items-center justify-end gap-2">
                    <button
                      onClick={() => onEdit(rate)}
                      className="p-2 text-blue-600 hover:bg-blue-50 rounded-lg transition-colors"
                      title="Edit rate"
                    >
                      <Edit className="w-4 h-4" />
                    </button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
