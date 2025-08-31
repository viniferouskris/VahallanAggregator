// Create a new file: Services/Implementations/ImageDownloaderService.cs

using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Vahallan_Ingredient_Aggregator.Services.Interfaces;

namespace Vahallan_Ingredient_Aggregator.Services.Implementations
{
    public class ImageDownloaderService : IImageDownloaderService
    {
        private readonly ILogger<ImageDownloaderService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public ImageDownloaderService(
            ILogger<ImageDownloaderService> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<MemoryStream> DownloadImageAsync(string imageUrl)
        {
            _logger.LogInformation($"Starting download of image from: {imageUrl}");

            try
            {
                var client = _httpClientFactory.CreateClient("ImageDownloader");
                client.Timeout = TimeSpan.FromSeconds(30);

                // Use a memory stream to store the image data
                var memoryStream = new MemoryStream();

                // Download the image
                using (var response = await client.GetAsync(imageUrl, HttpCompletionOption.ResponseHeadersRead))
                {
                    // Check if the request was successful
                    response.EnsureSuccessStatusCode();

                    // Verify content type is an image
                    var contentType = response.Content.Headers.ContentType?.MediaType;
                    if (contentType == null || !contentType.StartsWith("image/"))
                    {
                        throw new InvalidOperationException($"URL does not contain an image. Content type: {contentType}");
                    }

                    // Copy the image data to our memory stream
                    await response.Content.CopyToAsync(memoryStream);

                    // Reset position to beginning
                    memoryStream.Position = 0;

                    _logger.LogInformation($"Successfully downloaded image: {imageUrl}, Size: {memoryStream.Length} bytes");
                    return memoryStream;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"HTTP error downloading image from {imageUrl}: {ex.Message}");
                throw;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, $"Timeout downloading image from {imageUrl}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error downloading image from {imageUrl}: {ex.Message}");
                throw;
            }
        }
    }
}