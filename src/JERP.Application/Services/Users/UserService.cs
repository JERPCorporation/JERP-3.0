/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of ninoyerbas.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

using Microsoft.EntityFrameworkCore;
using JERP.Application.DTOs.Users;
using JERP.Core.Entities;
using JERP.Infrastructure.Data;

namespace JERP.Application.Services.Users;

public class UserService : IUserService
{
    private readonly JerpDbContext _context;

    public UserService(JerpDbContext context)
    {
        _context = context;
    }

    public async Task<object> GetAllAsync(int page, int pageSize)
    {
        var query = _context.Users.Include(u => u.Roles);
        var total = await query.CountAsync();

        var users = await query
            .OrderBy(u => u.Username)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => MapToDto(u))
            .ToListAsync();

        return new
        {
            Users = users,
            Total = total,
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(total / (double)pageSize)
        };
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _context.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == Guid.Parse(id.ToString()));
        return user != null ? MapToDto(user) : null;
    }

    public async Task<UserDto> CreateAsync(CreateUserRequest request)
    {
        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            IsActive = true
        };

        // Load roles if any are specified
        if (request.RoleIds.Any())
        {
            var roles = await _context.Roles
                .Where(r => request.RoleIds.Contains(r.Id))
                .ToListAsync();
            user.Roles = roles;
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return MapToDto(user);
    }

    public async Task<UserDto?> UpdateAsync(int id, UpdateUserRequest request)
    {
        var user = await _context.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == Guid.Parse(id.ToString()));
        if (user == null) return null;

        user.Email = request.Email;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.IsActive = request.IsActive;

        // Update roles if specified
        if (request.RoleIds.Any())
        {
            user.Roles.Clear();
            var roles = await _context.Roles
                .Where(r => request.RoleIds.Contains(r.Id))
                .ToListAsync();
            foreach (var role in roles)
            {
                user.Roles.Add(role);
            }
        }

        await _context.SaveChangesAsync();
        return MapToDto(user);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(id.ToString()));
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsActive = user.IsActive,
            Roles = user.Roles?.Select(r => r.Name).ToList() ?? new List<string>()
        };
    }
}
