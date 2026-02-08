# Claude AI Assistant Setup

JERP 3.0 includes a built-in AI Assistant powered by Claude (Anthropic).

## Quick Setup (5 minutes)

### 1. Get an API Key
- Go to [console.anthropic.com](https://console.anthropic.com)
- Create an account or sign in
- Navigate to API Keys → Create Key
- Copy the key (starts with `sk-ant-`)

### 2. Configure Locally

**Option A: appsettings.Development.json (Recommended)**

Create `src/JERP.Api/appsettings.Development.json`:
```json
{
  "Claude": {
    "ApiKey": "sk-ant-your-actual-key-here"
  }
}
```
This file is gitignored and won't be committed.

**Option B: Environment Variable**
```powershell
$env:Claude__ApiKey = "sk-ant-your-actual-key-here"
```

**Option C: User Secrets (dotnet)**
```bash
cd src/JERP.Api
dotnet user-secrets set "Claude:ApiKey" "sk-ant-your-actual-key-here"
```

### 3. Test It

Start the API and send a POST request:
```bash
curl -X POST https://localhost:7001/api/aiassistant/ask \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -d '{"question": "What is a balance sheet?"}'
```

Expected response:
```json
{
  "question": "What is a balance sheet?",
  "answer": "A balance sheet is a financial statement that...",
  "timestamp": "2026-02-07T..."
}
```

### 4. Usage in JERP Desktop
The WPF Desktop app can call the AI Assistant via the ApiClient:
```csharp
var response = await _apiClient.PostAsync("api/aiassistant/ask", new { Question = "Explain accounts receivable" });
```

## Configuration Reference

| Setting | Default | Description |
|---------|---------|-------------|
| `Claude:ApiKey` | `OVERRIDE_IN_ENVIRONMENT` | Your Anthropic API key |
| `Claude:ApiUrl` | `https://api.anthropic.com/v1/messages` | Anthropic Messages API endpoint |
| `Claude:Model` | `claude-sonnet-4-20250514` | Claude model to use |

## Cost Estimate
- Claude Sonnet: ~$3 per 1M input tokens, ~$15 per 1M output tokens
- Typical accounting question: ~500 tokens in, ~1000 tokens out
- **~$0.02 per question** — very affordable for business use
