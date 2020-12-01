---
Title: Use nginx in docker to serve static files
Subtitle: 
Date: 2020-12-01
Tags: docker, nginx
---

Not sure how practical this really is but I thought it was neat. Run nginx and tell it to
serve some static content on your system.

<!--more-->

```cmd
docker run --name static-nginx -p 8080:80 -v /d/path/to/my/files:/usr/share/nginx/html:ro -d nginx
```

`/d/path/to/my/files/` is the syntax to use if you're running docker on
[WSL 2](https://docs.microsoft.com/en-us/windows/wsl/wsl2-faq).

