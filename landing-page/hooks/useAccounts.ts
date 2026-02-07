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
import { accountsApi } from '../services/api/accountsApi';
import { CreateAccountRequest, UpdateAccountRequest } from '../types/finance';
import toast from 'react-hot-toast';

export const useAccounts = (companyId: string) => {
  const queryClient = useQueryClient();

  // Get all accounts
  const { data: accounts, isLoading, error } = useQuery({
    queryKey: ['accounts', companyId],
    queryFn: () => accountsApi.getAll(companyId),
    enabled: !!companyId,
  });

  // Create account mutation
  const createAccount = useMutation({
    mutationFn: (data: CreateAccountRequest) => accountsApi.create(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['accounts', companyId] });
      toast.success('Account created successfully');
    },
    onError: (error: any) => {
      toast.error(error.response?.data?.message || 'Failed to create account');
    },
  });

  // Update account mutation
  const updateAccount = useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateAccountRequest }) =>
      accountsApi.update(id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['accounts', companyId] });
      toast.success('Account updated successfully');
    },
    onError: (error: any) => {
      toast.error(error.response?.data?.message || 'Failed to update account');
    },
  });

  return {
    accounts: accounts || [],
    isLoading,
    error,
    createAccount: createAccount.mutate,
    updateAccount: updateAccount.mutate,
    isCreating: createAccount.isPending,
    isUpdating: updateAccount.isPending,
  };
};
