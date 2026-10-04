# FixFlow - AI Architecture & Trust Boundaries

FixFlow integrates local, privacy-first Large Language Models via Ollama to transform unstructured user issue descriptions into actionable, structured data.

---

## 1. Trust Boundaries & Guardrails
1. **AI Output is UNTRUSTED INPUT**: All output from Ollama is parsed into schema models, validated against JSON schema rules, and normalized before reaching business entities.
2. **Deterministic Rules Over AI Opinions**: AI provides classification candidates and confidence scores; final issue priority and technician eligibility are calculated deterministically by C# domain logic.
3. **No AI Verification Authority**: AI must NEVER autonomously verify technician qualifications, licenses, or diplomas. Only human administrators can mark evidence as verified.
4. **No Direct State Manipulation**: AI cannot alter `WorkOrder` or `Issue` state machines directly.

---

## 2. Asynchronous Flow
```
[React Client] -> (POST /api/issues) -> [API]
                                         |
                                         v (Publishes IssueAnalysisRequested)
                                    [RabbitMQ]
                                         |
                                         v (Consumes)
                                    [Worker Service]
                                         |
                                         v (Structured JSON Prompt)
                                    [Ollama LLM]
                                         |
                                         v (Validates, clamps, parses)
                                    [Worker Service]
                                         |
                                         v (Publishes IssueAnalyzed)
                                    [RabbitMQ]
```

## 3. Supported Local Models
Default model: `llama3.2:3b` (via `ollama pull llama3.2:3b`).
Alternative models: `mistral:7b`, `phi3:mini`, `qwen2.5:3b`.
