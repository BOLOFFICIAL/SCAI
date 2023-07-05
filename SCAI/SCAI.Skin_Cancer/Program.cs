using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using static System.Net.Mime.MediaTypeNames;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace SCAI.Skin_Cancer
{
    internal class Program
    {
        //private static readonly string modelPath = "Assets/Skin_Cancer.onnx"; //Путь к файлу onnx
        // Метод для предобработки изображения
        public static DenseTensor<float> PreprocessImage(string imagePath)
        {
            // Загрузка изображения с помощью библиотеки ImageSharp
            using (var image = SixLabors.ImageSharp.Image.Load<Rgb24>(imagePath))
            {
                // Изменение размера изображения до 224x224 пикселей
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(224, 224),
                    Mode = ResizeMode.Stretch
                }));

                // Нормализация значений пикселей в диапазоне от 0 до 1
                var pixelValues = NormalizePixelValues(image);

                // Преобразование изображения в формат float32 и изменение размерности тензора
                var tensor = new DenseTensor<float>(new[] { 1, 3, 224, 224 });

                for (int y = 0; y < 224; y++)
                {
                    for (int x = 0; x < 224; x++)
                    {
                        var pixel = pixelValues[x, y];
                        tensor[0, 0, y, x] = pixel.R;
                        tensor[0, 1, y, x] = pixel.G;
                        tensor[0, 2, y, x] = pixel.B;
                    }
                }

                return tensor;
            }
        }

        // Метод для нормализации значений пикселей изображения
        private static Image<Rgb24> NormalizePixelValues(Image<Rgb24> image)
        {
            var pixelValues = image.Clone();

            for (int y = 0; y < pixelValues.Height; y++)
            {
                for (int x = 0; x < pixelValues.Width; x++)
                {
                    var pixel = pixelValues[x, y];

                    pixelValues[x, y] = new Rgb24(
                        (byte)(pixel.R),
                        (byte)(pixel.G),
                        (byte)(pixel.B));
                }
            }

            return pixelValues;
        }

        static void Main(string[] args)
        {

            var modelPath = "C:\\Users\\mrzom\\source\\repos\\SCAI\\SCAI\\SCAI.Skin_Cancer\\Assets\\Skin_Cancer.onnx"; //Путь к файлу onnx
            // Создание сессии ONNX.Runtime
            using (var session = new InferenceSession(modelPath))
            {
                // Получение названий входных и выходных имен модели
                var inputName = session.InputMetadata.Keys.First();
                var outputName = session.OutputMetadata.Keys.First();

                // Загрузка и предобработка изображения
                //var image = LoadAndPreprocessImage(imagePath);
                var image = "C:\\Users\\mrzom\\source\\repos\\SCAI\\SCAI\\SCAI.Skin_Cancer\\Assets\\ISIC_0033359.jpg";
                // Предсказанный класс: 5, Вероятность: 34,844997 - ISIC_0024432.jpg
                // Предсказанный класс: 4, Вероятность: 51,315998 - ISIC_0033301.jpg
                // Предсказанный класс: 5, Вероятность: 60,9861 - ISIC_0033359.jpg
                var inputTensor = PreprocessImage(image);
                //var inputTensor = new DenseTensor<float>(new[] { 1, 3, 224, 224 });

                // Загрузка картинки
                //var image = "Assets/testpic.jpg";

                // Выполнение прямого прохода
                var inputs = new NamedOnnxValue[] { NamedOnnxValue.CreateFromTensor(inputName, inputTensor) };
                var outputs = session.Run(inputs);

                // Получение выходного тензора
                var outputTensor = outputs.First().AsTensor<float>();

                // Обработка результатов
                var results = outputTensor.ToArray();

                float maxProbability = 0;
                int predictedClass = 0;
                for (int i = 0; i < results.Length; i++)
                {
                    if (results[i] > maxProbability)
                    {
                        maxProbability = results[i];
                        predictedClass = i;
                    }
                }

                Console.WriteLine($"Предсказанный класс: {predictedClass}, Вероятность: {maxProbability}");
            }
        }
    }
}