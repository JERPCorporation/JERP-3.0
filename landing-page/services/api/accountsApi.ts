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
import { Account, CreateAccountRequest, UpdateAccountRequest } from '../../types/finance';

export const accountsApi = {
  // Get all accounts for a company
  getAll: async (companyId: string): Promise<Account[]> => {
    const response = await apiClient.get<Account[]>('/api/v1/finance/accounts', {
      params: { companyId },
    });
    return response.data;
  },

  // Get account by ID
  getById: async (id: string): Promise<Account> => {
    const response = await apiClient.get<Account>(`/api/v1/finance/accounts/${id}`);
    return response.data;
  },

  // Create new account
  create: async (data: CreateAccountRequest): Promise<Account> => {
    const response = await apiClient.post<Account>('/api/v1/finance/accounts', data);
    return response.data;
  },

  // Update account
  update: async (id: string, data: UpdateAccountRequest): Promise<Account> => {
    const response = await apiClient.put<Account>(`/api/v1/finance/accounts/${id}`, data);
    return response.data;
  },

  // Get FASB topics
  getFASBTopics: async (category?: string): Promise<any[]> => {
    const response = await apiClient.get('/api/v1/finance/fasb-topics', {
      params: { category },
    });
    return response.data;
  },

  // Get FASB subtopics
  getFASBSubtopics: async (topicId: string): Promise<any[]> => {
    const response = await apiClient.get(`/api/v1/finance/fasb-topics/${topicId}/subtopics`);
    return response.data;
  },
};
