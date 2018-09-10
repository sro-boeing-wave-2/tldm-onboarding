using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var result = await _controller.OnboardUser(user);
            //if (result != null)
            //{
            //    return Ok(result);
            //}
            return Ok(result);
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

            var result = await _controller.OnboardUserFromWorkspace(user);
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest(result);

        }

        [HttpPost("invite/verify")]
        public async Task<IActionResult> VerifyInvitedUser([FromBody]LoginViewModel otp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }  

            var result = await _controller.VerifyInvitedUser(otp);
            if (result != null)
            {
                return Ok(result);
            }
            return Unauthorized();
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

        [Authorize]
        [HttpPost("personaldetails")]
        public async Task<IActionResult> PersonalDetails([FromBody] UserAccount user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newuser = await _controller.PersonalDetails(user);
            return Ok(newuser);  
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

            var space1 = await _controller.WorkSpaceDetails(space);
            return Ok(space1);
        }

        [Authorize]
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

        [HttpGet("login/{list}")]
        public async Task<IActionResult> GetworkspaceByName([FromRoute] string list)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var space = await _controller.GetWorkspaceByName(list);

            return Ok(space);         

        }

    }
}