# Text Intelligence API

A REST API for core **Azure AI Language** text analysis capabilities, designed as an **AI-102 practice service** with clean contracts and predictable outputs.

- Tech: **.NET**, **ASP.NET Core**, **Azure AI Language** (Text Analytics via `Azure.AI.TextAnalytics`)
- Status: ⏸️ Paused (not actively maintained)

---

## Why this project exists (AI-102 alignment)

This API targets the AI-102 skill area **“Analyze and translate text”**, focusing on:
- Detect the language used in text
- Determine sentiment of text
- Extract key phrases and entities

AI-102 also includes:
- Detect personally identifiable information (PII) in text
- Translate text and documents using Azure Translator

Those are planned next.

---

## Current capabilities

### Implemented ✅
- **Language detection**
- **Sentiment analysis**
- **Key phrase extraction**
  - Single document
  - Batch
- **Prebuilt entity recognition (NER)**
  - Single document
  - Batch

### Maintenance note
This repository is currently **paused** and provided as a reference snapshot for AI-102 practice.
No new features are planned in the short term.


---

## API design principles

- **Predictable outputs** via a consistent `ResponseEnvelope<T>` contract
- **Correlation ID** returned on every response for traceability
- **SDK encapsulation**: Controllers depend on internal clients/services, not SDK calls directly
- **Batch endpoints** return per-document results and per-document errors when applicable

---

## Project structure

- `Common/` → shared utilities (envelope, errors, extensions)
- `Contracts/` → request/response DTOs
- `Controllers/` → HTTP endpoints
- `Services/AzureLanguage/` → Azure AI Language SDK wrappers (TextAnalyticsClient)
- `Middleware/` → correlation ID and pipeline behaviors
- `TextIntelligenceApi.http` → ready-to-run HTTP examples
- `Program.cs` → DI + API setup

---

## Getting started

### Prerequisites
- .NET SDK
- An **Azure AI Language** resource (endpoint + API key)

### Configuration

Set the Azure AI Language settings using `appsettings.Development.json` or environment variables.

Example `appsettings.Development.json`:
```json
{
  "AzureLanguage": {
    "Endpoint": "https://<your-resource-name>.cognitiveservices.azure.com/",
    "ApiKey": "<your-key>"
  }
}
```

### Run locally
```bash
dotnet restore
dotnet run
```

> Tip: you can also use `dotnet watch` for faster dev loops:
```bash
dotnet watch run
```

---

## Endpoints

> Base URL examples assume local HTTPS. Adjust ports based on your launch profile.

### Key Phrases
- `POST /api/key-phrases/extract`
- `POST /api/key-phrases/extract/batch`

**Single request**
```json
{
  "text": "The food was delicious and the staff was wonderful.",
  "language": "en"
}
```

**Batch request**
```json
{
  "documents": [
    { "id": "1", "text": "The food was delicious and the staff was wonderful.", "language": "en" },
    { "id": "2", "text": "Azure AI Language is useful for extracting key phrases from text.", "language": "en" }
  ]
}
```

### Entities (Prebuilt NER)
- `POST /api/entities/extract`
- `POST /api/entities/extract/batch`

**Single request**
```json
{
  "text": "Satya Nadella visited Microsoft in Seattle on January 10, 2025.",
  "language": "en"
}
```

**Batch request (French sample)**
```json
{
  "documents": [
    { "id": "fr-1", "text": "Emmanuel Macron a rencontré Microsoft à Paris le 10 janvier 2025.", "language": "fr" },
    { "id": "fr-2", "text": "Air France a annoncé un nouveau vol entre Montréal et Lyon en 2024.", "language": "fr" }
  ]
}
```
### Sentiment Analysis
- `POST /api/sentiment/extract`
- `POST /api/sentiment/extract/batch`

**Single request**
```json
{
  "text": "I love the new UI, but performance is slow sometimes.",
  "language": "en"
}
```

**Batch request**
```json
{
    "documents": [
      { "id": "1", "text": "I love this!", "language": "en" },
      { "id": "2", "text": "This is terrible. I want a refund.", "language": "en" },
      { "id": "3", "text": "", "language": "en" }
    ],
    "defaultLanguage": "en",
    "includeOpinionMining": false
}
```

### Language Detection
- `POST /api/language/detect`
- `POST /api/language/detect/batch`

**Single request**
```json
{
  "text": "Bonjour, je voudrais réserver un hôtel à Montréal."
}
```

**Batch request**
```json
{
  "items": [
    { "id": "1", "text": "Hello world" },
    { "id": "2", "text": "Hola mundo" }
  ]
}
```

---

## Testing

Use the included HTTP file:
- `TextIntelligenceApi.http`

Visual Studio supports running `.http` requests directly from the IDE (great for quick manual testing).

---

## Backlog (future work, paused)

The AI-102 study guide also includes:
- Detect personally identifiable information (PII) in text
- Translate text and documents

These are **not implemented yet** and are left as future work when the project resumes.

---

## License

MIT
