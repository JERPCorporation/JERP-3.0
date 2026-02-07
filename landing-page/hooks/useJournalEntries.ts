/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: ichbincesartobar@yahoo.com
 */

import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { journalEntriesApi } from '../services/api/journalEntriesApi';
import { CreateJournalEntryRequest, JournalEntryStatus } from '../types/finance';
import toast from 'react-hot-toast';

export const useJournalEntries = (
  companyId: string,
  startDate?: string,
  endDate?: string,
  status?: JournalEntryStatus
) => {
  const queryClient = useQueryClient();

  // Get all journal entries
  const { data: journalEntries, isLoading, error } = useQuery({
    queryKey: ['journalEntries', companyId, startDate, endDate, status],
    queryFn: () => journalEntriesApi.getAll(companyId, startDate, endDate, status),
    enabled: !!companyId,
  });

  // Create journal entry
  const createJournalEntry = useMutation({
    mutationFn: (data: CreateJournalEntryRequest) => journalEntriesApi.create(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['journalEntries', companyId] });
      toast.success('Journal entry created successfully');
    },
    onError: (error: any) => {
      toast.error(error.response?.data?.message || 'Failed to create journal entry');
    },
  });

  // Post journal entry
  const postJournalEntry = useMutation({
    mutationFn: (id: string) => journalEntriesApi.post(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['journalEntries', companyId] });
      toast.success('Journal entry posted successfully');
    },
    onError: (error: any) => {
      toast.error(error.response?.data?.message || 'Failed to post journal entry');
    },
  });

  // Void journal entry
  const voidJournalEntry = useMutation({
    mutationFn: (id: string) => journalEntriesApi.void(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['journalEntries', companyId] });
      toast.success('Journal entry voided successfully');
    },
    onError: (error: any) => {
      toast.error(error.response?.data?.message || 'Failed to void journal entry');
    },
  });

  return {
    journalEntries: journalEntries || [],
    isLoading,
    error,
    createJournalEntry: createJournalEntry.mutate,
    postJournalEntry: postJournalEntry.mutate,
    voidJournalEntry: voidJournalEntry.mutate,
    isCreating: createJournalEntry.isPending,
  };
};
