﻿using System;
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

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _controller.OnboardUser(user);
            return Ok();
        }

        [HttpPost("create/workspace/verify")]
        public async Task<IActionResult> Verify([FromBody]string otp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _controller.VerifyUser(otp);
            if (result != null)
            {
                return Ok(result);
            }
            return Unauthorized();
        }


        [HttpPost("invite")]
        public async Task<IActionResult> OnboardUserFromWorkspace([FromBody]LoginViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _controller.OnboardUserFromWorkspace(user);
            return Ok();
        }

        [HttpPost("create/workspace")]
        public async Task<IActionResult> CreateWorkspace([FromBody]Workspace workspace)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _controller.CreateWorkspace(workspace);
            return Ok();
        }

        [HttpPost("personaldetails")]
        public async Task<IActionResult> PersonalDetails([FromBody] UserAccount user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _controller.PersonalDetails(user);
            return Ok();  
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _controller.Login(login);
            if (result != null)
            {
                return Ok(result);
            }
            return Unauthorized();
            //return Ok(result);
        }

        [HttpPut("workspacedetails")]
        public async Task<IActionResult> WorkspaceDetails([FromBody] Workspace space)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _controller.WorkSpaceDetails(space);
            return Ok();
        }

        [HttpGet("{value}")]
        public async Task<IActionResult> GetAllWorkspace([FromRoute]string value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var list = await _controller.GetAllWorkspace(value);
            return Ok(list);
        }

    }
}