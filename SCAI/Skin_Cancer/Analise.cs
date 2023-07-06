using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace Skin_Cancer
{
    public class Analise
    {

        public static void Main() { }
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

        public List<float> AnalisePhoto(string puth)
        {
            var modelPath = "D:\\Projects\\SCAI\\SCAI\\Skin_Cancer\\Skin_Cancer.onnx";
            using (var session = new InferenceSession(modelPath))
            {
                var inputName = session.InputMetadata.Keys.First();
                var outputName = session.OutputMetadata.Keys.First();
                var inputTensor = PreprocessImage(puth);
                var inputs = new NamedOnnxValue[] { NamedOnnxValue.CreateFromTensor(inputName, inputTensor) };
                var outputs = session.Run(inputs);
                var outputTensor = outputs.First().AsTensor<float>();
                return outputTensor.ToList(); 
            }
        }
    }
}