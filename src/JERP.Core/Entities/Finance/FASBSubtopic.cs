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

namespace JERP.Core.Entities.Finance;

/// <summary>
/// Represents a FASB ASC subtopic within a topic
/// </summary>
public class FASBSubtopic : BaseEntity
{
    /// <summary>
    /// Foreign key to the parent FASB topic
    /// </summary>
    [Required]
    public Guid FASBTopicId { get; set; }
    
    /// <summary>
    /// Subtopic code (e.g., "10", "20", "30")
    /// </summary>
    [Required]
    [MaxLength(10)]
    public string SubtopicCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Subtopic name (e.g., "Overall", "Discontinued Operations")
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string SubtopicName { get; set; } = string.Empty;
    
    /// <summary>
    /// Full FASB reference (e.g., "ASC 606-10")
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string FullReference { get; set; } = string.Empty;
    
    /// <summary>
    /// Detailed description of the subtopic
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    /// <summary>
    /// Whether this subtopic has been repealed
    /// </summary>
    public bool IsRepealed { get; set; } = false;
    
    // Navigation properties
    
    /// <summary>
    /// Parent FASB topic
    /// </summary>
    public FASBTopic Topic { get; set; } = null!;
    
    /// <summary>
    /// Accounts mapped to this subtopic
    /// </summary>
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}
