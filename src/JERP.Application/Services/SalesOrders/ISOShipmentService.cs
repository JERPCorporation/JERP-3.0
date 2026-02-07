/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of Julio Cesar Mendez Tobar.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: ichbincesartobar@yahoo.com
 */

using JERP.Application.DTOs.SalesOrders;

namespace JERP.Application.Services.SalesOrders;

/// <summary>
/// Service interface for SO shipment management
/// </summary>
public interface ISOShipmentService
{
    /// <summary>
    /// Get shipment by ID with line items
    /// </summary>
    Task<SOShipmentDto?> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Get all shipments for a sales order
    /// </summary>
    Task<List<SOShipmentDto>> GetBySalesOrderAsync(Guid salesOrderId);
    
    /// <summary>
    /// Get all shipments for a company
    /// </summary>
    Task<List<SOShipmentDto>> GetAllAsync(Guid companyId);
    
    /// <summary>
    /// Create a new shipment
    /// </summary>
    Task<SOShipmentDto> CreateAsync(Guid companyId, CreateSOShipmentRequest request);
    
    /// <summary>
    /// Mark shipment as shipped
    /// </summary>
    Task<SOShipmentDto> ShipAsync(Guid id, string shippedBy);
    
    /// <summary>
    /// Cancel a shipment (reverse inventory)
    /// </summary>
    Task CancelAsync(Guid id);
}
