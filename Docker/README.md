# Redis Docker Setup on Windows

This repository provides a Docker Compose configuration to run Redis locally on Windows using Docker.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
  - [Clone the Repository](#1-clone-the-repository)
  - [Create the `docker-compose.yml` File](#2-create-the-docker-composeyml-file)
  - [Start Redis](#3-start-redis)
  - [Verify the Redis Container](#4-verify-the-redis-container)
  - [Connect to Redis CLI](#5-connect-to-redis-cli)
  - [Test Redis Commands](#6-test-redis-commands)
- [Stopping and Removing Redis](#stopping-and-removing-redis)
- [License](#license)

## Prerequisites

- Install [Docker Desktop](https://www.docker.com/products/docker-desktop/) on Windows.
- Ensure **WSL 2** is enabled in Docker settings.

## Getting Started

### 1. Clone the Repository
```sh
git clone https://github.com/yourusername/your-repo.git
cd your-repo
```

### 2. Create the `docker-compose.yml` File
Ensure your repository contains the following `docker-compose.yml` file:

```yaml
version: '3.8'

services:
  redis:
    image: redis:latest
    container_name: redis_local
    restart: always
    ports:
      - "6379:6379"
    volumes:
      - redis_data:/data
    command: ["redis-server", "--appendonly", "yes"]

volumes:
  redis_data:
    driver: local
```

### 3. Start Redis
Run the following command to start the Redis container:

```sh
docker-compose up -d
```

### 4. Verify the Redis Container
Check if Redis is running:

```sh
docker ps
```

You should see a running container named `redis_local`.

### 5. Connect to Redis CLI
To interact with Redis, execute:

```sh
docker exec -it redis_local redis-cli
```

### 6. Test Redis Commands
Once inside the Redis CLI, test the setup with:

```sh
SET mykey "Hello Redis"
GET mykey
```

If everything is set up correctly, you should see:
```
"Hello Redis"
```

## Stopping and Removing Redis
To stop Redis:

```sh
docker-compose down
```

To remove all data:

```sh
docker volume rm your-repo_redis_data
```

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
