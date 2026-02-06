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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JERP.Application.DTOs.Finance;
using JERP.Core.Enums;
using JERP.Infrastructure.Data;

namespace JERP.Api.Controllers;

/// <summary>
/// FASB ASC Topic and Subtopic lookup endpoints
/// </summary>
[Route("api/v1/finance/fasb")]
[Authorize]
public class FASBController : BaseApiController
{
    private readonly JerpDbContext _context;
    private readonly ILogger<FASBController> _logger;

    public FASBController(
        JerpDbContext context,
        ILogger<FASBController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all FASB topics
    /// </summary>
    [HttpGet("topics")]
    public async Task<IActionResult> GetTopics([FromQuery] FASBCategory? category = null)
    {
        var query = _context.FASBTopics.AsQueryable();

        if (category.HasValue)
        {
            query = query.Where(t => t.Category == category.Value);
        }

        var topics = await query
            .OrderBy(t => t.TopicCode)
            .Select(t => new FASBTopicDto
            {
                Id = t.Id,
                TopicCode = t.TopicCode,
                TopicName = t.TopicName,
                Category = t.Category,
                Description = t.Description,
                IsSuperseded = t.IsSuperseded,
                SupersededBy = t.SupersededBy,
                Subtopics = new List<FASBSubtopicDto>(),
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .ToListAsync();

        return Ok(topics);
    }

    /// <summary>
    /// Get subtopics for a specific topic
    /// </summary>
    [HttpGet("topics/{topicId}/subtopics")]
    public async Task<IActionResult> GetSubtopics(Guid topicId)
    {
        var subtopics = await _context.FASBSubtopics
            .Where(s => s.FASBTopicId == topicId)
            .OrderBy(s => s.SubtopicCode)
            .Select(s => new FASBSubtopicDto
            {
                Id = s.Id,
                FASBTopicId = s.FASBTopicId,
                SubtopicCode = s.SubtopicCode,
                SubtopicName = s.SubtopicName,
                FullReference = s.FullReference,
                Description = s.Description,
                IsRepealed = s.IsRepealed,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            })
            .ToListAsync();

        return Ok(subtopics);
    }

    /// <summary>
    /// Get a specific topic by ID with subtopics
    /// </summary>
    [HttpGet("topics/{topicId}")]
    public async Task<IActionResult> GetTopic(Guid topicId)
    {
        var topic = await _context.FASBTopics
            .Where(t => t.Id == topicId)
            .Select(t => new FASBTopicDto
            {
                Id = t.Id,
                TopicCode = t.TopicCode,
                TopicName = t.TopicName,
                Category = t.Category,
                Description = t.Description,
                IsSuperseded = t.IsSuperseded,
                SupersededBy = t.SupersededBy,
                Subtopics = t.Subtopics.Select(s => new FASBSubtopicDto
                {
                    Id = s.Id,
                    FASBTopicId = s.FASBTopicId,
                    SubtopicCode = s.SubtopicCode,
                    SubtopicName = s.SubtopicName,
                    FullReference = s.FullReference,
                    Description = s.Description,
                    IsRepealed = s.IsRepealed,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                }).ToList(),
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .FirstOrDefaultAsync();

        if (topic == null)
        {
            return NotFound();
        }

        return Ok(topic);
    }

    /// <summary>
    /// Search FASB references
    /// </summary>
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest("Search query is required");
        }

        var results = await _context.FASBSubtopics
            .Include(s => s.Topic)
            .Where(s => s.FullReference.Contains(query) || 
                       s.SubtopicName.Contains(query) ||
                       s.Topic.TopicName.Contains(query))
            .Take(50)
            .Select(s => new
            {
                s.Id,
                s.FullReference,
                s.SubtopicName,
                TopicName = s.Topic.TopicName,
                TopicCode = s.Topic.TopicCode,
                s.IsRepealed,
                TopicIsSuperseded = s.Topic.IsSuperseded,
                SupersededBy = s.Topic.SupersededBy
            })
            .ToListAsync();

        return Ok(results);
    }
}
