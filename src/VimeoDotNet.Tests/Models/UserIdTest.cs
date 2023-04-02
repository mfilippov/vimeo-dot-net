using Shouldly;
using VimeoDotNet.Models;
using Xunit;

namespace VimeoDotNet.Tests.Models
{
    public class UserIdTests
    {
        [Fact]
        public void UserIdWithSameIdShouldBeEquals()
        {
            UserId.Me.ShouldBe(UserId.Me);
            UserId.Me.IsMe.ShouldBeTrue();
        }
    }
}