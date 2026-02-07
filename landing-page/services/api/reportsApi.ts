/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: ichbincesartobar@yahoo.com
 */

import apiClient from './apiClient';
import { ReportRequest, ProfitAndLossReport, BalanceSheetReport } from '../../types/finance';

export const reportsApi = {
  // Generate P&L
  getProfitAndLoss: async (request: ReportRequest): Promise<ProfitAndLossReport> => {
    const response = await apiClient.post<ProfitAndLossReport>(
      '/api/v1/reports/financial/profit-and-loss',
      request
    );
    return response.data;
  },

  // Generate Balance Sheet
  getBalanceSheet: async (request: ReportRequest): Promise<BalanceSheetReport> => {
    const response = await apiClient.post<BalanceSheetReport>(
      '/api/v1/reports/financial/balance-sheet',
      request
    );
    return response.data;
  },

  // Generate Cash Flow
  getCashFlow: async (request: ReportRequest): Promise<any> => {
    const response = await apiClient.post('/api/v1/reports/financial/cash-flow', request);
    return response.data;
  },

  // Export P&L to PDF
  exportProfitAndLossPdf: async (request: ReportRequest): Promise<Blob> => {
    const response = await apiClient.post('/api/v1/reports/financial/profit-and-loss/pdf', request, {
      responseType: 'blob',
    });
    return response.data;
  },

  // Export Balance Sheet to Excel
  exportBalanceSheetExcel: async (request: ReportRequest): Promise<Blob> => {
    const response = await apiClient.post('/api/v1/reports/financial/balance-sheet/excel', request, {
      responseType: 'blob',
    });
    return response.data;
  },
};
