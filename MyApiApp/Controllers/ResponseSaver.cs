using System.Text.Json;

namespace MyApiApp.Controllers
{
    /// <summary>
    /// user to save responses
    /// </summary>
    public static class ResponseSaver
    {
        /// <summary>
        /// used to save response into files
        /// </summary>
        /// <param name="response"></param>
        /// <param name="endpointName"></param>
        /// <param name="httpOperationName"></param>
        /// <returns></returns>
        public static async Task SaveResponseAsync(object response, string endpointName, string httpOperationName)
        {
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "ApiResponseFiles");
            Directory.CreateDirectory(directoryPath);

            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            string fileName = $"{endpointName.ToLower()}_{httpOperationName.ToLower()}_{timestamp}.json";
            string filePath = Path.Combine(directoryPath, fileName);
            string jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });

            await File.WriteAllTextAsync(filePath, jsonResponse);
        }
    }
}
