using Shouldly;
using VimeoDotNet.Helpers;
using Xunit;

namespace VimeoDotNet.Tests
{
    public class ModelHelpersTests
    {
        [Fact]
        public void ShouldCorrectlyParseId()
        {
            ModelHelpers.ParseModelUriId("/a/b/c/12345").ShouldBe(12345);
            ModelHelpers.ParseModelUriId("/a/b/c/12345:xyz").ShouldBe(12345);
            ModelHelpers.ParseModelUriId("https://player.vimeo.com/video/815922745?h=a3e90a211f").ShouldBe(815922745);
        }
    }
}