using Identity.Common;
using Identity.Models;
using Identity.Services.Contracts;
using KnowledgeManagementAPI.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeManagementAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        readonly IAppRoleManager RoleManager;
        public RolesController(IAppRoleManager roleManager)
        {
            RoleManager = roleManager;
        }


        [HttpPost]
        public async Task<IActionResult> AddRole([FromBody] AddRoleDto roleDto)
        {
            if (roleDto is null)
                throw new BadHttpRequestException("Parameters are not set.");
            var role = await RoleManager.FindByNameAsync(roleDto.Name);
            //if (role != null)
            //    throw new BadHttpRequestException("This Role Already Exists.");
            var result = await RoleManager.CreateAsync(new AppRole(roleDto.Name));
            if (result.Succeeded)
                return Ok($"Role '{roleDto.Name}' successfully created.");
            if (result.Errors.Any())
            {
                string errordesc = "";
                foreach (var item in result.Errors)
                {
                    errordesc += item.Description + Environment.NewLine;
                }
                return BadRequest(errordesc);
            }

            return BadRequest();
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> EditRole(string Id, [FromBody] EditRoleDto roleDto)
        {
            if (roleDto is null)
                throw new BadHttpRequestException("Parameters are not set.");
            var role = await RoleManager.FindByIdAsync(Id);
            if (!string.IsNullOrEmpty(roleDto.Description))
                role.Description = roleDto.Description;
            role.IsActive = roleDto.IsActive;
            var result = await RoleManager.UpdateAsync(role);
            if (result.Succeeded)
                return Ok($"Role '{role.Name}' successfully updated.");
            if (result.Errors.Any())
                return BadRequest(result.DumpErrors());

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await RoleManager.Roles.ToListAsync();
            // return Ok(JsonConvert.SerializeObject(response));
            return Ok(response);
        }


        [HttpGet("{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRolebyId(string Id)
        {
            var response = await RoleManager.FindByIdAsync(Id);
            return Ok(response);
            // return Ok(JsonConvert.SerializeObject(response));
        }

        //  public async Task<IActionResult> GetRole


    }
}
