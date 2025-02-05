markdown
# Redis Queue Management Module

## Table of Contents
- [Introduction](#introduction)
- [Features](#features)
- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
  - [Queue Types](#queue-types)
  - [Sending Messages](#sending-messages)
  - [Receiving Messages](#receiving-messages)
  - [Distributed Lock Management](#distributed-lock-management)
- [API Endpoints](#api-endpoints)
- [License](#license)

---

## Introduction
This module is designed for educational purposes and provides an implementation of Redis-based queues for message processing and distributed locking. It uses `StackExchange.Redis` for Redis integration and supports various queue types, including **ListQueue, DelayedQueue, PubSubQueue, and StreamQueue**.

The module enables developers to:
- Send and receive messages through different types of Redis queues.
- Implement a distributed locking mechanism to handle concurrency.

## Features
- 📌 **Multiple queue types**: FIFO list queues, delayed queues, pub/sub messaging, and stream-based queues.
- 🔄 **Asynchronous message processing** with Redis.
- 🔐 **Distributed locking system** to handle concurrent operations safely.
- 🔧 **Dependency injection** support for easy integration into ASP.NET applications.
- 📑 **Swagger and OpenAPI** support for API documentation.

## Installation
To integrate this module into your project, install the necessary dependencies:

```sh
dotnet add package StackExchange.Redis
```

Ensure that Redis is running and accessible in your environment.

## Configuration
The Redis configuration settings are managed in `appsettings.json`:

```json
{
  "RedisSettings": {
    "Host": "localhost",
    "Port": 6379,
    "Password": "your_redis_password",
    "Database": 0,
    "QueuePrefix": "app_queue_",
    "MaxRetries": 5
  }
}
```

In `Program.cs`, the module is registered as follows:

```csharp
using RedisManagement.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Redis services
builder.Services.RedisConfiguration();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

## Usage

### Queue Types
This module provides four types of Redis-based queues:
1. **ListQueue**: Standard FIFO queue using Redis lists.
2. **DelayedQueue**: Uses Redis sorted sets to delay message processing.
3. **PubSubQueue**: Implements Redis Pub/Sub for real-time messaging.
4. **StreamQueue**: Uses Redis Streams for structured message handling.

### Sending Messages
To send a message to a Redis queue:

```csharp
var queueFactory = serviceProvider.GetRequiredService<IRedisQueueFactory>();
var queue = queueFactory.GetQueue(EnRedisQueueType.ListQueue);
await queue.SendMessageAsync(EnRedisQueueName.Queue1, "Hello, Redis Queue!");
```

### Receiving Messages
To retrieve messages from the queue:

```csharp
var queue = queueFactory.GetQueue(EnRedisQueueType.ListQueue);
var message = await queue.ReceiveMessageAsync(EnRedisQueueName.Queue1);
Console.WriteLine($"Received: {message}");
```

### Distributed Lock Management
To acquire and release a distributed lock in Redis:

```csharp
var lockManager = serviceProvider.GetRequiredService<IRedisLockManager>();

// Acquire a lock
bool isAcquired = await lockManager.AcquireLockAsync(EnRedisLockCategory.Database, "my-resource", TimeSpan.FromMinutes(1));

if (isAcquired) {
    Console.WriteLine("Lock acquired!");
}

// Release the lock
await lockManager.ReleaseLockAsync(EnRedisLockCategory.Database, "my-resource");
Console.WriteLine("Lock released!");
```

## API Endpoints
This module exposes RESTful endpoints through `HomeController`:

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/home/send-message/{queue}/{queueName}` | `POST` | Sends a message to a specific Redis queue. |
| `/api/home/acquire-lock/{category}/{lockID}` | `POST` | Acquires a distributed lock. |
| `/api/home/release-lock/{category}/{lockID}` | `POST` | Releases a distributed lock. |
| `/api/home/extend-lock/{category}/{lockID}` | `POST` | Extends an existing lock duration. |
| `/api/home/is-locked/{category}/{lockID}` | `GET` | Checks if a lock is active. |

## License
This project is open-source and available under the MIT License.

---

## 👤 Author
**Kambiz Shahriarynasab**  
📧 [saiprogrammerk@gmail.com](mailto:saiprogrammerk@gmail.com)  
🔗 [Telegram](https://t.me/pr_kami)  
📷 [Instagram](https://www.instagram.com/pr.kami.sh/)  
📺 [YouTube](https://www.youtube.com/channel/UCqjjdsFRXliDa7K612BZtmA)  
💼 [LinkedIn](https://www.linkedin.com/public-profile/settings?trk=d_flagship3_profile_self_view_public_profile)

## ⚠️ Disclaimer
The author assumes no responsibility for any issues, damages, or losses that may arise from the use of this code. The project is provided **"as is"** without any warranties. Users should verify the implementation in their environments before deploying it in production.

---
