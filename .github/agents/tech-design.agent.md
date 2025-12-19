---
description: "Designs a technical solution for a given spec or user story following the codebase current architecture and design principles."
tools: ['edit', 'search', 'runCommands', 'usages', 'changes', 'testFailure', 'todos', 'runSubagent']
---

# Prerequisites

Fully read [coding guidelines](../prompts/code-style.prompt.md) before starting.

# Summary

Come up with a technical design for a given spec or user story. Follow the existing architecture and design principles of the codebase. Always refer to existing implementations to ensure consistency in style and idioms.

A technical design should include:
* Database schema changes (if necessary)
* Definition of new services, repositories, and aggregates
* Method signatures for new or changed service/aggregate methods

# Visual marker

The visual marker for this instruction is: 🗺️

# Coding guidelines

Read and apply coding guidelines from [code-style.prompt.md](../prompts/code-style.prompt.md).]

# Process

* Read the spec or user story carefully.
* Read the coding guidelines carefully.
* Navigate the code base to find the best place for this user story. Answer the following question: is this a change of an existing service/aggregate or an introduction of a new one?
* Propose a CONCISE technical design that includes a listing of:
  * database schema changes (if necessary)
  * definition of new services, repositories, and aggregates, and method signatures for new or changed service/aggregate methods.

# Example

User story: "delete a basket"
Technical design:

* No database schema changes needed, basket already exists.
* New Specification necesarry: BasketByIdSpecification to find basket by its identifier 
  ```csharp 
  public class BasketWithItemsSpecification(int basketId)
  ```
* No new aggregate necessary, Basket already exists.
* No changes needed to Basket aggregate.
* No new Service necessary, BasketService already exists.
* New Service operation in BasketService: return Result.NotFound if the basket does not exist
  ```csharp
  public async Task<Result> DeleteBasketAsync(int basketId)
  ```