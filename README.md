vimeo-dot-net
=============

A .NET 4.5 wrapper for Vimeo API v3.0. Provides asynchronous API operations.

[NuGet URL](https://www.nuget.org/packages/VimeoDotNet/)

[![Build status](https://ci.appveyor.com/api/projects/status/i2ojpb8i9o2v3kk4?svg=true)](https://ci.appveyor.com/project/mfilippov/vimeo-dot-net)

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
| Get info on an Album. | :heavy_check_mark: | `GetAlbumAsync()` |
| Edit an Album. | :heavy_check_mark:  | `UpdateAlbumAsync()`  |
| Delete an Album. |  :heavy_check_mark: |  `DeleteAlbumAsync` |
| Get the list of videos in an Album. | :heavy_check_mark:  | `GetAlbumVideosAsync()`  |
| Check if an Album contains a video. | :heavy_check_mark: | `GetAlbumVideoAsync()` |
| Add a video to an Album. | :heavy_check_mark: | `AddToAlbumAsync()` |
| Remove a video from an Album. | :heavy_check_mark: | `RemoveFromAlbumAsync()` |
| [/me/videos](https://developer.vimeo.com/api/endpoints/me#/videos)|Complete?|API Method|
| Get a list of videos uploaded by a user. | :heavy_check_mark: | `GetVideosAsync()` |
| Begin the video upload process. | :heavy_check_mark: | `GetUploadTicketAsync()` |
| Check if a user owns a clip. | :heavy_check_mark: | `GetUserVideo()` |




REFERENCE
---------
[API 3 Guide](https://developer.vimeo.com/api/start)  
[API 3 Endpoints](https://developer.vimeo.com/api/endpoints)

Pushing to NuGet:  
`nuget pack VimeoDotNet.csproj -Prop Configuration=Release`  
`nuget push VimeoDotNet.version.nupkg`
