namespace VimeoDotNet.Constants
{
    /// <summary>
    /// Class Endpoints.
    /// </summary>
    internal static class Endpoints
    {
        /// <summary>
        /// The authorize
        /// </summary>
        public const string Authorize = "/oauth/authorize";
        /// <summary>
        /// The access token
        /// </summary>
        public const string AccessToken = "/oauth/access_token";
        /// <summary>
        /// The verify
        /// </summary>
        public const string Verify = "/oauth/verify";
        /// <summary>
        /// The authorize client
        /// </summary>
        public const string AuthorizeClient = "/oauth/authorize/client";

        /// <summary>
        /// The albums
        /// </summary>
        public const string Albums = "/albums";
        /// <summary>
        /// The album
        /// </summary>
        public const string Album = "/albums/{albumId}";
        /// <summary>
        /// The album videos
        /// </summary>
        public const string AlbumVideos = "/albums/{albumId}/videos";
        /// <summary>
        /// The album video
        /// </summary>
        public const string AlbumVideo = "/albums/{albumId}/videos/{clipId}";

        /// <summary>
        /// The categories
        /// </summary>
        public const string Categories = "/categories";
        /// <summary>
        /// The category
        /// </summary>
        public const string Category = "/categories/{category}";
        /// <summary>
        /// The category channels
        /// </summary>
        public const string CategoryChannels = "/categories/{category}/channels";
        /// <summary>
        /// The category groups
        /// </summary>
        public const string CategoryGroups = "/categories/{category}/groups";
        /// <summary>
        /// The category users
        /// </summary>
        public const string CategoryUsers = "/categories/{category}/users";
        /// <summary>
        /// The category videos
        /// </summary>
        public const string CategoryVideos = "/categories/{category}/videos";

        /// <summary>
        /// The channels
        /// </summary>
        public const string Channels = "/channels";
        /// <summary>
        /// The channel
        /// </summary>
        public const string Channel = "/channels/{channelId}";
        /// <summary>
        /// The channel videos
        /// </summary>
        public const string ChannelVideos = "/channels/{channelId}/videos";
        /// <summary>
        /// The channel video
        /// </summary>
        public const string ChannelVideo = "/channels/{channelId}/videos/{clipId}";
        /// <summary>
        /// The channel users
        /// </summary>
        public const string ChannelUsers = "/channels/{channelId}/users";

        /// <summary>
        /// The groups
        /// </summary>
        public const string Groups = "/groups";
        /// <summary>
        /// The group
        /// </summary>
        public const string Group = "/groups/{groupId}";
        /// <summary>
        /// The group videos
        /// </summary>
        public const string GroupVideos = "/groups/{groupId}/videos";
        /// <summary>
        /// The group video
        /// </summary>
        public const string GroupVideo = "/groups/{groupId}/videos/{clipId}";
        /// <summary>
        /// The group users
        /// </summary>
        public const string GroupUsers = "/groups/{groupId}/users";

        /// <summary>
        /// The upload ticket
        /// </summary>
        public const string UploadTicket = "/me/videos";
        /// <summary>
        /// The upload ticket replace
        /// </summary>
        public const string UploadTicketReplace = "/me/videos/{clipId}";
        /// <summary>
        /// The upload ticket status
        /// </summary>
        public const string UploadTicketStatus = "/uploadtickets/{ticketId}";

        /// <summary>
        /// The users
        /// </summary>
        public const string Users = "/users";
        /// <summary>
        /// The user
        /// </summary>
        public const string User = "/users/{userId}";
        /// <summary>
        /// The user activities
        /// </summary>
        public const string UserActivities = "/users/{userId}/activities";
        /// <summary>
        /// Me albums
        /// </summary>
        public const string MeAlbums = "/me/albums";
        /// <summary>
        /// The user albums
        /// </summary>
        public const string UserAlbums = "/users/{userId}/albums";
        /// <summary>
        /// The user channels
        /// </summary>
        public const string UserChannels = "/users/{userId}/channels";
        /// <summary>
        /// Me channels
        /// </summary>
        public const string MeChannels = "/me/channels";
        /// <summary>
        /// Me album
        /// </summary>
        public const string MeAlbum = "/me/albums/{albumId}";
        /// <summary>
        /// Me album video
        /// </summary>
        public const string MeAlbumVideo = "/me/albums/{albumId}/videos/{clipId}";
        /// <summary>
        /// Me album videos
        /// </summary>
        public const string MeAlbumVideos = "/me/albums/{albumId}/videos";
        /// <summary>
        /// The user album
        /// </summary>
        public const string UserAlbum = "/users/{userId}/albums/{albumId}";
        /// <summary>
        /// The user album videos
        /// </summary>
        public const string UserAlbumVideos = "/users/{userId}/albums/{albumId}/videos";
        /// <summary>
        /// The user album video
        /// </summary>
        public const string UserAlbumVideo = "/users/{userId}/albums/{albumId}/videos/{clipId}";
        /// <summary>
        /// The user appearances
        /// </summary>
        public const string UserAppearances = "/users/{userId}/appearances";
        /// <summary>
        /// The user channel subscriptions
        /// </summary>
        public const string UserChannelSubscriptions = "/users/{userId}/channels";
        /// <summary>
        /// The user channel subscription
        /// </summary>
        public const string UserChannelSubscription = "/users/{userId}/channels/{channelId}";
        /// <summary>
        /// The user group memberships
        /// </summary>
        public const string UserGroupMemberships = "/users/{userId}/groups";
        /// <summary>
        /// The user group membership
        /// </summary>
        public const string UserGroupMembership = "/users/{userId}/groups/{groupId}";
        /// <summary>
        /// The user feed
        /// </summary>
        public const string UserFeed = "/users/{userId}/feed";
        /// <summary>
        /// The user folders
        /// </summary>
        public const string UserFolders = "/users/{userId}/folders";
        /// <summary>
        /// The user folder
        /// </summary>
        public const string UserFolder = "/users/{userId}/projects/{folderId}";
        /// <summary>
        /// The user followers
        /// </summary>
        public const string UserFollowers = "/users/{userId}/followers";
        /// <summary>
        /// The user following
        /// </summary>
        public const string UserFollowing = "/users/{userId}/following";
        /// <summary>
        /// The user follow
        /// </summary>
        public const string UserFollow = "/users/{userId}/following/{followingUserId}";
        /// <summary>
        /// The user likes
        /// </summary>
        public const string UserLikes = "/users/{userId}/likes";
        /// <summary>
        /// The user like
        /// </summary>
        public const string UserLike = "/users/{userId}/like/{clipId}";
        /// <summary>
        /// The user presets
        /// </summary>
        public const string UserPresets = "/users/{userId}/presets";
        /// <summary>
        /// The user preset
        /// </summary>
        public const string UserPreset = "/users/{userId}/presets/{presetId}";
        /// <summary>
        /// The user videos
        /// </summary>
        public const string UserVideos = "/users/{userId}/videos";
        /// <summary>
        /// The user video
        /// </summary>
        public const string UserVideo = "/users/{userId}/videos/{clipId}";
        /// <summary>
        /// The user portfolios
        /// </summary>
        public const string UserPortfolios = "/users/{userId}/portfolios";
        /// <summary>
        /// The user portfolio
        /// </summary>
        public const string UserPortfolio = "/users/{userId}/portfolios/{portfolioId}";
        /// <summary>
        /// The user portfolio videos
        /// </summary>
        public const string UserPortfolioVideos = "/users/{userId}/portfolios/{portfolioId}/videos";
        /// <summary>
        /// The user portfolio video
        /// </summary>
        public const string UserPortfolioVideo = "/users/{userId}/portfolios/{portfolioId}/videos/{clipId}";
        /// <summary>
        /// The user upload ticket
        /// </summary>
        public const string UserUploadTicket = "/users/{userId}/tickets/{ticket}";

        /// <summary>
        /// The videos
        /// </summary>
        public const string Videos = "/videos";
        /// <summary>
        /// The video
        /// </summary>
        public const string Video = "/videos/{clipId}";
        /// <summary>
        /// The video comments
        /// </summary>
        public const string VideoComments = "/videos/{clipId}/comments";
        /// <summary>
        /// The video comment
        /// </summary>
        public const string VideoComment = "/videos/{clipId}/comments/{commentId}";
        /// <summary>
        /// The video credits
        /// </summary>
        public const string VideoCredits = "/videos/{clipId}/credits";
        /// <summary>
        /// The video credit
        /// </summary>
        public const string VideoCredit = "/videos/{clipId}/credits/{creditId}";
        /// <summary>
        /// The video likes
        /// </summary>
        public const string VideoLikes = "/videos/{clipId}/likes";
        /// <summary>
        /// The video preset
        /// </summary>
        public const string VideoPreset = "/videos/{clipId}/presets/{presetId}";
        /// <summary>
        /// The video tags
        /// </summary>
        public const string VideoTags = "/videos/{clipId}/tags";
        /// <summary>
        /// The video tag
        /// </summary>
        public const string VideoTag = "/videos/{clipId}/tags/{tagId}";
        /// <summary>
        /// The videos by tag
        /// </summary>
        public const string VideosByTag = "/tags/{tagId}/videos";
        /// <summary>
        /// The video allowed users
        /// </summary>
        public const string VideoAllowedUsers = "/videos/{clipId}/privacy/users";
        /// <summary>
        /// The video allowed user
        /// </summary>
        public const string VideoAllowedUser = "/videos/{clipId}/privacy/users/{userId}";
        /// <summary>
        /// The video allowed domains
        /// </summary>
        public const string VideoAllowedDomains = "/videos/{clipId}/privacy/domains";
        /// <summary>
        /// The video allowed domain
        /// </summary>
        public const string VideoAllowedDomain = "/videos/{clipId}/privacy/domains/{domain}";
        /// <summary>
        /// The video related videos
        /// </summary>
        public const string VideoRelatedVideos = "/videos/{clipId}/related";
        /// <summary>
        /// The video replace file
        /// </summary>
        public const string VideoReplaceFile = "/videos/{clipId}/files";
        /// <summary>
        /// The video versions
        /// </summary>
        public const string VideoVersions = "/videos/{clipId}/versions";

        /// <summary>
        /// The text tracks
        /// </summary>
        public const string TextTracks = "/videos/{clipId}/texttracks/";
        /// <summary>
        /// The text track
        /// </summary>
        public const string TextTrack = "/videos/{clipId}/texttracks/{trackId}";

        /// <summary>
        /// The picture
        /// </summary>
        public const string Picture = "/videos/{clipId}/pictures/{pictureId}";
        /// <summary>
        /// The pictures
        /// </summary>
        public const string Pictures = "/videos/{clipId}/pictures";

        /// <summary>
        /// The tag
        /// </summary>
        public const string Tag = "/tags/{tagId}";

        /// <summary>
        /// Me project video
        /// </summary>
        public const string MeProjectVideo = "/me/projects/{projectId}/videos/{clipId}";
        /// <summary>
        /// The project video
        /// </summary>
        public const string ProjectVideo = "/users/{userId}/projects/{projectId}/videos/{clipId}";
        /// <summary>
        /// The project videos
        /// </summary>
        public const string ProjectVideos = "/users/{userId}/projects/{projectId}/videos";
        /// <summary>
        /// The project items
        /// </summary>
        public const string ProjectItems = "/users/{userId}/projects/{projectId}/items";

        /// <summary>
        /// The thumbnail
        /// </summary>
        public const string Thumbnail = "/videos/{clipId}/pictures/{pictureId}";

        /// <summary>
        /// Gets the current user endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>System.String.</returns>
        public static string GetCurrentUserEndpoint(string endpoint)
        {
            return endpoint.Replace("users/{userId}", "me");
        }
    }
}
