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
import { GeneralLedgerEntry } from '../../types/finance';

export const generalLedgerApi = {
  // Get GL entries
  getEntries: async (
    companyId: string,
    startDate?: string,
    endDate?: string,
    accountId?: string
  ): Promise<GeneralLedgerEntry[]> => {
    const response = await apiClient.get<GeneralLedgerEntry[]>('/api/v1/finance/general-ledger', {
      params: { companyId, startDate, endDate, accountId },
    });
    return response.data;
  },

  // Get account activity
  getAccountActivity: async (
    accountId: string,
    startDate?: string,
    endDate?: string
  ): Promise<any> => {
    const response = await apiClient.get(`/api/v1/finance/general-ledger/account/${accountId}`, {
      params: { startDate, endDate },
    });
    return response.data;
  },
};
