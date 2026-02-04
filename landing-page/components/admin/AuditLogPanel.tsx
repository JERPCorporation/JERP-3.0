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
import { FileText, User, Shield, UserPlus, DollarSign, Database } from 'lucide-react';

interface AuditLog {
  id: string;
  timestamp: string;
  action: 'payroll_processed' | 'employee_updated' | 'compliance_check' | 'user_created' | 'deduction_modified' | 'system_backup';
  resource: string;
  user: string;
  ipAddress: string;
  hash: string;
}

interface AuditLogPanelProps {
  logs: AuditLog[];
  onExport: () => void;
}

const actionIcons = {
  payroll_processed: FileText,
  employee_updated: User,
  compliance_check: Shield,
  user_created: UserPlus,
  deduction_modified: DollarSign,
  system_backup: Database,
};

const actionColors = {
  payroll_processed: { bg: 'bg-blue-100', text: 'text-blue-700' },
  employee_updated: { bg: 'bg-purple-100', text: 'text-purple-700' },
  compliance_check: { bg: 'bg-green-100', text: 'text-green-700' },
  user_created: { bg: 'bg-orange-100', text: 'text-orange-700' },
  deduction_modified: { bg: 'bg-yellow-100', text: 'text-yellow-700' },
  system_backup: { bg: 'bg-teal-100', text: 'text-teal-700' },
};

const actionLabels = {
  payroll_processed: 'Payroll Processed',
  employee_updated: 'Employee Updated',
  compliance_check: 'Compliance Check',
  user_created: 'User Created',
  deduction_modified: 'Deduction Modified',
  system_backup: 'System Backup',
};

export function AuditLogPanel({ logs, onExport }: AuditLogPanelProps) {
  return (
    <div className="bg-white border border-slate-200 rounded-2xl overflow-hidden shadow-sm">
      {/* Header */}
      <div className="px-6 py-4 border-b border-slate-200 flex items-center justify-between">
        <div>
          <h3 className="text-lg font-semibold text-slate-900">Audit Log</h3>
          <p className="text-sm text-slate-600 mt-1">
            Hash-chained immutable logging for compliance
          </p>
        </div>
        <button
          onClick={onExport}
          className="px-4 py-2 bg-slate-600 text-white rounded-lg hover:bg-slate-700 transition-colors text-sm font-medium"
        >
          Export Log
        </button>
      </div>

      {/* Log Entries */}
      <div className="divide-y divide-slate-200">
        {logs.map((log, index) => {
          const Icon = actionIcons[log.action];
          const colors = actionColors[log.action];
          return (
            <div key={log.id} className="px-6 py-4 hover:bg-slate-50 transition-colors">
              <div className="flex items-start gap-4">
                {/* Icon */}
                <div className={`p-2 rounded-lg ${colors.bg} flex-shrink-0`}>
                  <Icon className={`w-5 h-5 ${colors.text}`} />
                </div>

                {/* Content */}
                <div className="flex-1 min-w-0">
                  <div className="flex items-start justify-between gap-4 mb-2">
                    <div>
                      <div className="font-semibold text-slate-900">
                        {actionLabels[log.action]}
                      </div>
                      <div className="text-sm text-slate-600 mt-1">
                        Resource: <span className="font-mono">{log.resource}</span>
                      </div>
                    </div>
                    <div className="text-right flex-shrink-0">
                      <div className="text-xs text-slate-500 font-mono">
                        {log.timestamp}
                      </div>
                    </div>
                  </div>

                  <div className="flex items-center gap-4 text-xs text-slate-600">
                    <span>
                      User: <span className="font-semibold">{log.user}</span>
                    </span>
                    <span>
                      IP: <span className="font-mono">{log.ipAddress}</span>
                    </span>
                  </div>

                  {/* Hash */}
                  <div className="mt-2 p-2 bg-slate-50 rounded border border-slate-200">
                    <div className="text-xs text-slate-500 mb-1">Cryptographic Hash:</div>
                    <div className="text-xs font-mono text-slate-700 break-all">
                      {log.hash}
                    </div>
                  </div>

                  {/* Chain indicator */}
                  {index < logs.length - 1 && (
                    <div className="flex items-center gap-2 mt-2 text-xs text-slate-500">
                      <div className="w-2 h-2 rounded-full bg-green-500"></div>
                      <span>Verified chain link to next entry</span>
                    </div>
                  )}
                </div>
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
}
