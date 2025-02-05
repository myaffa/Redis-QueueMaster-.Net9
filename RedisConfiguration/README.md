# Redis Configuration Module

## Table of Contents
- [Introduction](#introduction)
- [Features](#features)
- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
  - [RedisQueueSender](#redisqueuesender)
  - [RedisQueueListener](#redisqueuelistener)
  - [RedisQueueProcessor](#redisqueueprocessor)
  - [IRedisLockManager](#iredislockmanager)
- [Enums](#enums)
- [License](#license)

---

## Introduction
This module provides a robust Redis-based queueing system with distributed locking capabilities. It is designed to facilitate asynchronous messaging and distributed task execution using Redis. The module integrates with `StackExchange.Redis` and follows best practices for dependency injection and background processing.

## Features
- **Redis-based queueing system** with multiple queue types
- **Background service** to process queued messages
- **Distributed locking mechanism** to prevent race conditions
- **Customizable configurations** for Redis connection settings
- **Easy-to-use interface** for queue producers and consumers

## Installation
To integrate this module into your project, install the required dependencies and reference the module in your `.NET` project:

```sh
# Install Redis client library
dotnet add package StackExchange.Redis
```

## Configuration
The module relies on a JSON configuration file (`appsettings.redis.json`) to set up Redis connection details. Ensure you have the following structure in your configuration:

```json
{
  "redis": {
    "Host": "localhost",
    "Port": 6379,
    "Password": "your_redis_password",
    "Database": 0,
    "QueuePrefix": "app_queue_",
    "MaxRetries": 5
  }
}
```

## Usage

### RedisQueueSender
`RedisQueueSender` is responsible for sending messages to Redis queues.

#### Example:
```csharp
var sender = serviceProvider.GetRequiredService<IRedisQueueSender>();
await sender.SendToQueueAsync(EnRedisQueueName.Queue1, "Sample message");
```

### RedisQueueListener
`RedisQueueListener` runs as a background service and continuously monitors queues for new messages.

### RedisQueueProcessor
`RedisQueueProcessor` processes received messages.

#### Example:
```csharp
public class CustomProcessor : IRedisQueueProcessor {
    public async Task ProcessMessageAsync(EnRedisQueueName queueName, string message) {
        Console.WriteLine($"Processing message from {queueName}: {message}");
    }
}
```

### IRedisLockManager
Provides distributed locking functionality.

#### Example:
```csharp
var lockManager = serviceProvider.GetRequiredService<IRedisLockManager>();
bool acquired = await lockManager.AcquireLockAsync(EnRedisLockCategory.Database, "resource-key", TimeSpan.FromMinutes(1));
```

## Enums

### EnRedisLockCategory
- `Database`: Locks related to database operations.
- `Actions`: Locks for specific system actions.

### EnRedisQueueName
- `Queue1`, `Queue2`, `Queue3`: Names for different Redis queues.

### EnRedisQueueType
- `ListQueue`
- `DelayedQueue`
- `PubSubQueue`
- `StreamQueue`

## License
This project is open-source and available under the MIT License.

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
