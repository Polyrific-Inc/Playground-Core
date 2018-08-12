// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PG.BLL;
using PG.Model.Identity;

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

        /// <summary>
        /// Register new User
        /// </summary>
        /// <param name="model">Values model to create the new User</param>
        /// <returns>
        /// The User Id and confirmation code
        /// </returns>
        /// <response code="200">Returns the User Id and confirmation code</response>
        /// <response code="500">If there is error when executing the code</response>
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        [HttpPost("Register")]
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

        /// <summary>
        /// Confirm email address
        /// </summary>        
        /// <param name="userId">User's ID</param>
        /// <param name="code">Confirmation code</param>
        /// <returns>
        /// Reset password token
        /// </returns>
        /// <response code="200">Returns reset password token</response>
        /// <response code="400">If the request malsyntax</response>
        /// <response code="500">If there is error when executing the code</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpGet("Confirm", Name = "ConfirmEmail")]
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
                    return Json(new {resetPasswordToken = passwordToken});
                }
            }

            return BadRequest("Email could not be confirmed.");

        }

        /// <summary>
        /// Get email confirmation URL
        /// </summary>        
        /// <param name="userId">User's ID</param>
        /// <returns>
        /// URL for email confirmation
        /// </returns>
        /// <response code="200">Returns URL for email confirmation</response>
        /// <response code="400">If the request malsyntax</response>
        /// <response code="500">If there is error when executing the code</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        //[Authorize]
        [HttpGet("{userId}/ConfirmEmailUrl")]
        public async Task<IActionResult> GetConfirmEmailUrl(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return BadRequest();

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var url = Url.Link("ConfirmEmail", new {userId, code = HttpUtility.UrlEncode(code)});

            return Ok(new { url });
        }

        /// <summary>
        /// Activate a User
        /// </summary>
        /// <param name="userId">User's ID</param>
        /// <returns>
        /// </returns>
        /// <response code="200">Success</response>
        /// <response code="400">If the request malsyntax</response>
        /// <response code="500">If there is error when executing the code</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        //[Authorize]
        [HttpPost("{userId}/Activate")]
        public async Task<IActionResult> ActivateUser(int userId)
        {
            return await SetActiveStatus(userId, true);
        }

        /// <summary>
        /// Deactivate a User
        /// </summary>
        /// <param name="userId">User's ID</param>
        /// <returns>
        /// </returns>
        /// <response code="200">Success</response>
        /// <response code="400">If the request malsyntax</response>
        /// <response code="500">If there is error when executing the code</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        //[Authorize]
        [HttpPost("{userId}/Deactivate")]
        public async Task<IActionResult> DeactivateUser(int userId)
        {
            return await SetActiveStatus(userId, false);
        }

        /// <summary>
        /// Get URL for password reset
        /// </summary>        
        /// <param name="userId">User's ID</param>
        /// <returns>
        /// URL for password reset and template with needed data for updating password
        /// </returns>
        /// <response code="200">Returns URL for password reset and template with needed data for updating password</response>
        /// <response code="500">If there is error when executing the code</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        //[Authorize]
        [HttpGet("{userId}/ResetPasswordUrl")]
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
            }

            return Ok(new {url, data});
        }

        /// <summary>
        /// Reset User's password
        /// </summary>
        /// <param name="model">Value model for resetting the password</param>
        /// <response code="200">Success</response>
        /// <response code="500">If there is error when executing the code</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [HttpPost("ResetPassword", Name = "ResetPassword")]
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

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="userId">User id to delete</param>
        /// <response code="204">If the delete successfull</response>
        /// <response code="500">If there is error when executing the code</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        //[Authorize]
        [HttpDelete("{userId}/Delete")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                    return GetErrorResult(result);
            }
            
            return NoContent();
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