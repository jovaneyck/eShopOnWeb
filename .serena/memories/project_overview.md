# eShopOnWeb Project Overview

## Purpose
Microsoft eShopOnWeb is a .NET 9.0 ASP.NET Core reference application demonstrating Clean Architecture principles in a single-process (monolithic) e-commerce application. It serves as a practical example for the eBook "Architecting Modern Web Applications with ASP.NET Core and Azure."

## Key Characteristics
- **Architecture**: Clean Architecture with clear separation of concerns
- **Deployment Model**: Single-process monolithic application (vs microservices)
- **Target Framework**: .NET 9.0
- **Primary Use**: Educational reference application, not a production e-commerce system
- **Maintainer**: NimblePros (Microsoft-endorsed)

## Main Applications
1. **Web (MVC)**: ASP.NET Core MVC application with Razor Pages
2. **PublicApi**: FastEndpoints-based API for external integrations  
3. **BlazorAdmin**: Blazor WebAssembly admin interface
4. **eShopWeb.AppHost**: .NET Aspire orchestration support

## Core Business Domains
- **Catalog Management**: Products, brands, types
- **Basket/Shopping Cart**: User shopping sessions
- **Order Processing**: Order management and tracking
- **User Management**: Identity and authentication
- **Payment Processing**: Payment method handling