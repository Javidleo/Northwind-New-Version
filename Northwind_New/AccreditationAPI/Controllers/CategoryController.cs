using API.DTOs.CategoryDTOs;
using Application.Services.CategoryServices.Commands.Delete;
using Application.Services.CategoryServices.Commands.Upsert;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseController
    {

        [HttpPost("[action]")]
        public async Task<IActionResult> Upsert(UpsertCategoryDTO dto)
        {
            if (dto == null)
                return BadRequest();

            byte[] picture = Array.Empty<byte>();
            for (int i = 0; i < dto.Picture.Length; i++)
            {
                int item = dto.Picture[i];
                picture[i] = Convert.ToByte(item);
            }

            await Mediator.Send(new UpsertCategoryCommand(dto.Id, dto.Name, dto.Description, picture, dto.ParentId));
            return Ok();
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            await Mediator.Send(new DeleteCategoryCommand(id));
            return Ok();
        }
    }
}
