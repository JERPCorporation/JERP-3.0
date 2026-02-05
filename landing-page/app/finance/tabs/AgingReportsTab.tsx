/**
 * JERP 3.0 - Finance Module - Aging Reports Tab Component
 * Copyright (c) 2024 JERP. All rights reserved.
 * 
 * Displays AP and AR aging reports with color-coded time buckets
 */

'use client';

import { useState, useEffect } from 'react';
import { financeAPI } from '@/lib/finance/api-service';
import { formatCurrency } from '@/lib/finance/utils';

type AgingType = 'AP' | 'AR';

interface AgingEntry {
  name: string;
  current: number;
  days1to30: number;
  days31to60: number;
  days61to90: number;
  days90plus: number;
  total: number;
}

export default function AgingReportsTab() {
  const [reportType, setReportType] = useState<AgingType>('AR');
  const [agingData, setAgingData] = useState<AgingEntry[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    loadAgingData();
  }, [reportType]);

  const loadAgingData = async () => {
    setIsLoading(true);
    try {
      const data = await financeAPI.generateAgingSummary(reportType);
      setAgingData(data);
    } catch (error) {
      console.error('Failed to load aging data:', error);
      setAgingData([]);
    } finally {
      setIsLoading(false);
    }
  };

  const handleExport = (format: string) => {
    alert(`Exporting ${reportType} Aging Report as ${format.toUpperCase()}...\nThis would trigger a download in production.`);
  };

  const computeTotals = () => {
    return agingData.reduce(
      (totals, entry) => ({
        current: totals.current + entry.current,
        days1to30: totals.days1to30 + entry.days1to30,
        days31to60: totals.days31to60 + entry.days31to60,
        days61to90: totals.days61to90 + entry.days61to90,
        days90plus: totals.days90plus + entry.days90plus,
        total: totals.total + entry.total
      }),
      { current: 0, days1to30: 0, days31to60: 0, days61to90: 0, days90plus: 0, total: 0 }
    );
  };

  const totals = computeTotals();

  const getAmountColor = (amount: number, column: string): string => {
    if (amount === 0) return '#9ca3af';
    if (column === 'current') return '#059669';
    if (column === 'days1to30') return '#d97706';
    if (column === 'days31to60') return '#ea580c';
    if (column === 'days61to90') return '#dc2626';
    if (column === 'days90plus') return '#991b1b';
    return '#1f2937';
  };

  const getColumnBackground = (column: string): string => {
    if (column === 'current') return 'linear-gradient(135deg, #f0fdf4 0%, #dcfce7 100%)';
    if (column === 'days1to30') return 'linear-gradient(135deg, #fffbeb 0%, #fef3c7 100%)';
    if (column === 'days31to60') return 'linear-gradient(135deg, #fff7ed 0%, #fed7aa 100%)';
    if (column === 'days61to90') return 'linear-gradient(135deg, #fef2f2 0%, #fecaca 100%)';
    if (column === 'days90plus') return 'linear-gradient(135deg, #fef2f2 0%, #fee2e2 100%)';
    return 'white';
  };

  return (
    <div style={{ padding: '24px' }}>
      {/* Header with Toggle */}
      <div style={{ 
        display: 'flex', 
        justifyContent: 'space-between', 
        alignItems: 'center',
        marginBottom: '28px',
        flexWrap: 'wrap',
        gap: '16px'
      }}>
        <div>
          <h2 style={{ 
            fontSize: '26px', 
            fontWeight: '700', 
            margin: '0 0 8px 0',
            color: '#111827'
          }}>
            Aging Reports
          </h2>
          <p style={{ 
            fontSize: '14px', 
            color: '#6b7280',
            margin: 0
          }}>
            Analyze outstanding {reportType === 'AP' ? 'payables' : 'receivables'} by age category
          </p>
        </div>

        {/* Report Type Toggle */}
        <div style={{
          display: 'flex',
          background: '#f3f4f6',
          padding: '6px',
          borderRadius: '12px',
          border: '2px solid #e5e7eb'
        }}>
          <button
            onClick={() => setReportType('AR')}
            style={{
              padding: '12px 28px',
              border: 'none',
              background: reportType === 'AR' 
                ? 'linear-gradient(135deg, #3b82f6 0%, #2563eb 100%)' 
                : 'transparent',
              color: reportType === 'AR' ? 'white' : '#6b7280',
              borderRadius: '8px',
              fontSize: '14px',
              fontWeight: '700',
              cursor: 'pointer',
              transition: 'all 0.2s',
              boxShadow: reportType === 'AR' ? '0 2px 4px rgba(37, 99, 235, 0.3)' : 'none'
            }}
          >
            AR Aging
          </button>
          <button
            onClick={() => setReportType('AP')}
            style={{
              padding: '12px 28px',
              border: 'none',
              background: reportType === 'AP' 
                ? 'linear-gradient(135deg, #3b82f6 0%, #2563eb 100%)' 
                : 'transparent',
              color: reportType === 'AP' ? 'white' : '#6b7280',
              borderRadius: '8px',
              fontSize: '14px',
              fontWeight: '700',
              cursor: 'pointer',
              transition: 'all 0.2s',
              boxShadow: reportType === 'AP' ? '0 2px 4px rgba(37, 99, 235, 0.3)' : 'none'
            }}
          >
            AP Aging
          </button>
        </div>
      </div>

      {/* Export Buttons */}
      <div style={{ 
        display: 'flex', 
        gap: '10px', 
        marginBottom: '24px',
        flexWrap: 'wrap'
      }}>
        {['PDF', 'Excel', 'CSV'].map(format => (
          <button
            key={format}
            onClick={() => handleExport(format)}
            style={{
              padding: '10px 20px',
              background: 'white',
              border: '2px solid #e5e7eb',
              borderRadius: '8px',
              fontSize: '14px',
              fontWeight: '600',
              color: '#374151',
              cursor: 'pointer',
              transition: 'all 0.2s',
              display: 'flex',
              alignItems: 'center',
              gap: '8px'
            }}
            onMouseOver={(e) => {
              e.currentTarget.style.borderColor = '#3b82f6';
              e.currentTarget.style.color = '#3b82f6';
              e.currentTarget.style.transform = 'translateY(-2px)';
              e.currentTarget.style.boxShadow = '0 4px 6px rgba(0, 0, 0, 0.1)';
            }}
            onMouseOut={(e) => {
              e.currentTarget.style.borderColor = '#e5e7eb';
              e.currentTarget.style.color = '#374151';
              e.currentTarget.style.transform = 'translateY(0)';
              e.currentTarget.style.boxShadow = 'none';
            }}
          >
            <span>📥</span>
            <span>Export {format}</span>
          </button>
        ))}
      </div>

      {/* Aging Table */}
      {isLoading ? (
        <div style={{
          background: 'white',
          borderRadius: '16px',
          padding: '60px',
          textAlign: 'center',
          boxShadow: '0 1px 3px rgba(0, 0, 0, 0.1)'
        }}>
          <div style={{ 
            fontSize: '16px', 
            color: '#6b7280',
            fontWeight: '500'
          }}>
            Loading aging report...
          </div>
        </div>
      ) : (
        <div style={{ 
          background: 'white',
          borderRadius: '16px',
          boxShadow: '0 4px 6px rgba(0, 0, 0, 0.07)',
          overflow: 'hidden',
          border: '1px solid #e5e7eb'
        }}>
          <div style={{ overflowX: 'auto' }}>
            <table style={{ 
              width: '100%', 
              borderCollapse: 'collapse',
              minWidth: '1100px'
            }}>
              <thead>
                <tr style={{ 
                  background: 'linear-gradient(to right, #1f2937 0%, #374151 100%)',
                  color: 'white'
                }}>
                  <th style={headerStyle}>{reportType === 'AP' ? 'Vendor' : 'Customer'} Name</th>
                  <th style={{ ...headerStyle, textAlign: 'right' }}>Current<br/>(Not Due)</th>
                  <th style={{ ...headerStyle, textAlign: 'right' }}>1-30<br/>Days</th>
                  <th style={{ ...headerStyle, textAlign: 'right' }}>31-60<br/>Days</th>
                  <th style={{ ...headerStyle, textAlign: 'right' }}>61-90<br/>Days</th>
                  <th style={{ ...headerStyle, textAlign: 'right' }}>90+<br/>Days</th>
                  <th style={{ ...headerStyle, textAlign: 'right' }}>Total<br/>Due</th>
                </tr>
              </thead>
              <tbody>
                {agingData.map((entry, index) => (
                  <tr 
                    key={index}
                    style={{ 
                      borderBottom: '1px solid #f3f4f6',
                      transition: 'background-color 0.2s'
                    }}
                    onMouseOver={(e) => e.currentTarget.style.backgroundColor = '#fafafa'}
                    onMouseOut={(e) => e.currentTarget.style.backgroundColor = 'transparent'}
                  >
                    <td style={nameColumnStyle}>
                      <span style={{ fontWeight: '600', color: '#111827', fontSize: '14px' }}>
                        {entry.name}
                      </span>
                    </td>
                    <td style={{ 
                      ...amountColumnStyle, 
                      background: getColumnBackground('current')
                    }}>
                      <span style={{ 
                        color: getAmountColor(entry.current, 'current'),
                        fontWeight: '700'
                      }}>
                        {formatCurrency(entry.current)}
                      </span>
                    </td>
                    <td style={{ 
                      ...amountColumnStyle, 
                      background: getColumnBackground('days1to30')
                    }}>
                      <span style={{ 
                        color: getAmountColor(entry.days1to30, 'days1to30'),
                        fontWeight: '700'
                      }}>
                        {formatCurrency(entry.days1to30)}
                      </span>
                    </td>
                    <td style={{ 
                      ...amountColumnStyle, 
                      background: getColumnBackground('days31to60')
                    }}>
                      <span style={{ 
                        color: getAmountColor(entry.days31to60, 'days31to60'),
                        fontWeight: '700'
                      }}>
                        {formatCurrency(entry.days31to60)}
                      </span>
                    </td>
                    <td style={{ 
                      ...amountColumnStyle, 
                      background: getColumnBackground('days61to90')
                    }}>
                      <span style={{ 
                        color: getAmountColor(entry.days61to90, 'days61to90'),
                        fontWeight: '700'
                      }}>
                        {formatCurrency(entry.days61to90)}
                      </span>
                    </td>
                    <td style={{ 
                      ...amountColumnStyle, 
                      background: getColumnBackground('days90plus')
                    }}>
                      <span style={{ 
                        color: getAmountColor(entry.days90plus, 'days90plus'),
                        fontWeight: '700'
                      }}>
                        {formatCurrency(entry.days90plus)}
                      </span>
                    </td>
                    <td style={totalColumnStyle}>
                      <span style={{ 
                        color: '#111827',
                        fontWeight: '800',
                        fontSize: '15px'
                      }}>
                        {formatCurrency(entry.total)}
                      </span>
                    </td>
                  </tr>
                ))}
              </tbody>
              {/* Totals Footer */}
              <tfoot>
                <tr style={{ 
                  background: 'linear-gradient(135deg, #f9fafb 0%, #f3f4f6 100%)',
                  borderTop: '3px solid #d1d5db'
                }}>
                  <td style={{ ...nameColumnStyle, fontWeight: '800', fontSize: '15px' }}>
                    TOTAL
                  </td>
                  <td style={{ 
                    ...amountColumnStyle, 
                    background: getColumnBackground('current'),
                    borderTop: '3px solid #86efac'
                  }}>
                    <span style={{ 
                      color: getAmountColor(totals.current, 'current'),
                      fontWeight: '800',
                      fontSize: '15px'
                    }}>
                      {formatCurrency(totals.current)}
                    </span>
                  </td>
                  <td style={{ 
                    ...amountColumnStyle, 
                    background: getColumnBackground('days1to30'),
                    borderTop: '3px solid #fcd34d'
                  }}>
                    <span style={{ 
                      color: getAmountColor(totals.days1to30, 'days1to30'),
                      fontWeight: '800',
                      fontSize: '15px'
                    }}>
                      {formatCurrency(totals.days1to30)}
                    </span>
                  </td>
                  <td style={{ 
                    ...amountColumnStyle, 
                    background: getColumnBackground('days31to60'),
                    borderTop: '3px solid #fdba74'
                  }}>
                    <span style={{ 
                      color: getAmountColor(totals.days31to60, 'days31to60'),
                      fontWeight: '800',
                      fontSize: '15px'
                    }}>
                      {formatCurrency(totals.days31to60)}
                    </span>
                  </td>
                  <td style={{ 
                    ...amountColumnStyle, 
                    background: getColumnBackground('days61to90'),
                    borderTop: '3px solid #fca5a5'
                  }}>
                    <span style={{ 
                      color: getAmountColor(totals.days61to90, 'days61to90'),
                      fontWeight: '800',
                      fontSize: '15px'
                    }}>
                      {formatCurrency(totals.days61to90)}
                    </span>
                  </td>
                  <td style={{ 
                    ...amountColumnStyle, 
                    background: getColumnBackground('days90plus'),
                    borderTop: '3px solid #f87171'
                  }}>
                    <span style={{ 
                      color: getAmountColor(totals.days90plus, 'days90plus'),
                      fontWeight: '800',
                      fontSize: '15px'
                    }}>
                      {formatCurrency(totals.days90plus)}
                    </span>
                  </td>
                  <td style={{ 
                    ...totalColumnStyle,
                    borderTop: '3px solid #3b82f6'
                  }}>
                    <span style={{ 
                      color: '#1e40af',
                      fontWeight: '900',
                      fontSize: '16px'
                    }}>
                      {formatCurrency(totals.total)}
                    </span>
                  </td>
                </tr>
              </tfoot>
            </table>
          </div>

          {/* Empty State */}
          {agingData.length === 0 && (
            <div style={{ 
              padding: '60px 24px',
              textAlign: 'center',
              color: '#9ca3af'
            }}>
              <div style={{ fontSize: '48px', marginBottom: '16px' }}>📊</div>
              <p style={{ fontSize: '16px', margin: 0, fontWeight: '500' }}>
                No aging data available for this report
              </p>
            </div>
          )}
        </div>
      )}

      {/* Legend */}
      <div style={{ 
        marginTop: '24px',
        padding: '20px',
        background: 'linear-gradient(135deg, #fef9e7 0%, #fef3c7 100%)',
        borderRadius: '12px',
        border: '2px solid #fbbf24'
      }}>
        <div style={{ 
          fontSize: '13px', 
          fontWeight: '700', 
          color: '#92400e',
          marginBottom: '12px'
        }}>
          Color Coding Legend:
        </div>
        <div style={{ 
          display: 'flex', 
          gap: '20px', 
          flexWrap: 'wrap'
        }}>
          <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
            <div style={{ 
              width: '24px', 
              height: '24px', 
              background: 'linear-gradient(135deg, #dcfce7 0%, #bbf7d0 100%)',
              borderRadius: '4px',
              border: '2px solid #86efac'
            }} />
            <span style={{ fontSize: '13px', color: '#065f46', fontWeight: '600' }}>
              Current (Not Due)
            </span>
          </div>
          <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
            <div style={{ 
              width: '24px', 
              height: '24px', 
              background: 'linear-gradient(135deg, #fef3c7 0%, #fde047 100%)',
              borderRadius: '4px',
              border: '2px solid #fcd34d'
            }} />
            <span style={{ fontSize: '13px', color: '#92400e', fontWeight: '600' }}>
              1-30 Days
            </span>
          </div>
          <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
            <div style={{ 
              width: '24px', 
              height: '24px', 
              background: 'linear-gradient(135deg, #fed7aa 0%, #fb923c 100%)',
              borderRadius: '4px',
              border: '2px solid #fdba74'
            }} />
            <span style={{ fontSize: '13px', color: '#9a3412', fontWeight: '600' }}>
              31-60 Days
            </span>
          </div>
          <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
            <div style={{ 
              width: '24px', 
              height: '24px', 
              background: 'linear-gradient(135deg, #fecaca 0%, #f87171 100%)',
              borderRadius: '4px',
              border: '2px solid #fca5a5'
            }} />
            <span style={{ fontSize: '13px', color: '#991b1b', fontWeight: '600' }}>
              61-90 Days
            </span>
          </div>
          <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
            <div style={{ 
              width: '24px', 
              height: '24px', 
              background: 'linear-gradient(135deg, #fee2e2 0%, #ef4444 100%)',
              borderRadius: '4px',
              border: '2px solid #f87171'
            }} />
            <span style={{ fontSize: '13px', color: '#7f1d1d', fontWeight: '600' }}>
              90+ Days
            </span>
          </div>
        </div>
      </div>
    </div>
  );
}

const headerStyle: React.CSSProperties = {
  padding: '18px 20px',
  textAlign: 'left',
  fontSize: '11px',
  fontWeight: '800',
  textTransform: 'uppercase',
  letterSpacing: '0.8px',
  lineHeight: '1.4'
};

const nameColumnStyle: React.CSSProperties = {
  padding: '16px 20px',
  fontSize: '14px',
  color: '#1f2937'
};

const amountColumnStyle: React.CSSProperties = {
  padding: '16px 20px',
  fontSize: '14px',
  textAlign: 'right'
};

const totalColumnStyle: React.CSSProperties = {
  padding: '16px 20px',
  fontSize: '14px',
  textAlign: 'right',
  background: 'linear-gradient(135deg, #dbeafe 0%, #bfdbfe 100%)'
};
