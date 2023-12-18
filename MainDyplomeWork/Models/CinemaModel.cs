using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SmartReservationCinema.FilmContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace SmartReservationCinema.Models
{
    public class CinemaModel : Cinema, IValidatableObject
    {
        public IFormFile uploadFile { get; set; }
        public string NewImage { get; set; } = "";

        public CinemaModel() { }
        public CinemaModel(Cinema cinema)
        {
            CinemaName = cinema.CinemaName;
            Localisation = cinema.Localisation;
            LongCoordinate = cinema.LongCoordinate;
            LatCoordinate = cinema.LatCoordinate;
            CinemaRating = cinema.CinemaRating;
            Image = cinema.Image;
            TownId = cinema.TownId;
            Town = cinema.Town;
        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            IWebHostEnvironment env = (IWebHostEnvironment)validationContext.GetService(typeof(IWebHostEnvironment));
            string imgFolder = env.WebRootPath + "/img/cinemaImage/";
            List<ValidationResult> errorList = new List<ValidationResult>();
            if (uploadFile != null)
            {
                string extension = getExtension(errorList);

                if (uploadFile.Length == 0)
                {
                    errorList.Add(new ValidationResult("File length must be more than 0!", new List<string>() { "uploadFile" }));
                }
                if (uploadFile.Length >= 200 * 1024)
                {
                    errorList.Add(new ValidationResult("File length are to big!", new List<string>() { "uploadFile" }));
                }
                if (errorList.Count == 0)
                {
                    string fileName = getName(imgFolder, extension);
                    using (FileStream fileStream = new FileStream(imgFolder + fileName, FileMode.Create))
                    {
                        uploadFile.CopyTo(fileStream);
                        NewImage = fileName;
                    }

                }
            }
            return errorList;
        }
        public string getExtension(List<ValidationResult> errorList)
        {
            string extension = "";
            if (uploadFile.ContentType == "image/jpeg")
            {
                extension = ".jpg";
            }
            else if (uploadFile.ContentType == "image/png")
            {
                extension = ".png";
            }
            else if (uploadFile.ContentType == "image/gif")
            {
                extension = ".gif";
            }
            else
            {
                errorList.Add(new ValidationResult("Bad file extension!", new List<string>()
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
            do
            {
                int value = random.Next(100000, 999999);
                fileName = "Image" + value.ToString() + extension;
                filePath = imgDir + fileName;
            } while (File.Exists(filePath));
            return fileName;
        }
    }
}
