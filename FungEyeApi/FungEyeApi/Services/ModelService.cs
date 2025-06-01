using FungEyeApi.Data;
using FungEyeApi.Interfaces;
using Newtonsoft.Json;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace FungEyeApi.Services
{
    public class ModelService : IModelService
    {
        private readonly DataContext db;
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public ModelService(DataContext db, IConfiguration configuration)
        {
            this.db = db;
            this.httpClient = new HttpClient();
            this.configuration = configuration;
        }

        public async Task<List<(string, double)>> Predict(IFormFile file)
        {
            try
            {
                var imageArray = await PreprocessImage(file);
                var predictions = await GetPredictions(imageArray);
                var predictionList = await ParsePredictions(predictions);

                return predictionList;
            }
            catch
            {
                throw;
            }
        }

        private static async Task<float[,,]> PreprocessImage(IFormFile file)
        {
            // Preprocess the image
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            // Load the image using ImageSharp
            memoryStream.Seek(0, SeekOrigin.Begin);
            using var image = SixLabors.ImageSharp.Image.Load<Rgb24>(memoryStream);

            // Resize the image to 299x299
            image.Mutate(x => x.Resize(299, 299));

            // Normalize the image data to [0, 1] and reshape it into [1, 299, 299, 3]
            float[,,] imageArray = new float[299, 299, 3]; // Batch of 1 image, 299x299 size, 3 channels (RGB)

            for (int i = 0; i < 299; i++)
            {
                for (int j = 0; j < 299; j++)
                {
                    var pixel = image[i, j];
                    imageArray[i, j, 0] = pixel.R / 255.0f; // Red channel
                    imageArray[i, j, 1] = pixel.G / 255.0f; // Green channel
                    imageArray[i, j, 2] = pixel.B / 255.0f; // Blue channel
                }
            }

            return imageArray;
        }

        private async Task<dynamic> GetPredictions(float[,,] imageArray)
        {
            try
            {
                var batchJson = new
                {
                    signature_name = "serving_default",
                    instances = new[] { imageArray }
                };

                var endpoint = configuration["ModelEndpoint"];
                var response = await httpClient.PostAsync(endpoint, new StringContent(JsonConvert.SerializeObject(batchJson), System.Text.Encoding.UTF8, "application/json"));

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                var predictions = JsonConvert.DeserializeObject<dynamic>(responseContent)?.predictions ?? throw new InvalidOperationException("Predictions of AI model could not be retrieved from the response.");

                return predictions;
            }
            catch
            {
                throw;
            }

        }

        private static async Task<List<(string, double)>> ParsePredictions(dynamic predictions)
        {
            try
            {
                var predictionList = new List<(string, double)>();

                using (var fileReader = new StreamReader(@"Resources/mushroom_names.txt"))
                {
                    var classNames = (await fileReader.ReadToEndAsync()).Split('\n').Select(x => x.Trim()).ToList();
                    for (int i = 0; i < predictions[0].Count; i++)
                    {
                        predictionList.Add((classNames[i], (double)predictions[0][i]));
                    }
                }

                predictionList.Sort((x, y) => y.Item2.CompareTo(x.Item2));
                return predictionList.Take(5).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error during parsing predictions: " + ex.Message);
            }
        }
    }
}
