/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: licensing@jerp.io
 */

type BadgeVariant = 'success' | 'warning' | 'danger' | 'info' | 'neutral';

interface StatusIndicatorProps {
  label: string;
  variant: BadgeVariant;
  showDot?: boolean;
}

const variantStyles: Record<BadgeVariant, { bg: string; text: string; dot: string }> = {
  success: {
    bg: 'rgba(16, 185, 129, 0.15)',
    text: '#10b981',
    dot: '#10b981'
  },
  warning: {
    bg: 'rgba(245, 158, 11, 0.15)',
    text: '#f59e0b',
    dot: '#f59e0b'
  },
  danger: {
    bg: 'rgba(239, 68, 68, 0.15)',
    text: '#ef4444',
    dot: '#ef4444'
  },
  info: {
    bg: 'rgba(59, 130, 246, 0.15)',
    text: '#3b82f6',
    dot: '#3b82f6'
  },
  neutral: {
    bg: 'rgba(100, 116, 139, 0.15)',
    text: '#94a3b8',
    dot: '#94a3b8'
  }
};

export function StatusIndicator({ label, variant, showDot = false }: StatusIndicatorProps) {
  const styles = variantStyles[variant];
  
  return (
    <span style={{
      display: "inline-flex",
      alignItems: "center",
      gap: "6px",
      padding: "4px 12px",
      borderRadius: "12px",
      background: styles.bg,
      color: styles.text,
      fontSize: "11px",
      fontWeight: "600",
      textTransform: "uppercase",
      letterSpacing: "0.05em"
    }}>
      {showDot && (
        <span style={{
          width: "6px",
          height: "6px",
          borderRadius: "50%",
          background: styles.dot
        }} />
      )}
      {label}
    </span>
  );
}
