using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using thirdparty_api_intigration.Service;

namespace thirdparty_api_intigration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HomeController : ControllerBase
    {
        private readonly IThirdPartyApiService _sendGridService;

        public HomeController(IThirdPartyApiService apiService)
        {
            _sendGridService = apiService;
        }

        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailRequest request)
        {
            try
            {
                await _sendGridService.SendEmailAsync(request.ToEmail, request.Subject, request.Body);
                return Ok(new { success = true, message = "Email sent successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
    }
