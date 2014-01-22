using System;

namespace VimeoDotNet.Models
{
    public class UserMetadata
    {
        public UserConnections connections { get; set; }
        public UserInteractions interactions { get; set; }
        public Follower follower { get; set; }
    }
}
