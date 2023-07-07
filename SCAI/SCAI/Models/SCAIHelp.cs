namespace SCAI.Models
{
    public class SCAIHelp
    {
        public static string GetLocalFilePath(string filePath)
        {
            string folderName = "wwwroot";
            string currentDirectory = Directory.GetCurrentDirectory();
            string filesFolderPath = Path.Combine(currentDirectory, folderName);

            if (filePath.Contains(filesFolderPath))
            {
                string localFilePath = filePath.Replace(filesFolderPath, string.Empty);
                return localFilePath;
            }

            return string.Empty; // Возвращаем пустую строку, если файл не находится в папке "files"
        }
    }
}
