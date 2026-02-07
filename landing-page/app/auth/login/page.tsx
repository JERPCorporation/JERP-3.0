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

import { useState, FormEvent } from "react";
import { mockLogin } from "@/lib/auth-mock";
import Link from "next/link";

export default function LoginPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [rememberMe, setRememberMe] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const handleLogin = async (e: FormEvent) => {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      // TODO: Replace with actual API call to /api/auth/login
      const result = await mockLogin(email, password);

      // Store in localStorage
      localStorage.setItem("jerp-user", JSON.stringify(result.user));
      localStorage.setItem("jerp-token", result.token);

      // Redirect based on role
      if (result.user.role === "super_admin") {
        window.location.href = "/admin";
      } else {
        window.location.href = "/dashboard";
      }
    } catch (err: any) {
      setError(err.message || "Error al iniciar sesión");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div
      style={{
        minHeight: "100vh",
        background: "linear-gradient(135deg, #f0f9ff 0%, #e0f2fe 50%, #f0f9ff 100%)",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        padding: "20px",
        fontFamily: "'IBM Plex Sans', -apple-system, BlinkMacSystemFont, sans-serif",
      }}
    >
      <div
        style={{
          width: "100%",
          maxWidth: "500px",
          background: "#ffffff",
          borderRadius: "20px",
          boxShadow: "0 8px 24px rgba(0, 0, 0, 0.12)",
          padding: "48px",
        }}
      >
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
            Inicia sesión en tu cuenta
          </p>
        </div>

        {/* Error Message */}
        {error && (
          <div
            style={{
              background: "#fee2e2",
              border: "1px solid #ef4444",
              borderRadius: "12px",
              padding: "12px 16px",
              marginBottom: "24px",
              color: "#ef4444",
              fontSize: "14px",
            }}
          >
            {error}
          </div>
        )}

        {/* Login Form */}
        <form onSubmit={handleLogin}>
          {/* Email Field */}
          <div style={{ marginBottom: "20px" }}>
            <label
              htmlFor="email"
              style={{
                display: "block",
                fontSize: "14px",
                fontWeight: "600",
                color: "#334155",
                marginBottom: "8px",
              }}
            >
              Email
            </label>
            <input
              id="email"
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="tu@email.com"
              required
              style={{
                width: "100%",
                padding: "12px 16px",
                fontSize: "16px",
                border: "1px solid #cbd5e1",
                borderRadius: "12px",
                outline: "none",
                transition: "border-color 0.2s",
              }}
              onFocus={(e) => (e.target.style.borderColor = "#8b5cf6")}
              onBlur={(e) => (e.target.style.borderColor = "#cbd5e1")}
            />
          </div>

          {/* Password Field */}
          <div style={{ marginBottom: "20px" }}>
            <label
              htmlFor="password"
              style={{
                display: "block",
                fontSize: "14px",
                fontWeight: "600",
                color: "#334155",
                marginBottom: "8px",
              }}
            >
              Contraseña
            </label>
            <div style={{ position: "relative" }}>
              <input
                id="password"
                type={showPassword ? "text" : "password"}
                value={password}
                onChange={(e) => setPassword(e.target.value)}
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
                  transition: "border-color 0.2s",
                }}
                onFocus={(e) => (e.target.style.borderColor = "#8b5cf6")}
                onBlur={(e) => (e.target.style.borderColor = "#cbd5e1")}
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
                  color: "#64748b",
                }}
              >
                {showPassword ? "👁️" : "👁️‍🗨️"}
              </button>
            </div>
          </div>

          {/* Remember Me & Forgot Password */}
          <div
            style={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              marginBottom: "24px",
            }}
          >
            <label
              style={{
                display: "flex",
                alignItems: "center",
                fontSize: "14px",
                color: "#475569",
                cursor: "pointer",
              }}
            >
              <input
                type="checkbox"
                checked={rememberMe}
                onChange={(e) => setRememberMe(e.target.checked)}
                style={{ marginRight: "8px", cursor: "pointer" }}
              />
              Recordar mis datos
            </label>
            <Link
              href="/auth/forgot-password"
              style={{
                fontSize: "14px",
                color: "#8b5cf6",
                textDecoration: "none",
                fontWeight: "500",
              }}
            >
              ¿Olvidaste tu contraseña?
            </Link>
          </div>

          {/* Login Button */}
          <button
            type="submit"
            disabled={loading}
            style={{
              width: "100%",
              padding: "16px",
              fontSize: "16px",
              fontWeight: "600",
              color: "#ffffff",
              background: loading
                ? "#94a3b8"
                : "linear-gradient(135deg, #8b5cf6, #3b82f6)",
              border: "none",
              borderRadius: "12px",
              cursor: loading ? "not-allowed" : "pointer",
              boxShadow: loading ? "none" : "0 4px 16px rgba(139, 92, 246, 0.3)",
              transition: "all 0.2s",
            }}
            onMouseOver={(e) => {
              if (!loading) {
                e.currentTarget.style.transform = "translateY(-2px)";
                e.currentTarget.style.boxShadow =
                  "0 6px 20px rgba(139, 92, 246, 0.4)";
              }
            }}
            onMouseOut={(e) => {
              if (!loading) {
                e.currentTarget.style.transform = "translateY(0)";
                e.currentTarget.style.boxShadow =
                  "0 4px 16px rgba(139, 92, 246, 0.3)";
              }
            }}
          >
            {loading ? "Iniciando sesión..." : "Iniciar Sesión"}
          </button>
        </form>

        {/* Divider */}
        <div
          style={{
            display: "flex",
            alignItems: "center",
            margin: "32px 0",
          }}
        >
          <div
            style={{ flex: 1, height: "1px", background: "#e2e8f0" }}
          ></div>
          <span
            style={{
              padding: "0 16px",
              fontSize: "14px",
              color: "#94a3b8",
            }}
          >
            O continúa con
          </span>
          <div
            style={{ flex: 1, height: "1px", background: "#e2e8f0" }}
          ></div>
        </div>

        {/* Social Login Buttons */}
        <div style={{ display: "grid", gap: "12px", marginBottom: "32px" }}>
          <button
            type="button"
            style={{
              width: "100%",
              padding: "12px",
              fontSize: "14px",
              fontWeight: "500",
              color: "#334155",
              background: "#ffffff",
              border: "1px solid #cbd5e1",
              borderRadius: "12px",
              cursor: "pointer",
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              gap: "8px",
              transition: "all 0.2s",
            }}
            onMouseOver={(e) => {
              e.currentTarget.style.borderColor = "#8b5cf6";
              e.currentTarget.style.background = "#faf5ff";
            }}
            onMouseOut={(e) => {
              e.currentTarget.style.borderColor = "#cbd5e1";
              e.currentTarget.style.background = "#ffffff";
            }}
          >
            <span style={{ fontSize: "18px" }}>🔍</span> Continuar con Google
          </button>
          <button
            type="button"
            style={{
              width: "100%",
              padding: "12px",
              fontSize: "14px",
              fontWeight: "500",
              color: "#334155",
              background: "#ffffff",
              border: "1px solid #cbd5e1",
              borderRadius: "12px",
              cursor: "pointer",
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              gap: "8px",
              transition: "all 0.2s",
            }}
            onMouseOver={(e) => {
              e.currentTarget.style.borderColor = "#8b5cf6";
              e.currentTarget.style.background = "#faf5ff";
            }}
            onMouseOut={(e) => {
              e.currentTarget.style.borderColor = "#cbd5e1";
              e.currentTarget.style.background = "#ffffff";
            }}
          >
            <span style={{ fontSize: "18px" }}>🪟</span> Continuar con Microsoft
          </button>
        </div>

        {/* Footer Links */}
        <div style={{ textAlign: "center" }}>
          <p style={{ fontSize: "14px", color: "#64748b", marginBottom: "16px" }}>
            ¿No tienes una cuenta?{" "}
            <Link
              href="/auth/signup"
              style={{
                color: "#8b5cf6",
                textDecoration: "none",
                fontWeight: "600",
              }}
            >
              Crear cuenta
            </Link>
          </p>
          <div style={{ fontSize: "12px", color: "#94a3b8" }}>
            <Link
              href="/terms"
              style={{ color: "#94a3b8", textDecoration: "none", marginRight: "16px" }}
            >
              Términos
            </Link>
            <Link href="/privacy" style={{ color: "#94a3b8", textDecoration: "none" }}>
              Privacidad
            </Link>
          </div>
        </div>

        {/* Demo Credentials */}
        <div
          style={{
            marginTop: "32px",
            padding: "16px",
            background: "#f8fafc",
            borderRadius: "12px",
            fontSize: "12px",
            color: "#475569",
          }}
        >
          <strong>Demo Credentials:</strong>
          <br />
          Admin: ichbincesartobar@yahoo.com / admin123
          <br />
          User: user@jerp.com / user123
        </div>
      </div>
    </div>
  );
}
