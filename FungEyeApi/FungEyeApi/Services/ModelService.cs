using FungEyeApi.Data;
using FungEyeApi.Interfaces;
using Newtonsoft.Json;
using System.Drawing;

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
                // Preprocess the image
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                // Resize the image to 224x224
                using var image = System.Drawing.Image.FromStream(memoryStream);
                using var resizedImage = new Bitmap(image, new Size(224, 224));

                // Normalize the image data to [0, 1] and reshape it into [1, 224, 224, 3]
                float[,,] imageArray = new float[224, 224, 3]; // Batch of 1 image, 224x224 size, 3 channels (RGB)

                for (int i = 0; i < 224; i++)
                {
                    for (int j = 0; j < 224; j++)
                    {
                        var pixel = resizedImage.GetPixel(i, j);
                        imageArray[i, j, 0] = pixel.R / 255.0f; // Red channel
                        imageArray[i, j, 1] = pixel.G / 255.0f; // Green channel
                        imageArray[i, j, 2] = pixel.B / 255.0f; // Blue channel
                    }
                }


                var batchJson = new
                {
                    signature_name = "serving_default",
                    instances = new[] { imageArray }
                };


                using var resizedStream = new MemoryStream();
                resizedImage.Save(resizedStream, System.Drawing.Imaging.ImageFormat.Jpeg); // Save in JPEG format
                var imageData = resizedStream.ToArray();

                // Normalize the image data to [0, 1]
                var imageList = new List<List<double>> { imageData.Select(b => (double)b / 255.0).ToList() };

                // Specify the endpoint and make the request
                var endpoint = "http://localhost:8501/v1/models/inception:predict";
                var headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
                //var batchJson = new { signature_name = "serving_default", instances = new[] { imageList } };

                var response = await httpClient.PostAsync(endpoint, new StringContent(JsonConvert.SerializeObject(batchJson), System.Text.Encoding.UTF8, "application/json"));

                var objec = JsonConvert.SerializeObject(batchJson).ToString();

                response.EnsureSuccessStatusCode(); // Ensure we throw if response is not successful

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response: {responseContent}");

                var predictions = JsonConvert.DeserializeObject<dynamic>(responseContent).predictions;

                // Make the predictions more readable
                var predictionList = new List<(string, double)>();
                using (var fileReader = new StreamReader(@"Resources\mushroom_names.txt"))
                {
                    var classNames = fileReader.ReadToEnd().Split('\n').Select(x => x.Trim()).ToList();
                    for (int i = 0; i < predictions[0].Count; i++)
                    {
                        predictionList.Add((classNames[i], (double)predictions[0][i]));
                    }
                }

                // Sort the predictions by probability
                predictionList.Sort((x, y) => y.Item2.CompareTo(x.Item2));

                return predictionList.Take(5).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Handle any exceptions
                return new List<(string, double)>();
            }
        }
    }
}

