---
Title: Use VSCode to remove comments with regex
Subtitle:
Date: 2025-01-20
Tags: vscode, regex
---

In you need to remove multiline comments (`/* Comment */`) from files in VSCode here is an easy regex to help you find them:

```shell
\/\*\*([\s\S\n]+?)\*\/
```
