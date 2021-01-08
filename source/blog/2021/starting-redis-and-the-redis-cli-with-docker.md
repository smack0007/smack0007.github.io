---
Title: Starting Redis and the Redis CLI with Docker
Subtitle: 
Date: 2021-01-08
Tags: docker, redis
---

If you'd like to get a Redis instance up and running to play with it's fairly easy via Docker.

<!--more-->

```cmd
docker run --name my-redis -p 6379:6379 -d redis redis-server --appendonly yes
```

This will start a Redis instance with the name `my-redis` with persistent storage turned on. You can also
start with Redis CLI via Docker.

```cmd
docker run --link my-redis -it --rm redis redis-cli -h my-redis 
```

You'll need to replace `my-redis` with whatever you named your Redis instance of course.