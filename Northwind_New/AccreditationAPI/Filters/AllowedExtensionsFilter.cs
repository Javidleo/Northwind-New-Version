using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;
using System.Linq;
using UseCases.Common.Exceptions;

namespace KnowledgeManagementAPI.Filters
{
    public class AllowedExtensionsFilter : ActionFilterAttribute
    {
        private readonly string[] _extensions =
            { ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".psd", ".svg",
                ".zip", ".rar", ".mp3", ".mpeg", ".wav", ".jpeg",".jpg", ".png", ".gif",
                ".bmp", ".tif", ".mpeg4", ".mov", ".avi", ".flv", ".mkv", ".mp4", ".txt" };

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var dto = context.HttpContext.Request.Form;
            if (dto != null)
                foreach (IFormFile file in dto.Files)
                {
                    var extension = Path.GetExtension(file.FileName);
                    if (!_extensions.Contains(extension.ToLower()))
                    {
                        throw new BadRequestException(GetErrorMessage());
                    }
                }
        }

        public string GetErrorMessage()
        => $"نوع فایل معتبر نیست.";
    }
}
