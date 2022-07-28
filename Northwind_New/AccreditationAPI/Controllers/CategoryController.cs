using API.DTOs.CategoryDTOs;
using Application.Categories.Commands.Delete;
using Application.Categories.Commands.Upsert;
using CommandHandling.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICommandBus _commandBus;
        public CategoryController(ICommandBus commandBus)
        => _commandBus = commandBus;

        [HttpPost("Upsert")]
        public async Task<IActionResult> Upsert(UpsertCategoryDTO dto)
        {
            var picture = Array.ConvertAll(dto.Picture, i => byte.Parse(i));

            await _commandBus.Send(UpsertCategoryCommand.Create(dto.Id, dto.Name, dto.Description, picture));
            return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            await _commandBus.Send(DeleteCategoryCommand.Create(Id));
            return Ok();
        }
    }
}
