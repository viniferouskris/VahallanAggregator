using System.IO;
using System.Threading.Tasks;

namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Services.Interfaces
{
    public interface IImageDownloaderService
    {
        Task<MemoryStream> DownloadImageAsync(string imageUrl);
    }
}