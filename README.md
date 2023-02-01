## Overview
Proposes a way to implement Transient Fault Handling policies around MSAL calls to build resilient applications with Polly.
Implement retries for HTTP error codes 400-600 or intermittent errors caused by Azure Active Directory Pass-through Authentication.

## HttpClientFactory
A custom strongly-type HttpClient is created that implements `IMsalHttpClientFactory`.
Using this custom http client, during dependency injection registration, we can tail our own delegating handlers alongside Polly policies to intercept outgoing requests to MSAL.

## Caching
A custom IDistributedCache implementation is implemented to persist MSAL tokens to Redis,using `Initialize` from `Microsoft.Identity.Web.TokenCacheProviders`.

```csharp
var msalApiClient = sp.GetRequiredService<IMsalHttpClientFactory>();
var activeDirectoryTokenCache = sp.GetRequiredService<IActiveDirectoryTokenCache>();
var msalTokenCacheProvider = sp.GetRequiredService<IMsalTokenCacheProvider>();

var confidentialClientApplication = ConfidentialClientApplicationBuilder.Create(ClientId)
                                                                        .WithTenantId(TenantId)
                                                                        .WithAuthority(Authority)
                                                                        .WithClientSecret(ClientSecret)
                                                                        .WithHttpClientFactory(msalApiClient)
                                                                        .Build();

activeDirectoryTokenCache.EnableSerialization(confidentialClientApplication.AppTokenCache);
msalTokenCacheProvider.Initialize(confidentialClientApplication.AppTokenCache);
```
