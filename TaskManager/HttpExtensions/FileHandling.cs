using Microsoft.IdentityModel.Tokens;

namespace TaskManager.HttpExtensions

{
    public static class FileHandling
    {


        public static int FileSizeLimit = 5500000;
        public static (bool, string) validate(this IFormFile file)
        {

            List<string> AllowedExtension = new List<string> { ".pdf", ".docx", ".txt", ".jpg", ".png", ".jpeg" };



            // check if file name is empty
            if (file.FileName.IsNullOrEmpty())
                return (false, "file name is null or empty");

            // validate extensions
            if (!AllowedExtension.Contains(Path.GetExtension(file.FileName).ToLowerInvariant()))
                return (false, "File extension not allowed");

            // validate length
            if (file.Length > FileSizeLimit)
                return (false, $"The file is too large");


            return (true, "valid");
        }


        public static string GetExtansion(this string FileName)
        {
            return Path.GetExtension(FileName).ToLowerInvariant();
        }
        public static string GetFileName(this string path) {
        return Path.GetFileName(path);
        }
    }
}
