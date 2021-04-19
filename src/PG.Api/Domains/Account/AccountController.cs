// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PG.BLL;
using PG.Model.Identity;
using Swashbuckle.AspNetCore.Annotations;

namespace PG.Api.Domains.Account
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserProfileService _userProfileService;

        public AccountController(UserManager<ApplicationUser> userManager, IUserProfileService userProfileService)
        {
            _userManager = userManager;
            _userProfileService = userProfileService;
        }

        [HttpPost("Register")]
        [SwaggerOperation(Summary = "Register New User")]
        [SwaggerResponse(200, "Success: User registered")]
        [SwaggerResponse(400, "Error: User data invalid")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var profile = new Model.UserProfile
                {
                    AppUser = user,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                _userProfileService.Create(profile);
            }
            else
            {
                return GetErrorResult(result);
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            return Ok(new { userId = user.Id, confirmCode = HttpUtility.UrlEncode(code) });
        }

        [HttpGet("Confirm", Name = "ConfirmEmail")]
        [SwaggerOperation(Summary = "Confirm User Email")]
        [SwaggerResponse(200, "Success: Email confirmed")]
        [SwaggerResponse(400, "Error: Email could not be confirmed")]
        public async Task<IActionResult> ConfirmEmail(int userId, string code)
        {
            if (code == null)
            {
                return BadRequest("Email could not be confirmed.");
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, code);
                if (result.Succeeded)
                {
                    var passwordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    return Json(new { resetPasswordToken = passwordToken });
                }
            }

            return BadRequest("Email could not be confirmed.");

        }

        [Authorize]
        [HttpGet("{userId}/ConfirmEmailUrl")]
        [SwaggerOperation(Summary = "Get Email Confirmation")]
        [SwaggerResponse(200, "Success: Confirmation url generated")]
        [SwaggerResponse(400, "Error: User not found")]
        public async Task<IActionResult> GetConfirmEmailUrl(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return BadRequest();

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var url = Url.Link("ConfirmEmail", new { userId, code = HttpUtility.UrlEncode(code) });

            return Ok(new { url });
        }

        [Authorize]
        [HttpPost("{userId}/Activate")]
        [SwaggerOperation(Summary = "Activate User")]
        [SwaggerResponse(200, "Success: User activated")]
        [SwaggerResponse(400, "Error: User not found")]
        public async Task<IActionResult> ActivateUser(int userId)
        {
            return await SetActiveStatus(userId, true);
        }

        [Authorize]
        [HttpPost("{userId}/Deactivate")]
        [SwaggerOperation(Summary = "Deactivate User")]
        [SwaggerResponse(200, "Success: User deactivated")]
        [SwaggerResponse(400, "Error: User not found")]
        public async Task<IActionResult> DeactivateUser(int userId)
        {
            return await SetActiveStatus(userId, false);
        }

        [Authorize]
        [HttpGet("{userId}/ResetPasswordUrl")]
        [SwaggerOperation(Summary = "Get Password Reset Url")]
        [SwaggerResponse(200, "Success: Reset token generated")]
        [SwaggerResponse(400, "Error: User not found")]
        public async Task<IActionResult> GetResetPasswordUrl(int userId)
        {
            var url = Url.Link("ResetPassword", null);
            ResetPasswordDto data = null;

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                data = new ResetPasswordDto
                {
                    Email = user.Email,
                    Code = HttpUtility.UrlEncode(code),
                    Password = "[new-password]",
                    ConfirmPassword = "[confirm-password]"
                };
                return Ok(new { url, data });
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("ResetPassword", Name = "ResetPassword")]
        [SwaggerOperation(Summary = "Reset User Password")]
        [SwaggerResponse(200, "Success: Password reset success")]
        [SwaggerResponse(400, "Error: Data invalid")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                if (!result.Succeeded)
                    return GetErrorResult(result);
            }

            return Ok();
        }

        [Authorize]
        [HttpDelete("{userId}/Delete")]
        [SwaggerOperation(Summary = "Delete User")]
        [SwaggerResponse(204, "Success: User deleted")]
        [SwaggerResponse(400, "Error: User not found")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                    return GetErrorResult(result);

                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        private IActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private async Task<IActionResult> SetActiveStatus(int userId, bool activeStatus)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return BadRequest();

            if (user.IsActive != activeStatus)
            {
                user.IsActive = activeStatus;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    return GetErrorResult(result);
            }

            return Ok();
        }
    }
}