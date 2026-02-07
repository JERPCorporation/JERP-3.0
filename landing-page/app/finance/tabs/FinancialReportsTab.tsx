/**
 * JERP 3.0 - Finance Module - Financial Reports Tab Component
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: licensing@jerp.io
 *
 * Displays Profit & Loss Statement and Balance Sheet side-by-side
 */

'use client';

import { useState, useEffect } from 'react';
import { financeAPI } from '@/lib/finance/api-service';
import { formatCurrency } from '@/lib/finance/utils';

interface ProfitLossStatement {
  revenue: {
    salesRevenue: number;
    serviceRevenue: number;
    otherRevenue: number;
    totalRevenue: number;
  };
  costOfGoodsSold: {
    materials: number;
    labor: number;
    overhead: number;
    totalCOGS: number;
  };
  grossProfit: number;
  grossMargin: number;
  operatingExpenses: {
    salaries: number;
    rent: number;
    utilities: number;
    marketing: number;
    insurance: number;
    depreciation: number;
    otherExpenses: number;
    totalOperatingExpenses: number;
  };
  operatingIncome: number;
  operatingMargin: number;
  otherIncomeExpense: {
    interestIncome: number;
    interestExpense: number;
    otherIncome: number;
    totalOtherIncomeExpense: number;
  };
  netIncome: number;
  netMargin: number;
}

interface BalanceSheet {
  assets: {
    currentAssets: {
      cash: number;
      accountsReceivable: number;
      inventory: number;
      prepaidExpenses: number;
      totalCurrentAssets: number;
    };
    fixedAssets: {
      propertyPlantEquipment: number;
      accumulatedDepreciation: number;
      netFixedAssets: number;
    };
    otherAssets: {
      intangibleAssets: number;
      investments: number;
      totalOtherAssets: number;
    };
    totalAssets: number;
  };
  liabilities: {
    currentLiabilities: {
      accountsPayable: number;
      shortTermDebt: number;
      accruedExpenses: number;
      totalCurrentLiabilities: number;
    };
    longTermLiabilities: {
      longTermDebt: number;
      deferredTaxes: number;
      totalLongTermLiabilities: number;
    };
    totalLiabilities: number;
  };
  equity: {
    commonStock: number;
    retainedEarnings: number;
    totalEquity: number;
  };
  totalLiabilitiesAndEquity: number;
  isBalanced: boolean;
}

export function FinancialReportsTab() {
  const [profitLoss, setProfitLoss] = useState<ProfitLossStatement | null>(null);
  const [balanceSheet, setBalanceSheet] = useState<BalanceSheet | null>(null);
  const [loadingPL, setLoadingPL] = useState(true);
  const [loadingBS, setLoadingBS] = useState(true);

  useEffect(() => {
    loadFinancialReports();
  }, []);

  const loadFinancialReports = async () => {
    try {
      const currentDate = new Date();
      const yearStart = `${currentDate.getFullYear()}-01-01`;
      const today = currentDate.toISOString().split('T')[0];
      
      const [plResponse, bsResponse] = await Promise.all([
        financeAPI.generateProfitLossStatement(yearStart, today),
        financeAPI.generateBalanceSheet(today)
      ]);
      
      // Transform API response to component format
      const plTransformed: ProfitLossStatement = {
        revenue: {
          salesRevenue: plResponse.data.revenueLines[0]?.amount || 0,
          serviceRevenue: plResponse.data.revenueLines[1]?.amount || 0,
          otherRevenue: plResponse.data.revenueLines[2]?.amount || 0,
          totalRevenue: plResponse.data.totalRevenue
        },
        costOfGoodsSold: {
          materials: plResponse.data.cogsLines[0]?.amount || 0,
          labor: plResponse.data.cogsLines[1]?.amount || 0,
          overhead: plResponse.data.cogsLines[2]?.amount || 0,
          totalCOGS: plResponse.data.totalCOGS
        },
        grossProfit: plResponse.data.grossProfit,
        grossMargin: plResponse.data.grossMargin,
        operatingExpenses: {
          salaries: plResponse.data.expenseLines.find(e => e.accountName.includes('Payroll'))?.amount || 0,
          rent: plResponse.data.expenseLines.find(e => e.accountName.includes('Rent'))?.amount || 0,
          utilities: plResponse.data.expenseLines.find(e => e.accountName.includes('Utilities'))?.amount || 0,
          marketing: plResponse.data.expenseLines.find(e => e.accountName.includes('Marketing'))?.amount || 0,
          insurance: 0,
          depreciation: 0,
          otherExpenses: 0,
          totalOperatingExpenses: plResponse.data.totalExpenses
        },
        operatingIncome: plResponse.data.netIncome,
        operatingMargin: plResponse.data.netMargin,
        otherIncomeExpense: {
          interestIncome: 0,
          interestExpense: 0,
          otherIncome: 0,
          totalOtherIncomeExpense: 0
        },
        netIncome: plResponse.data.netIncome,
        netMargin: plResponse.data.netMargin
      };
      
      const bsTransformed: BalanceSheet = {
        assets: {
          currentAssets: {
            cash: bsResponse.data.assets.currentAssets[0]?.amount || 0,
            accountsReceivable: bsResponse.data.assets.currentAssets[2]?.amount || 0,
            inventory: bsResponse.data.assets.currentAssets[3]?.amount || 0,
            prepaidExpenses: 0,
            totalCurrentAssets: bsResponse.data.assets.totalCurrentAssets
          },
          fixedAssets: {
            propertyPlantEquipment: bsResponse.data.assets.fixedAssets[0]?.amount || 0,
            accumulatedDepreciation: 0,
            netFixedAssets: bsResponse.data.assets.totalFixedAssets
          },
          otherAssets: {
            intangibleAssets: 0,
            investments: 0,
            totalOtherAssets: 0
          },
          totalAssets: bsResponse.data.assets.totalAssets
        },
        liabilities: {
          currentLiabilities: {
            accountsPayable: bsResponse.data.liabilities.currentLiabilities[0]?.amount || 0,
            shortTermDebt: 0,
            accruedExpenses: bsResponse.data.liabilities.currentLiabilities[1]?.amount || 0,
            totalCurrentLiabilities: bsResponse.data.liabilities.totalCurrentLiabilities
          },
          longTermLiabilities: {
            longTermDebt: bsResponse.data.liabilities.longTermLiabilities[0]?.amount || 0,
            deferredTaxes: 0,
            totalLongTermLiabilities: bsResponse.data.liabilities.totalLongTermLiabilities
          },
          totalLiabilities: bsResponse.data.liabilities.totalLiabilities
        },
        equity: {
          commonStock: bsResponse.data.equity.equityAccounts[0]?.amount || 0,
          retainedEarnings: bsResponse.data.equity.equityAccounts[1]?.amount || 0,
          totalEquity: bsResponse.data.equity.totalEquity
        },
        totalLiabilitiesAndEquity: bsResponse.data.totalLiabilitiesAndEquity,
        isBalanced: bsResponse.data.isBalanced
      };
      
      setProfitLoss(plTransformed);
      setBalanceSheet(bsTransformed);
    } catch (error) {
      console.error('Failed to load financial reports:', error);
    } finally {
      setLoadingPL(false);
      setLoadingBS(false);
    }
  };

  return (
    <div style={{ padding: '24px' }}>
      {/* Header */}
      <div style={{ marginBottom: '28px' }}>
        <h2 style={{ 
          fontSize: '28px', 
          fontWeight: '800', 
          margin: '0 0 8px 0',
          background: 'linear-gradient(135deg, #1e3a8a 0%, #7c3aed 100%)',
          WebkitBackgroundClip: 'text',
          WebkitTextFillColor: 'transparent',
          backgroundClip: 'text'
        }}>
          Financial Statements
        </h2>
        <p style={{ 
          fontSize: '14px', 
          color: '#6b7280',
          margin: 0,
          lineHeight: '1.5'
        }}>
          Comprehensive view of financial performance and position
        </p>
      </div>

      {/* Side-by-Side Reports */}
      <div style={{ 
        display: 'grid',
        gridTemplateColumns: 'repeat(auto-fit, minmax(500px, 1fr))',
        gap: '24px'
      }}>
        {/* Profit & Loss Statement */}
        <div style={{ 
          background: 'white',
          borderRadius: '16px',
          boxShadow: '0 4px 6px rgba(0, 0, 0, 0.07)',
          overflow: 'hidden',
          border: '2px solid #e5e7eb'
        }}>
          <div style={{
            background: 'linear-gradient(135deg, #1e40af 0%, #3b82f6 100%)',
            padding: '20px 24px',
            color: 'white'
          }}>
            <h3 style={{ 
              margin: 0, 
              fontSize: '20px', 
              fontWeight: '800',
              letterSpacing: '0.5px'
            }}>
              Profit & Loss Statement
            </h3>
            <p style={{ margin: '4px 0 0 0', fontSize: '13px', opacity: 0.9 }}>
              Income statement for the period
            </p>
          </div>

          {loadingPL ? (
            <div style={{ padding: '60px 24px', textAlign: 'center', color: '#9ca3af' }}>
              Loading statement...
            </div>
          ) : profitLoss ? (
            <div style={{ padding: '24px' }}>
              {/* Revenue Section */}
              <div style={sectionContainerStyle}>
                <div style={sectionHeaderStyle}>REVENUE</div>
                <div style={lineItemStyle}>
                  <span>Sales Revenue</span>
                  <span>{formatCurrency(profitLoss.revenue.salesRevenue)}</span>
                </div>
                <div style={lineItemStyle}>
                  <span>Service Revenue</span>
                  <span>{formatCurrency(profitLoss.revenue.serviceRevenue)}</span>
                </div>
                <div style={lineItemStyle}>
                  <span>Other Revenue</span>
                  <span>{formatCurrency(profitLoss.revenue.otherRevenue)}</span>
                </div>
                <div style={subtotalStyle}>
                  <span>Total Revenue</span>
                  <span style={{ fontWeight: '800' }}>
                    {formatCurrency(profitLoss.revenue.totalRevenue)}
                  </span>
                </div>
              </div>

              {/* Cost of Goods Sold */}
              <div style={sectionContainerStyle}>
                <div style={sectionHeaderStyle}>COST OF GOODS SOLD</div>
                <div style={lineItemStyle}>
                  <span>Materials</span>
                  <span>{formatCurrency(profitLoss.costOfGoodsSold.materials)}</span>
                </div>
                <div style={lineItemStyle}>
                  <span>Labor</span>
                  <span>{formatCurrency(profitLoss.costOfGoodsSold.labor)}</span>
                </div>
                <div style={lineItemStyle}>
                  <span>Overhead</span>
                  <span>{formatCurrency(profitLoss.costOfGoodsSold.overhead)}</span>
                </div>
                <div style={subtotalStyle}>
                  <span>Total COGS</span>
                  <span style={{ fontWeight: '800' }}>
                    {formatCurrency(profitLoss.costOfGoodsSold.totalCOGS)}
                  </span>
                </div>
              </div>

              {/* Gross Profit */}
              <div style={{
                ...majorTotalStyle,
                background: 'linear-gradient(135deg, #dcfce7 0%, #bbf7d0 100%)',
                border: '2px solid #86efac'
              }}>
                <div>
                  <div style={{ fontWeight: '800', fontSize: '16px', color: '#065f46' }}>
                    Gross Profit
                  </div>
                  <div style={{ fontSize: '12px', color: '#059669', marginTop: '2px' }}>
                    Margin: {profitLoss.grossMargin.toFixed(1)}%
                  </div>
                </div>
                <div style={{ fontWeight: '800', fontSize: '18px', color: '#065f46' }}>
                  {formatCurrency(profitLoss.grossProfit)}
                </div>
              </div>

              {/* Operating Expenses */}
              <div style={sectionContainerStyle}>
                <div style={sectionHeaderStyle}>OPERATING EXPENSES</div>
                <div style={lineItemStyle}>
                  <span>Salaries & Wages</span>
                  <span>{formatCurrency(profitLoss.operatingExpenses.salaries)}</span>
                </div>
                <div style={lineItemStyle}>
                  <span>Rent</span>
                  <span>{formatCurrency(profitLoss.operatingExpenses.rent)}</span>
                </div>
                <div style={lineItemStyle}>
                  <span>Utilities</span>
                  <span>{formatCurrency(profitLoss.operatingExpenses.utilities)}</span>
                </div>
                <div style={lineItemStyle}>
                  <span>Marketing</span>
                  <span>{formatCurrency(profitLoss.operatingExpenses.marketing)}</span>
                </div>
                <div style={lineItemStyle}>
                  <span>Insurance</span>
                  <span>{formatCurrency(profitLoss.operatingExpenses.insurance)}</span>
                </div>
                <div style={lineItemStyle}>
                  <span>Depreciation</span>
                  <span>{formatCurrency(profitLoss.operatingExpenses.depreciation)}</span>
                </div>
                <div style={lineItemStyle}>
                  <span>Other Expenses</span>
                  <span>{formatCurrency(profitLoss.operatingExpenses.otherExpenses)}</span>
                </div>
                <div style={subtotalStyle}>
                  <span>Total Operating Expenses</span>
                  <span style={{ fontWeight: '800' }}>
                    {formatCurrency(profitLoss.operatingExpenses.totalOperatingExpenses)}
                  </span>
                </div>
              </div>

              {/* Operating Income */}
              <div style={{
                ...majorTotalStyle,
                background: 'linear-gradient(135deg, #dbeafe 0%, #bfdbfe 100%)',
                border: '2px solid #60a5fa'
              }}>
                <div>
                  <div style={{ fontWeight: '800', fontSize: '16px', color: '#1e40af' }}>
                    Operating Income
                  </div>
                  <div style={{ fontSize: '12px', color: '#2563eb', marginTop: '2px' }}>
                    Margin: {profitLoss.operatingMargin.toFixed(1)}%
                  </div>
                </div>
                <div style={{ fontWeight: '800', fontSize: '18px', color: '#1e40af' }}>
                  {formatCurrency(profitLoss.operatingIncome)}
                </div>
              </div>

              {/* Other Income/Expense */}
              <div style={sectionContainerStyle}>
                <div style={sectionHeaderStyle}>OTHER INCOME / (EXPENSE)</div>
                <div style={lineItemStyle}>
                  <span>Interest Income</span>
                  <span>{formatCurrency(profitLoss.otherIncomeExpense.interestIncome)}</span>
                </div>
                <div style={lineItemStyle}>
                  <span>Interest Expense</span>
                  <span>{formatCurrency(profitLoss.otherIncomeExpense.interestExpense)}</span>
                </div>
                <div style={lineItemStyle}>
                  <span>Other Income</span>
                  <span>{formatCurrency(profitLoss.otherIncomeExpense.otherIncome)}</span>
                </div>
                <div style={subtotalStyle}>
                  <span>Total Other Income/(Expense)</span>
                  <span style={{ fontWeight: '800' }}>
                    {formatCurrency(profitLoss.otherIncomeExpense.totalOtherIncomeExpense)}
                  </span>
                </div>
              </div>

              {/* Net Income */}
              <div style={{
                ...majorTotalStyle,
                background: profitLoss.netIncome >= 0
                  ? 'linear-gradient(135deg, #d1fae5 0%, #a7f3d0 100%)'
                  : 'linear-gradient(135deg, #fee2e2 0%, #fecaca 100%)',
                border: profitLoss.netIncome >= 0 ? '3px solid #10b981' : '3px solid #ef4444'
              }}>
                <div>
                  <div style={{ 
                    fontWeight: '900', 
                    fontSize: '18px', 
                    color: profitLoss.netIncome >= 0 ? '#065f46' : '#991b1b'
                  }}>
                    NET INCOME
                  </div>
                  <div style={{ 
                    fontSize: '13px', 
                    color: profitLoss.netIncome >= 0 ? '#059669' : '#dc2626',
                    marginTop: '2px',
                    fontWeight: '600'
                  }}>
                    Net Margin: {profitLoss.netMargin.toFixed(1)}%
                  </div>
                </div>
                <div style={{ 
                  fontWeight: '900', 
                  fontSize: '22px', 
                  color: profitLoss.netIncome >= 0 ? '#065f46' : '#991b1b'
                }}>
                  {formatCurrency(profitLoss.netIncome)}
                </div>
              </div>
            </div>
          ) : (
            <div style={{ padding: '60px 24px', textAlign: 'center', color: '#ef4444' }}>
              Failed to load data
            </div>
          )}
        </div>

        {/* Balance Sheet */}
        <div style={{ 
          background: 'white',
          borderRadius: '16px',
          boxShadow: '0 4px 6px rgba(0, 0, 0, 0.07)',
          overflow: 'hidden',
          border: '2px solid #e5e7eb'
        }}>
          <div style={{
            background: 'linear-gradient(135deg, #7c3aed 0%, #a855f7 100%)',
            padding: '20px 24px',
            color: 'white'
          }}>
            <h3 style={{ 
              margin: 0, 
              fontSize: '20px', 
              fontWeight: '800',
              letterSpacing: '0.5px'
            }}>
              Balance Sheet
            </h3>
            <p style={{ margin: '4px 0 0 0', fontSize: '13px', opacity: 0.9 }}>
              Financial position snapshot
            </p>
          </div>

          {loadingBS ? (
            <div style={{ padding: '60px 24px', textAlign: 'center', color: '#9ca3af' }}>
              Loading statement...
            </div>
          ) : balanceSheet ? (
            <div style={{ padding: '24px' }}>
              {/* Assets Section */}
              <div style={sectionContainerStyle}>
                <div style={sectionHeaderStyle}>ASSETS</div>
                
                {/* Current Assets */}
                <div style={{ marginLeft: '12px' }}>
                  <div style={{ ...subHeaderStyle }}>Current Assets</div>
                  <div style={lineItemStyle}>
                    <span>Cash & Cash Equivalents</span>
                    <span>{formatCurrency(balanceSheet.assets.currentAssets.cash)}</span>
                  </div>
                  <div style={lineItemStyle}>
                    <span>Accounts Receivable</span>
                    <span>{formatCurrency(balanceSheet.assets.currentAssets.accountsReceivable)}</span>
                  </div>
                  <div style={lineItemStyle}>
                    <span>Inventory</span>
                    <span>{formatCurrency(balanceSheet.assets.currentAssets.inventory)}</span>
                  </div>
                  <div style={lineItemStyle}>
                    <span>Prepaid Expenses</span>
                    <span>{formatCurrency(balanceSheet.assets.currentAssets.prepaidExpenses)}</span>
                  </div>
                  <div style={subtotalStyle}>
                    <span>Total Current Assets</span>
                    <span style={{ fontWeight: '700' }}>
                      {formatCurrency(balanceSheet.assets.currentAssets.totalCurrentAssets)}
                    </span>
                  </div>
                </div>

                {/* Fixed Assets */}
                <div style={{ marginLeft: '12px', marginTop: '12px' }}>
                  <div style={subHeaderStyle}>Fixed Assets</div>
                  <div style={lineItemStyle}>
                    <span>Property, Plant & Equipment</span>
                    <span>{formatCurrency(balanceSheet.assets.fixedAssets.propertyPlantEquipment)}</span>
                  </div>
                  <div style={lineItemStyle}>
                    <span>Less: Accumulated Depreciation</span>
                    <span>({formatCurrency(Math.abs(balanceSheet.assets.fixedAssets.accumulatedDepreciation))})</span>
                  </div>
                  <div style={subtotalStyle}>
                    <span>Net Fixed Assets</span>
                    <span style={{ fontWeight: '700' }}>
                      {formatCurrency(balanceSheet.assets.fixedAssets.netFixedAssets)}
                    </span>
                  </div>
                </div>

                {/* Other Assets */}
                <div style={{ marginLeft: '12px', marginTop: '12px' }}>
                  <div style={subHeaderStyle}>Other Assets</div>
                  <div style={lineItemStyle}>
                    <span>Intangible Assets</span>
                    <span>{formatCurrency(balanceSheet.assets.otherAssets.intangibleAssets)}</span>
                  </div>
                  <div style={lineItemStyle}>
                    <span>Investments</span>
                    <span>{formatCurrency(balanceSheet.assets.otherAssets.investments)}</span>
                  </div>
                  <div style={subtotalStyle}>
                    <span>Total Other Assets</span>
                    <span style={{ fontWeight: '700' }}>
                      {formatCurrency(balanceSheet.assets.otherAssets.totalOtherAssets)}
                    </span>
                  </div>
                </div>

                <div style={{
                  ...majorTotalStyle,
                  background: 'linear-gradient(135deg, #dbeafe 0%, #93c5fd 100%)',
                  border: '2px solid #3b82f6',
                  marginTop: '12px'
                }}>
                  <span style={{ fontWeight: '900', fontSize: '17px', color: '#1e40af' }}>
                    TOTAL ASSETS
                  </span>
                  <span style={{ fontWeight: '900', fontSize: '19px', color: '#1e40af' }}>
                    {formatCurrency(balanceSheet.assets.totalAssets)}
                  </span>
                </div>
              </div>

              {/* Liabilities Section */}
              <div style={{ ...sectionContainerStyle, marginTop: '24px' }}>
                <div style={sectionHeaderStyle}>LIABILITIES</div>
                
                {/* Current Liabilities */}
                <div style={{ marginLeft: '12px' }}>
                  <div style={subHeaderStyle}>Current Liabilities</div>
                  <div style={lineItemStyle}>
                    <span>Accounts Payable</span>
                    <span>{formatCurrency(balanceSheet.liabilities.currentLiabilities.accountsPayable)}</span>
                  </div>
                  <div style={lineItemStyle}>
                    <span>Short-term Debt</span>
                    <span>{formatCurrency(balanceSheet.liabilities.currentLiabilities.shortTermDebt)}</span>
                  </div>
                  <div style={lineItemStyle}>
                    <span>Accrued Expenses</span>
                    <span>{formatCurrency(balanceSheet.liabilities.currentLiabilities.accruedExpenses)}</span>
                  </div>
                  <div style={subtotalStyle}>
                    <span>Total Current Liabilities</span>
                    <span style={{ fontWeight: '700' }}>
                      {formatCurrency(balanceSheet.liabilities.currentLiabilities.totalCurrentLiabilities)}
                    </span>
                  </div>
                </div>

                {/* Long-term Liabilities */}
                <div style={{ marginLeft: '12px', marginTop: '12px' }}>
                  <div style={subHeaderStyle}>Long-term Liabilities</div>
                  <div style={lineItemStyle}>
                    <span>Long-term Debt</span>
                    <span>{formatCurrency(balanceSheet.liabilities.longTermLiabilities.longTermDebt)}</span>
                  </div>
                  <div style={lineItemStyle}>
                    <span>Deferred Taxes</span>
                    <span>{formatCurrency(balanceSheet.liabilities.longTermLiabilities.deferredTaxes)}</span>
                  </div>
                  <div style={subtotalStyle}>
                    <span>Total Long-term Liabilities</span>
                    <span style={{ fontWeight: '700' }}>
                      {formatCurrency(balanceSheet.liabilities.longTermLiabilities.totalLongTermLiabilities)}
                    </span>
                  </div>
                </div>

                <div style={{
                  ...majorTotalStyle,
                  background: 'linear-gradient(135deg, #fee2e2 0%, #fca5a5 100%)',
                  border: '2px solid #ef4444',
                  marginTop: '12px'
                }}>
                  <span style={{ fontWeight: '900', fontSize: '17px', color: '#991b1b' }}>
                    TOTAL LIABILITIES
                  </span>
                  <span style={{ fontWeight: '900', fontSize: '19px', color: '#991b1b' }}>
                    {formatCurrency(balanceSheet.liabilities.totalLiabilities)}
                  </span>
                </div>
              </div>

              {/* Equity Section */}
              <div style={{ ...sectionContainerStyle, marginTop: '24px' }}>
                <div style={sectionHeaderStyle}>SHAREHOLDERS' EQUITY</div>
                <div style={lineItemStyle}>
                  <span>Common Stock</span>
                  <span>{formatCurrency(balanceSheet.equity.commonStock)}</span>
                </div>
                <div style={lineItemStyle}>
                  <span>Retained Earnings</span>
                  <span>{formatCurrency(balanceSheet.equity.retainedEarnings)}</span>
                </div>
                <div style={{
                  ...majorTotalStyle,
                  background: 'linear-gradient(135deg, #d1fae5 0%, #6ee7b7 100%)',
                  border: '2px solid #10b981',
                  marginTop: '8px'
                }}>
                  <span style={{ fontWeight: '900', fontSize: '17px', color: '#065f46' }}>
                    TOTAL EQUITY
                  </span>
                  <span style={{ fontWeight: '900', fontSize: '19px', color: '#065f46' }}>
                    {formatCurrency(balanceSheet.equity.totalEquity)}
                  </span>
                </div>
              </div>

              {/* Total Liabilities & Equity */}
              <div style={{
                ...majorTotalStyle,
                background: 'linear-gradient(135deg, #fef3c7 0%, #fbbf24 100%)',
                border: '3px solid #f59e0b',
                marginTop: '20px'
              }}>
                <span style={{ fontWeight: '900', fontSize: '18px', color: '#78350f' }}>
                  TOTAL LIABILITIES & EQUITY
                </span>
                <span style={{ fontWeight: '900', fontSize: '20px', color: '#78350f' }}>
                  {formatCurrency(balanceSheet.totalLiabilitiesAndEquity)}
                </span>
              </div>

              {/* Balance Indicator */}
              <div style={{
                marginTop: '16px',
                padding: '14px 20px',
                background: balanceSheet.isBalanced
                  ? 'linear-gradient(135deg, #d1fae5 0%, #a7f3d0 100%)'
                  : 'linear-gradient(135deg, #fee2e2 0%, #fecaca 100%)',
                borderRadius: '10px',
                border: balanceSheet.isBalanced ? '2px solid #10b981' : '2px solid #ef4444',
                textAlign: 'center'
              }}>
                <span style={{
                  fontSize: '14px',
                  fontWeight: '800',
                  color: balanceSheet.isBalanced ? '#065f46' : '#991b1b'
                }}>
                  {balanceSheet.isBalanced ? '✓ BALANCED' : '⚠ NOT BALANCED'}
                </span>
              </div>
            </div>
          ) : (
            <div style={{ padding: '60px 24px', textAlign: 'center', color: '#ef4444' }}>
              Failed to load data
            </div>
          )}
        </div>
      </div>
    </div>
  );
}

const sectionContainerStyle: React.CSSProperties = {
  marginBottom: '20px'
};

const sectionHeaderStyle: React.CSSProperties = {
  fontSize: '13px',
  fontWeight: '800',
  color: '#374151',
  marginBottom: '12px',
  paddingBottom: '8px',
  borderBottom: '2px solid #d1d5db',
  letterSpacing: '0.8px'
};

const subHeaderStyle: React.CSSProperties = {
  fontSize: '12px',
  fontWeight: '700',
  color: '#6b7280',
  marginBottom: '8px',
  textTransform: 'uppercase',
  letterSpacing: '0.5px'
};

const lineItemStyle: React.CSSProperties = {
  display: 'flex',
  justifyContent: 'space-between',
  alignItems: 'center',
  padding: '8px 0',
  fontSize: '14px',
  color: '#4b5563'
};

const subtotalStyle: React.CSSProperties = {
  display: 'flex',
  justifyContent: 'space-between',
  alignItems: 'center',
  padding: '10px 0',
  fontSize: '14px',
  color: '#1f2937',
  borderTop: '1px solid #e5e7eb',
  marginTop: '4px',
  fontWeight: '600'
};

const majorTotalStyle: React.CSSProperties = {
  display: 'flex',
  justifyContent: 'space-between',
  alignItems: 'center',
  padding: '16px 20px',
  borderRadius: '10px',
  marginTop: '8px'
};
