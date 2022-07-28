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
    public record PostCommand(string PostTitle, string PostContent, long? CategoryId, Guid UserId,
        string Tags, bool IsPrivate, bool IsDraft, long? GroupId, long? SubGroupId, List<PostAttachmentFileDataStructure> AttachmentList) : Acommand(0)
    {
        public static PostCommand Create(string PostTitle, string PostContent, long? CategoryId, Guid UserId,
            string Tags, bool IsPrivate, bool IsDraft, long? GroupId, long? SubGroupId, List<PostAttachmentFileDataStructure> NewAttachmentList)
        {
            if (PostTitle.Trim().Length == 0)
                throw new BadRequestException("PostTitle must be not null and empty.");

            if (PostContent.Trim().Length == 0)
                throw new BadRequestException("PostContent must be not null and empty.");

            return new PostCommand(PostTitle, PostContent, CategoryId, UserId, Tags, IsPrivate, IsDraft, GroupId, SubGroupId, NewAttachmentList);
        }


        public class PostHandler : IHandleCommand<PostCommand>
        {
            private readonly IWriteDbContext _context;
            //private readonly ICategoryRepository Categories;
            //private readonly IGroupRepository Groups;
            public PostHandler(IWriteDbContext context)//, ICategoryRepository Categories, IGroupRepository groups)
            {
                _context = context;
                //this.Categories = Categories;
                //this.Groups = groups;
            }

            public async Task Handle(PostCommand command)
            {
                //if (command.CategoryId != null)
                //    if (Categories.Find(command.CategoryId.Value) == null)
                //        throw new NotFoundException("دسته بندی پیدا نشد.");

                /////Karani
                //if (command.GroupId != null)
                //    if (Groups.GetById(command.GroupId.Value) == null)
                //        throw new NotFoundException("گروه پیدا نشد.");

                //if (command.SubGroupId != null)
                //    if (!Groups.DoesSubGroupExist(command.GroupId.Value,command.SubGroupId.Value))
                //        throw new NotFoundException("زیرگروه پیدا نشد.");
                ///Karani

                if (command.IsDraft == true && command.IsPrivate == true)
                    throw new BadRequestException("پست خصوصی نمی تواند به صورت پیش نویس ذخیره شود.");

                Post post = Post.DefinePost(command.PostTitle, command.PostContent, command.CategoryId, command.UserId,
                    command.Tags, command.IsPrivate, command.IsDraft, command.GroupId, command.SubGroupId);
                foreach (PostAttachmentFileDataStructure File in command.AttachmentList)
                {
                    using Stream stream = File.File.OpenReadStream();
                    BinaryReader reader = new(stream);
                    byte[] file = reader.ReadBytes(Convert.ToInt32(File.File.Length));
                    string fileName = File.File.FileName;
                    long fileSize = File.File.Length;
                    string fileExtention = Path.GetExtension(File.File.FileName);

                    post.AttachFile(File.Title, command.Id, command.UserId, fileName, fileExtention, File.File.ContentType,
                        fileSize, string.Empty, file);
                }
                try
                {
                    await _context.Posts.AddAsync(post);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

    }
}
