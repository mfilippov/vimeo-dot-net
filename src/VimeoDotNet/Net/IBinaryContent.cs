using System.IO;
using System.Threading.Tasks;

namespace VimeoDotNet.Net
{
    public interface IBinaryContent
    {
        string ContentType { get; set; }
        Stream Data { get; set; }
        string OriginalFileName { get; set; }
        byte[] Read(int startIndex, int endIndex);
        byte[] ReadAll();
        Task<byte[]> ReadAllAsync();
        Task<byte[]> ReadAsync(long startIndex, long endIndex);
    }
}