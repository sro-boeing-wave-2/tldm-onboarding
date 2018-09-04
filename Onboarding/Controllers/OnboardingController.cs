using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onboarding.Contract;
using Onboarding.Models;

namespace Onboarding.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OnboardingController : ControllerBase
    {
        private readonly IOnboardingService _controller;

        public OnboardingController(IOnboardingService controller)
        {
            _controller = controller;
        }

        [HttpPost("create/workspace/email")]
        public async Task<IActionResult> OnboardUser([FromBody]LoginViewModel user)
        {
          await _controller.OnboardUser(user);
            return Ok();
        }

        [HttpPost("create/workspace/verify")]
        public async Task<IActionResult> Verify([FromBody]string otp)
        {
           await  _controller.VerifyUser(otp);
            return Ok();
        }


        [HttpPost("workspace")]
        public IActionResult OnboardUserFromWorkspace([FromBody]LoginViewModel user)
        {
            _controller.OnboardUserFromWorkspace(user);
            return Ok();
        }

        [HttpPost("create/workspace")]
        public async Task<IActionResult> CreateWorkspace([FromBody]Workspace workspace)
        {
           await _controller.CreateWorkspace(workspace);
            return Ok();
        }

        [HttpPost("personaldetails")]
        public async Task<IActionResult> PersonalDetails([FromBody] UserAccount user)
        {
            await _controller.PersonalDetails(user);
            return Ok();  
        }

        [HttpPut("workspacedetails")]
        public async Task<IActionResult> WorkspaceDetails([FromBody] Workspace space)
        {
            await _controller.WorkSpaceDetails(space);
            return Ok();
        }

        //[HttpGet]
        //public IActionResult GetAllWorkspace([FromRoute]string value)
        //{
        //    var list = _controller.GetAllWorkspace(value);
        //    return Ok(list);
        //}

    }
}