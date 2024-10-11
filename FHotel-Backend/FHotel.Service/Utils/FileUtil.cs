﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Office.Interop.Excel;
using Spire.Xls;
using Workbook = Spire.Xls.Workbook;
using Worksheet = Spire.Xls.Worksheet;
using System.IO;
namespace FHotel.Service.Utils
{
    public static class FileUtil
    {

        #region Convert IFormFile to FileStream
        public static FileStream ConvertFormFileToStream(IFormFile file)
        {
            // Create a unique temporary file path
            string tempFilePath = Path.GetTempFileName();

            // Open a FileStream to write the file
            using (FileStream stream = new FileStream(tempFilePath, FileMode.Create))
            {
                // Copy the contents of the IFormFile to the FileStream
                file.CopyToAsync(stream).GetAwaiter().GetResult();
            }

            // Open the temporary file in read mode and return the FileStream
            FileStream fileStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read);

            return fileStream;
        }
        #endregion

        #region Convert Excel Picture to FileStream
        public static FileStream ConvertExcelPictureToStream(ExcelPicture file)
        {
            // Create a unique temporary file path
            string tempFilePath = Path.GetTempFileName();

            // Open a FileStream to write the file
            using (FileStream stream = new FileStream(tempFilePath, FileMode.Create))
            {
                // Copy the contents of the IFormFile to the FileStream
                file.SaveToImage(stream);
            }

            // Open the temporary file in read mode and return the FileStream
            FileStream fileStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read);

            return fileStream;
        }
        #endregion

        #region HaveSupportedFileType
        public static bool HaveSupportedFileType(string fileName)
        {
            string[] validFileTypes = { ".png", ".jpg", ".jpeg", ".webp" };
            string extensionFile = Path.GetExtension(fileName);
            if (validFileTypes.Contains(extensionFile))
            {
                return true;
            }
            return false;
        }
        #endregion

        public static string GetImageIdFromUrlImage(string urlImage, string queryName)
        {
            try
            {
                Uri imageUri = new Uri(urlImage);

                string imageId = HttpUtility.ParseQueryString(imageUri.Query).Get(queryName);

                return imageId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
