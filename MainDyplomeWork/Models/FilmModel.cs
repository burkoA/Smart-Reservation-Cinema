using MainDyplomeWork.FilmContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace MainDyplomeWork.Models
{
    public class FilmModel : Film, IValidatableObject
    {
        public IFormFile uploadFile { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            IWebHostEnvironment env=(IWebHostEnvironment)validationContext.GetService(typeof(IWebHostEnvironment));
            string imgFolder = env.WebRootPath + "/img/filmsImage/";
            List<ValidationResult> errorList = new List<ValidationResult>();
            if(uploadFile != null)
            {
                string extension = getExtension(errorList);

                if(uploadFile.Length == 0)
                {
                    errorList.Add(new ValidationResult("File length must be more than 0!", new List<string>(){"uploadFile"}));
                }
                if(uploadFile.Length >= 200*1024)
                {
                    errorList.Add(new ValidationResult("File length are to big!", new List<string>() { "uploadFile" }));
                }
                if(errorList.Count == 0)
                {
                    string fileName = getName(imgFolder, extension);
                    using (FileStream fileStream = new FileStream(imgFolder+fileName, FileMode.Create)) {
                        uploadFile.CopyTo(fileStream);
                        base.Image = fileName;
                    }                    
                }
            }
            return errorList;
        }
        public string getExtension(List<ValidationResult> errorList)
        {
            string extension = "";
            if(uploadFile.ContentType == "image/jpeg")
            {
                extension = ".jpg";
            } else if (uploadFile.ContentType == "image/png")
            {
                extension = ".png";
            } else if (uploadFile.ContentType == "image/gif")
            {
                extension = ".gif";
            } else
            {
                errorList.Add(new ValidationResult("Bad file extension!",new List<string>()
                {
                    "uploadFile"
                }));
            }
            return extension;
        }
        public string getName(string imgDir, string extension)
        {
            string filePath, fileName;
            Random random = new Random();
            do { 
                int value = random.Next(100000, 999999);
                fileName= "Image"+value.ToString() + extension;
                filePath = imgDir + fileName;
            }while(System.IO.File.Exists(filePath));
            return fileName;
        }
    }
}
