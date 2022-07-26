using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http.Headers;
using UseCases.Common.Exceptions;


namespace UseCases.Commands.PostCommands
{
    public record PostAttachmentFileDataStructure(string Title, IFormFile File)
    {
        public static PostAttachmentFileDataStructure Create(string Title, IFormFile File)
        {
            if (File == null)
                throw new BadRequestException("File must be not null and empty.");

            if (File.Length == 0)
                throw new BadRequestException("File must be not null and empty.");

            string filename = ContentDispositionHeaderValue.Parse(File.ContentDisposition).FileName.ToString().Trim('"');

            if (string.IsNullOrWhiteSpace(filename))
                throw new BadRequestException("FileName must be not null and empty.");
            return new(Title, File);
        }
    }
}
