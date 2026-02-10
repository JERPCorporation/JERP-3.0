# JERP MVP - Sistema Contador-Céntrico

## 🎯 Arquitectura del Sistema

```
┌─────────────────┐
│  Cliente (App)  │ ← MAUI Mobile/Desktop
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  Backend API    │ ← .NET Core
└────────┬────────┘
         │
    ┌────┴────┐
    ▼         ▼
┌────────┐  ┌──────────┐
│   DB   │  │ Claude   │
│        │  │   API    │
└────────┘  └──────────┘
         │
         ▼
┌─────────────────┐
│ Contador (Web)  │ ← Next.js Dashboard
└─────────────────┘
```

## 📦 Componentes Generados

### 1. Dashboard del Contador (Next.js)
- `AccountantDashboard.tsx` - Vista principal
- `api-claude-analysis.ts` - Endpoint Claude

### 2. Backend API (.NET)
- `AccountantController.cs` - API endpoints
- `ClaudeService.cs` - Integración Anthropic

### 3. App Cliente (MAUI)
- `ClientHomePage.xaml` - Vista "Mi Contador"

## 🚀 Setup Rápido

### Paso 1: Backend

```bash
cd src/JERP.Api

# Agregar paquete Anthropic
dotnet add package Anthropic.SDK

# Configurar appsettings.json
```

```json
{
  "Anthropic": {
    "ApiKey": "sk-ant-..." // Tu API key
  }
}
```

### Paso 2: Frontend (Dashboard Contador)

```bash
cd landing-page

# Instalar Anthropic SDK
npm install @anthropic-ai/sdk

# Crear .env.local
```

```env
ANTHROPIC_API_KEY=sk-ant-...
NEXT_PUBLIC_API_URL=http://localhost:7001
```

### Paso 3: MAUI App

```bash
cd src/JERP.Maui

# Ya configurado, solo compilar
dotnet build
```

## 💰 Modelo de Costos

### Claude API Pricing (Approximate)

```
Haiku (simple):   $0.25 / 1M input, $1.25 / 1M output
Sonnet (normal):  $3 / 1M input, $15 / 1M output
Opus (complex):   $15 / 1M input, $75 / 1M output
```

### Costos por operación típica:

```javascript
// Categorizar transacción (Haiku)
Input:  ~200 tokens  = $0.00005
Output: ~20 tokens   = $0.000025
Total:  ~$0.000075 por transacción

// Análisis mensual completo (Sonnet)
Input:  ~3,000 tokens  = $0.009
Output: ~1,000 tokens  = $0.015
Total:  ~$0.024 por análisis

// Cliente promedio (50 transacciones/mes):
- 50 categorizaciones:  50 × $0.000075 = $0.00375
- 1 análisis mensual:   $0.024
- TOTAL:                ~$0.03/mes por cliente
```

### Pricing del Servicio:

```
Contador Básico:      $10/mes
- Hasta 50 clientes
- ~$1.50 en Claude API
- Margen: $8.50 (85%)

Cliente Solo:         $2/mes  
- ~$0.03 en Claude API
- Margen: $1.97 (98.5%)

Contador Pro:         $50/mes
- Hasta 200 clientes
- ~$6 en Claude API
- Margen: $44 (88%)
```

## 🔑 Features Principales

### Para el Contador

1. **Dashboard Multi-Cliente**
   - Ver todos los clientes en un solo lugar
   - Alertas de transacciones pendientes
   - Sugerencias de Claude

2. **Sistema de Aprobación**
   - Aprobar/Rechazar transacciones
   - Modificar antes de aprobar
   - Firma digital

3. **Análisis con Claude**
   - Detección de anomalías
   - Cálculos preliminares ISR/IVA
   - Sugerencias de optimización

### Para el Cliente

1. **Vista "Mi Contador"**
   - Contacto directo con su contador
   - Ver estado de aprobaciones
   - Chat integrado

2. **Registro Simple**
   - Agregar ventas/gastos rápidamente
   - Subir fotos de facturas
   - Categorización automática (Claude)

3. **Reportes en Tiempo Real**
   - Ingresos vs Gastos
   - ISR estimado (preliminar)
   - Alertas y sugerencias

## 📊 Flujo de Trabajo Típico

### Día a día del Cliente:

```
8:00 AM - Cliente hace venta
        - Registra en app: "Venta Q500"
        - Claude categoriza automáticamente
        - Estado: "Pendiente revisión contador"

12:00 PM - Cliente sube foto de factura de compra
         - App extrae datos (OCR)
         - Claude sugiere categoría
         - Notifica a contador

6:00 PM - Cliente ve en app:
        - "Tu contador aprobó 5 transacciones"
        - ISR estimado actualizado
```

### Día a día del Contador:

```
8:00 AM - Abre dashboard
        - Ve 23 transacciones pendientes
        - Claude ya detectó 2 posibles errores

8:15 AM - Revisa las 2 alertas
        - Corrige categorías
        - Aprueba

8:30 AM - Aprueba resto con un click
        - 21 transacciones aprobadas
        - Clientes notificados automáticamente

9:00 AM - Pide análisis a Claude para cliente X
        - Claude sugiere: "Puede deducir Q800 más"
        - Llama al cliente para asesoría estratégica

Resto del día: LIBRE o atender más clientes
```

## 🔧 Implementación por Fases

### Fase 1 (Semana 1-2): Core MVP

- [ ] Backend: AccountantController
- [ ] Backend: ClaudeService básico
- [ ] Frontend: Dashboard básico
- [ ] MAUI: Vista "Mi Contador"
- [ ] Testing con 5 usuarios beta

### Fase 2 (Semana 3-4): Features Avanzadas

- [ ] Chat contador-cliente
- [ ] Reportes automáticos SAT
- [ ] Facturación electrónica FEL
- [ ] Notificaciones push

### Fase 3 (Mes 2): Optimización

- [ ] Cache de respuestas Claude
- [ ] Batch processing
- [ ] Modo offline
- [ ] Analytics dashboard

## 💡 Trucos para Optimizar Costos

### 1. Usar modelo correcto:

```csharp
// Simple task = Haiku (barato)
await _claude.Haiku("Categoriza: Venta pan");

// Complex analysis = Sonnet (normal)
await _claude.Sonnet("Analiza finanzas completas...");

// Nunca uses Opus a menos que sea crítico
```

### 2. Cache inteligente:

```csharp
// Cache respuestas comunes
private Dictionary<string, string> _cache = new()
{
    ["Venta pan"] = "Ingresos por Ventas",
    ["Compra harina"] = "Costo de Ventas",
    // ...
};

public async Task<string> CategorizeWithCache(string description)
{
    if (_cache.ContainsKey(description))
        return _cache[description]; // $0 cost!
        
    var result = await _claude.Categorize(description);
    _cache[description] = result;
    return result;
}
```

### 3. Batch processing:

```csharp
// Procesar 50 transacciones en 1 llamada
// vs 50 llamadas separadas
// Ahorra ~80% en overhead
```

## 🎓 Próximos Pasos

1. **Obtén API Key de Anthropic:**
   - https://console.anthropic.com

2. **Implementa los archivos:**
   - Copia AccountantController.cs a `src/JERP.Api/Controllers/`
   - Copia ClaudeService.cs a `src/JERP.Api/Services/`
   - Copia AccountantDashboard.tsx a `landing-page/components/`

3. **Configura appsettings.json**

4. **Prueba con datos reales:**
   ```bash
   # Backend
   cd src/JERP.Api
   dotnet run
   
   # Frontend
   cd landing-page
   npm run dev
   ```

5. **Invita 5 contadores beta:**
   - Dales acceso gratis 3 meses
   - Recopila feedback
   - Itera rápido

## 📞 Soporte

- Anthropic Docs: https://docs.anthropic.com
- JERP Issues: Tu repo de GitHub

---

**¡Listo para cambiar la contabilidad en LATAM! 🚀**
