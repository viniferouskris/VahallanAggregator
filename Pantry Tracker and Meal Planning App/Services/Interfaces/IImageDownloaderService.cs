using System.IO;
using System.Threading.Tasks;

namespace Vahallan_Ingredient_Aggregator.Services.Interfaces
{
    public interface IImageDownloaderService
    {
        Task<MemoryStream> DownloadImageAsync(string imageUrl);
    }
}