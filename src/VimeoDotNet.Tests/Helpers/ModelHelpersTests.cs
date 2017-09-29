using Shouldly;
using VimeoDotNet.Helpers;
using Xunit;

namespace VimeoDotNet.Tests.Helpers
{
    public class ModelHelpersTests
    {
        [Fact]
        public void ShouldCorrectlyParseId()
        {
            ModelHelpers.ParseModelUriId("/a/b/c/12345").ShouldBe(12345);
            ModelHelpers.ParseModelUriId("/a/b/c/12345:xyz").ShouldBe(12345);
        }
    }
}