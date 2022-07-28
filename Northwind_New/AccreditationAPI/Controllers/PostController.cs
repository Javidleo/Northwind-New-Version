using AccreditationAPI.DTOs.Post;
using CommandHandling.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QueryHandling.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccreditationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ICommandBus commandBus;
        private readonly IQueryBus queryBus;

        public PostController(ICommandBus commandBus, IQueryBus queryBus)
        {
            this.commandBus = commandBus;
            this.queryBus = queryBus;
        }

        //[HttpPost]
        ////[PersianConvertorFilter("postDTO")]
        //public async Task<IActionResult> Post(IFormCollection dto)
        //{
        //    if (dto == null)
        //        throw new BadRequestException("اطلاعاتی برای ثبت وجود ندارد.");

        //    PostDTO postDTO = JsonConvert.DeserializeObject<PostDTO>(dto["data"]);
        //    if (postDTO.NewFilesDetail.Count != dto.Files.Count)
        //        throw new BadRequestException("اطلاعات فایل نامعتبر است.");

        //    List<PostAttachmentFileDataStructure> FileList = new();
        //    if (dto.Files != null)
        //        for (int i = 0; i < dto.Files.Count; i++)
        //            FileList.Add(PostAttachmentFileDataStructure.Create(postDTO.NewFilesDetail[i].Title, dto.Files[i]));
        //    try
        //    {
        //        await commandBus.Send(PostCommand.Create(postDTO.PostTitle, postDTO.PostContent, postDTO.CategoryId,
        //                                            new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"), postDTO.Tags.NormalizedInput(),
        //                                            postDTO.IsPrivate, postDTO.IsDraft, postDTO.GroupId, postDTO.SubGroupId, FileList));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }
        //    return Ok();
        //}

        //[HttpPut]
        //public async Task<IActionResult> Put(IFormCollection dto)
        //{
        //    if (dto == null)
        //        throw new BadRequestException("اطلاعاتی برای ثبت وجود ندارد.");

        //    ChangePostPropertiesDTO postDTO = JsonConvert.DeserializeObject<ChangePostPropertiesDTO>(dto["data"]);
        //    if (postDTO.NewFilesDetail.Count != dto.Files.Count)
        //        throw new BadRequestException("اطلاعات فایل نامعتبر است.");

        //    List<PostAttachmentFileDataStructure> FileList = new();
        //    if (dto.Files != null)
        //        for (int i = 0; i < dto.Files.Count; i++)
        //            FileList.Add(PostAttachmentFileDataStructure.Create(postDTO.NewFilesDetail[i].Title, dto.Files[i]));
        //    List<long> DeletedAttachmentIds = new();
        //    foreach (long item in postDTO.DeletedFiles)
        //        DeletedAttachmentIds.Add(item);

        //    try
        //    {
        //        await commandBus.Send(ChanagePostPropertiesCommand.Create(postDTO.Id, postDTO.PostTitle, postDTO.PostContent, postDTO.CategoryId,
        //                                            new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"), postDTO.Tags.NormalizedInput(),
        //                                            postDTO.IsPrivate, postDTO.IsDraft, postDTO.GroupId, postDTO.SubGroupId, FileList, DeletedAttachmentIds));
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500);
        //    }
        //    return Ok();
        //}

        //[HttpDelete("{Id}")]
        //public async Task<IActionResult> Delete(long Id)
        //{
        //    try
        //    {
        //        await commandBus.Send(DeletePostCommand.Create(Id));
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500);
        //    }
        //    return Ok();
        //}

        //[HttpGet]
        //public async Task<IActionResult> Get([FromQuery] GetPostDTO getPostDTO)
        //{
        //    if (getPostDTO == null)
        //        throw new ArgumentNullException(nameof(getPostDTO));
        //    var response = await queryBus.Send<PagedViewModel<PostViewModelOutPut>, GetPostListQuery>
        //                                                        (new GetPostListQuery(getPostDTO.PageNumber, getPostDTO.PageSize, getPostDTO.CategoryID,
        //                                                                                getPostDTO.GroupId, getPostDTO.SubGroupId,
        //                                                                                getPostDTO.PostTitle, getPostDTO.PostContent, getPostDTO.Tags,
        //                                                                                getPostDTO.Private, getPostDTO.Draft,
        //                                                                                getPostDTO.StartDate, getPostDTO.EndDate,
        //                                                                                getPostDTO.SortOrder.NormalizedInput()));

        //    return Ok(JsonConvert.SerializeObject(response));
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(long id)
        //{
        //    var response = await queryBus.Send<PostViewModelWithContentOutPut, GetPostQuery>(
        //                        new GetPostQuery(id, new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6")));
        //    return Ok(JsonConvert.SerializeObject(response));
        //}

        //[HttpGet]
        //[ActionName("DownloadFile")]
        //[Route("DownloadFile")]
        //public async Task<IActionResult> DownloadFile(long postAttachmentId)
        //{
        //    var Response = await queryBus
        //                    .Send<PostAttachmentFileViewModelOutPut, GetPostAttachmentFileQuery>(new GetPostAttachmentFileQuery(postAttachmentId));
        //    return Ok(JsonConvert.SerializeObject(Response));
        //}
    }
}
