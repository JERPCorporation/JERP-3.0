/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: ichbincesartobar@yahoo.com
 */

"use client";

import { useState } from "react";
import Link from "next/link";

type Plan = "free" | "starter" | "pro" | "enterprise";

interface SignupData {
  plan: Plan;
  companyName: string;
  companySize: string;
  industry: string;
  taxId: string;
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  phone: string;
}

const PLANS = [
  {
    id: "free" as Plan,
    name: "Free",
    price: "$0",
    period: "/mes",
    features: [
      "Hasta 5 empleados",
      "Nómina básica",
      "Reportes estándar",
      "Soporte por email",
    ],
    color: "#64748b",
  },
  {
    id: "starter" as Plan,
    name: "Starter",
    price: "$49",
    period: "/mes",
    features: [
      "Hasta 25 empleados",
      "Nómina completa",
      "Compliance automático",
      "Soporte prioritario",
      "Reportes avanzados",
    ],
    color: "#3b82f6",
    recommended: true,
  },
  {
    id: "pro" as Plan,
    name: "Pro",
    price: "$149",
    period: "/mes",
    features: [
      "Hasta 100 empleados",
      "Todo en Starter +",
      "API access",
      "Integraciones",
      "Auditoría completa",
      "Soporte 24/7",
    ],
    color: "#8b5cf6",
  },
  {
    id: "enterprise" as Plan,
    name: "Enterprise",
    price: "Custom",
    period: "",
    features: [
      "Empleados ilimitados",
      "Todo en Pro +",
      "Cuenta dedicada",
      "SLA garantizado",
      "Customización",
      "Onboarding personalizado",
    ],
    color: "#10b981",
  },
];

export default function SignupPage() {
  const [step, setStep] = useState(1);
  const [data, setData] = useState<SignupData>({
    plan: "starter",
    companyName: "",
    companySize: "",
    industry: "",
    taxId: "",
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    phone: "",
  });
  const [loading, setLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);

  const handleNext = () => {
    if (step < 4) setStep(step + 1);
  };

  const handleBack = () => {
    if (step > 1) setStep(step - 1);
  };

  const handleSubmit = async () => {
    setLoading(true);
    // TODO: Replace with actual API call
    setTimeout(() => {
      setLoading(false);
      if (data.plan === "free") {
        setStep(5); // Success screen
      } else {
        setStep(4); // Payment screen
      }
    }, 1000);
  };

  const handleComplete = () => {
    // Store user data and redirect
    localStorage.setItem("jerp-user", JSON.stringify({
      id: `user-${Date.now()}-${Math.random().toString(36).substr(2, 9)}`,
      email: data.email,
      name: `${data.firstName} ${data.lastName}`,
      role: "user",
    }));
    window.location.href = "/dashboard";
  };

  return (
    <div
      style={{
        minHeight: "100vh",
        background: "linear-gradient(135deg, #f0f9ff 0%, #e0f2fe 50%, #f0f9ff 100%)",
        padding: "40px 20px",
        fontFamily: "'IBM Plex Sans', -apple-system, BlinkMacSystemFont, sans-serif",
      }}
    >
      <div style={{ maxWidth: "1000px", margin: "0 auto" }}>
        {/* Header */}
        <div style={{ textAlign: "center", marginBottom: "40px" }}>
          <h1
            style={{
              fontSize: "36px",
              fontWeight: "700",
              background: "linear-gradient(135deg, #8b5cf6, #3b82f6)",
              WebkitBackgroundClip: "text",
              WebkitTextFillColor: "transparent",
              backgroundClip: "text",
              marginBottom: "8px",
            }}
          >
            JERP Payroll
          </h1>
          <p style={{ fontSize: "16px", color: "#64748b" }}>
            {step === 5 ? "¡Bienvenido a JERP!" : "Crea tu cuenta en minutos"}
          </p>
        </div>

        {/* Progress Steps */}
        {step < 5 && (
          <div
            style={{
              display: "flex",
              justifyContent: "center",
              marginBottom: "40px",
              gap: "16px",
            }}
          >
            {["Plan", "Empresa", "Admin", "Pago"].map((label, idx) => (
              <div
                key={idx}
                style={{
                  display: "flex",
                  alignItems: "center",
                  gap: "8px",
                }}
              >
                <div
                  style={{
                    width: "32px",
                    height: "32px",
                    borderRadius: "50%",
                    background:
                      step > idx + 1
                        ? "#10b981"
                        : step === idx + 1
                        ? "linear-gradient(135deg, #8b5cf6, #3b82f6)"
                        : "#e2e8f0",
                    color: step >= idx + 1 ? "#ffffff" : "#94a3b8",
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
                    fontSize: "14px",
                    fontWeight: "600",
                  }}
                >
                  {step > idx + 1 ? "✓" : idx + 1}
                </div>
                <span
                  style={{
                    fontSize: "14px",
                    fontWeight: "500",
                    color: step >= idx + 1 ? "#334155" : "#94a3b8",
                  }}
                >
                  {label}
                </span>
                {idx < 3 && (
                  <div
                    style={{
                      width: "40px",
                      height: "2px",
                      background: step > idx + 1 ? "#10b981" : "#e2e8f0",
                    }}
                  />
                )}
              </div>
            ))}
          </div>
        )}

        {/* Step Content */}
        <div
          style={{
            background: "#ffffff",
            borderRadius: "20px",
            boxShadow: "0 8px 24px rgba(0, 0, 0, 0.12)",
            padding: "48px",
          }}
        >
          {/* Step 1: Plan Selection */}
          {step === 1 && (
            <div>
              <h2
                style={{
                  fontSize: "24px",
                  fontWeight: "600",
                  color: "#1e293b",
                  marginBottom: "8px",
                  textAlign: "center",
                }}
              >
                Selecciona tu plan
              </h2>
              <p
                style={{
                  fontSize: "14px",
                  color: "#64748b",
                  marginBottom: "32px",
                  textAlign: "center",
                }}
              >
                Puedes cambiar o cancelar en cualquier momento
              </p>

              <div
                style={{
                  display: "grid",
                  gridTemplateColumns: "repeat(auto-fit, minmax(200px, 1fr))",
                  gap: "20px",
                  marginBottom: "32px",
                }}
              >
                {PLANS.map((plan) => (
                  <div
                    key={plan.id}
                    onClick={() => setData({ ...data, plan: plan.id })}
                    style={{
                      border:
                        data.plan === plan.id
                          ? `2px solid ${plan.color}`
                          : "2px solid #e2e8f0",
                      borderRadius: "16px",
                      padding: "24px",
                      cursor: "pointer",
                      position: "relative",
                      transition: "all 0.2s",
                      background:
                        data.plan === plan.id
                          ? `${plan.color}10`
                          : "#ffffff",
                    }}
                  >
                    {plan.recommended && (
                      <div
                        style={{
                          position: "absolute",
                          top: "-10px",
                          right: "16px",
                          background: plan.color,
                          color: "#ffffff",
                          fontSize: "11px",
                          fontWeight: "600",
                          padding: "4px 12px",
                          borderRadius: "12px",
                        }}
                      >
                        Recomendado
                      </div>
                    )}
                    <h3
                      style={{
                        fontSize: "18px",
                        fontWeight: "600",
                        color: plan.color,
                        marginBottom: "8px",
                      }}
                    >
                      {plan.name}
                    </h3>
                    <div style={{ marginBottom: "16px" }}>
                      <span
                        style={{
                          fontSize: "32px",
                          fontWeight: "700",
                          color: "#1e293b",
                        }}
                      >
                        {plan.price}
                      </span>
                      <span style={{ fontSize: "14px", color: "#64748b" }}>
                        {plan.period}
                      </span>
                    </div>
                    <ul style={{ listStyle: "none", padding: 0, margin: 0 }}>
                      {plan.features.map((feature, idx) => (
                        <li
                          key={idx}
                          style={{
                            fontSize: "13px",
                            color: "#475569",
                            marginBottom: "8px",
                            display: "flex",
                            alignItems: "start",
                            gap: "8px",
                          }}
                        >
                          <span style={{ color: plan.color }}>✓</span>
                          {feature}
                        </li>
                      ))}
                    </ul>
                  </div>
                ))}
              </div>

              <button
                onClick={handleNext}
                style={{
                  width: "100%",
                  padding: "16px",
                  fontSize: "16px",
                  fontWeight: "600",
                  color: "#ffffff",
                  background: "linear-gradient(135deg, #8b5cf6, #3b82f6)",
                  border: "none",
                  borderRadius: "12px",
                  cursor: "pointer",
                  boxShadow: "0 4px 16px rgba(139, 92, 246, 0.3)",
                }}
              >
                Continuar
              </button>
            </div>
          )}

          {/* Step 2: Company Information */}
          {step === 2 && (
            <div>
              <h2
                style={{
                  fontSize: "24px",
                  fontWeight: "600",
                  color: "#1e293b",
                  marginBottom: "8px",
                  textAlign: "center",
                }}
              >
                Información de la empresa
              </h2>
              <p
                style={{
                  fontSize: "14px",
                  color: "#64748b",
                  marginBottom: "32px",
                  textAlign: "center",
                }}
              >
                Completa los datos de tu organización
              </p>

              <div style={{ display: "grid", gap: "20px", marginBottom: "32px" }}>
                <div>
                  <label
                    style={{
                      display: "block",
                      fontSize: "14px",
                      fontWeight: "600",
                      color: "#334155",
                      marginBottom: "8px",
                    }}
                  >
                    Nombre de la empresa *
                  </label>
                  <input
                    type="text"
                    value={data.companyName}
                    onChange={(e) =>
                      setData({ ...data, companyName: e.target.value })
                    }
                    placeholder="Mi Empresa Inc."
                    required
                    style={{
                      width: "100%",
                      padding: "12px 16px",
                      fontSize: "16px",
                      border: "1px solid #cbd5e1",
                      borderRadius: "12px",
                      outline: "none",
                    }}
                  />
                </div>

                <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "16px" }}>
                  <div>
                    <label
                      style={{
                        display: "block",
                        fontSize: "14px",
                        fontWeight: "600",
                        color: "#334155",
                        marginBottom: "8px",
                      }}
                    >
                      Tamaño de empresa *
                    </label>
                    <select
                      value={data.companySize}
                      onChange={(e) =>
                        setData({ ...data, companySize: e.target.value })
                      }
                      required
                      style={{
                        width: "100%",
                        padding: "12px 16px",
                        fontSize: "16px",
                        border: "1px solid #cbd5e1",
                        borderRadius: "12px",
                        outline: "none",
                      }}
                    >
                      <option value="">Seleccionar</option>
                      <option value="1-10">1-10 empleados</option>
                      <option value="11-50">11-50 empleados</option>
                      <option value="51-200">51-200 empleados</option>
                      <option value="201+">201+ empleados</option>
                    </select>
                  </div>

                  <div>
                    <label
                      style={{
                        display: "block",
                        fontSize: "14px",
                        fontWeight: "600",
                        color: "#334155",
                        marginBottom: "8px",
                      }}
                    >
                      Industria *
                    </label>
                    <select
                      value={data.industry}
                      onChange={(e) =>
                        setData({ ...data, industry: e.target.value })
                      }
                      required
                      style={{
                        width: "100%",
                        padding: "12px 16px",
                        fontSize: "16px",
                        border: "1px solid #cbd5e1",
                        borderRadius: "12px",
                        outline: "none",
                      }}
                    >
                      <option value="">Seleccionar</option>
                      <option value="technology">Tecnología</option>
                      <option value="retail">Retail</option>
                      <option value="healthcare">Salud</option>
                      <option value="finance">Finanzas</option>
                      <option value="manufacturing">Manufactura</option>
                      <option value="other">Otra</option>
                    </select>
                  </div>
                </div>

                <div>
                  <label
                    style={{
                      display: "block",
                      fontSize: "14px",
                      fontWeight: "600",
                      color: "#334155",
                      marginBottom: "8px",
                    }}
                  >
                    Tax ID / RFC *
                  </label>
                  <input
                    type="text"
                    value={data.taxId}
                    onChange={(e) => setData({ ...data, taxId: e.target.value })}
                    placeholder="XX-XXXXXXX"
                    required
                    style={{
                      width: "100%",
                      padding: "12px 16px",
                      fontSize: "16px",
                      border: "1px solid #cbd5e1",
                      borderRadius: "12px",
                      outline: "none",
                    }}
                  />
                </div>
              </div>

              <div style={{ display: "flex", gap: "16px" }}>
                <button
                  onClick={handleBack}
                  style={{
                    flex: 1,
                    padding: "16px",
                    fontSize: "16px",
                    fontWeight: "600",
                    color: "#334155",
                    background: "#ffffff",
                    border: "1px solid #cbd5e1",
                    borderRadius: "12px",
                    cursor: "pointer",
                  }}
                >
                  Atrás
                </button>
                <button
                  onClick={handleNext}
                  disabled={!data.companyName || !data.companySize || !data.industry || !data.taxId}
                  style={{
                    flex: 1,
                    padding: "16px",
                    fontSize: "16px",
                    fontWeight: "600",
                    color: "#ffffff",
                    background: (!data.companyName || !data.companySize || !data.industry || !data.taxId)
                      ? "#cbd5e1"
                      : "linear-gradient(135deg, #8b5cf6, #3b82f6)",
                    border: "none",
                    borderRadius: "12px",
                    cursor: (!data.companyName || !data.companySize || !data.industry || !data.taxId)
                      ? "not-allowed"
                      : "pointer",
                    boxShadow: (!data.companyName || !data.companySize || !data.industry || !data.taxId)
                      ? "none"
                      : "0 4px 16px rgba(139, 92, 246, 0.3)",
                  }}
                >
                  Continuar
                </button>
              </div>
            </div>
          )}

          {/* Step 3: Admin Account */}
          {step === 3 && (
            <div>
              <h2
                style={{
                  fontSize: "24px",
                  fontWeight: "600",
                  color: "#1e293b",
                  marginBottom: "8px",
                  textAlign: "center",
                }}
              >
                Crea tu cuenta de administrador
              </h2>
              <p
                style={{
                  fontSize: "14px",
                  color: "#64748b",
                  marginBottom: "32px",
                  textAlign: "center",
                }}
              >
                Esta será tu cuenta principal
              </p>

              <div style={{ display: "grid", gap: "20px", marginBottom: "32px" }}>
                <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "16px" }}>
                  <div>
                    <label
                      style={{
                        display: "block",
                        fontSize: "14px",
                        fontWeight: "600",
                        color: "#334155",
                        marginBottom: "8px",
                      }}
                    >
                      Nombre *
                    </label>
                    <input
                      type="text"
                      value={data.firstName}
                      onChange={(e) =>
                        setData({ ...data, firstName: e.target.value })
                      }
                      placeholder="Juan"
                      required
                      style={{
                        width: "100%",
                        padding: "12px 16px",
                        fontSize: "16px",
                        border: "1px solid #cbd5e1",
                        borderRadius: "12px",
                        outline: "none",
                      }}
                    />
                  </div>

                  <div>
                    <label
                      style={{
                        display: "block",
                        fontSize: "14px",
                        fontWeight: "600",
                        color: "#334155",
                        marginBottom: "8px",
                      }}
                    >
                      Apellido *
                    </label>
                    <input
                      type="text"
                      value={data.lastName}
                      onChange={(e) =>
                        setData({ ...data, lastName: e.target.value })
                      }
                      placeholder="Pérez"
                      required
                      style={{
                        width: "100%",
                        padding: "12px 16px",
                        fontSize: "16px",
                        border: "1px solid #cbd5e1",
                        borderRadius: "12px",
                        outline: "none",
                      }}
                    />
                  </div>
                </div>

                <div>
                  <label
                    style={{
                      display: "block",
                      fontSize: "14px",
                      fontWeight: "600",
                      color: "#334155",
                      marginBottom: "8px",
                    }}
                  >
                    Email *
                  </label>
                  <input
                    type="email"
                    value={data.email}
                    onChange={(e) => setData({ ...data, email: e.target.value })}
                    placeholder="tu@email.com"
                    required
                    style={{
                      width: "100%",
                      padding: "12px 16px",
                      fontSize: "16px",
                      border: "1px solid #cbd5e1",
                      borderRadius: "12px",
                      outline: "none",
                    }}
                  />
                </div>

                <div>
                  <label
                    style={{
                      display: "block",
                      fontSize: "14px",
                      fontWeight: "600",
                      color: "#334155",
                      marginBottom: "8px",
                    }}
                  >
                    Teléfono *
                  </label>
                  <input
                    type="tel"
                    value={data.phone}
                    onChange={(e) => setData({ ...data, phone: e.target.value })}
                    placeholder="+1 (555) 123-4567"
                    required
                    style={{
                      width: "100%",
                      padding: "12px 16px",
                      fontSize: "16px",
                      border: "1px solid #cbd5e1",
                      borderRadius: "12px",
                      outline: "none",
                    }}
                  />
                </div>

                <div>
                  <label
                    style={{
                      display: "block",
                      fontSize: "14px",
                      fontWeight: "600",
                      color: "#334155",
                      marginBottom: "8px",
                    }}
                  >
                    Contraseña *
                  </label>
                  <div style={{ position: "relative" }}>
                    <input
                      type={showPassword ? "text" : "password"}
                      value={data.password}
                      onChange={(e) =>
                        setData({ ...data, password: e.target.value })
                      }
                      placeholder="••••••••"
                      required
                      style={{
                        width: "100%",
                        padding: "12px 16px",
                        paddingRight: "48px",
                        fontSize: "16px",
                        border: "1px solid #cbd5e1",
                        borderRadius: "12px",
                        outline: "none",
                      }}
                    />
                    <button
                      type="button"
                      onClick={() => setShowPassword(!showPassword)}
                      style={{
                        position: "absolute",
                        right: "12px",
                        top: "50%",
                        transform: "translateY(-50%)",
                        background: "none",
                        border: "none",
                        cursor: "pointer",
                        fontSize: "20px",
                      }}
                    >
                      {showPassword ? "👁️" : "👁️‍🗨️"}
                    </button>
                  </div>
                  <p style={{ fontSize: "12px", color: "#64748b", marginTop: "4px" }}>
                    Mínimo 8 caracteres
                  </p>
                </div>
              </div>

              <div style={{ display: "flex", gap: "16px" }}>
                <button
                  onClick={handleBack}
                  style={{
                    flex: 1,
                    padding: "16px",
                    fontSize: "16px",
                    fontWeight: "600",
                    color: "#334155",
                    background: "#ffffff",
                    border: "1px solid #cbd5e1",
                    borderRadius: "12px",
                    cursor: "pointer",
                  }}
                >
                  Atrás
                </button>
                <button
                  onClick={handleSubmit}
                  disabled={loading || !data.firstName || !data.lastName || !data.email || !data.phone || !data.password || data.password.length < 8}
                  style={{
                    flex: 1,
                    padding: "16px",
                    fontSize: "16px",
                    fontWeight: "600",
                    color: "#ffffff",
                    background: (loading || !data.firstName || !data.lastName || !data.email || !data.phone || !data.password || data.password.length < 8)
                      ? "#cbd5e1"
                      : "linear-gradient(135deg, #8b5cf6, #3b82f6)",
                    border: "none",
                    borderRadius: "12px",
                    cursor: (loading || !data.firstName || !data.lastName || !data.email || !data.phone || !data.password || data.password.length < 8)
                      ? "not-allowed"
                      : "pointer",
                    boxShadow: (loading || !data.firstName || !data.lastName || !data.email || !data.phone || !data.password || data.password.length < 8)
                      ? "none"
                      : "0 4px 16px rgba(139, 92, 246, 0.3)",
                  }}
                >
                  {loading ? "Creando cuenta..." : data.plan === "free" ? "Crear cuenta gratis" : "Continuar al pago"}
                </button>
              </div>
            </div>
          )}

          {/* Step 4: Payment (for paid plans) */}
          {step === 4 && data.plan !== "free" && (
            <div>
              <h2
                style={{
                  fontSize: "24px",
                  fontWeight: "600",
                  color: "#1e293b",
                  marginBottom: "8px",
                  textAlign: "center",
                }}
              >
                Información de pago
              </h2>
              <p
                style={{
                  fontSize: "14px",
                  color: "#64748b",
                  marginBottom: "32px",
                  textAlign: "center",
                }}
              >
                Completa tu suscripción al plan {PLANS.find((p) => p.id === data.plan)?.name}
              </p>

              <div
                style={{
                  background: "#f8fafc",
                  borderRadius: "12px",
                  padding: "24px",
                  marginBottom: "32px",
                }}
              >
                <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "12px" }}>
                  <span style={{ color: "#64748b" }}>Plan seleccionado:</span>
                  <span style={{ fontWeight: "600", color: "#1e293b" }}>
                    {PLANS.find((p) => p.id === data.plan)?.name}
                  </span>
                </div>
                <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "12px" }}>
                  <span style={{ color: "#64748b" }}>Precio mensual:</span>
                  <span style={{ fontWeight: "600", color: "#1e293b" }}>
                    {PLANS.find((p) => p.id === data.plan)?.price}/mes
                  </span>
                </div>
                <div
                  style={{
                    borderTop: "1px solid #cbd5e1",
                    marginTop: "12px",
                    paddingTop: "12px",
                    display: "flex",
                    justifyContent: "space-between",
                  }}
                >
                  <span style={{ fontWeight: "600", color: "#1e293b" }}>
                    Total hoy:
                  </span>
                  <span
                    style={{
                      fontSize: "20px",
                      fontWeight: "700",
                      color: "#8b5cf6",
                    }}
                  >
                    {PLANS.find((p) => p.id === data.plan)?.price}
                  </span>
                </div>
              </div>

              <div
                style={{
                  background: "#fef3c7",
                  border: "1px solid #f59e0b",
                  borderRadius: "12px",
                  padding: "16px",
                  marginBottom: "32px",
                  fontSize: "14px",
                  color: "#92400e",
                }}
              >
                <strong>💳 Demo Mode:</strong> Esta es una demostración. No se procesarán pagos reales.
              </div>

              <div style={{ display: "flex", gap: "16px" }}>
                <button
                  onClick={handleBack}
                  style={{
                    flex: 1,
                    padding: "16px",
                    fontSize: "16px",
                    fontWeight: "600",
                    color: "#334155",
                    background: "#ffffff",
                    border: "1px solid #cbd5e1",
                    borderRadius: "12px",
                    cursor: "pointer",
                  }}
                >
                  Atrás
                </button>
                <button
                  onClick={handleComplete}
                  style={{
                    flex: 1,
                    padding: "16px",
                    fontSize: "16px",
                    fontWeight: "600",
                    color: "#ffffff",
                    background: "linear-gradient(135deg, #8b5cf6, #3b82f6)",
                    border: "none",
                    borderRadius: "12px",
                    cursor: "pointer",
                    boxShadow: "0 4px 16px rgba(139, 92, 246, 0.3)",
                  }}
                >
                  Completar registro
                </button>
              </div>
            </div>
          )}

          {/* Step 5: Success */}
          {step === 5 && (
            <div style={{ textAlign: "center" }}>
              <div
                style={{
                  fontSize: "64px",
                  marginBottom: "24px",
                }}
              >
                🎉
              </div>
              <h2
                style={{
                  fontSize: "28px",
                  fontWeight: "600",
                  color: "#1e293b",
                  marginBottom: "16px",
                }}
              >
                ¡Cuenta creada exitosamente!
              </h2>
              <p
                style={{
                  fontSize: "16px",
                  color: "#64748b",
                  marginBottom: "32px",
                }}
              >
                Tu cuenta ha sido configurada. Ya puedes comenzar a usar JERP Payroll.
              </p>

              <div
                style={{
                  background: "#f0fdf4",
                  border: "1px solid #10b981",
                  borderRadius: "12px",
                  padding: "24px",
                  marginBottom: "32px",
                  textAlign: "left",
                }}
              >
                <h3
                  style={{
                    fontSize: "16px",
                    fontWeight: "600",
                    color: "#065f46",
                    marginBottom: "12px",
                  }}
                >
                  Próximos pasos:
                </h3>
                <ul
                  style={{
                    listStyle: "none",
                    padding: 0,
                    margin: 0,
                    fontSize: "14px",
                    color: "#047857",
                  }}
                >
                  <li style={{ marginBottom: "8px", display: "flex", alignItems: "start", gap: "8px" }}>
                    <span>✓</span>
                    <span>Configura los datos de tu empresa</span>
                  </li>
                  <li style={{ marginBottom: "8px", display: "flex", alignItems: "start", gap: "8px" }}>
                    <span>✓</span>
                    <span>Agrega a tus empleados</span>
                  </li>
                  <li style={{ marginBottom: "8px", display: "flex", alignItems: "start", gap: "8px" }}>
                    <span>✓</span>
                    <span>Procesa tu primera nómina</span>
                  </li>
                </ul>
              </div>

              <button
                onClick={handleComplete}
                style={{
                  width: "100%",
                  padding: "16px",
                  fontSize: "16px",
                  fontWeight: "600",
                  color: "#ffffff",
                  background: "linear-gradient(135deg, #8b5cf6, #3b82f6)",
                  border: "none",
                  borderRadius: "12px",
                  cursor: "pointer",
                  boxShadow: "0 4px 16px rgba(139, 92, 246, 0.3)",
                }}
              >
                Ir al Dashboard
              </button>
            </div>
          )}
        </div>

        {/* Footer */}
        {step < 5 && (
          <div style={{ textAlign: "center", marginTop: "24px" }}>
            <p style={{ fontSize: "14px", color: "#64748b" }}>
              ¿Ya tienes una cuenta?{" "}
              <Link
                href="/auth/login"
                style={{
                  color: "#8b5cf6",
                  textDecoration: "none",
                  fontWeight: "600",
                }}
              >
                Iniciar sesión
              </Link>
            </p>
          </div>
        )}
      </div>
    </div>
  );
}
