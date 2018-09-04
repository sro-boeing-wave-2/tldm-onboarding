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
        public IActionResult OnboardUser([FromBody]LoginViewModel user)
        {
           _controller.OnboardUser(user);
            return Ok();
        }

        [HttpPost("create/workspace/verify")]
        public IActionResult Verify([FromBody]string otp)
        {
            _controller.VerifyUser(otp);
            return Ok();
        }


        [HttpPost("workspace")]
        public IActionResult OnboardUserFromWorkspace([FromBody]LoginViewModel user)
        {
            _controller.OnboardUserFromWorkspace(user);
            return Ok();
        }

        [HttpPost("workspace")]
        public IActionResult CreateWorkspace([FromBody]Workspace workspace)
        {
            _controller.CreateWorkspace(workspace);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetAllWorkspace(string value)
        {
            var list = _controller.GetAllWorkspace(value);
            return Ok(list);
        }

    }
}