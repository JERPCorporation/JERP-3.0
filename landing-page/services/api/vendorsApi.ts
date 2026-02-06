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
import { Vendor, CreateVendorDto, UpdateVendorDto } from '../../types/finance';

export const vendorsApi = {
  // Get all vendors
  getAll: async (companyId: string, isActive?: boolean): Promise<Vendor[]> => {
    const response = await apiClient.get<Vendor[]>('/api/v1/vendors', {
      params: { companyId, isActive },
    });
    return response.data;
  },

  // Get vendor by ID
  getById: async (id: string): Promise<Vendor> => {
    const response = await apiClient.get<Vendor>(`/api/v1/vendors/${id}`);
    return response.data;
  },

  // Create vendor
  create: async (companyId: string, data: CreateVendorDto): Promise<Vendor> => {
    const response = await apiClient.post<Vendor>('/api/v1/vendors', data, {
      params: { companyId },
    });
    return response.data;
  },

  // Update vendor
  update: async (id: string, data: UpdateVendorDto): Promise<Vendor> => {
    const response = await apiClient.put<Vendor>(`/api/v1/vendors/${id}`, data);
    return response.data;
  },

  // Delete vendor
  delete: async (id: string): Promise<void> => {
    await apiClient.delete(`/api/v1/vendors/${id}`);
  },

  // Get vendor balance
  getBalance: async (id: string): Promise<{ vendorId: string; balance: number }> => {
    const response = await apiClient.get(`/api/v1/vendors/${id}/balance`);
    return response.data;
  },
};
