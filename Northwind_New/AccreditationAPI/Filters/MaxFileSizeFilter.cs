using CustomException.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AccreditationAPI.Filters
{
    public class MaxFileSizeFilter : ActionFilterAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeFilter(int maxFileSize)
        => _maxFileSize = maxFileSize;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var dto = context.HttpContext.Request.Form;
            if (dto != null)
                foreach (IFormFile file in dto.Files)
                {
                    if (file.Length > _maxFileSize)
                    {
                        throw new BadRequestException(GetErrorMessage());
                    }
                }
        }

        public string GetErrorMessage()
        => $"حداکثر اندازه فایل باید { _maxFileSize / 1024} کیلو بایت باشد.";
    }
}
