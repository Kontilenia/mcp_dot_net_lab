---
name: petstore-swagger-api
description: "Implement or modify Petstore-related APIs, clients, integrations, SDKs, tests, or examples that call the Swagger Petstore service. Use when: a request mentions petstore, Swagger Petstore, pets, pet inventory, store orders, or Petstore users. Always inspect the live Swagger v2 specification before writing code and use only endpoints and schemas defined there."
---

# Swagger Petstore API

Build Petstore integrations against the operations currently published by the
Swagger Petstore API.

## Required Workflow

1. Before designing or writing code, retrieve
   `https://petstore.swagger.io/v2/swagger.json`.
2. Confirm the response is a valid Swagger document and inspect:
   - `schemes`, `host`, and `basePath`
   - `paths` and HTTP methods relevant to the request
   - operation parameters, request bodies, response codes, and response schemas
   - referenced definitions, serialization formats, and security requirements
3. Select existing operations that satisfy the request. Do not invent routes,
   methods, parameters, fields, or response shapes that are absent from the
   retrieved specification.
4. If the document cannot be retrieved or parsed, stop and report the error
   explicitly. Do not implement from memory or silently use a substitute API.
5. Implement code that calls the Petstore service described by the retrieved
   document. Derive the base URL from the specification, preferring HTTPS when
   the specification supports it.
6. Follow the repository's existing HTTP-client, dependency-injection,
   configuration, error-handling, logging, and testing patterns.
7. Keep the remote API base URL configurable unless the repository explicitly
   requires a fixed value.
8. Handle every response status needed by the selected operations. Do not
   deserialize error responses as successful payloads.
9. Add or update focused tests. Mock the HTTP boundary for automated tests
   unless the user explicitly requests live integration tests.
10. Verify that request paths, methods, parameters, payloads, and expected
    responses match the live Swagger document before finishing.

## Implementation Notes

- Treat the retrieved Swagger document as the source of truth, even if it
  differs from prior knowledge or existing examples.
- Reuse generated or existing models and clients when they accurately match the
  current specification.
- Preserve exact parameter locations (`path`, `query`, `header`, `formData`, or
  `body`) and required/optional semantics.
- Respect declared content types and authentication requirements.
- Implement only the operations needed for the user's request unless broader
  client generation is explicitly requested.
