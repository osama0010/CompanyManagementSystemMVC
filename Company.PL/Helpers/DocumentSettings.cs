using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace Company.PL.Helpers
{
    public static class DocumentSettings
    {
        // Upload
        public static string UploadFile(IFormFile file, string FolderName)
        {
            //1. Get Located Folder Path 
            //C:\Users\osama\Desktop\.Net Projects\CompanyManagementSystem Solution\Company.PL\wwwroot\Files\Images\
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName);
            //2. Get File Name and Make it Unique 
            string FileName = $"{Guid.NewGuid()}{file.FileName}";
            //3. Get File Path[Folder Path + FileName]
            string FilePath = Path.Combine(FolderPath, FileName);
            //4. Save File As Streams
            using var FileStream = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(FileStream);
            //5. Return File Name
            return FileName;

        }


        // Delete
        public static void DeleteFile(string FileName, string FolderName)
        {
        // 1. Get File Path 
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName, FileName);
            
        // 2.Check if File Exists Or Not
            if(File.Exists(FilePath))
            {
                // If Exists Remove It
                File.Delete(FilePath);
            }


        }

    }
}
