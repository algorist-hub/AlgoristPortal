# AlgoristPortal

## Introduction

Algorist Platform is a C# application built with .NET 10 and Blazor Server/Client.
It serves as the Algorist website and as a platform for hosting demonstration applications.

The project also showcases practical software solutions and technical capabilities through demonstration applications.
It provides an overview of the platform's architecture and implementation.

This repository reflects a professional, full-stack .NET development approach.
It combines a modern Blazor user interface with reusable application components.
It is also intended as a portfolio of the skills and solutions available for software development and contractor engagements.

Some architectural decisions in this project prioritize simplicity, clarity, and ease of maintenance for a small, demonstration-focused system.
These choices are appropriate for the platform's purpose: presenting the company and example applications without introducing unnecessary complexity.
Production applications with larger teams, higher traffic, stricter availability requirements, or more complex business domains would require a more robust architecture.
Depending on the context, that could include stronger separation of responsibilities, dedicated infrastructure and deployment strategies, resilient integrations, comprehensive observability, advanced security controls, and scalability mechanisms.
This distinction is intentional: the project demonstrates practical development capabilities while recognizing that architecture should be adapted to the requirements, risks, and scale of each real-world solution.


# Demonstration Applications


## MCP Server

The MCP Server is a demonstration application that provides a set of tools to A.I. agents.


### Provided Demonstration Tools


#### GetServerInfo

Returns information about the Algorist Server.


#### GenerateAlgoristHashFromWord

Generates a hash (SHA256) from an input word joined with "_algorist".

The result could be checked in https://emn178.github.io/online-tools/sha256.html


#### ConvertTemperature

Converts temperature values between Kelvin, Celsius and Fahrenheit.

