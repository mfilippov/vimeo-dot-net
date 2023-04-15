using Shouldly;
using VimeoDotNet.Enums;
using VimeoDotNet.Parameters;
using Xunit;

namespace VimeoDotNet.Tests
{
    public class EnumExtensionsTests
    {
        [Fact]
        public void GetParameterValueTests()
        {
            EditAlbumPrivacyOption.Password.GetParameterValue().ShouldBe("password");
        }
    }
}