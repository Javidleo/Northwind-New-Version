using CommandHandling.Abstractions;
using DataAccess;
using DomainModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UseCases.Common;
using UseCases.Common.Exceptions;

namespace UseCases.Commands.PostCommands
{
    public record ChanagePostPropertiesCommand(long Id, string PostTitle, string PostContent, long? CategoryId, Guid UserId,
        string Tags, bool IsPrivate, bool IsDraft, long? GroupId, long? SubGroupId, List<PostAttachmentFileDataStructure> AttachmentList,
        List<long> DeletedFiles) : Acommand(Id)
    {
        public static ChanagePostPropertiesCommand Create(long Id, string PostTitle, string PostContent, long? CategoryId, Guid UserId,
        string Tags, bool IsPrivate, bool IsDraft, long? GroupId, long? SubGroupId, List<PostAttachmentFileDataStructure> AttachmentList,
        List<long> DeletedFiles)
        {
            if (PostTitle.Trim().Length == 0)
                throw new BadRequestException("PostTitle must be not null and empty.");

            if (PostContent.Trim().Length == 0)
                throw new BadRequestException("PostContent must be not null and empty.");

            return new(Id, PostTitle, PostContent, CategoryId, UserId, Tags, IsPrivate, IsDraft, GroupId, SubGroupId, AttachmentList, DeletedFiles);
        }


        public class ChanagePostPropertiesHandler : IHandleCommand<ChanagePostPropertiesCommand>
        {
            private readonly IWriteDbContext _context;
            //private readonly ICategoryRepository Categories;
            //private readonly IGroupRepository Groups;

            public ChanagePostPropertiesHandler(IWriteDbContext dbContext)//, ICategoryRepository Categories, IGroupRepository groups)
            => _context = dbContext;

            public async Task Handle(ChanagePostPropertiesCommand command)
            {
                //if (command.CategoryId != null)
                //    if (Categories.Find(command.CategoryId.Value) == null)
                //        throw new NotFoundException("دسته بندی پیدا نشد.");

                /////Karani
                //if (command.GroupId != null)
                //    if (Groups.GetById(command.GroupId.Value) == null)
                //        throw new NotFoundException("گروه پیدا نشد.");

                //if (command.SubGroupId != null)
                //    if (!Groups.DoesSubGroupExist(command.GroupId.Value, command.SubGroupId.Value))
                //        throw new NotFoundException("زیرگروه پیدا نشد.");
                /////Karani


                if (command.IsDraft == true && command.IsPrivate == true)
                    throw new ForbiddenException("پست خصوصی نمی تواند به صورت پیش نویس ذخیره شود.");

                Post post = await _context.Posts.FindAsync(command.Id);
                if (post == null)
                    throw new NotFoundException("پست پیدا نشد.");

                if (post.UserId != command.UserId)
                    throw new ForbiddenException("کاربر مجاز به تغییر پست نیست.");

                if (post.IsDraft && command.IsPrivate)
                    throw new ForbiddenException("پست پیش نویس را نمی توانید به حالت خصوصی تغییر دهید.");

                if (!post.IsDraft && command.IsDraft)
                    throw new ForbiddenException("پست منتشر شده را نمی توانید به حالت پیش نویس تغییر دهید.");

                if (post.LogicalDelete)
                    throw new ForbiddenException("امکان ویرایش پست حذف شده وجود ندارد.");

                post.ChangePostProperties(command.CategoryId, command.PostTitle, command.PostContent, command.Tags, command.IsPrivate,
                    command.IsDraft, command.GroupId, command.SubGroupId);

                foreach (PostAttachmentFileDataStructure File in command.AttachmentList)
                {
                    using Stream stream = File.File.OpenReadStream();
                    BinaryReader reader = new(stream);
                    byte[] file = reader.ReadBytes(Convert.ToInt32(File.File.Length));
                    string fileName = File.File.FileName;
                    long fileSize = File.File.Length;
                    string fileExtention = Path.GetExtension(File.File.FileName);

                    post.AttachFile(File.Title, command.Id,
                        command.UserId, fileName, fileExtention, File.File.ContentType, fileSize, string.Empty, file);
                }

                foreach (long postAttachmentIds in command.DeletedFiles)
                    post.DetachFile(postAttachmentIds);

                try
                {
                    _context.Posts.Update(post);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
