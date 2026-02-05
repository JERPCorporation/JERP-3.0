/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: licensing@jerp.io
 */

interface TablePaginatorProps {
  currentPageIndex: number;
  totalRecords: number;
  recordsPerPage: number;
  onPageIndexChange: (newIndex: number) => void;
  onRecordsPerPageChange: (newSize: number) => void;
}

export function TablePaginator({
  currentPageIndex,
  totalRecords,
  recordsPerPage,
  onPageIndexChange,
  onRecordsPerPageChange
}: TablePaginatorProps) {
  const totalPageCount = Math.ceil(totalRecords / recordsPerPage);
  const startRecord = currentPageIndex * recordsPerPage + 1;
  const endRecord = Math.min((currentPageIndex + 1) * recordsPerPage, totalRecords);
  
  const availableSizes = [10, 25, 50, 100];
  
  return (
    <div style={{
      display: "flex",
      alignItems: "center",
      justifyContent: "space-between",
      padding: "16px 0",
      flexWrap: "wrap",
      gap: "16px"
    }}>
      <div style={{ display: "flex", alignItems: "center", gap: "12px" }}>
        <span style={{ color: "#94a3b8", fontSize: "14px" }}>
          Rows per page:
        </span>
        <select
          value={recordsPerPage}
          onChange={(e) => onRecordsPerPageChange(Number(e.target.value))}
          style={{
            padding: "6px 32px 6px 12px",
            borderRadius: "6px",
            border: "1px solid rgba(71, 85, 105, 0.5)",
            background: "rgba(15, 23, 42, 0.8)",
            color: "#f1f5f9",
            fontSize: "14px",
            cursor: "pointer",
            outline: "none"
          }}
        >
          {availableSizes.map(size => (
            <option key={size} value={size}>{size}</option>
          ))}
        </select>
      </div>
      
      <div style={{ color: "#94a3b8", fontSize: "14px" }}>
        {totalRecords > 0 ? `${startRecord}-${endRecord} of ${totalRecords}` : '0 records'}
      </div>
      
      <div style={{ display: "flex", gap: "8px" }}>
        <button
          onClick={() => onPageIndexChange(0)}
          disabled={currentPageIndex === 0}
          style={{
            padding: "8px 12px",
            borderRadius: "6px",
            border: "1px solid rgba(71, 85, 105, 0.5)",
            background: currentPageIndex === 0 ? "rgba(15, 23, 42, 0.5)" : "rgba(15, 23, 42, 0.8)",
            color: currentPageIndex === 0 ? "#64748b" : "#f1f5f9",
            fontSize: "14px",
            cursor: currentPageIndex === 0 ? "not-allowed" : "pointer"
          }}
        >
          ⟪
        </button>
        <button
          onClick={() => onPageIndexChange(currentPageIndex - 1)}
          disabled={currentPageIndex === 0}
          style={{
            padding: "8px 12px",
            borderRadius: "6px",
            border: "1px solid rgba(71, 85, 105, 0.5)",
            background: currentPageIndex === 0 ? "rgba(15, 23, 42, 0.5)" : "rgba(15, 23, 42, 0.8)",
            color: currentPageIndex === 0 ? "#64748b" : "#f1f5f9",
            fontSize: "14px",
            cursor: currentPageIndex === 0 ? "not-allowed" : "pointer"
          }}
        >
          ‹
        </button>
        
        {Array.from({ length: totalPageCount }, (_, i) => i)
          .filter(i => {
            if (totalPageCount <= 7) return true;
            if (i === 0 || i === totalPageCount - 1) return true;
            return Math.abs(i - currentPageIndex) <= 1;
          })
          .map((pageNum, idx, arr) => {
            if (idx > 0 && arr[idx - 1] !== pageNum - 1) {
              return (
                <span key={`ellipsis-${pageNum}`} style={{
                  padding: "8px 4px",
                  color: "#64748b"
                }}>
                  ...
                </span>
              );
            }
            return (
              <button
                key={pageNum}
                onClick={() => onPageIndexChange(pageNum)}
                style={{
                  padding: "8px 12px",
                  borderRadius: "6px",
                  border: "1px solid rgba(71, 85, 105, 0.5)",
                  background: pageNum === currentPageIndex ? "#8b5cf6" : "rgba(15, 23, 42, 0.8)",
                  color: "#f1f5f9",
                  fontSize: "14px",
                  cursor: "pointer",
                  fontWeight: pageNum === currentPageIndex ? "600" : "400"
                }}
              >
                {pageNum + 1}
              </button>
            );
          })}
        
        <button
          onClick={() => onPageIndexChange(currentPageIndex + 1)}
          disabled={currentPageIndex >= totalPageCount - 1}
          style={{
            padding: "8px 12px",
            borderRadius: "6px",
            border: "1px solid rgba(71, 85, 105, 0.5)",
            background: currentPageIndex >= totalPageCount - 1 ? "rgba(15, 23, 42, 0.5)" : "rgba(15, 23, 42, 0.8)",
            color: currentPageIndex >= totalPageCount - 1 ? "#64748b" : "#f1f5f9",
            fontSize: "14px",
            cursor: currentPageIndex >= totalPageCount - 1 ? "not-allowed" : "pointer"
          }}
        >
          ›
        </button>
        <button
          onClick={() => onPageIndexChange(totalPageCount - 1)}
          disabled={currentPageIndex >= totalPageCount - 1}
          style={{
            padding: "8px 12px",
            borderRadius: "6px",
            border: "1px solid rgba(71, 85, 105, 0.5)",
            background: currentPageIndex >= totalPageCount - 1 ? "rgba(15, 23, 42, 0.5)" : "rgba(15, 23, 42, 0.8)",
            color: currentPageIndex >= totalPageCount - 1 ? "#64748b" : "#f1f5f9",
            fontSize: "14px",
            cursor: currentPageIndex >= totalPageCount - 1 ? "not-allowed" : "pointer"
          }}
        >
          ⟫
        </button>
      </div>
    </div>
  );
}
