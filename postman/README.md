# WIP v2 Postman collection

Import `Wip-v2-Auth.collection.json` into Postman and set the collection variables with safe local test values. The collection covers the stabilized authentication, profile, and permissions contracts. It contains no real credentials, tokens, production URLs, or workspace publication configuration.

Run Login first; its test script stores temporary access and refresh tokens in collection variables. Refresh rotates the refresh token. Logout revokes the current refresh session. Use a disposable local account for profile update requests.
