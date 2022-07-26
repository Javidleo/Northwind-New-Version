using QueryHandling.Abstractions;
using ReadModels.Common;
using ReadModels.DomainModel.Document;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ReadModels.Queries.PostAttachmentQueries.GetPostAttachmentFile
{
    public record GetPostAttachmentFileQuery(long PostAttachmentId) : Query<PostAttachmentFileViewModelOutPut>
    {
        public class GetPostAttachmentFileQueryHandler : IHandleQuery<GetPostAttachmentFileQuery, PostAttachmentFileViewModelOutPut>
        {
            private readonly IReadDbContext readDbContext;
            public GetPostAttachmentFileQueryHandler(IReadDbContext readDbContext)
            => this.readDbContext = readDbContext;

            public Task<PostAttachmentFileViewModelOutPut> Handle(GetPostAttachmentFileQuery query)
            {
                PostAttachmentViewModel File = readDbContext.PostAttachmentViewModels
                    .FirstOrDefault(c => c.Id == query.PostAttachmentId);

                if (File == null)
                    throw new Exception("FileAttachment does not exist!!!");

                SqlParameter param1 = new("path_locator", System.Data.SqlDbType.VarChar, 4000) { Value = File.FilePath };
                DocumentView d = readDbContext.DocumentView.Where(c => c.path_locator == File.FilePath).FirstOrDefault();

                PostAttachmentFileViewModelOutPut result = new()
                {
                    Id = query.PostAttachmentId,
                    File = d.file_stream
                };
                return Task.FromResult(result);
            }
        }

    }
}
