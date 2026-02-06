/**
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * For licensing inquiries: licensing@jerp.io
 */

import type { Metadata } from 'next';
import '../styles/globals.css';
import { Providers } from './providers';

export const metadata: Metadata = {
  title: 'JERP 3.0 - Planilla de Pagos Inteligente para PYMEs',
  description: 'Software de nómina y RRHH con cálculo automático de impuestos, compliance integrado y generación de recibos de pago en PDF.',
  keywords: 'planilla de pagos, nómina, payroll, RRHH, PYMEs, compliance, impuestos',
  openGraph: {
    title: 'JERP 3.0 - Planilla de Pagos para PYMEs',
    description: 'Automatiza tu nómina en minutos. Prueba gratis 14 días.',
    images: ['/og-image.png'],
  },
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="es">
      <body className="font-sans">
        <Providers>{children}</Providers>
      </body>
    </html>
  );
}
