# FixFlow - Architecture Specification

## 1. System Topology
FixFlow is built as a **Modular Monolith** paired with **Distributed Background Workers**.

```
+-------------------------------------------------------------+
|                 React + TypeScript Web Client               |
+-------------------------------------------------------------+
                              |
                              | REST / JSON (HTTP)
                              v
+-------------------------------------------------------------+
|                     ASP.NET Core API                        |
|                                                             |
|  [Identity]  [Issues]  [Matching]  [Technicians]           |
|  [WorkOrders]  [Resolutions]  [Performance]  [Admin]       |
+-------------------------------------------------------------+
         |                                         |
         | EF Core / SQL                           | MassTransit / AMQP
         v                                         v
+-------------------+                    +--------------------+
|    PostgreSQL     |                    |      RabbitMQ      |
+-------------------+                    +--------------------+
                                                   |
                                                   v
                                         +--------------------+
                                         |    .NET Worker     |
                                         | (Consumers & AI)   |
                                         +--------------------+
                                                   |
                                                   | Local HTTP
                                                   v
                                         +--------------------+
                                         |    Ollama LLM      |
                                         +--------------------+
```

## 2. Core Architectural Decisions
1. **Modular Monolith**: Eliminates premature network boundaries, distributed transaction overhead, and microservice complexity while retaining clean domain separation for future extraction.
2. **MassTransit + RabbitMQ**: Decouples slow, asynchronous operations (AI issue classification, email/push notifications, performance aggregations) from the user-facing request thread.
3. **Local AI via Ollama**: Eliminates expensive cloud API costs and keeps user reports private on local infrastructure.
4. **Authoritative Backend**: All state transitions, authorization checks, and algorithm calculations are strictly enforced by the backend.
