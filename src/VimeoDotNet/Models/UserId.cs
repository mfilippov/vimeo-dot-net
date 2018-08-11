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

        private bool Equals(UserId other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((UserId) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(UserId left, UserId right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UserId left, UserId right)
        {
            return !Equals(left, right);
        }
    }
}