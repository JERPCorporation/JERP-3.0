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
import { JournalEntry, CreateJournalEntryRequest, JournalEntryStatus } from '../../types/finance';

export const journalEntriesApi = {
  // Get all journal entries
  getAll: async (
    companyId: string,
    startDate?: string,
    endDate?: string,
    status?: JournalEntryStatus
  ): Promise<JournalEntry[]> => {
    const response = await apiClient.get<JournalEntry[]>('/api/v1/finance/journal-entries', {
      params: { companyId, startDate, endDate, status },
    });
    return response.data;
  },

  // Get journal entry by ID
  getById: async (id: string): Promise<JournalEntry> => {
    const response = await apiClient.get<JournalEntry>(`/api/v1/finance/journal-entries/${id}`);
    return response.data;
  },

  // Create journal entry
  create: async (data: CreateJournalEntryRequest): Promise<JournalEntry> => {
    const response = await apiClient.post<JournalEntry>('/api/v1/finance/journal-entries', data);
    return response.data;
  },

  // Post journal entry
  post: async (id: string): Promise<{ message: string }> => {
    const response = await apiClient.post(`/api/v1/finance/journal-entries/${id}/post`);
    return response.data;
  },

  // Void journal entry
  void: async (id: string): Promise<{ message: string }> => {
    const response = await apiClient.post(`/api/v1/finance/journal-entries/${id}/void`);
    return response.data;
  },
};
