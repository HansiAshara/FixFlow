# FixFlow - Deployment & Containerization Guide

## 1. Local Development via Docker Compose

```bash
# 1. Clone the repository and copy environment template
cp .env.example .env

# 2. Spin up all infrastructure and service containers
docker compose up -d

# 3. Pull the local AI model for Ollama
powershell infra/scripts/pull-ollama-model.ps1
# Or on Linux/macOS:
docker exec -it fixflow_ollama ollama pull llama3.2:3b
```

## 2. Container Ports Reference

| Service | Port | Description |
| :--- | :--- | :--- |
| `web` | `3000` | React Web Application (Nginx) |
| `api` | `5000` | ASP.NET Core API |
| `worker` | N/A | .NET Worker Service (MassTransit background processor) |
| `postgres` | `5432` | PostgreSQL 16 Database |
| `rabbitmq` | `5672` / `15672` | RabbitMQ Broker & Web Management Dashboard |
| `ollama` | `11434` | Ollama Local AI Inference API |
