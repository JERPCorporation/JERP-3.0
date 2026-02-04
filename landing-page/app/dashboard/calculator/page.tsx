/**
 * JERP 3.0 - Tax & Payroll Calculator
 * Copyright (c) 2025 JERP. All rights reserved.
 */

'use client';

import { useState } from 'react';
import Link from 'next/link';

interface TaxCalculation {
  grossPay: number;
  federalIncomeTax: number;
  socialSecurity: number;
  medicare: number;
  additionalMedicare: number;
  stateSDI: number;
  statePFL: number;
  totalDeductions: number;
  netPay: number;
  employerSSMatch: number;
  employerMedicareMatch: number;
  futa: number;
  caTrainingTax: number;
  totalEmployerCost: number;
}

export default function TaxCalculatorPage() {
  const [grossPay, setGrossPay] = useState('75000');
  const [payPeriod, setPayPeriod] = useState('Annual');
  const [state, setState] = useState('California');
  const [maritalStatus, setMaritalStatus] = useState('Single');
  const [allowances, setAllowances] = useState('0');
  const [showEmployer, setShowEmployer] = useState(false);
  const [calculation, setCalculation] = useState<TaxCalculation | null>(null);

  const calculateTaxes = () => {
    const annualGross = convertToAnnual(parseFloat(grossPay) || 0, payPeriod);
    
    // Social Security: 6.2% up to $168,600
    const ssTaxableWages = Math.min(annualGross, 168600);
    const socialSecurity = ssTaxableWages * 0.062;
    
    // Medicare: 1.45%
    const medicare = annualGross * 0.0145;
    
    // Additional Medicare: 0.9% over $200,000
    const additionalMedicare = annualGross > 200000 ? (annualGross - 200000) * 0.009 : 0;
    
    // California SDI: 0.9% up to $153,164
    const sdiTaxableWages = Math.min(annualGross, 153164);
    const stateSDI = state === 'California' ? sdiTaxableWages * 0.009 : 0;
    
    // California PFL (included in SDI rate)
    const statePFL = 0;
    
    // Federal Income Tax (simplified approximation based on 2025 brackets)
    const federalIncomeTax = calculateFederalTax(annualGross, maritalStatus, parseInt(allowances) || 0);
    
    // Total deductions
    const totalDeductions = federalIncomeTax + socialSecurity + medicare + additionalMedicare + stateSDI + statePFL;
    const netPay = annualGross - totalDeductions;
    
    // Employer costs
    const employerSSMatch = ssTaxableWages * 0.062;
    const employerMedicareMatch = annualGross * 0.0145;
    
    // FUTA: 0.6% up to $7,000
    const futaTaxableWages = Math.min(annualGross, 7000);
    const futa = futaTaxableWages * 0.006;
    
    // CA Training Tax: 0.1% up to $7,000
    const caTrainingTax = state === 'California' ? Math.min(annualGross, 7000) * 0.001 : 0;
    
    const totalEmployerCost = annualGross + employerSSMatch + employerMedicareMatch + futa + caTrainingTax;
    
    setCalculation({
      grossPay: annualGross,
      federalIncomeTax,
      socialSecurity,
      medicare,
      additionalMedicare,
      stateSDI,
      statePFL,
      totalDeductions,
      netPay,
      employerSSMatch,
      employerMedicareMatch,
      futa,
      caTrainingTax,
      totalEmployerCost,
    });
  };

  const convertToAnnual = (amount: number, period: string): number => {
    switch (period) {
      case 'Monthly':
        return amount * 12;
      case 'Bi-weekly':
        return amount * 26;
      case 'Weekly':
        return amount * 52;
      default:
        return amount;
    }
  };

  const calculateFederalTax = (income: number, status: string, allowances: number): number => {
    // 2025 Federal Tax Brackets (simplified)
    const brackets = status === 'Married' ? [
      { limit: 23200, rate: 0.10 },
      { limit: 94300, rate: 0.12 },
      { limit: 201050, rate: 0.22 },
      { limit: 383900, rate: 0.24 },
      { limit: 487450, rate: 0.32 },
      { limit: 731200, rate: 0.35 },
      { limit: Infinity, rate: 0.37 },
    ] : [
      { limit: 11600, rate: 0.10 },
      { limit: 47150, rate: 0.12 },
      { limit: 100525, rate: 0.22 },
      { limit: 191950, rate: 0.24 },
      { limit: 243725, rate: 0.32 },
      { limit: 609350, rate: 0.35 },
      { limit: Infinity, rate: 0.37 },
    ];

    const standardDeduction = status === 'Married' ? 29200 : 14600;
    const allowanceDeduction = allowances * 5000;
    const taxableIncome = Math.max(0, income - standardDeduction - allowanceDeduction);

    let tax = 0;
    let previousLimit = 0;

    for (const bracket of brackets) {
      if (taxableIncome > previousLimit) {
        const taxableInBracket = Math.min(taxableIncome, bracket.limit) - previousLimit;
        tax += taxableInBracket * bracket.rate;
        previousLimit = bracket.limit;
      } else {
        break;
      }
    }

    return tax;
  };

  const formatCurrency = (amount: number): string => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    }).format(amount);
  };

  const formatPercentage = (rate: number): string => {
    return `${(rate * 100).toFixed(2)}%`;
  };

  return (
    <div style={{
      minHeight: '100vh',
      background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
      padding: '2rem',
    }}>
      <div style={{
        maxWidth: '1200px',
        margin: '0 auto',
      }}>
        {/* Header */}
        <div style={{
          marginBottom: '2rem',
        }}>
          <Link href="/dashboard" style={{
            display: 'inline-flex',
            alignItems: 'center',
            color: 'white',
            textDecoration: 'none',
            fontSize: '0.95rem',
            marginBottom: '1rem',
            transition: 'opacity 0.2s',
          }}
          onMouseEnter={(e) => e.currentTarget.style.opacity = '0.8'}
          onMouseLeave={(e) => e.currentTarget.style.opacity = '1'}>
            ← Back to Dashboard
          </Link>
          <h1 style={{
            fontSize: '2.5rem',
            fontWeight: 'bold',
            background: 'linear-gradient(to right, #fff, #e0e7ff)',
            WebkitBackgroundClip: 'text',
            WebkitTextFillColor: 'transparent',
            marginBottom: '0.5rem',
          }}>
            Tax & Payroll Calculator
          </h1>
          <p style={{
            color: 'rgba(255, 255, 255, 0.9)',
            fontSize: '1.1rem',
          }}>
            2025 Federal & California Tax Rates
          </p>
        </div>

        {/* Input Section */}
        <div style={{
          background: 'white',
          borderRadius: '12px',
          padding: '2rem',
          boxShadow: '0 4px 6px rgba(0, 0, 0, 0.1)',
          marginBottom: '2rem',
        }}>
          <h2 style={{
            fontSize: '1.5rem',
            fontWeight: '600',
            color: '#1f2937',
            marginBottom: '1.5rem',
          }}>
            Income Information
          </h2>

          <div style={{
            display: 'grid',
            gridTemplateColumns: 'repeat(auto-fit, minmax(250px, 1fr))',
            gap: '1.5rem',
          }}>
            <div>
              <label style={{
                display: 'block',
                fontSize: '0.9rem',
                fontWeight: '500',
                color: '#374151',
                marginBottom: '0.5rem',
              }}>
                Gross Pay
              </label>
              <input
                type="number"
                value={grossPay}
                onChange={(e) => setGrossPay(e.target.value)}
                style={{
                  width: '100%',
                  padding: '0.75rem',
                  border: '1px solid #d1d5db',
                  borderRadius: '8px',
                  fontSize: '1rem',
                  outline: 'none',
                  transition: 'border-color 0.2s',
                }}
                onFocus={(e) => e.target.style.borderColor = '#667eea'}
                onBlur={(e) => e.target.style.borderColor = '#d1d5db'}
              />
            </div>

            <div>
              <label style={{
                display: 'block',
                fontSize: '0.9rem',
                fontWeight: '500',
                color: '#374151',
                marginBottom: '0.5rem',
              }}>
                Pay Period
              </label>
              <select
                value={payPeriod}
                onChange={(e) => setPayPeriod(e.target.value)}
                style={{
                  width: '100%',
                  padding: '0.75rem',
                  border: '1px solid #d1d5db',
                  borderRadius: '8px',
                  fontSize: '1rem',
                  outline: 'none',
                  backgroundColor: 'white',
                  cursor: 'pointer',
                }}
              >
                <option>Annual</option>
                <option>Monthly</option>
                <option>Bi-weekly</option>
                <option>Weekly</option>
              </select>
            </div>

            <div>
              <label style={{
                display: 'block',
                fontSize: '0.9rem',
                fontWeight: '500',
                color: '#374151',
                marginBottom: '0.5rem',
              }}>
                State
              </label>
              <select
                value={state}
                onChange={(e) => setState(e.target.value)}
                style={{
                  width: '100%',
                  padding: '0.75rem',
                  border: '1px solid #d1d5db',
                  borderRadius: '8px',
                  fontSize: '1rem',
                  outline: 'none',
                  backgroundColor: 'white',
                  cursor: 'pointer',
                }}
              >
                <option>California</option>
                <option>New York</option>
                <option>Texas</option>
                <option>Florida</option>
                <option>Illinois</option>
                <option>Washington</option>
              </select>
            </div>

            <div>
              <label style={{
                display: 'block',
                fontSize: '0.9rem',
                fontWeight: '500',
                color: '#374151',
                marginBottom: '0.5rem',
              }}>
                Marital Status
              </label>
              <select
                value={maritalStatus}
                onChange={(e) => setMaritalStatus(e.target.value)}
                style={{
                  width: '100%',
                  padding: '0.75rem',
                  border: '1px solid #d1d5db',
                  borderRadius: '8px',
                  fontSize: '1rem',
                  outline: 'none',
                  backgroundColor: 'white',
                  cursor: 'pointer',
                }}
              >
                <option>Single</option>
                <option>Married</option>
                <option>Head of Household</option>
              </select>
            </div>

            <div>
              <label style={{
                display: 'block',
                fontSize: '0.9rem',
                fontWeight: '500',
                color: '#374151',
                marginBottom: '0.5rem',
              }}>
                Allowances
              </label>
              <input
                type="number"
                value={allowances}
                onChange={(e) => setAllowances(e.target.value)}
                min="0"
                style={{
                  width: '100%',
                  padding: '0.75rem',
                  border: '1px solid #d1d5db',
                  borderRadius: '8px',
                  fontSize: '1rem',
                  outline: 'none',
                  transition: 'border-color 0.2s',
                }}
                onFocus={(e) => e.target.style.borderColor = '#667eea'}
                onBlur={(e) => e.target.style.borderColor = '#d1d5db'}
              />
            </div>
          </div>

          <button
            onClick={calculateTaxes}
            style={{
              marginTop: '1.5rem',
              padding: '0.875rem 2rem',
              background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
              color: 'white',
              border: 'none',
              borderRadius: '8px',
              fontSize: '1rem',
              fontWeight: '600',
              cursor: 'pointer',
              transition: 'transform 0.2s, box-shadow 0.2s',
              boxShadow: '0 4px 6px rgba(102, 126, 234, 0.4)',
            }}
            onMouseEnter={(e) => {
              e.currentTarget.style.transform = 'translateY(-2px)';
              e.currentTarget.style.boxShadow = '0 6px 12px rgba(102, 126, 234, 0.5)';
            }}
            onMouseLeave={(e) => {
              e.currentTarget.style.transform = 'translateY(0)';
              e.currentTarget.style.boxShadow = '0 4px 6px rgba(102, 126, 234, 0.4)';
            }}
          >
            Calculate Taxes
          </button>
        </div>

        {/* Results Display */}
        {calculation && (
          <div style={{
            background: 'white',
            borderRadius: '12px',
            padding: '2rem',
            boxShadow: '0 4px 6px rgba(0, 0, 0, 0.1)',
            marginBottom: '2rem',
          }}>
            <div style={{
              display: 'flex',
              justifyContent: 'space-between',
              alignItems: 'center',
              marginBottom: '1.5rem',
              flexWrap: 'wrap',
              gap: '1rem',
            }}>
              <h2 style={{
                fontSize: '1.5rem',
                fontWeight: '600',
                color: '#1f2937',
              }}>
                Calculation Results
              </h2>

              <div style={{
                display: 'flex',
                background: '#f3f4f6',
                borderRadius: '8px',
                padding: '0.25rem',
              }}>
                <button
                  onClick={() => setShowEmployer(false)}
                  style={{
                    padding: '0.5rem 1.5rem',
                    background: !showEmployer ? 'white' : 'transparent',
                    border: 'none',
                    borderRadius: '6px',
                    fontSize: '0.9rem',
                    fontWeight: '500',
                    color: !showEmployer ? '#667eea' : '#6b7280',
                    cursor: 'pointer',
                    transition: 'all 0.2s',
                    boxShadow: !showEmployer ? '0 2px 4px rgba(0, 0, 0, 0.1)' : 'none',
                  }}
                >
                  Employee Deductions
                </button>
                <button
                  onClick={() => setShowEmployer(true)}
                  style={{
                    padding: '0.5rem 1.5rem',
                    background: showEmployer ? 'white' : 'transparent',
                    border: 'none',
                    borderRadius: '6px',
                    fontSize: '0.9rem',
                    fontWeight: '500',
                    color: showEmployer ? '#667eea' : '#6b7280',
                    cursor: 'pointer',
                    transition: 'all 0.2s',
                    boxShadow: showEmployer ? '0 2px 4px rgba(0, 0, 0, 0.1)' : 'none',
                  }}
                >
                  Employer Costs
                </button>
              </div>
            </div>

            {!showEmployer ? (
              <div>
                <div style={{
                  display: 'grid',
                  gap: '1rem',
                }}>
                  <div style={{
                    display: 'flex',
                    justifyContent: 'space-between',
                    padding: '0.75rem',
                    background: '#f9fafb',
                    borderRadius: '6px',
                  }}>
                    <span style={{ color: '#374151', fontWeight: '500' }}>Gross Pay (Annual)</span>
                    <span style={{ color: '#1f2937', fontWeight: '600' }}>{formatCurrency(calculation.grossPay)}</span>
                  </div>

                  <div style={{
                    display: 'flex',
                    justifyContent: 'space-between',
                    padding: '0.75rem',
                  }}>
                    <span style={{ color: '#6b7280' }}>Federal Income Tax</span>
                    <span style={{ color: '#ef4444' }}>-{formatCurrency(calculation.federalIncomeTax)}</span>
                  </div>

                  <div style={{
                    display: 'flex',
                    justifyContent: 'space-between',
                    padding: '0.75rem',
                  }}>
                    <span style={{ color: '#6b7280' }}>Social Security (6.2%)</span>
                    <span style={{ color: '#ef4444' }}>-{formatCurrency(calculation.socialSecurity)}</span>
                  </div>

                  <div style={{
                    display: 'flex',
                    justifyContent: 'space-between',
                    padding: '0.75rem',
                  }}>
                    <span style={{ color: '#6b7280' }}>Medicare (1.45%)</span>
                    <span style={{ color: '#ef4444' }}>-{formatCurrency(calculation.medicare)}</span>
                  </div>

                  {calculation.additionalMedicare > 0 && (
                    <div style={{
                      display: 'flex',
                      justifyContent: 'space-between',
                      padding: '0.75rem',
                    }}>
                      <span style={{ color: '#6b7280' }}>Additional Medicare (0.9%)</span>
                      <span style={{ color: '#ef4444' }}>-{formatCurrency(calculation.additionalMedicare)}</span>
                    </div>
                  )}

                  {state === 'California' && (
                    <>
                      <div style={{
                        display: 'flex',
                        justifyContent: 'space-between',
                        padding: '0.75rem',
                      }}>
                        <span style={{ color: '#6b7280' }}>CA SDI (0.9%)</span>
                        <span style={{ color: '#ef4444' }}>-{formatCurrency(calculation.stateSDI)}</span>
                      </div>

                      <div style={{
                        display: 'flex',
                        justifyContent: 'space-between',
                        padding: '0.75rem',
                      }}>
                        <span style={{ color: '#6b7280' }}>CA PFL (included in SDI)</span>
                        <span style={{ color: '#6b7280' }}>{formatCurrency(0)}</span>
                      </div>
                    </>
                  )}

                  <div style={{
                    height: '1px',
                    background: '#e5e7eb',
                    margin: '0.5rem 0',
                  }}></div>

                  <div style={{
                    display: 'flex',
                    justifyContent: 'space-between',
                    padding: '0.75rem',
                    background: '#fef2f2',
                    borderRadius: '6px',
                  }}>
                    <span style={{ color: '#991b1b', fontWeight: '600' }}>Total Deductions</span>
                    <span style={{ color: '#991b1b', fontWeight: '600' }}>-{formatCurrency(calculation.totalDeductions)}</span>
                  </div>

                  <div style={{
                    display: 'flex',
                    justifyContent: 'space-between',
                    padding: '1.25rem',
                    background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                    borderRadius: '8px',
                    marginTop: '0.5rem',
                  }}>
                    <span style={{ color: 'white', fontSize: '1.25rem', fontWeight: '700' }}>Net Pay (Annual)</span>
                    <span style={{ color: 'white', fontSize: '1.5rem', fontWeight: '700' }}>{formatCurrency(calculation.netPay)}</span>
                  </div>
                </div>
              </div>
            ) : (
              <div>
                <div style={{
                  display: 'grid',
                  gap: '1rem',
                }}>
                  <div style={{
                    display: 'flex',
                    justifyContent: 'space-between',
                    padding: '0.75rem',
                    background: '#f9fafb',
                    borderRadius: '6px',
                  }}>
                    <span style={{ color: '#374151', fontWeight: '500' }}>Gross Pay (Annual)</span>
                    <span style={{ color: '#1f2937', fontWeight: '600' }}>{formatCurrency(calculation.grossPay)}</span>
                  </div>

                  <div style={{
                    display: 'flex',
                    justifyContent: 'space-between',
                    padding: '0.75rem',
                  }}>
                    <span style={{ color: '#6b7280' }}>Social Security Match (6.2%)</span>
                    <span style={{ color: '#f59e0b' }}>+{formatCurrency(calculation.employerSSMatch)}</span>
                  </div>

                  <div style={{
                    display: 'flex',
                    justifyContent: 'space-between',
                    padding: '0.75rem',
                  }}>
                    <span style={{ color: '#6b7280' }}>Medicare Match (1.45%)</span>
                    <span style={{ color: '#f59e0b' }}>+{formatCurrency(calculation.employerMedicareMatch)}</span>
                  </div>

                  <div style={{
                    display: 'flex',
                    justifyContent: 'space-between',
                    padding: '0.75rem',
                  }}>
                    <span style={{ color: '#6b7280' }}>FUTA (0.6%)</span>
                    <span style={{ color: '#f59e0b' }}>+{formatCurrency(calculation.futa)}</span>
                  </div>

                  {state === 'California' && (
                    <div style={{
                      display: 'flex',
                      justifyContent: 'space-between',
                      padding: '0.75rem',
                    }}>
                      <span style={{ color: '#6b7280' }}>CA Training Tax (0.1%)</span>
                      <span style={{ color: '#f59e0b' }}>+{formatCurrency(calculation.caTrainingTax)}</span>
                    </div>
                  )}

                  <div style={{
                    height: '1px',
                    background: '#e5e7eb',
                    margin: '0.5rem 0',
                  }}></div>

                  <div style={{
                    display: 'flex',
                    justifyContent: 'space-between',
                    padding: '1.25rem',
                    background: 'linear-gradient(135deg, #f59e0b 0%, #d97706 100%)',
                    borderRadius: '8px',
                    marginTop: '0.5rem',
                  }}>
                    <span style={{ color: 'white', fontSize: '1.25rem', fontWeight: '700' }}>Total Employer Cost (Annual)</span>
                    <span style={{ color: 'white', fontSize: '1.5rem', fontWeight: '700' }}>{formatCurrency(calculation.totalEmployerCost)}</span>
                  </div>
                </div>
              </div>
            )}
          </div>
        )}

        {/* Tax Rates Reference Table */}
        <div style={{
          background: 'white',
          borderRadius: '12px',
          padding: '2rem',
          boxShadow: '0 4px 6px rgba(0, 0, 0, 0.1)',
          marginBottom: '2rem',
        }}>
          <h2 style={{
            fontSize: '1.5rem',
            fontWeight: '600',
            color: '#1f2937',
            marginBottom: '1.5rem',
          }}>
            2025 Tax Rates Reference
          </h2>

          <div style={{
            marginBottom: '2rem',
          }}>
            <h3 style={{
              fontSize: '1.1rem',
              fontWeight: '600',
              color: '#667eea',
              marginBottom: '1rem',
            }}>
              Federal Payroll Taxes
            </h3>
            <div style={{
              overflowX: 'auto',
            }}>
              <table style={{
                width: '100%',
                borderCollapse: 'collapse',
              }}>
                <thead>
                  <tr style={{ background: '#f9fafb' }}>
                    <th style={{ padding: '0.75rem', textAlign: 'left', fontSize: '0.9rem', fontWeight: '600', color: '#374151', borderBottom: '2px solid #e5e7eb' }}>Tax Type</th>
                    <th style={{ padding: '0.75rem', textAlign: 'left', fontSize: '0.9rem', fontWeight: '600', color: '#374151', borderBottom: '2px solid #e5e7eb' }}>Rate</th>
                    <th style={{ padding: '0.75rem', textAlign: 'left', fontSize: '0.9rem', fontWeight: '600', color: '#374151', borderBottom: '2px solid #e5e7eb' }}>Wage Base Limit</th>
                    <th style={{ padding: '0.75rem', textAlign: 'left', fontSize: '0.9rem', fontWeight: '600', color: '#374151', borderBottom: '2px solid #e5e7eb' }}>Applies To</th>
                  </tr>
                </thead>
                <tbody>
                  <tr>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937', borderBottom: '1px solid #f3f4f6' }}>Social Security</td>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937', borderBottom: '1px solid #f3f4f6' }}>6.2%</td>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937', borderBottom: '1px solid #f3f4f6' }}>$168,600</td>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937', borderBottom: '1px solid #f3f4f6' }}>Employee & Employer</td>
                  </tr>
                  <tr>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937', borderBottom: '1px solid #f3f4f6' }}>Medicare</td>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937', borderBottom: '1px solid #f3f4f6' }}>1.45%</td>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937', borderBottom: '1px solid #f3f4f6' }}>No limit</td>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937', borderBottom: '1px solid #f3f4f6' }}>Employee & Employer</td>
                  </tr>
                  <tr>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937', borderBottom: '1px solid #f3f4f6' }}>Additional Medicare</td>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937', borderBottom: '1px solid #f3f4f6' }}>0.9%</td>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937', borderBottom: '1px solid #f3f4f6' }}>Over $200,000</td>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937', borderBottom: '1px solid #f3f4f6' }}>Employee only</td>
                  </tr>
                  <tr>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937' }}>FUTA</td>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937' }}>0.6%</td>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937' }}>$7,000</td>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937' }}>Employer only</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>

          <div>
            <h3 style={{
              fontSize: '1.1rem',
              fontWeight: '600',
              color: '#667eea',
              marginBottom: '1rem',
            }}>
              California State Taxes
            </h3>
            <div style={{
              overflowX: 'auto',
            }}>
              <table style={{
                width: '100%',
                borderCollapse: 'collapse',
              }}>
                <thead>
                  <tr style={{ background: '#f9fafb' }}>
                    <th style={{ padding: '0.75rem', textAlign: 'left', fontSize: '0.9rem', fontWeight: '600', color: '#374151', borderBottom: '2px solid #e5e7eb' }}>Tax Type</th>
                    <th style={{ padding: '0.75rem', textAlign: 'left', fontSize: '0.9rem', fontWeight: '600', color: '#374151', borderBottom: '2px solid #e5e7eb' }}>Rate</th>
                    <th style={{ padding: '0.75rem', textAlign: 'left', fontSize: '0.9rem', fontWeight: '600', color: '#374151', borderBottom: '2px solid #e5e7eb' }}>Wage Base Limit</th>
                    <th style={{ padding: '0.75rem', textAlign: 'left', fontSize: '0.9rem', fontWeight: '600', color: '#374151', borderBottom: '2px solid #e5e7eb' }}>Applies To</th>
                  </tr>
                </thead>
                <tbody>
                  <tr>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937', borderBottom: '1px solid #f3f4f6' }}>SDI (State Disability Insurance)</td>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937', borderBottom: '1px solid #f3f4f6' }}>0.9%</td>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937', borderBottom: '1px solid #f3f4f6' }}>$153,164</td>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937', borderBottom: '1px solid #f3f4f6' }}>Employee only</td>
                  </tr>
                  <tr>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937', borderBottom: '1px solid #f3f4f6' }}>PFL (Paid Family Leave)</td>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937', borderBottom: '1px solid #f3f4f6' }}>Included in SDI</td>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937', borderBottom: '1px solid #f3f4f6' }}>$153,164</td>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937', borderBottom: '1px solid #f3f4f6' }}>Employee only</td>
                  </tr>
                  <tr>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937', borderBottom: '1px solid #f3f4f6' }}>Training Tax</td>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937', borderBottom: '1px solid #f3f4f6' }}>0.1%</td>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937', borderBottom: '1px solid #f3f4f6' }}>$7,000</td>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937', borderBottom: '1px solid #f3f4f6' }}>Employer only</td>
                  </tr>
                  <tr>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937' }}>State Income Tax</td>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937' }}>1% - 13.3%</td>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937' }}>No limit</td>
                    <td style={{ padding: '0.75rem', fontSize: '0.9rem', color: '#1f2937' }}>Employee only</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>

        {/* Calculation Example */}
        <div style={{
          background: 'white',
          borderRadius: '12px',
          padding: '2rem',
          boxShadow: '0 4px 6px rgba(0, 0, 0, 0.1)',
        }}>
          <h2 style={{
            fontSize: '1.5rem',
            fontWeight: '600',
            color: '#1f2937',
            marginBottom: '1rem',
          }}>
            Example Calculation
          </h2>
          <p style={{
            color: '#6b7280',
            marginBottom: '1rem',
            fontSize: '0.95rem',
          }}>
            For a single employee in California earning $75,000 annually with 0 allowances:
          </p>

          <div style={{
            background: '#f9fafb',
            borderRadius: '8px',
            padding: '1.5rem',
            fontSize: '0.9rem',
            color: '#374151',
            lineHeight: '1.8',
          }}>
            <div style={{ marginBottom: '1rem' }}>
              <strong>Income:</strong> $75,000.00
            </div>
            <div style={{ marginBottom: '1rem' }}>
              <strong>Deductions:</strong>
              <ul style={{ marginTop: '0.5rem', marginLeft: '1.5rem' }}>
                <li>Federal Income Tax: ~$8,689.50</li>
                <li>Social Security (6.2%): $4,650.00</li>
                <li>Medicare (1.45%): $1,087.50</li>
                <li>CA SDI (0.9%): $675.00</li>
              </ul>
            </div>
            <div style={{
              borderTop: '2px solid #e5e7eb',
              paddingTop: '1rem',
              marginTop: '1rem',
            }}>
              <strong>Net Pay:</strong> ~$60,000 (80% of gross)
            </div>
            <div style={{ marginTop: '1rem' }}>
              <strong>Total Employer Cost:</strong> ~$80,775 (includes employer taxes)
            </div>
          </div>

          <div style={{
            marginTop: '1.5rem',
            padding: '1rem',
            background: '#eff6ff',
            borderLeft: '4px solid #667eea',
            borderRadius: '4px',
          }}>
            <p style={{
              color: '#1e40af',
              fontSize: '0.9rem',
              margin: 0,
            }}>
              <strong>Note:</strong> This calculator provides estimates based on 2025 tax rates. 
              Actual withholding may vary based on additional factors such as pre-tax deductions, 
              supplemental wages, and local taxes. Consult a tax professional for precise calculations.
            </p>
          </div>
        </div>

        {/* Footer */}
        <div style={{
          marginTop: '2rem',
          textAlign: 'center',
          color: 'rgba(255, 255, 255, 0.8)',
          fontSize: '0.9rem',
        }}>
          <p>© 2025 JERP. All rights reserved.</p>
        </div>
      </div>
    </div>
  );
}
