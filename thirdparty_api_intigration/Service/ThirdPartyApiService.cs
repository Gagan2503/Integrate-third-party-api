using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

namespace thirdparty_api_intigration.Service
{
    public class ThirdPartyApiService : IThirdPartyApiService
    {
        private readonly string _apiKey;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ThirdPartyApiService> _logger;

        public ThirdPartyApiService(IConfiguration configuration, ILogger<ThirdPartyApiService> logger)
        {
            _configuration = configuration;
            _apiKey = _configuration["SendGrid:ApiKey"];  // Retrieve SendGrid API Key from appsettings.json
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var client = new SendGridClient(_apiKey);
                var from = new EmailAddress("no-reply@yourdomain.com", "Your Application Name");
                var to = new EmailAddress(toEmail);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, body, body);

                var response = await client.SendEmailAsync(msg);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    var errorMessage = $"Failed to send email. Status code: {response.StatusCode}";
                    _logger.LogError(errorMessage);
                    throw new Exception(errorMessage);
                }

                _logger.LogInformation("Email sent successfully to {ToEmail}", toEmail);
            }
            catch (HttpRequestException httpEx)
            {
                // Handle network-related issues (e.g., server down, no internet connection)
                var errorMessage = $"Network error while sending email: {httpEx.Message}";
                _logger.LogError(errorMessage);
                throw new Exception(errorMessage, httpEx);
            }
            catch (TimeoutException timeoutEx)
            {
                // Handle timeout issues
                var errorMessage = $"Request timeout while sending email: {timeoutEx.Message}";
                _logger.LogError(errorMessage);
                throw new Exception(errorMessage, timeoutEx);
            }
            catch (Exception ex)
            {
                // Catch any unexpected errors
                var errorMessage = $"Unexpected error: {ex.Message}";
                _logger.LogError(errorMessage);
                throw new Exception(errorMessage, ex);
            }
        }
    }
}
