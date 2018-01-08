using JetBrains.Annotations;

namespace VimeoDotNet.Models
{
    public class UserId
    {
        [PublicAPI]
        public long Id { get; }

        public static UserId Me => -1;

        public bool IsMe => Id == -1;

        private UserId(long id)
        {
            Id = id;
        }

        public static implicit operator UserId(long id)
        {
            return new UserId(id);
        }

        public override string ToString()
        {
            return IsMe ? "me" : Id.ToString();
        }
    }
}