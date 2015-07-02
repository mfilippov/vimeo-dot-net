vimeo-dot-net
=============

A .NET 4.5 wrapper for Vimeo API v3.0. Provides asynchronous API operations.

[NuGet URL](https://www.nuget.org/packages/VimeoDotNet/)

COMPLETED
---------
- Account Authentication (OAuth2)
- Account information retrieval
- Account video and video list retrieval
- User information retrieval
- User video and video list retrieval
- Chunked video upload with retry capability
- Video metadata update

TODO
----
- Verify OAuth2 functionality for multi-user clients.
- *Everything else...*

REFERENCE
---------
[API 3 Guide](https://developer.vimeo.com/api/start)  
[API 3 Endpoints](https://developer.vimeo.com/api/endpoints)

Pushing to NuGet:  
`nuget pack VimeoDotNet.csproj -Prop Configuration=Release`  
`nuget push VimeoDotNet.version.nupkg`
