/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: licensing@jerp.io
 */

import apiClient from './apiClient';
import { VendorBill } from '../../types/finance';

export const billsApi = {
  // Get bill by ID
  getById: async (id: string): Promise<VendorBill> => {
    const response = await apiClient.get<VendorBill>(`/api/v1/vendors/bills/${id}`);
    return response.data;
  },

  // Get bills by vendor
  getByVendor: async (vendorId: string): Promise<VendorBill[]> => {
    const response = await apiClient.get<VendorBill[]>(`/api/v1/vendors/bills/vendor/${vendorId}`);
    return response.data;
  },

  // Create bill
  create: async (companyId: string, data: any): Promise<VendorBill> => {
    const response = await apiClient.post<VendorBill>('/api/v1/vendors/bills', data, {
      params: { companyId },
    });
    return response.data;
  },
};
