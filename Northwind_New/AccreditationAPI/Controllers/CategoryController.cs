using API.DTOs.CategoryDTOs;
using Application.Categories.Commands.Delete;
using Application.Categories.Commands.Upsert;
using Application.Categories.Queries.GetAll;
using CommandHandling.Abstractions;
using Microsoft.AspNetCore.Mvc;
using QueryHandling.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;
        public CategoryController(ICommandBus commandBus, IQueryBus queryBus)
        {
            _commandBus = commandBus;
            _queryBus = queryBus;
        }

        [HttpPost("Upsert")]
        public async Task<IActionResult> Upsert(UpsertCategoryDTO dto)
        {
            var picture = Array.ConvertAll(dto.Picture, i => byte.Parse(i));

            await _commandBus.Send(UpsertCategoryCommand.Create(dto.Id, dto.Name, dto.Description, picture));
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<CategoryListViewModel>> GetAll()
        => Ok(await _queryBus.Send<CategoryListViewModel, GetCategoryListQuery>(new GetCategoryListQuery()));


        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            await _commandBus.Send(DeleteCategoryCommand.Create(Id));
            return Ok();
        }
    }
}
