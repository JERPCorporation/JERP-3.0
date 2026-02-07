/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: ichbincesartobar@yahoo.com
 */

/**
 * ⚠️ WARNING: DEVELOPMENT ONLY - DO NOT USE IN PRODUCTION
 * 
 * This file contains mock authentication for development and demonstration purposes only.
 * Passwords are stored in plain text and authentication bypasses all security measures.
 * 
 * Before production deployment:
 * - Replace with actual API authentication
 * - Implement proper password hashing (bcrypt, argon2)
 * - Add rate limiting and brute force protection
 * - Use secure session management
 * - Implement CSRF protection
 */

// Mock user data for development
const MOCK_USERS = [
  {
    id: '1',
    email: 'ichbincesartobar@yahoo.com',
    password: 'admin123',
    name: 'Julio Cesar Mendez Tobar Jr.',
    role: 'super_admin',
  },
  {
    id: '2',
    email: 'user@jerp.com',
    password: 'user123',
    name: 'María González',
    role: 'user',
  },
  {
    id: '3',
    email: 'carlos@jerp.com',
    password: 'user123',
    name: 'Carlos Ruiz',
    role: 'payroll_manager',
  },
];

export interface MockUser {
  id: string;
  email: string;
  name: string;
  role: string;
}

export interface MockLoginResponse {
  user: MockUser;
  token: string;
}

export async function mockLogin(email: string, password: string): Promise<MockLoginResponse> {
  // Simulate API delay
  await new Promise(resolve => setTimeout(resolve, 500));
  
  const user = MOCK_USERS.find(u => u.email === email && u.password === password);
  
  if (!user) {
    throw new Error('Email o contraseña incorrectos');
  }
  
  return {
    user: {
      id: user.id,
      email: user.email,
      name: user.name,
      role: user.role,
    },
    token: 'mock-jwt-token-' + user.id,
  };
}

export function getCurrentUser(): MockUser | null {
  if (typeof window === 'undefined') return null;
  
  const userStr = localStorage.getItem('jerp-user');
  if (!userStr) return null;
  
  try {
    return JSON.parse(userStr) as MockUser;
  } catch {
    return null;
  }
}

export function logout() {
  if (typeof window === 'undefined') return;
  
  localStorage.removeItem('jerp-user');
  localStorage.removeItem('jerp-token');
  window.location.href = '/auth/login';
}
