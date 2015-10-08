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

API COVERAGE
----

| [/me/information](https://developer.vimeo.com/api/endpoints/me#)|Complete?|API Method|
|:---|:---:|:---|
| Get user information | :heavy_check_mark: | `GetAccountInformationAsync()` |
| Edit user information | :heavy_check_mark: | `UpdateAccountInformationAsync()` |
| [/me/albums](https://developer.vimeo.com/api/endpoints/me#/albums)|Complete?|API Method|
| Get a list of a user's Albums. | :heavy_check_mark:  | `GetAccountAlbumsAsync()`  |
| Create an Album. | :heavy_check_mark:  | `CreateAlbumAsync()`  |
| Get info on an Album. |   |   |
| Edit an Album. | :heavy_check_mark:  | `UpdateAlbumAsync()`  |
| Delete an Album. |  :heavy_check_mark: |  `DeleteAlbumAsync` |
| Get the list of videos in an Album. | :heavy_check_mark:  | `GetAlbumVideosAsync()`  |
| Check if an Album contains a video. |   |   |
| Add a video to an Album. |   |   |
| Remove a video from an Album. |   |   |



REFERENCE
---------
[API 3 Guide](https://developer.vimeo.com/api/start)  
[API 3 Endpoints](https://developer.vimeo.com/api/endpoints)

Pushing to NuGet:  
`nuget pack VimeoDotNet.csproj -Prop Configuration=Release`  
`nuget push VimeoDotNet.version.nupkg`
