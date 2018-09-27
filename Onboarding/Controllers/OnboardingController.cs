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
        private readonly IJWTTokenService _tokengenerator;

        public OnboardingController(IOnboardingService controller, IJWTTokenService tokenService)
        {
            _controller = controller;
            _tokengenerator = tokenService; 
        }

        [HttpPost("create/workspace/email")]
        public async Task<IActionResult> OnboardUser([FromBody]LoginViewModel user)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _controller.OnboardUser(user);
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
                var token = _tokengenerator.GetToken(result);
                var tokenObject = new
                {
                    token = token
                };
                return Ok(tokenObject);
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
                var token = _tokengenerator.GetToken(result);
                var tokenObject = new
                {
                    token = token
                };
                return Ok(tokenObject);
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

           var result =  await _controller.CreateWorkspace(workspace);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        //[Authorize]
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
               var token =  _tokengenerator.GetToken(result);
                var tokenObject = new
                {
                    token = token
                };
                return Ok(tokenObject);
            }
            return Unauthorized();
        }

        [HttpPut("workspacedetails")]
        public async Task<IActionResult> WorkspaceDetails([FromBody] Workspace space)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var space1 = await _controller.WorkSpaceDetails(space);
            if (space1 != null)
            {
                return Ok(space1);
            }
            return NotFound();
        }

        //[Authorize]
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

        [HttpPost("bot/verify")]
        public IActionResult BotVerfication(LoginViewModel value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _controller.BotVerification(value);
            if(result != null)
            {
                return Ok();
            }
            return BadRequest();

        }
    }
}