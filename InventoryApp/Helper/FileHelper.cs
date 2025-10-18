namespace InventoryApp.Helper
{
    public class FilelHelper
    {
        public static string UploadFile(IFormFile file, string FolderName)
        {
       
            // 1- Get Located Folder Path
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName);
            if (!Directory.Exists(FolderPath)) 
            {
                Directory.CreateDirectory(FolderPath);
            }
            // 2- Get File Name and Make it Unique
            string FileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            // 3- Get File Path [Folder Path + FileName]
            string FilePath = Path.Combine(FolderPath, FileName);
            // 4- Save File As Streams
            using var fs = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(fs);
            // 5- Return File Name
            return FileName;

        }
        //Delete
        public static void DeleteFile(string FileName, string FolderName)
        {
            ///1- Get File Path
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName, FileName);

            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }
    }
}
