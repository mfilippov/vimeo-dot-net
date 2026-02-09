# Vimeo.NET API Improvement Plan

Comprehensive plan for improving model completeness and API endpoint coverage in the vimeo-dot-net library. Each task is atomic, self-contained, and independently implementable.

**Source analysis:** Comparison of current C# models against JSON test fixtures in `src/VimeoDotNet.Tests/TestData/`, the Vimeo OpenAPI spec, go-vimeo client, and hexdocs Elixir client.

---

## Table of Contents

- [Priority 1: Critical Model Fixes](#priority-1-critical-model-fixes)
- [Priority 2: High-Impact Missing API Endpoints](#priority-2-high-impact-missing-api-endpoints)
- [Priority 3: Model Field Gaps (Core Models)](#priority-3-model-field-gaps-core-models)
- [Priority 4: Nested Model Expansion](#priority-4-nested-model-expansion)
- [Priority 5: Cross-Cutting Model Fixes](#priority-5-cross-cutting-model-fixes)
- [Priority 6: Lower-Priority API Endpoints](#priority-6-lower-priority-api-endpoints)
- [Priority 7: Type Consistency](#priority-7-type-consistency)

---

## Priority 1: Critical Model Fixes

### TASK-001: Restore Upload property in Video.cs

**File:** `src/VimeoDotNet/Models/Video.cs` (lines 356-363)

The `Upload` property is commented out even though the `Upload.cs` model exists and the API returns this object in every video response.

**Current state (commented out):**
```csharp
///// <summary>
///// Gets or sets the upload.
///// </summary>
///// <value>The upload.</value>
//[PublicAPI]
//[CanBeNull]
//[JsonProperty(PropertyName = "upload")]
//public Upload Upload { get; set; }
```

**Action:** Uncomment lines 356-363.

**JSON reference** (`TestData/Video/video-530969457.json`):
```json
"upload": {
    "status": "complete",
    "upload_link": null,
    "form": null,
    "complete_uri": null,
    "approach": null,
    "size": null,
    "redirect_url": null,
    "link": null
}
```

---

### TASK-002: Fix UserUploadQuota structural mismatch

**File:** `src/VimeoDotNet/Models/UserUploadQuota.cs`

The C# model has a `Resets` (int) property that no longer exists in the API. The API now returns `periodic` and `lifetime` objects instead.

**Current C# model:**
```csharp
public class UserUploadQuota
{
    public Space Space { get; set; }
    public int Resets { get; set; }        // <-- does not exist in API
    public UserQuota Quota { get; set; }
}
```

**Actual API response** (`TestData/User/user.json`):
```json
"upload_quota": {
    "space": {
        "free": 518777128,
        "max": 524288000,
        "used": 5510872,
        "showing": "periodic",
        "unit": "video_size"
    },
    "periodic": {
        "period": "week",
        "unit": "video_size",
        "free": 518777128,
        "max": 524288000,
        "used": 5510872,
        "reset_date": "2023-04-13T00:00:00-04:00"
    },
    "lifetime": {
        "unit": "video_size",
        "free": 5363198248,
        "max": 5368709120,
        "used": 5510872
    },
    "quota": { "hd": true, "sd": true }
}
```

**Action:**
1. Create `src/VimeoDotNet/Models/UploadQuotaPeriodic.cs`:
   - `Period` (string), `Unit` (string), `Free` (long), `Max` (long), `Used` (long), `ResetDate` (DateTime?)
2. Create `src/VimeoDotNet/Models/UploadQuotaLifetime.cs`:
   - `Unit` (string), `Free` (long), `Max` (long), `Used` (long)
3. Update `UserUploadQuota.cs`:
   - Remove `Resets` property
   - Add `Periodic` (UploadQuotaPeriodic) and `Lifetime` (UploadQuotaLifetime) properties

---

### TASK-003: Fix Folder.Id JsonProperty on computed property

**File:** `src/VimeoDotNet/Models/Folder.cs` (line 20-21)

The `Id` property has `[JsonProperty(PropertyName = "id")]` but it's a computed property with no setter (`=> ModelHelpers.ParseModelUriId(Uri)`). The `[JsonProperty]` attribute is misleading and causes silent deserialization failures if the API ever sends an `id` field.

**Current:**
```csharp
[JsonProperty(PropertyName = "id")]
public long? Id => ModelHelpers.ParseModelUriId(Uri);
```

**Action:** Remove `[JsonProperty(PropertyName = "id")]` from the computed `Id` property. This matches the pattern in `Video.cs` (line 29) where `Id` has no `[JsonProperty]`.

---

## Priority 2: High-Impact Missing API Endpoints

### TASK-010: Add Comments API

**Resource:** Video comments CRUD
**Endpoints:**
- `GET /videos/{video_id}/comments` - List comments
- `POST /videos/{video_id}/comments` - Add comment
- `GET /videos/{video_id}/comments/{comment_id}` - Get comment
- `PATCH /videos/{video_id}/comments/{comment_id}` - Edit comment
- `DELETE /videos/{video_id}/comments/{comment_id}` - Delete comment
- `GET /videos/{video_id}/comments/{comment_id}/replies` - List replies
- `POST /videos/{video_id}/comments/{comment_id}/replies` - Add reply

**Files to create/modify:**
- Create `src/VimeoDotNet/VimeoClient_Comments.cs` (partial class)
- Model `Comment.cs` already exists -- verify it has all fields
- Add endpoint constants to `src/VimeoDotNet/Constants/Endpoints.cs`
- Add methods to `src/VimeoDotNet/IVimeoClient.cs`

**Comment model fields** (from Vimeo API):
- `uri`, `type`, `text`, `created_on`, `resource_key`
- `user` (User), `metadata` (connections: replies)

---

### TASK-011: Add Likes API

**Resource:** Like/unlike videos, list liked videos
**Endpoints:**
- `GET /me/likes` - List videos the user has liked
- `GET /videos/{video_id}/likes` - List users who have liked a video
- `PUT /me/likes/{video_id}` - Like a video
- `DELETE /me/likes/{video_id}` - Unlike a video

**Files to create/modify:**
- Create `src/VimeoDotNet/VimeoClient_Likes.cs` (partial class)
- Add endpoint constants to `Endpoints.cs`
- Add methods to `IVimeoClient.cs`

**Note:** `VideoConnections.Likes` already exists to provide the count. This task adds actual CRUD operations.

---

### TASK-012: Add Followers/Following API

**Resource:** Follow/unfollow users, list followers/following
**Endpoints:**
- `GET /me/followers` - List followers
- `GET /me/following` - List followed users
- `PUT /me/following/{user_id}` - Follow a user
- `DELETE /me/following/{user_id}` - Unfollow a user
- `GET /users/{user_id}/followers` - List a user's followers
- `GET /users/{user_id}/following` - List users a user follows

**Files to create/modify:**
- Create `src/VimeoDotNet/VimeoClient_Followers.cs` (partial class)
- Add endpoint constants to `Endpoints.cs`
- Add methods to `IVimeoClient.cs`

---

## Priority 3: Model Field Gaps (Core Models)

### TASK-020: Add missing fields to User.cs

**File:** `src/VimeoDotNet/Models/User.cs`
**Test data:** `src/VimeoDotNet.Tests/TestData/User/user.json`

Add these properties that exist in the API response but are missing from the model:

| Property | C# Type | JSON field | JSON example |
|----------|---------|------------|--------------|
| `Gender` | `string` | `gender` | `"m"` |
| `ShortBio` | `string` | `short_bio` | `null` |
| `Capabilities` | `UserCapabilities` | `capabilities` | `{"hasLiveSubscription": false, ...}` |
| `LocationDetails` | `LocationDetails` | `location_details` | `{"formatted_address": "England", ...}` |
| `Skills` | `List<object>` | `skills` | `[]` |
| `AvailableForHire` | `bool` | `available_for_hire` | `false` |
| `CanWorkRemotely` | `bool` | `can_work_remotely` | `false` |
| `Preferences` | `UserPreferences` | `preferences` | `{"videos": {"rating": [...], "privacy": "anybody"}}` |
| `ResourceKey` | `string` | `resource_key` | `"30149738db924d..."` |

**New model classes to create:**

1. **`src/VimeoDotNet/Models/UserCapabilities.cs`:**
   ```
   HasLiveSubscription (bool), HasEnterpriseLihp (bool),
   HasSvvTimecodedComments (bool), HasSimplifiedEnterpriseAccount (bool)
   ```

2. **`src/VimeoDotNet/Models/LocationDetails.cs`:**
   ```
   FormattedAddress (string), Latitude (double?), Longitude (double?),
   City (string), State (string), Neighborhood (string),
   SubLocality (string), StateIsoCode (string),
   Country (string), CountryIsoCode (string)
   ```

3. **`src/VimeoDotNet/Models/UserPreferences.cs`:**
   ```
   Videos (UserVideoPreferences)
   ```
   Plus nested `UserVideoPreferences` with `Rating` (List<string>), `Privacy` (string).

---

### TASK-021: Add ~30 missing fields to Album.cs

**File:** `src/VimeoDotNet/Models/Album.cs`
**Test data:** `src/VimeoDotNet.Tests/TestData/Album/albums-115220313.json`

The Album model has the largest gap. Add these properties:

| Property | C# Type | JSON field |
|----------|---------|------------|
| `ModifiedTime` | `DateTime` | `modified_time` |
| `Sort` | `string` | `sort` |
| `Layout` | `string` | `layout` |
| `Theme` | `string` | `theme` |
| `BrandColor` | `string` | `brand_color` |
| `CustomLogo` | `object` | `custom_logo` |
| `ReviewMode` | `bool` | `review_mode` |
| `WebCustomLogo` | `bool` | `web_custom_logo` |
| `WebBrandColor` | `bool` | `web_brand_color` |
| `AllowDownloads` | `bool` | `allow_downloads` |
| `AllowContinuousPlay` | `bool` | `allow_continuous_play` |
| `AllowShare` | `bool` | `allow_share` |
| `HideNav` | `bool` | `hide_nav` |
| `UseCustomDomain` | `bool` | `use_custom_domain` |
| `Domain` | `string` | `domain` |
| `DomainCertificateState` | `string` | `domain_certificate_state` |
| `Url` | `string` | `url` |
| `HideVimeoLogo` | `bool?` | `hide_vimeo_logo` |
| `EmbedCustomLogo` | `object` | `embed_custom_logo` |
| `EmbedBrandColor` | `string` | `embed_brand_color` |
| `Autoplay` | `bool` | `autoplay` |
| `Loop` | `bool` | `loop` |
| `RokuProviderName` | `string` | `roku_provider_name` |
| `RokuLanguage` | `string` | `roku_language` |
| `RokuGenres` | `List<string>` | `roku_genres` |
| `ShareLink` | `string` | `share_link` |
| `HideUpcoming` | `bool` | `hide_upcoming` |
| `SeoTitle` | `string` | `seo_title` |
| `SeoDescription` | `string` | `seo_description` |
| `SeoAllowIndexed` | `bool` | `seo_allow_indexed` |
| `SeoKeywords` | `List<string>` | `seo_keywords` |
| `HideFromVimeo` | `bool` | `hide_from_vimeo` |
| `ResourceKey` | `string` | `resource_key` |

---

### TASK-022: Add missing fields and metadata to Folder.cs

**File:** `src/VimeoDotNet/Models/Folder.cs`
**Test data:** `src/VimeoDotNet.Tests/TestData/Folder/post-folder.json`

Add these missing properties:

| Property | C# Type | JSON field |
|----------|---------|------------|
| `LastUserActionEventDate` | `DateTime?` | `last_user_action_event_date` |
| `PinnedOn` | `DateTime?` | `pinned_on` |
| `IsPinned` | `bool` | `is_pinned` |
| `IsPrivateToUser` | `bool` | `is_private_to_user` |
| `AccessGrant` | `object` | `access_grant` |
| `Metadata` | `FolderMetadata` | `metadata` |

**New model classes to create:**

1. **`src/VimeoDotNet/Models/FolderMetadata.cs`:**
   - `Connections` (FolderConnections)
   - `Interactions` (FolderInteractions)

2. **`src/VimeoDotNet/Models/FolderConnections.cs`:**
   - `Items` (VideoConnectionsEntry), `Videos` (VideoConnectionsEntry), `Folders` (VideoConnectionsEntry), `AncestorPath` (List<object>)

3. **`src/VimeoDotNet/Models/FolderInteractions.cs`:**
   - `Edit`, `MoveVideo`, `UploadVideo`, `View`, `EditSettings`, `Delete`, `DeleteVideo`, `AddSubfolder` -- each as an `Interaction`-type object

**JSON reference** (`post-folder.json`):
```json
"metadata": {
    "connections": {
        "items": {"uri": "/users/2433258/projects/15721704/items", "options": ["GET"], "total": 0},
        "videos": {"uri": "...", "options": ["GET","DELETE","PUT"], "total": 0},
        "folders": {"uri": "...", "options": ["GET","DELETE","PUT"], "total": 0},
        "ancestor_path": []
    },
    "interactions": {
        "edit": {"uri": "...", "options": ["PATCH"]},
        "move_video": {"uri": "...", "options": ["PATCH"]},
        "upload_video": {"uri": "...", "options": ["POST"]},
        "view": {"uri": "...", "options": ["GET"]},
        "edit_settings": {"uri": "...", "options": ["PATCH"]},
        "delete": {"uri": "...", "options": ["DELETE"]},
        "delete_video": {"uri": "...", "options": ["DELETE"]},
        "add_subfolder": {"uri": "...", "options": ["POST"], "can_add_subfolders": true, ...}
    }
}
```

---

### TASK-023: Add missing fields to Video.cs

**File:** `src/VimeoDotNet/Models/Video.cs`
**Test data:** `src/VimeoDotNet.Tests/TestData/Video/video-530969457.json`

Add these properties that exist in the API response but are missing:

| Property | C# Type | JSON field | JSON example |
|----------|---------|------------|--------------|
| `Uploader` | `User` | `uploader` | Full user object with pictures |
| `Play` | `VideoPlay` | `play` | `{"status": "unavailable"}` |
| `App` | `VideoApp` | `app` | `{"name": "...", "uri": "..."}` |
| `ResourceKey` | `string` | `resource_key` | `"01c32c8..."` |
| `RatingModLocked` | `bool` | `rating_mod_locked` | `false` |

**New model classes:**
1. **`src/VimeoDotNet/Models/VideoPlay.cs`:** `Status` (string)
2. **`src/VimeoDotNet/Models/VideoApp.cs`:** `Name` (string), `Uri` (string)

---

## Priority 4: Nested Model Expansion

### TASK-030: Expand Embed.cs with player settings

**File:** `src/VimeoDotNet/Models/Embed.cs`

Currently has only `Uri` and `Html`. The API returns ~20 additional fields for player customization.

**Properties to add:**

| Property | C# Type | JSON field |
|----------|---------|------------|
| `Badges` | `EmbedBadges` | `badges` |
| `Buttons` | `EmbedButtons` | `buttons` |
| `Logos` | `EmbedLogos` | `logos` |
| `Title` | `EmbedTitle` | `title` |
| `EndScreen` | `List<object>` | `end_screen` |
| `Playbar` | `bool` | `playbar` |
| `Pip` | `bool` | `pip` |
| `Autopip` | `bool` | `autopip` |
| `Volume` | `bool` | `volume` |
| `Color` | `string` | `color` |
| `Colors` | `EmbedColors` | `colors` |
| `EventSchedule` | `bool` | `event_schedule` |
| `Interactive` | `bool` | `interactive` |
| `HasCards` | `bool` | `has_cards` |
| `OutroType` | `string` | `outro_type` |
| `Speed` | `bool` | `speed` |

**New model classes:**

1. **`EmbedBadges.cs`:** `Hdr` (bool), `Live` (object), `StaffPick` (object), `Vod` (bool), `WeekendChallenge` (bool)
2. **`EmbedButtons.cs`:** `Watchlater` (bool), `Share` (bool), `Embed` (bool), `Hd` (bool), `Fullscreen` (bool), `Scaling` (bool), `Like` (bool)
3. **`EmbedLogos.cs`:** `Vimeo` (bool), `Custom` (object)
4. **`EmbedTitle.cs`:** `Name` (string), `Owner` (string), `Portrait` (string)
5. **`EmbedColors.cs`:** `ColorOne` (string), `ColorTwo` (string), `ColorThree` (string), `ColorFour` (string)

---

### TASK-031: Expand VideoConnections.cs

**File:** `src/VimeoDotNet/Models/VideoConnections.cs`

Currently has 5 connections. Add these missing connections from the API:

| Property | C# Type | JSON field |
|----------|---------|------------|
| `Related` | `VideoConnectionsEntry` | `related` |
| `Albums` | `VideoConnectionsEntry` | `albums` |
| `AvailableAlbums` | `VideoConnectionsEntry` | `available_albums` |
| `AvailableChannels` | `VideoConnectionsEntry` | `available_channels` |
| `Versions` | `VideoConnectionsEntry` | `versions` |
| `Recommendations` | `VideoConnectionsEntry` | `recommendations` |
| `Publish` | `VideoConnectionsEntry` | `publish_to_social` |

**Note:** The `versions` connection has extra fields (`current_uri`, `resource_key`, `latest_incomplete_version`). Consider a specialized `VideoVersionsConnection` class or add nullable properties to `VideoConnectionsEntry`.

---

### TASK-032: Expand VideoInteractions.cs

**File:** `src/VimeoDotNet/Models/VideoInteractions.cs`

Currently has only `WatchLater` and `Rent`. Add these missing interactions:

| Property | C# Type | JSON field |
|----------|---------|------------|
| `Report` | `Interaction` | `report` |
| `ViewTeamMembers` | `Interaction` | `view_team_members` |
| `Edit` | `Interaction` | `edit` |
| `EditContentRating` | `Interaction` | `edit_content_rating` |
| `EditPrivacy` | `Interaction` | `edit_privacy` |
| `Delete` | `Interaction` | `delete` |
| `CanUpdatePrivacyToPublic` | `Interaction` | `can_update_privacy_to_public` |
| `Trim` | `Interaction` | `trim` |
| `Validate` | `Interaction` | `validate` |
| `Like` | `Interaction` | `like` |

---

### TASK-033: Expand UserConnections.cs

**File:** `src/VimeoDotNet/Models/UserConnections.cs`

Currently has 11 connections. Add these 10 missing connections from `TestData/User/user.json`:

| Property | C# Type | JSON field |
|----------|---------|------------|
| `Appearances` | `UserConnectionsEntry` | `appearances` |
| `Categories` | `UserConnectionsEntry` | `categories` |
| `Membership` | `UserConnectionsEntry` | `membership` |
| `ModeratedChannels` | `UserConnectionsEntry` | `moderated_channels` |
| `Shared` | `UserConnectionsEntry` | `shared` |
| `Pictures` | `UserConnectionsEntry` | `pictures` |
| `WatchedVideos` | `UserConnectionsEntry` | `watched_videos` |
| `FoldersRoot` | `UserConnectionsEntry` | `folders_root` |
| `Teams` | `UserConnectionsEntry` | `teams` |
| `Block` | `UserConnectionsEntry` | `block` |

---

### TASK-034: Add interactions to AlbumMetadata.cs

**File:** `src/VimeoDotNet/Models/AlbumMetadata.cs`

Currently only has `Connections`. The API also returns an `interactions` field (nullable).

**Action:** Add `Interactions` property (can be `AlbumInteractions` or a generic object initially).

---

### TASK-035: Fix VideoStats.cs outdated structure

**File:** `src/VimeoDotNet/Models/VideoStats.cs`

Currently has `Plays`, `Likes`, and `Comments`. The API now only returns `plays` in the `stats` object. The `likes` and `comments` counts have moved to `metadata.connections.likes.total` and `metadata.connections.comments.total`.

**Action:** Mark `Likes` and `Comments` as `[Obsolete]` with a message pointing to `VideoMetadata.Connections.Likes.Total` and `VideoMetadata.Connections.Comments.Total`.

---

## Priority 5: Cross-Cutting Model Fixes

### TASK-040: Add `default_picture` to Pictures.cs

**File:** `src/VimeoDotNet/Models/Pictures.cs`

Every `pictures` object in every JSON test file contains `"default_picture": false` but the model has no such property.

**Action:** Add:
```csharp
[PublicAPI]
[JsonProperty(PropertyName = "default_picture")]
public bool DefaultPicture { get; set; }
```

---

### TASK-041: Add `showing` to Space.cs

**File:** `src/VimeoDotNet/Models/Space.cs`

The JSON `upload_quota.space` object contains `"showing": "periodic"` but `Space.cs` has no `Showing` property.

**Action:** Add:
```csharp
[PublicAPI]
[JsonProperty(PropertyName = "showing")]
public string Showing { get; set; }
```

---

### TASK-042: Add `comments` to Privacy.cs

**File:** `src/VimeoDotNet/Models/Privacy.cs`

Video privacy objects return `"comments": "anybody"` but Privacy.cs only has `View`, `Embed`, `Download`, and `Add`.

**Action:** Add:
```csharp
[PublicAPI]
[JsonProperty(PropertyName = "comments")]
public string Comments { get; set; }
```

Optionally add a computed property with enum:
```csharp
[PublicAPI]
public VideoCommentsEnum CommentsPrivacy
{
    get => ModelHelpers.GetEnumValue<VideoCommentsEnum>(Comments);
    set => Comments = ModelHelpers.GetEnumString(value);
}
```

Note: `VideoCommentsEnum` already exists in `src/VimeoDotNet/Enums/VideoCommentsEnum.cs`.

---

## Priority 6: Lower-Priority API Endpoints

### TASK-050: Add Groups API

**Endpoints:**
- `GET /groups` - List groups
- `GET /groups/{group_id}` - Get group
- `POST /groups` - Create group
- `DELETE /groups/{group_id}` - Delete group
- `GET /groups/{group_id}/videos` - List group videos
- `PUT /groups/{group_id}/videos/{video_id}` - Add video to group
- `DELETE /groups/{group_id}/videos/{video_id}` - Remove video from group
- `GET /me/groups` - List user's groups
- `PUT /me/groups/{group_id}` - Join group
- `DELETE /me/groups/{group_id}` - Leave group

**Files:** Create `VimeoClient_Groups.cs`, add to `IVimeoClient.cs`, `Endpoints.cs`

---

### TASK-051: Add Categories API

**Endpoints:**
- `GET /categories` - List categories
- `GET /categories/{category}` - Get category
- `GET /categories/{category}/videos` - List category videos
- `GET /categories/{category}/channels` - List category channels
- `GET /categories/{category}/groups` - List category groups
- `GET /me/categories` - List followed categories
- `PUT /me/categories/{category}` - Follow category
- `DELETE /me/categories/{category}` - Unfollow category

**Files:** Create `VimeoClient_Categories.cs`, add to `IVimeoClient.cs`, `Endpoints.cs`

---

### TASK-052: Add Portfolios API

**Endpoints:**
- `GET /users/{user_id}/portfolios` - List portfolios
- `GET /users/{user_id}/portfolios/{portfolio_id}` - Get portfolio
- `GET /users/{user_id}/portfolios/{portfolio_id}/videos` - List portfolio videos
- `PUT /users/{user_id}/portfolios/{portfolio_id}/videos/{video_id}` - Add video
- `DELETE /users/{user_id}/portfolios/{portfolio_id}/videos/{video_id}` - Remove video

**Files:** Create `VimeoClient_Portfolios.cs`, add to `IVimeoClient.cs`, `Endpoints.cs`

---

### TASK-053: Add Video Credits API

**Endpoints:**
- `GET /videos/{video_id}/credits` - List credits
- `POST /videos/{video_id}/credits` - Add credit
- `GET /videos/{video_id}/credits/{credit_id}` - Get credit
- `PATCH /videos/{video_id}/credits/{credit_id}` - Edit credit
- `DELETE /videos/{video_id}/credits/{credit_id}` - Delete credit

**Files:** Create model `Credit.cs`, create `VimeoClient_Credits.cs`

---

### TASK-054: Add reference data APIs

**Endpoints:**
- `GET /contentratings` - List content ratings
- `GET /creativecommons` - List Creative Commons licenses
- `GET /languages` - List languages

**Files:** Add methods to `VimeoClient_Async.cs` or create `VimeoClient_Reference.cs`

---

### TASK-055: Add Channel Moderators API

**Endpoints:**
- `GET /channels/{channel_id}/moderators` - List moderators
- `PUT /channels/{channel_id}/moderators/{user_id}` - Add moderator
- `DELETE /channels/{channel_id}/moderators/{user_id}` - Remove moderator

**Files:** Add to `VimeoClient_Channels.cs`, update `IVimeoClient.cs`

---

### TASK-056: Add User Feed and Appearances API

**Endpoints:**
- `GET /me/feed` - Get user feed
- `GET /me/appearances` - List videos user appears in
- `GET /me/watched/videos` - List watched videos
- `DELETE /me/watched/videos` - Clear watch history
- `DELETE /me/watched/videos/{video_id}` - Remove from watch history

**Files:** Create `VimeoClient_Feed.cs` or add to `VimeoClient_Async.cs`

---

## Priority 7: Type Consistency

### TASK-060: Fix DateTime type for Album.CreatedTime

**File:** `src/VimeoDotNet/Models/Album.cs` (line 67)

**Current:** `public string CreatedTime { get; set; }`
**Expected:** `public DateTime CreatedTime { get; set; }`

JSON value: `"2020-05-11T12:44:34+00:00"` -- same ISO 8601 format as Video.CreatedTime which uses `DateTime`.

**Note:** This is a breaking change for consumers who access `CreatedTime` as a string. Consider adding the new DateTime property alongside the old string property (marked `[Obsolete]`).

---

### TASK-061: Fix DateTime type for Channel.CreatedTime and ModifiedTime

**File:** `src/VimeoDotNet/Models/Channel.cs` (lines 51, 59)

**Current:**
```csharp
public string CreatedTime { get; set; }
public string ModifiedTime { get; set; }
```

**Expected:** Both should be `DateTime` to match `Video.cs` and `Folder.cs` patterns.

JSON values: `"2023-04-02T16:49:45+00:00"` -- standard ISO 8601.

**Same breaking change consideration as TASK-060.**

---

## Summary

| Priority | Tasks | Category |
|----------|-------|----------|
| P1 | TASK-001 to TASK-003 | Critical fixes (bugs, broken deserialization) |
| P2 | TASK-010 to TASK-012 | High-impact missing API endpoints |
| P3 | TASK-020 to TASK-023 | Missing fields on core models |
| P4 | TASK-030 to TASK-035 | Nested model expansion |
| P5 | TASK-040 to TASK-042 | Cross-cutting model fixes |
| P6 | TASK-050 to TASK-056 | Lower-priority API endpoints |
| P7 | TASK-060 to TASK-061 | Type consistency (breaking changes) |

**Total: 28 tasks**

Each task is independently implementable. P1 tasks should be done first as they fix actual bugs. P2 tasks add the most commonly requested missing functionality. P3-P5 improve model fidelity. P6-P7 are nice-to-haves.
