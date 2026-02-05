/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: licensing@jerp.io
 */

interface NoResultsDisplayProps {
  iconEmoji: string;
  headingText: string;
  descriptionText: string;
  actionButton?: {
    buttonText: string;
    onClickHandler: () => void;
  };
}

export function NoResultsDisplay({ 
  iconEmoji, 
  headingText, 
  descriptionText, 
  actionButton 
}: NoResultsDisplayProps) {
  return (
    <div style={{
      display: "flex",
      flexDirection: "column",
      alignItems: "center",
      justifyContent: "center",
      padding: "64px 32px",
      textAlign: "center"
    }}>
      <div style={{
        fontSize: "64px",
        marginBottom: "24px",
        opacity: 0.5
      }}>
        {iconEmoji}
      </div>
      <h3 style={{
        color: "#f1f5f9",
        fontSize: "20px",
        fontWeight: "600",
        marginBottom: "12px"
      }}>
        {headingText}
      </h3>
      <p style={{
        color: "#94a3b8",
        fontSize: "14px",
        marginBottom: "24px",
        maxWidth: "400px"
      }}>
        {descriptionText}
      </p>
      {actionButton && (
        <button
          onClick={actionButton.onClickHandler}
          style={{
            padding: "10px 24px",
            borderRadius: "8px",
            border: "none",
            background: "linear-gradient(135deg, #8b5cf6 0%, #7c3aed 100%)",
            color: "white",
            fontSize: "14px",
            fontWeight: "500",
            cursor: "pointer",
            transition: "transform 0.2s"
          }}
          onMouseEnter={(e) => {
            e.currentTarget.style.transform = "translateY(-2px)";
          }}
          onMouseLeave={(e) => {
            e.currentTarget.style.transform = "translateY(0)";
          }}
        >
          {actionButton.buttonText}
        </button>
      )}
    </div>
  );
}
