// Add this to a new file called Utilities/StreamToFormFileConverter.cs

using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Utilities
{
    public static class StreamToFormFileConverter
    {
        public static IFormFile ConvertToFormFile(Stream stream, string fileName, string contentType)
        {
            // Position stream at beginning
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            // Create a form file from the stream
            return new FormFile(
                stream,
                0,
                stream.Length,
                "FileFromStream", // name
                fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }

        // Create a reusable stream that can be read multiple times
        public static async Task<MemoryStream> CreateReusableStreamAsync(Stream inputStream)
        {
            var memoryStream = new MemoryStream();
            await inputStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}