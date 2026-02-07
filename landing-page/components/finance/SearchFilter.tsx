/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: licensing@jerp.io
 */

interface SearchFilterProps {
  searchTerm: string;
  onSearchChange: (term: string) => void;
  placeholderText: string;
}

export function SearchFilter({ searchTerm, onSearchChange, placeholderText }: SearchFilterProps) {
  return (
    <div style={{
      position: "relative",
      width: "100%"
    }}>
      <div style={{
        position: "absolute",
        left: "16px",
        top: "50%",
        transform: "translateY(-50%)",
        color: "#64748b",
        fontSize: "18px",
        pointerEvents: "none"
      }}>
        🔍
      </div>
      <input
        type="text"
        value={searchTerm}
        onChange={(e) => onSearchChange(e.target.value)}
        placeholder={placeholderText}
        style={{
          width: "100%",
          padding: "12px 16px 12px 48px",
          borderRadius: "8px",
          border: "1px solid rgba(71, 85, 105, 0.5)",
          background: "rgba(15, 23, 42, 0.8)",
          color: "#f1f5f9",
          fontSize: "14px",
          outline: "none",
          transition: "all 0.2s"
        }}
        onFocus={(e) => {
          e.currentTarget.style.borderColor = "#8b5cf6";
          e.currentTarget.style.boxShadow = "0 0 0 3px rgba(139, 92, 246, 0.1)";
        }}
        onBlur={(e) => {
          e.currentTarget.style.borderColor = "rgba(71, 85, 105, 0.5)";
          e.currentTarget.style.boxShadow = "none";
        }}
      />
      {searchTerm && (
        <button
          onClick={() => onSearchChange('')}
          style={{
            position: "absolute",
            right: "12px",
            top: "50%",
            transform: "translateY(-50%)",
            background: "transparent",
            border: "none",
            color: "#94a3b8",
            fontSize: "16px",
            cursor: "pointer",
            padding: "4px 8px",
            borderRadius: "4px"
          }}
          onMouseEnter={(e) => {
            e.currentTarget.style.background = "rgba(71, 85, 105, 0.3)";
          }}
          onMouseLeave={(e) => {
            e.currentTarget.style.background = "transparent";
          }}
        >
          ✕
        </button>
      )}
    </div>
  );
}
