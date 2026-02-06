/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of Julio Cesar Mendez Tobar.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

using JERP.Core.Enums;

namespace JERP.Application.DTOs.Finance;

/// <summary>
/// FASB topic data transfer object
/// </summary>
public class FASBTopicDto
{
    public Guid Id { get; set; }
    public required string TopicCode { get; set; }
    public required string TopicName { get; set; }
    public FASBCategory Category { get; set; }
    public string? Description { get; set; }
    public bool IsSuperseded { get; set; }
    public string? SupersededBy { get; set; }
    public List<FASBSubtopicDto> Subtopics { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// FASB subtopic data transfer object
/// </summary>
public class FASBSubtopicDto
{
    public Guid Id { get; set; }
    public Guid FASBTopicId { get; set; }
    public required string SubtopicCode { get; set; }
    public required string SubtopicName { get; set; }
    public required string FullReference { get; set; }
    public string? Description { get; set; }
    public bool IsRepealed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
