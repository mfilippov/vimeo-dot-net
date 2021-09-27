namespace VimeoDotNet.Constants
{
    internal static class Endpoints
    {
        public const string Authorize = "/oauth/authorize";
        public const string AccessToken = "/oauth/access_token";
        public const string Verify = "/oauth/verify";
        public const string AuthorizeClient = "/oauth/authorize/client";

        public const string Albums = "/albums";
        public const string Album = "/albums/{albumId}";
        public const string AlbumVideos = "/albums/{albumId}/videos";
        public const string AlbumVideo = "/albums/{albumId}/videos/{clipId}";

        public const string Categories = "/categories";
        public const string Category = "/categories/{category}";
        public const string CategoryChannels = "/categories/{category}/channels";
        public const string CategoryGroups = "/categories/{category}/groups";
        public const string CategoryUsers = "/categories/{category}/users";
        public const string CategoryVideos = "/categories/{category}/videos";

        public const string Channels = "/channels";
        public const string Channel = "/channels/{channelId}";
        public const string ChannelVideos = "/channels/{channelId}/videos";
        public const string ChannelVideo = "/channels/{channelId}/videos/{clipId}";
        public const string ChannelUsers = "/channels/{channelId}/users";

        public const string Groups = "/groups";
        public const string Group = "/groups/{groupId}";
        public const string GroupVideos = "/groups/{groupId}/videos";
        public const string GroupVideo = "/groups/{groupId}/videos/{clipId}";
        public const string GroupUsers = "/groups/{groupId}/users";

        public const string UploadTicket = "/me/videos";
        public const string UploadTicketReplace = "/me/videos/{clipId}";
        public const string UploadTicketStatus = "/uploadtickets/{ticketId}";

        public const string Users = "/users";
        public const string User = "/users/{userId}";
        public const string UserActivities = "/users/{userId}/activities";
        public const string MeAlbums = "/me/albums";
        public const string UserAlbums = "/users/{userId}/albums";
        public const string UserChannels = "/users/{userId}/channels";
        public const string MeAlbum = "/me/albums/{albumId}";
        public const string UserAlbum = "/users/{userId}/albums/{albumId}";
        public const string UserAlbumVideos = "/users/{userId}/albums/{albumId}/videos";
        public const string UserAlbumVideo = "/users/{userId}/albums/{albumId}/videos/{clipId}";
        public const string UserAppearances = "/users/{userId}/appearances";
        public const string UserChannelSubscriptions = "/users/{userId}/channels";
        public const string UserChannelSubscription = "/users/{userId}/channels/{channelId}";
        public const string UserGroupMemberships = "/users/{userId}/groups";
        public const string UserGroupMembership = "/users/{userId}/groups/{groupId}";
        public const string UserFeed = "/users/{userId}/feed";
        public const string UserFolders = "/users/{userId}/projects";
        public const string UserFollowers = "/users/{userId}/followers";
        public const string UserFollowing = "/users/{userId}/following";
        public const string UserFollow = "/users/{userId}/following/{followingUserId}";
        public const string UserLikes = "/users/{userId}/likes";
        public const string UserLike = "/users/{userId}/like/{clipId}";
        public const string UserPresets = "/users/{userId}/presets";
        public const string UserPreset = "/users/{userId}/presets/{presetId}";
        public const string UserVideos = "/users/{userId}/videos";
        public const string UserVideo = "/users/{userId}/videos/{clipId}";
        public const string UserPortfolios = "/users/{userId}/portfolios";
        public const string UserPortfolio = "/users/{userId}/portfolios/{portfolioId}";
        public const string UserPortfolioVideos = "/users/{userId}/portfolios/{portfolioId}/videos";
        public const string UserPortfolioVideo = "/users/{userId}/portfolios/{portfolioId}/videos/{clipId}";
        public const string UserUploadTicket = "/users/{userId}/tickets/{ticket}";

        public const string Videos = "/videos";
        public const string Video = "/videos/{clipId}";
        public const string VideoComments = "/videos/{clipId}/comments";
        public const string VideoComment = "/videos/{clipId}/comments/{commentId}";
        public const string VideoCredits = "/videos/{clipId}/credits";
        public const string VideoCredit = "/videos/{clipId}/credits/{creditId}";
        public const string VideoLikes = "/videos/{clipId}/likes";
        public const string VideoPreset = "/videos/{clipId}/presets/{presetId}";
        public const string VideoTags = "/videos/{clipId}/tags";
        public const string VideoTag = "/videos/{clipId}/tags/{tagId}";
        public const string VideosByTag = "/tags/{tagId}/videos";
        public const string VideoAllowedUsers = "/videos/{clipId}/privacy/users";
        public const string VideoAllowedUser = "/videos/{clipId}/privacy/users/{userId}";
        public const string VideoAllowedDomains = "/videos/{clipId}/privacy/domains";
        public const string VideoAllowedDomain = "/videos/{clipId}/privacy/domains/{domain}";
        public const string VideoRelatedVideos = "/videos/{clipId}/related";
        public const string VideoReplaceFile = "/videos/{clipId}/files";
        public const string VideoVersions = "/videos/{clipId}/versions?fields=upload";

        public const string TextTracks = "/videos/{clipId}/texttracks/";
        public const string TextTrack = "/videos/{clipId}/texttracks/{trackId}";

        public const string Picture = "/videos/{clipId}/pictures/{pictureId}";
        public const string Pictures = "/videos/{clipId}/pictures";

        public const string Tag = "/tags/{tagId}";

        public const string MeProjectVideo = "/me/projects/{projectId}/videos/{clipId}";
        public const string ProjectVideo = "/users/{userId}/projects/{projectId}/videos/{clipId}";
        public const string ProjectVideos = "/users/{userId}/projects/{projectId}/videos";

        public static string GetCurrentUserEndpoint(string endpoint)
        {
            return endpoint.Replace("users/{userId}", "me");
        }
    }
}
