vimeo-dot-net
=============

A .NET 4.5/.NET Standard 1.3 wrapper for Vimeo API v3.0. Provides asynchronous API operations.

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

API COVERAGE
----

| [/me/information](https://developer.vimeo.com/api/endpoints/me#)|Complete?|API Method|
|:---|:---:|:---|
| Get user information | :heavy_check_mark: | `GetAccountInformationAsync()` |
| Edit user information | :heavy_check_mark: | `UpdateAccountInformationAsync()` |
| Get a list of a user's Albums. | :heavy_check_mark:  | `GetAccountAlbumsAsync()`  |
| Create an Album. | :heavy_check_mark:  | `CreateAlbumAsync()`  |
| Get info on an Album. | :heavy_check_mark: | `GetAlbumAsync()` |
| Edit an Album. | :heavy_check_mark:  | `UpdateAlbumAsync()`  |
| Delete an Album. |  :heavy_check_mark: |  `DeleteAlbumAsync` |
| Get the list of videos in an Album. | :heavy_check_mark:  | `GetAlbumVideosAsync()`  |
| Check if an Album contains a video. | :heavy_check_mark: | `GetAlbumVideoAsync()` |
| Add a video to an Album. | :heavy_check_mark: | `AddToAlbumAsync()` |
| Remove a video from an Album. | :heavy_check_mark: | `RemoveFromAlbumAsync()` |
| Get a list of videos uploaded by a user. | :heavy_check_mark: | `GetVideosAsync()` |
| Begin the video upload process. | :heavy_check_mark: | `GetUploadTicketAsync()` |
| Check if a user owns a clip. | :heavy_check_mark: | `GetUserVideo()` |
| Get a all videos uploaded into the folder by user. | :heavy_check_mark: | `GetAllVideosFromFolderAsync()` |
| Delete a thumbnail. | :heavy_check_mark: | `DeleteThumbnailVideoAsync()` |




REFERENCE
---------
[API 3 Guide](https://developer.vimeo.com/api/start)  
[API 3 Endpoints](https://developer.vimeo.com/api/endpoints)

HOW TO AUTHENTICATE
-------------------
Video uploads and other secure operations to a user account require you to authenticate as your app and also authenticate the user you are managing.  This will require setting up an app in your Vimeo account via developer.vimeo.com, requesting upload permission, and waiting for permission approval.  Once you have your app set up and approved for uploads, you are ready to perform authentication.  Here is a simplified example of authenticating your app and then authenticating your user, giving you access to all of the features this library offers:
```C#
var clientId = "your_client_id_here";
var clientSecret = "your_client_secret_here";
// This URL needs to be added to your 
// callback url list on your app settings page in developer.vimeo.com.
var redirectionUrl = "https://your_website_here.com/wherever-you-send-users-after-grant";
// You can put state information here that gets sent
// to your callback url in the ?state= parameter
var stateInformation = "1337";
var client = new VimeoDotNet.VimeoClient(clientId, clientSecret);
var url = client.GetOauthUrl(redirectionUrl, new List<string>() 
  {
    "public",
    "private", 
    "purchased", 
    "create", 
    "edit", 
    "delete", 
    "interact", 
    "upload", 
    "promo_codes",
    "video_files"
    }, stateInformation);
// The user will use this URL to log in and allow access to your app.
// The web page will redirect to your redirection URL with the access code in the query parameters.
// If you are also the user, 
// you can just pull the code out of the URL yourself and use it right here.
Console.WriteLine(url);
Console.WriteLine("Give me your access code...");
var accessCode = Console.ReadLine();
var token = await client.GetAccessTokenAsync(accessCode, redirectionUrl);
//we need a new client now, if it is a one off job you can just
//you are now ready to upload or whatever using the userAuthenticatedClient
var userAuthenticatedClient = new VimeoDotNet.VimeoClient(token.AccessToken);
            
```
