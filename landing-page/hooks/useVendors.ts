/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: licensing@jerp.io
 */

import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { vendorsApi } from '../services/api/vendorsApi';
import { CreateVendorDto, UpdateVendorDto } from '../types/finance';
import toast from 'react-hot-toast';

export const useVendors = (companyId: string, isActive?: boolean) => {
  const queryClient = useQueryClient();

  // Get all vendors
  const { data: vendors, isLoading, error } = useQuery({
    queryKey: ['vendors', companyId, isActive],
    queryFn: () => vendorsApi.getAll(companyId, isActive),
    enabled: !!companyId,
  });

  // Create vendor
  const createVendor = useMutation({
    mutationFn: (data: CreateVendorDto) => vendorsApi.create(companyId, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['vendors', companyId] });
      toast.success('Vendor created successfully');
    },
    onError: (error: any) => {
      toast.error(error.response?.data?.message || 'Failed to create vendor');
    },
  });

  // Update vendor
  const updateVendor = useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateVendorDto }) =>
      vendorsApi.update(id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['vendors', companyId] });
      toast.success('Vendor updated successfully');
    },
    onError: (error: any) => {
      toast.error(error.response?.data?.message || 'Failed to update vendor');
    },
  });

  // Delete vendor
  const deleteVendor = useMutation({
    mutationFn: (id: string) => vendorsApi.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['vendors', companyId] });
      toast.success('Vendor deleted successfully');
    },
    onError: (error: any) => {
      toast.error(error.response?.data?.message || 'Failed to delete vendor');
    },
  });

  return {
    vendors: vendors || [],
    isLoading,
    error,
    createVendor: createVendor.mutate,
    updateVendor: updateVendor.mutate,
    deleteVendor: deleteVendor.mutate,
    isCreating: createVendor.isPending,
    isUpdating: updateVendor.isPending,
    isDeleting: deleteVendor.isPending,
  };
};
