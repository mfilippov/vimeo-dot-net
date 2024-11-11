using JetBrains.Annotations;

namespace VimeoDotNet.Models
{
    /// <summary>
    /// Class UserId.
    /// </summary>
    public class UserId
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [PublicAPI]
        public long Id { get; }

        /// <summary>
        /// Gets me.
        /// </summary>
        /// <value>Me.</value>
        public static UserId Me => -1;

        /// <summary>
        /// Gets a value indicating whether this instance is me.
        /// </summary>
        /// <value><c>true</c> if this instance is me; otherwise, <c>false</c>.</value>
        public bool IsMe => Id == -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserId" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        private UserId(long id)
        {
            Id = id;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int64" /> to <see cref="UserId" />.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator UserId(long id)
        {
            return new UserId(id);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return IsMe ? "me" : Id.ToString();
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool Equals(UserId other)
        {
            return Id == other.Id;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((UserId) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Implements the == operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(UserId left, UserId right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Implements the != operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(UserId left, UserId right)
        {
            return !Equals(left, right);
        }
    }
}