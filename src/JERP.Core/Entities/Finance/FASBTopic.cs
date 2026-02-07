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

using System.ComponentModel.DataAnnotations;
using JERP.Core.Enums;

namespace JERP.Core.Entities.Finance;

/// <summary>
/// Represents a FASB ASC (Accounting Standards Codification) topic
/// </summary>
public class FASBTopic : BaseEntity
{
    /// <summary>
    /// Topic code (e.g., "205", "310", "606")
    /// </summary>
    [Required]
    [MaxLength(10)]
    public string TopicCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Topic name (e.g., "Presentation of Financial Statements", "Receivables")
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string TopicName { get; set; } = string.Empty;
    
    /// <summary>
    /// FASB category classification
    /// </summary>
    [Required]
    public FASBCategory Category { get; set; }
    
    /// <summary>
    /// Detailed description of the topic
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    /// <summary>
    /// Whether this topic has been superseded by newer guidance
    /// </summary>
    public bool IsSuperseded { get; set; } = false;
    
    /// <summary>
    /// Reference to superseding topic if applicable
    /// </summary>
    [MaxLength(100)]
    public string? SupersededBy { get; set; }
    
    // Navigation properties
    
    /// <summary>
    /// Subtopics belonging to this topic
    /// </summary>
    public ICollection<FASBSubtopic> Subtopics { get; set; } = new List<FASBSubtopic>();
    
    /// <summary>
    /// Accounts mapped to this topic
    /// </summary>
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}
