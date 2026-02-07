/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: ichbincesartobar@yahoo.com
 */

import { useQuery } from '@tanstack/react-query';
import { reportsApi } from '../services/api/reportsApi';
import { ReportRequest } from '../types/finance';

export const useFinancialReports = (request: ReportRequest) => {
  // Profit & Loss
  const {
    data: profitAndLoss,
    isLoading: isLoadingPL,
    error: errorPL,
  } = useQuery({
    queryKey: ['profitAndLoss', request.companyId, request.startDate, request.endDate],
    queryFn: () => reportsApi.getProfitAndLoss(request),
    enabled: !!request.companyId && !!request.startDate && !!request.endDate,
  });

  // Balance Sheet
  const {
    data: balanceSheet,
    isLoading: isLoadingBS,
    error: errorBS,
  } = useQuery({
    queryKey: ['balanceSheet', request.companyId, request.endDate],
    queryFn: () => reportsApi.getBalanceSheet(request),
    enabled: !!request.companyId && !!request.endDate,
  });

  return {
    profitAndLoss,
    balanceSheet,
    isLoading: isLoadingPL || isLoadingBS,
    error: errorPL || errorBS,
  };
};
