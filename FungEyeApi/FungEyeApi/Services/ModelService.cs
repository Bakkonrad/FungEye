using FungEyeApi.Data;
using FungEyeApi.Interfaces;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace FungEyeApi.Services
{
    public class ModelService : IModelService
    {
        private readonly DataContext db;
        private readonly HttpClient httpClient;

        public ModelService(DataContext db)
        {
            this.db = db;
            this.httpClient = new HttpClient();
        }

        public async Task<List<(string, double)>> Predict(IFormFile file)
        {
            try
            {
                Console.WriteLine("Starting image preprocessing...");

                // Preprocess the image
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                Console.WriteLine("Image copied to memory stream.");

                memoryStream.Seek(0, SeekOrigin.Begin);
                using var image = SixLabors.ImageSharp.Image.Load<Rgb24>(memoryStream);
                Console.WriteLine("Image loaded using ImageSharp.");

                // Resize the image to 224x224
                image.Mutate(x => x.Resize(224, 224));
                Console.WriteLine("Image resized to 224x224.");

                // Normalize the image data to [0, 1] and reshape it into [1, 224, 224, 3]
                float[,,] imageArray = new float[224, 224, 3]; // Batch of 1 image, 224x224 size, 3 channels (RGB)

                for (int i = 0; i < 224; i++)
                {
                    for (int j = 0; j < 224; j++)
                    {
                        var pixel = image[i, j];
                        imageArray[i, j, 0] = pixel.R / 255.0f; // Red channel
                        imageArray[i, j, 1] = pixel.G / 255.0f; // Green channel
                        imageArray[i, j, 2] = pixel.B / 255.0f; // Blue channel
                    }
                }
                Console.WriteLine("Image normalized and reshaped.");

                var batchJson = new
                {
                    signature_name = "serving_default",
                    instances = new[] { imageArray }
                };

                Console.WriteLine("Batch JSON created: " + JsonConvert.SerializeObject(batchJson));

                // Specify the endpoint and make the request
                var endpoint = "http://tfserving:8501/v1/models/inception:predict";
                Console.WriteLine("Sending request to endpoint: " + endpoint);

                var response = await httpClient.PostAsync(endpoint, new StringContent(JsonConvert.SerializeObject(batchJson), System.Text.Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Request failed. Status code: " + response.StatusCode);
                    Console.WriteLine("Response content: " + await response.Content.ReadAsStringAsync());
                }
                else
                {
                    Console.WriteLine("Request successful. Status code: " + response.StatusCode);
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response content: " + responseContent);

                var predictions = JsonConvert.DeserializeObject<dynamic>(responseContent).predictions;

                // Make the predictions more readable
                var predictionList = new List<(string, double)>();
                using (var fileReader = new StreamReader(@"Resources/mushroom_names.txt"))
                {
                    var classNames = fileReader.ReadToEnd().Split('\n').Select(x => x.Trim()).ToList();
                    for (int i = 0; i < predictions[0].Count; i++)
                    {
                        predictionList.Add((classNames[i], (double)predictions[0][i]));
                    }
                }

                // Sort the predictions by probability
                predictionList.Sort((x, y) => y.Item2.CompareTo(x.Item2));
                Console.WriteLine("Predictions sorted and top 5 selected.");

                return predictionList.Take(5).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                // Handle any exceptions
                return new List<(string, double)>();
            }
        }
    }
}