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

import { useState } from "react";
import { BarChart, Bar, LineChart, Line, PieChart, Pie, Cell, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, AreaChart, Area } from "recharts";

// Mock Data
const mockUsers = [
  { id: 1, name: "Admin Principal", email: "admin@company.com", role: "super_admin", status: "active", lastLogin: "2025-02-03 14:30", created: "2024-01-15" },
  { id: 2, name: "María González", email: "maria@company.com", role: "admin", status: "active", lastLogin: "2025-02-03 10:15", created: "2024-03-20" },
  { id: 3, name: "Carlos Ruiz", email: "carlos@company.com", role: "payroll_manager", status: "active", lastLogin: "2025-02-02 16:45", created: "2024-06-10" },
  { id: 4, name: "Ana Martínez", email: "ana@company.com", role: "hr_manager", status: "active", lastLogin: "2025-02-01 09:20", created: "2024-08-05" },
  { id: 5, name: "Luis Fernández", email: "luis@company.com", role: "accountant", status: "suspended", lastLogin: "2025-01-28 11:30", created: "2024-11-12" },
];

const mockAuditLogs = [
  { id: 1, timestamp: "2025-02-03 14:32:15", user: "admin@company.com", action: "payroll_processed", resource: "Payroll Period Jan-2025", ip: "192.168.1.100", hash: "a7f3c9e2d4b1..." },
  { id: 2, timestamp: "2025-02-03 13:18:42", user: "maria@company.com", action: "employee_updated", resource: "Employee #1234", ip: "192.168.1.105", hash: "b2e8d4a1f9c7..." },
  { id: 3, timestamp: "2025-02-03 11:05:33", user: "carlos@company.com", action: "compliance_check", resource: "Tax Withholdings", ip: "192.168.1.110", hash: "c9f1a5b3e2d8..." },
  { id: 4, timestamp: "2025-02-02 16:47:28", user: "admin@company.com", action: "user_created", resource: "User #5678", ip: "192.168.1.100", hash: "d3a7c2e8f4b1..." },
  { id: 5, timestamp: "2025-02-02 10:22:11", user: "ana@company.com", action: "deduction_modified", resource: "CA SDI Rate", ip: "192.168.1.112", hash: "e5b9d1f4a3c2..." },
  { id: 6, timestamp: "2025-02-01 15:33:50", user: "admin@company.com", action: "system_backup", resource: "Database Backup", ip: "192.168.1.100", hash: "f1c4e6a2b9d7..." },
];

const mockSystemMetrics = [
  { date: "28 Ene", apiCalls: 2340, users: 45, errors: 3, avgResponseTime: 145 },
  { date: "29 Ene", apiCalls: 2580, users: 52, errors: 1, avgResponseTime: 132 },
  { date: "30 Ene", apiCalls: 2890, users: 48, errors: 2, avgResponseTime: 158 },
  { date: "31 Ene", apiCalls: 3120, users: 55, errors: 0, avgResponseTime: 128 },
  { date: "01 Feb", apiCalls: 2950, users: 50, errors: 4, avgResponseTime: 167 },
  { date: "02 Feb", apiCalls: 3340, users: 58, errors: 1, avgResponseTime: 135 },
  { date: "03 Feb", apiCalls: 3100, users: 53, errors: 2, avgResponseTime: 142 },
];

const mockCompanySettings = {
  companyName: "Mi Empresa Inc.",
  taxId: "XX-XXXXXXX",
  address: "123 Main Street, San Francisco, CA 94102",
  payrollFrequency: "biweekly",
  fiscalYearStart: "January",
  timeZone: "America/Los_Angeles",
  currency: "USD",
};

const mockTaxRates = [
  { id: 1, name: "Social Security (OASDI)", type: "Federal", rate: 6.2, wageBase: 168600, applies: "Employee & Employer" },
  { id: 2, name: "Medicare (HI)", type: "Federal", rate: 1.45, wageBase: null, applies: "Employee & Employer" },
  { id: 3, name: "Additional Medicare", type: "Federal", rate: 0.9, wageBase: 200000, applies: "Employee only" },
  { id: 4, name: "FUTA", type: "Federal", rate: 0.6, wageBase: 7000, applies: "Employer only" },
  { id: 5, name: "CA SDI", type: "California", rate: 0.9, wageBase: null, applies: "Employee only" },
  { id: 6, name: "CA State Income Tax", type: "California", rate: 9.3, wageBase: null, applies: "Employee only (varies)" },
];

const COLORS = {
  primary: "#2563eb",
  success: "#10b981",
  warning: "#f59e0b",
  danger: "#ef4444",
  purple: "#8b5cf6",
  teal: "#14b8a6",
};

// MetricCard Component
function MetricCard({ title, value, change, icon, color }: { title: string; value: string | number; change?: string; icon: string; color: string }) {
  return (
    <div style={{
      background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
      borderRadius: "12px",
      padding: "24px",
      border: "1px solid rgba(71, 85, 105, 0.3)",
      boxShadow: "0 4px 6px rgba(0, 0, 0, 0.1)",
    }}>
      <div style={{ display: "flex", alignItems: "center", justifyContent: "space-between", marginBottom: "16px" }}>
        <span style={{ fontSize: "14px", color: "#94a3b8", fontWeight: "500" }}>{title}</span>
        <div style={{ fontSize: "24px" }}>{icon}</div>
      </div>
      <div style={{ fontSize: "32px", fontWeight: "700", color: "#f1f5f9", marginBottom: "8px" }}>{value}</div>
      {change && (
        <div style={{ fontSize: "12px", color: change.startsWith("+") ? COLORS.success : COLORS.danger }}>
          {change}
        </div>
      )}
    </div>
  );
}

// UserManagementTable Component
function UserManagementTable({ users }: { users: typeof mockUsers }) {
  const getRoleBadgeColor = (role: string) => {
    switch (role) {
      case "super_admin": return COLORS.purple;
      case "admin": return COLORS.primary;
      case "payroll_manager": return COLORS.teal;
      case "hr_manager": return COLORS.success;
      case "accountant": return COLORS.warning;
      default: return "#64748b";
    }
  };

  const getStatusColor = (status: string) => {
    return status === "active" ? COLORS.success : COLORS.danger;
  };

  return (
    <div style={{ overflowX: "auto" }}>
      <table style={{ width: "100%", borderCollapse: "collapse" }}>
        <thead>
          <tr style={{ borderBottom: "1px solid rgba(71, 85, 105, 0.3)" }}>
            <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>Usuario</th>
            <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>Rol</th>
            <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>Estado</th>
            <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>Último Acceso</th>
            <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>Acciones</th>
          </tr>
        </thead>
        <tbody>
          {users.map((user) => (
            <tr key={user.id} style={{ borderBottom: "1px solid rgba(71, 85, 105, 0.2)" }}>
              <td style={{ padding: "16px" }}>
                <div style={{ display: "flex", alignItems: "center", gap: "12px" }}>
                  <div style={{
                    width: "40px",
                    height: "40px",
                    borderRadius: "50%",
                    background: `linear-gradient(135deg, ${getRoleBadgeColor(user.role)} 0%, ${getRoleBadgeColor(user.role)}99 100%)`,
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
                    fontSize: "16px",
                    fontWeight: "600",
                    color: "white"
                  }}>
                    {user.name.split(" ").map(n => n[0]).join("")}
                  </div>
                  <div>
                    <div style={{ color: "#f1f5f9", fontWeight: "500", fontSize: "14px" }}>{user.name}</div>
                    <div style={{ color: "#94a3b8", fontSize: "12px" }}>{user.email}</div>
                  </div>
                </div>
              </td>
              <td style={{ padding: "16px" }}>
                <span style={{
                  background: getRoleBadgeColor(user.role),
                  color: "white",
                  padding: "4px 12px",
                  borderRadius: "6px",
                  fontSize: "12px",
                  fontWeight: "500"
                }}>
                  {user.role.replace(/_/g, " ").toUpperCase()}
                </span>
              </td>
              <td style={{ padding: "16px" }}>
                <span style={{
                  background: `${getStatusColor(user.status)}20`,
                  color: getStatusColor(user.status),
                  padding: "4px 12px",
                  borderRadius: "6px",
                  fontSize: "12px",
                  fontWeight: "500"
                }}>
                  {user.status.toUpperCase()}
                </span>
              </td>
              <td style={{ padding: "16px", color: "#94a3b8", fontSize: "14px" }}>{user.lastLogin}</td>
              <td style={{ padding: "16px" }}>
                <div style={{ display: "flex", gap: "8px" }}>
                  <button style={{
                    padding: "6px 12px",
                    borderRadius: "6px",
                    border: "1px solid rgba(71, 85, 105, 0.3)",
                    background: "rgba(30, 41, 59, 0.5)",
                    color: "#f1f5f9",
                    fontSize: "12px",
                    cursor: "pointer"
                  }}>
                    Editar
                  </button>
                  <button style={{
                    padding: "6px 12px",
                    borderRadius: "6px",
                    border: "1px solid rgba(239, 68, 68, 0.3)",
                    background: "rgba(239, 68, 68, 0.1)",
                    color: COLORS.danger,
                    fontSize: "12px",
                    cursor: "pointer"
                  }}>
                    Eliminar
                  </button>
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

// AuditLogPanel Component
function AuditLogPanel({ logs }: { logs: typeof mockAuditLogs }) {
  const getActionBadgeColor = (action: string) => {
    switch (action) {
      case "payroll_processed": return COLORS.success;
      case "employee_updated": return COLORS.primary;
      case "compliance_check": return COLORS.warning;
      case "user_created": return COLORS.teal;
      case "deduction_modified": return COLORS.purple;
      case "system_backup": return "#64748b";
      default: return "#64748b";
    }
  };

  return (
    <div>
      <div style={{ marginBottom: "20px", display: "flex", justifyContent: "space-between", alignItems: "center" }}>
        <h3 style={{ color: "#f1f5f9", fontSize: "18px", fontWeight: "600" }}>
          🔐 Hash-Chained Audit Trail
        </h3>
        <button style={{
          padding: "8px 16px",
          borderRadius: "8px",
          background: `linear-gradient(135deg, ${COLORS.primary} 0%, ${COLORS.purple} 100%)`,
          border: "none",
          color: "white",
          fontSize: "14px",
          fontWeight: "500",
          cursor: "pointer"
        }}>
          📥 Export para Compliance
        </button>
      </div>
      <div style={{ overflowX: "auto" }}>
        <table style={{ width: "100%", borderCollapse: "collapse" }}>
          <thead>
            <tr style={{ borderBottom: "1px solid rgba(71, 85, 105, 0.3)" }}>
              <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>Timestamp</th>
              <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>Usuario</th>
              <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>Acción</th>
              <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>Recurso</th>
              <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>IP</th>
              <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>Hash</th>
            </tr>
          </thead>
          <tbody>
            {logs.map((log) => (
              <tr key={log.id} style={{ borderBottom: "1px solid rgba(71, 85, 105, 0.2)" }}>
                <td style={{ padding: "12px", color: "#94a3b8", fontSize: "13px" }}>{log.timestamp}</td>
                <td style={{ padding: "12px", color: "#f1f5f9", fontSize: "13px" }}>{log.user}</td>
                <td style={{ padding: "12px" }}>
                  <span style={{
                    background: getActionBadgeColor(log.action),
                    color: "white",
                    padding: "4px 10px",
                    borderRadius: "6px",
                    fontSize: "11px",
                    fontWeight: "500"
                  }}>
                    {log.action.replace(/_/g, " ").toUpperCase()}
                  </span>
                </td>
                <td style={{ padding: "12px", color: "#94a3b8", fontSize: "13px" }}>{log.resource}</td>
                <td style={{ padding: "12px", color: "#94a3b8", fontSize: "13px", fontFamily: "monospace" }}>{log.ip}</td>
                <td style={{ padding: "12px" }}>
                  <code style={{
                    background: "rgba(30, 41, 59, 0.5)",
                    color: COLORS.teal,
                    padding: "4px 8px",
                    borderRadius: "4px",
                    fontSize: "11px",
                    fontFamily: "monospace"
                  }}>
                    {log.hash}
                  </code>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

// TaxRateConfiguration Component
function TaxRateConfiguration({ taxRates }: { taxRates: typeof mockTaxRates }) {
  return (
    <div>
      <div style={{ marginBottom: "20px", display: "flex", justifyContent: "space-between", alignItems: "center" }}>
        <h3 style={{ color: "#f1f5f9", fontSize: "18px", fontWeight: "600" }}>
          💰 Configuración de Tasas Impositivas
        </h3>
        <button style={{
          padding: "8px 16px",
          borderRadius: "8px",
          background: `linear-gradient(135deg, ${COLORS.success} 0%, ${COLORS.teal} 100%)`,
          border: "none",
          color: "white",
          fontSize: "14px",
          fontWeight: "500",
          cursor: "pointer"
        }}>
          ➕ Agregar Tasa
        </button>
      </div>
      <div style={{ overflowX: "auto" }}>
        <table style={{ width: "100%", borderCollapse: "collapse" }}>
          <thead>
            <tr style={{ borderBottom: "1px solid rgba(71, 85, 105, 0.3)" }}>
              <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>Nombre</th>
              <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>Tipo</th>
              <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>Tasa (%)</th>
              <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>Base Salarial</th>
              <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>Aplica a</th>
              <th style={{ padding: "12px", textAlign: "left", color: "#94a3b8", fontSize: "12px", fontWeight: "600", textTransform: "uppercase" }}>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {taxRates.map((tax) => (
              <tr key={tax.id} style={{ borderBottom: "1px solid rgba(71, 85, 105, 0.2)" }}>
                <td style={{ padding: "12px", color: "#f1f5f9", fontSize: "14px", fontWeight: "500" }}>{tax.name}</td>
                <td style={{ padding: "12px" }}>
                  <span style={{
                    background: tax.type === "Federal" ? COLORS.primary : COLORS.warning,
                    color: "white",
                    padding: "4px 10px",
                    borderRadius: "6px",
                    fontSize: "11px",
                    fontWeight: "500"
                  }}>
                    {tax.type}
                  </span>
                </td>
                <td style={{ padding: "12px", color: "#f1f5f9", fontSize: "14px" }}>{tax.rate}%</td>
                <td style={{ padding: "12px", color: "#94a3b8", fontSize: "14px" }}>
                  {tax.wageBase ? `$${tax.wageBase.toLocaleString()}` : "N/A"}
                </td>
                <td style={{ padding: "12px", color: "#94a3b8", fontSize: "13px" }}>{tax.applies}</td>
                <td style={{ padding: "12px" }}>
                  <button style={{
                    padding: "6px 12px",
                    borderRadius: "6px",
                    border: "1px solid rgba(71, 85, 105, 0.3)",
                    background: "rgba(30, 41, 59, 0.5)",
                    color: "#f1f5f9",
                    fontSize: "12px",
                    cursor: "pointer"
                  }}>
                    Editar
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

// SystemSettings Component
function SystemSettings({ settings }: { settings: typeof mockCompanySettings }) {
  const [formData, setFormData] = useState(settings);

  return (
    <div>
      <div style={{ marginBottom: "20px" }}>
        <h3 style={{ color: "#f1f5f9", fontSize: "18px", fontWeight: "600" }}>
          ⚙️ Configuración del Sistema
        </h3>
      </div>
      <div style={{
        background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
        borderRadius: "12px",
        padding: "24px",
        border: "1px solid rgba(71, 85, 105, 0.3)"
      }}>
        <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(300px, 1fr))", gap: "24px" }}>
          <div>
            <label style={{ display: "block", color: "#94a3b8", fontSize: "14px", fontWeight: "500", marginBottom: "8px" }}>
              Nombre de la Empresa
            </label>
            <input
              type="text"
              value={formData.companyName}
              onChange={(e) => setFormData({ ...formData, companyName: e.target.value })}
              style={{
                width: "100%",
                padding: "10px 12px",
                borderRadius: "8px",
                border: "1px solid rgba(71, 85, 105, 0.3)",
                background: "rgba(15, 23, 42, 0.5)",
                color: "#f1f5f9",
                fontSize: "14px"
              }}
            />
          </div>
          <div>
            <label style={{ display: "block", color: "#94a3b8", fontSize: "14px", fontWeight: "500", marginBottom: "8px" }}>
              Tax ID / EIN
            </label>
            <input
              type="text"
              value={formData.taxId}
              onChange={(e) => setFormData({ ...formData, taxId: e.target.value })}
              style={{
                width: "100%",
                padding: "10px 12px",
                borderRadius: "8px",
                border: "1px solid rgba(71, 85, 105, 0.3)",
                background: "rgba(15, 23, 42, 0.5)",
                color: "#f1f5f9",
                fontSize: "14px"
              }}
            />
          </div>
          <div style={{ gridColumn: "1 / -1" }}>
            <label style={{ display: "block", color: "#94a3b8", fontSize: "14px", fontWeight: "500", marginBottom: "8px" }}>
              Dirección
            </label>
            <input
              type="text"
              value={formData.address}
              onChange={(e) => setFormData({ ...formData, address: e.target.value })}
              style={{
                width: "100%",
                padding: "10px 12px",
                borderRadius: "8px",
                border: "1px solid rgba(71, 85, 105, 0.3)",
                background: "rgba(15, 23, 42, 0.5)",
                color: "#f1f5f9",
                fontSize: "14px"
              }}
            />
          </div>
          <div>
            <label style={{ display: "block", color: "#94a3b8", fontSize: "14px", fontWeight: "500", marginBottom: "8px" }}>
              Frecuencia de Nómina
            </label>
            <select
              value={formData.payrollFrequency}
              onChange={(e) => setFormData({ ...formData, payrollFrequency: e.target.value })}
              style={{
                width: "100%",
                padding: "10px 12px",
                borderRadius: "8px",
                border: "1px solid rgba(71, 85, 105, 0.3)",
                background: "rgba(15, 23, 42, 0.5)",
                color: "#f1f5f9",
                fontSize: "14px"
              }}
            >
              <option value="weekly">Semanal</option>
              <option value="biweekly">Quincenal</option>
              <option value="monthly">Mensual</option>
            </select>
          </div>
          <div>
            <label style={{ display: "block", color: "#94a3b8", fontSize: "14px", fontWeight: "500", marginBottom: "8px" }}>
              Inicio del Año Fiscal
            </label>
            <select
              value={formData.fiscalYearStart}
              onChange={(e) => setFormData({ ...formData, fiscalYearStart: e.target.value })}
              style={{
                width: "100%",
                padding: "10px 12px",
                borderRadius: "8px",
                border: "1px solid rgba(71, 85, 105, 0.3)",
                background: "rgba(15, 23, 42, 0.5)",
                color: "#f1f5f9",
                fontSize: "14px"
              }}
            >
              <option value="January">Enero</option>
              <option value="February">Febrero</option>
              <option value="March">Marzo</option>
              <option value="April">Abril</option>
            </select>
          </div>
          <div>
            <label style={{ display: "block", color: "#94a3b8", fontSize: "14px", fontWeight: "500", marginBottom: "8px" }}>
              Zona Horaria
            </label>
            <select
              value={formData.timeZone}
              onChange={(e) => setFormData({ ...formData, timeZone: e.target.value })}
              style={{
                width: "100%",
                padding: "10px 12px",
                borderRadius: "8px",
                border: "1px solid rgba(71, 85, 105, 0.3)",
                background: "rgba(15, 23, 42, 0.5)",
                color: "#f1f5f9",
                fontSize: "14px"
              }}
            >
              <option value="America/Los_Angeles">Pacific Time (PT)</option>
              <option value="America/Denver">Mountain Time (MT)</option>
              <option value="America/Chicago">Central Time (CT)</option>
              <option value="America/New_York">Eastern Time (ET)</option>
            </select>
          </div>
          <div>
            <label style={{ display: "block", color: "#94a3b8", fontSize: "14px", fontWeight: "500", marginBottom: "8px" }}>
              Moneda
            </label>
            <select
              value={formData.currency}
              onChange={(e) => setFormData({ ...formData, currency: e.target.value })}
              style={{
                width: "100%",
                padding: "10px 12px",
                borderRadius: "8px",
                border: "1px solid rgba(71, 85, 105, 0.3)",
                background: "rgba(15, 23, 42, 0.5)",
                color: "#f1f5f9",
                fontSize: "14px"
              }}
            >
              <option value="USD">USD - Dólar Estadounidense</option>
              <option value="EUR">EUR - Euro</option>
              <option value="GBP">GBP - Libra Esterlina</option>
            </select>
          </div>
        </div>
        <div style={{ marginTop: "24px", display: "flex", gap: "12px", justifyContent: "flex-end" }}>
          <button style={{
            padding: "10px 24px",
            borderRadius: "8px",
            border: "1px solid rgba(71, 85, 105, 0.3)",
            background: "rgba(30, 41, 59, 0.5)",
            color: "#f1f5f9",
            fontSize: "14px",
            fontWeight: "500",
            cursor: "pointer"
          }}>
            Cancelar
          </button>
          <button style={{
            padding: "10px 24px",
            borderRadius: "8px",
            background: `linear-gradient(135deg, ${COLORS.success} 0%, ${COLORS.teal} 100%)`,
            border: "none",
            color: "white",
            fontSize: "14px",
            fontWeight: "500",
            cursor: "pointer"
          }}>
            💾 Guardar Cambios
          </button>
        </div>
      </div>
    </div>
  );
}

// Main AdminPortal Component
export default function AdminPortal() {
  const [activeTab, setActiveTab] = useState("overview");

  const tabs = [
    { id: "overview", label: "📊 Overview", icon: "📊" },
    { id: "users", label: "👥 Usuarios", icon: "👥" },
    { id: "audit", label: "🔐 Auditoría", icon: "🔐" },
    { id: "taxes", label: "💰 Impuestos", icon: "💰" },
    { id: "settings", label: "⚙️ Configuración", icon: "⚙️" },
  ];

  return (
    <div style={{
      minHeight: "100vh",
      background: "linear-gradient(135deg, #0f172a 0%, #1e293b 50%, #0f172a 100%)",
      padding: "24px"
    }}>
      {/* Header */}
      <div style={{ marginBottom: "32px" }}>
        <h1 style={{
          fontSize: "32px",
          fontWeight: "700",
          background: "linear-gradient(135deg, #60a5fa 0%, #a78bfa 100%)",
          WebkitBackgroundClip: "text",
          WebkitTextFillColor: "transparent",
          backgroundClip: "text",
          marginBottom: "8px"
        }}>
          JERP 3.0 Admin Portal
        </h1>
        <p style={{ color: "#94a3b8", fontSize: "16px" }}>
          Sistema de administración completo con gestión de usuarios, auditoría y compliance
        </p>
      </div>

      {/* Tab Navigation */}
      <div style={{
        background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
        borderRadius: "12px",
        padding: "8px",
        marginBottom: "24px",
        display: "flex",
        gap: "8px",
        flexWrap: "wrap",
        border: "1px solid rgba(71, 85, 105, 0.3)"
      }}>
        {tabs.map((tab) => (
          <button
            key={tab.id}
            onClick={() => setActiveTab(tab.id)}
            style={{
              padding: "12px 24px",
              borderRadius: "8px",
              border: "none",
              background: activeTab === tab.id
                ? "linear-gradient(135deg, #2563eb 0%, #8b5cf6 100%)"
                : "transparent",
              color: activeTab === tab.id ? "white" : "#94a3b8",
              fontSize: "14px",
              fontWeight: "500",
              cursor: "pointer",
              transition: "all 0.3s ease"
            }}
          >
            {tab.label}
          </button>
        ))}
      </div>

      {/* Tab Content */}
      <div>
        {activeTab === "overview" && (
          <div>
            {/* System Metrics */}
            <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(250px, 1fr))", gap: "20px", marginBottom: "32px" }}>
              <MetricCard title="Usuarios Activos" value="53" change="+8% vs. ayer" icon="👥" color={COLORS.primary} />
              <MetricCard title="API Calls (24h)" value="3,100" change="+12% vs. ayer" icon="⚡" color={COLORS.success} />
              <MetricCard title="Tiempo Respuesta Promedio" value="142ms" change="-5ms vs. ayer" icon="⏱️" color={COLORS.teal} />
              <MetricCard title="Errores (24h)" value="2" change="-50% vs. ayer" icon="🔍" color={COLORS.warning} />
            </div>

            {/* System Status */}
            <div style={{
              background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
              borderRadius: "12px",
              padding: "24px",
              marginBottom: "32px",
              border: "1px solid rgba(71, 85, 105, 0.3)"
            }}>
              <h3 style={{ color: "#f1f5f9", fontSize: "18px", fontWeight: "600", marginBottom: "20px" }}>
                🔧 Estado del Sistema
              </h3>
              <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(200px, 1fr))", gap: "16px" }}>
                {[
                  { name: "API Server", status: "Operacional", color: COLORS.success },
                  { name: "Database", status: "Operacional", color: COLORS.success },
                  { name: "Authentication", status: "Operacional", color: COLORS.success },
                  { name: "Email Service", status: "Degradado", color: COLORS.warning },
                ].map((service, i) => (
                  <div key={i} style={{
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "space-between",
                    padding: "12px",
                    background: "rgba(15, 23, 42, 0.5)",
                    borderRadius: "8px",
                    border: "1px solid rgba(71, 85, 105, 0.3)"
                  }}>
                    <span style={{ color: "#f1f5f9", fontSize: "14px" }}>{service.name}</span>
                    <span style={{
                      color: service.color,
                      fontSize: "12px",
                      fontWeight: "500",
                      background: `${service.color}20`,
                      padding: "4px 8px",
                      borderRadius: "4px"
                    }}>
                      ● {service.status}
                    </span>
                  </div>
                ))}
              </div>
            </div>

            {/* Charts */}
            <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(400px, 1fr))", gap: "24px" }}>
              {/* API Usage Chart */}
              <div style={{
                background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
                borderRadius: "12px",
                padding: "24px",
                border: "1px solid rgba(71, 85, 105, 0.3)"
              }}>
                <h4 style={{ color: "#f1f5f9", fontSize: "16px", fontWeight: "600", marginBottom: "20px" }}>
                  API Usage (Últimos 7 días)
                </h4>
                <ResponsiveContainer width="100%" height={250}>
                  <AreaChart data={mockSystemMetrics}>
                    <defs>
                      <linearGradient id="colorApiCalls" x1="0" y1="0" x2="0" y2="1">
                        <stop offset="5%" stopColor={COLORS.primary} stopOpacity={0.8} />
                        <stop offset="95%" stopColor={COLORS.primary} stopOpacity={0.1} />
                      </linearGradient>
                    </defs>
                    <CartesianGrid strokeDasharray="3 3" stroke="rgba(71, 85, 105, 0.3)" />
                    <XAxis dataKey="date" stroke="#94a3b8" />
                    <YAxis stroke="#94a3b8" />
                    <Tooltip
                      contentStyle={{
                        background: "rgba(15, 23, 42, 0.95)",
                        border: "1px solid rgba(71, 85, 105, 0.3)",
                        borderRadius: "8px",
                        color: "#f1f5f9"
                      }}
                    />
                    <Area type="monotone" dataKey="apiCalls" stroke={COLORS.primary} fillOpacity={1} fill="url(#colorApiCalls)" />
                  </AreaChart>
                </ResponsiveContainer>
              </div>

              {/* Errors Chart */}
              <div style={{
                background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
                borderRadius: "12px",
                padding: "24px",
                border: "1px solid rgba(71, 85, 105, 0.3)"
              }}>
                <h4 style={{ color: "#f1f5f9", fontSize: "16px", fontWeight: "600", marginBottom: "20px" }}>
                  Errores (Últimos 7 días)
                </h4>
                <ResponsiveContainer width="100%" height={250}>
                  <LineChart data={mockSystemMetrics}>
                    <CartesianGrid strokeDasharray="3 3" stroke="rgba(71, 85, 105, 0.3)" />
                    <XAxis dataKey="date" stroke="#94a3b8" />
                    <YAxis stroke="#94a3b8" />
                    <Tooltip
                      contentStyle={{
                        background: "rgba(15, 23, 42, 0.95)",
                        border: "1px solid rgba(71, 85, 105, 0.3)",
                        borderRadius: "8px",
                        color: "#f1f5f9"
                      }}
                    />
                    <Line type="monotone" dataKey="errors" stroke={COLORS.danger} strokeWidth={2} />
                  </LineChart>
                </ResponsiveContainer>
              </div>
            </div>
          </div>
        )}

        {activeTab === "users" && (
          <div style={{
            background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
            borderRadius: "12px",
            padding: "24px",
            border: "1px solid rgba(71, 85, 105, 0.3)"
          }}>
            <div style={{ marginBottom: "20px", display: "flex", justifyContent: "space-between", alignItems: "center" }}>
              <h3 style={{ color: "#f1f5f9", fontSize: "18px", fontWeight: "600" }}>
                👥 Gestión de Usuarios
              </h3>
              <button style={{
                padding: "8px 16px",
                borderRadius: "8px",
                background: `linear-gradient(135deg, ${COLORS.primary} 0%, ${COLORS.purple} 100%)`,
                border: "none",
                color: "white",
                fontSize: "14px",
                fontWeight: "500",
                cursor: "pointer"
              }}>
                ➕ Crear Usuario
              </button>
            </div>
            <UserManagementTable users={mockUsers} />
          </div>
        )}

        {activeTab === "audit" && (
          <div style={{
            background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
            borderRadius: "12px",
            padding: "24px",
            border: "1px solid rgba(71, 85, 105, 0.3)"
          }}>
            <AuditLogPanel logs={mockAuditLogs} />
          </div>
        )}

        {activeTab === "taxes" && (
          <div style={{
            background: "linear-gradient(135deg, rgba(30, 41, 59, 0.9) 0%, rgba(15, 23, 42, 0.9) 100%)",
            borderRadius: "12px",
            padding: "24px",
            border: "1px solid rgba(71, 85, 105, 0.3)"
          }}>
            <TaxRateConfiguration taxRates={mockTaxRates} />
          </div>
        )}

        {activeTab === "settings" && (
          <SystemSettings settings={mockCompanySettings} />
        )}
      </div>
    </div>
  );
}
