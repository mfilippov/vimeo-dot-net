using System;
using System.IO;
using System.Threading.Tasks;
using Shouldly;
using VimeoDotNet.Net;
using Xunit;

namespace VimeoDotNet.Tests
{
    public class BinaryContentTests : BaseTest
    {
        [Fact]
        public async Task ShouldCorrectlyReadPartOfFile()
        {
            using var file = new BinaryContent(GetFileFromEmbeddedResources(TestVideoFilePath), "video/mp4");
            (await file.ReadAsync(17, 20)).Length.ShouldBe(3);
            (await file.ReadAsync(17000, 17020)).Length.ShouldBe(20);
        }

        [Fact]
        public async Task ShouldCorrectlyDoubleRead()
        {
            using var file = new BinaryContent(GetFileFromEmbeddedResources(TestVideoFilePath), "video/mp4");
            (await file.ReadAllAsync()).Length.ShouldBe(5510872);
            (await file.ReadAllAsync()).Length.ShouldBe(5510872);
        }

        [Fact]
        public void ShouldFireExceptionWhenDisposedStreamAccess()
        {
            var file = new BinaryContent(GetFileFromEmbeddedResources(TestVideoFilePath), "video/mp4");
            file.Dispose();
            Should.Throw<ObjectDisposedException>(() => file.Dispose());
            Should.Throw<ObjectDisposedException>(() => file.Data.Length.ShouldBe(0));
            Should.ThrowAsync<ObjectDisposedException>(async () => await file.ReadAllAsync());
        }

        [Fact]
        public void ShouldFireExceptionWhenInvalidStreams()
        {
            var nonReadableFile = new BinaryContent(new NonReadableStream(), "video/mp4");
            var nonSeekableFile = new BinaryContent(new NonSeekableStream(), "video/mp4");
            Should.ThrowAsync<InvalidOperationException>(async () => await nonReadableFile.ReadAllAsync(),
                "Content should be a readable Stream");
            Should.ThrowAsync<InvalidOperationException>(async () => await nonSeekableFile.ReadAsync(10, 20),
                "Content cannot be advanced to the specified start index: 10");
        }

        private class NonReadableStream : Stream
        {
            public NonReadableStream()
            {
                CanSeek = false;
                CanWrite = false;
                Length = 0;
            }

            public override void Flush()
            {
                throw new NotImplementedException();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotImplementedException();
            }

            public override void SetLength(long value)
            {
                throw new NotImplementedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override bool CanRead => false;
            public override bool CanSeek { get; }
            public override bool CanWrite { get; }
            public override long Length { get; }
            public override long Position { get; set; }
        }

        private class NonSeekableStream : Stream
        {
            public NonSeekableStream()
            {
                CanWrite = false;
                Length = 0;
            }

            public override void Flush()
            {
                throw new NotImplementedException();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotImplementedException();
            }

            public override void SetLength(long value)
            {
                throw new NotImplementedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override bool CanRead => true;
            public override bool CanSeek => false;
            public override bool CanWrite { get; }
            public override long Length { get; }
            public override long Position { get; set; }
        }
    }
}