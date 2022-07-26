using Identity.Common;
using Identity.Services;
using KnowledgeManagementAPI.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KnowledgeManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly EmailService EmailService;
        public EmailController(EmailService emailService)
        {
            EmailService = emailService;
        }

        //[HttpGet("[action]")]
        //public async Task<IActionResult> VerifyEmail(string token)
        //{
        //    if (string.IsNullOrWhiteSpace(token))
        //        return BadRequest("Parameters are not set.");

        //    var result = await EmailService.VerifyEmail(token);
        //    if (result.Succeeded)
        //        return Ok();
        //    return BadRequest(result.DumpErrors());
        //}


        [HttpGet("[action]")]
        public async Task<IActionResult> VerifyEmail([FromQuery] EmailVerificationDto emailVerificationDto)
        {
            if (emailVerificationDto is null)
                return BadRequest("Parameters are not set.");

            var result = await EmailService.VerifyEmail(emailVerificationDto.Email, emailVerificationDto.ConfirmToken);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.DumpErrors());
        }

        //[HttpPost("[action]")]
        //public async Task<IActionResult> VerifyEmail([FromBody] EmailVerificationDto emailVerificationDto)
        //{
        //    if (emailVerificationDto is null)
        //        return BadRequest("Parameters are not set.");

        //    var result = await EmailService.VerifyEmail(emailVerificationDto.Email, emailVerificationDto.Token);
        //    if (result.Succeeded)
        //        return Ok();
        //    return BadRequest(result.DumpErrors());
        //}

        [HttpPost("[action]")]
        public async Task<IActionResult> SendEmail([FromBody] EmailVerificationDto emailVerificationDto)
        {
            if (emailVerificationDto is null)
                return BadRequest("Parameters are not set.");

            await EmailService.SendEmail(emailVerificationDto.Email, emailVerificationDto.ConfirmToken);
            return Ok();
        }
    }
}
