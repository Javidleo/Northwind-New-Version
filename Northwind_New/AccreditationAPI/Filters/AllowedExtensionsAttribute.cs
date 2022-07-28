using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace API.Filters
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions =
            { ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".psd", ".svg",
                ".zip", ".rar", ".mp3", ".mpeg", ".wav", ".jpeg",".jpg", ".png", ".gif",
                ".bmp", ".tif", ".mpeg4", ".mov", ".avi", ".flv", ".mkv", ".mp4", ".txt" };

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"This file extension is not allowed!";
        }
    }
}
