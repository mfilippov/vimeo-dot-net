using System;

namespace VimeoDotNet.Net
{
    public interface IBinaryContent
    {
        string ContentType { get; set; }
        System.IO.Stream Data { get; set; }
        string OriginalFileName { get; set; }
        byte[] Read(int startIndex, int endIndex);
        byte[] ReadAll();
        System.Threading.Tasks.Task<byte[]> ReadAllAsync();
        System.Threading.Tasks.Task<byte[]> ReadAsync(long startIndex, long endIndex);
    }
}
