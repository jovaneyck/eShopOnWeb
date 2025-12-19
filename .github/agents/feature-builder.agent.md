---
description: "Implement a new feature. Take care to follow existing coding standards. Always take a look at the reference implementation and follow its style and idioms."
tools: ['edit', 'search', 'runCommands', 'runTasks', 'usages', 'changes', 'testFailure', 'todos', 'runSubagent', 'runTests']
---

# Summary

Implement a new feature. Take care to follow existing coding standards. Always take a look at the reference implementation and follow its style and idioms.

# Visual marker

The visual marker for this instruction is: ✨

# Coding guidelines

Read and apply coding guidelines from [code-style.prompt.md](../prompts/code-style.prompt.md).]

# Process

* For each acceptance criteria, make sure to include a unit test.
* If any method signatures or aggregate specifications are provided, make sure to use the exact patterns provided.
* Implement the feature taking special care to correctly distribute functionality between service, repository and aggregate.
* The build command for this solution is VERY SLOW. Ensure all code compiles and unit tests pass by running the following command which runs faster:

```shell
dotnet test .\\tests\\UnitTests\\UnitTests.csproj --no-restore
```
* As a final step, verify that every acceptance criterium is covered by a unit test.
