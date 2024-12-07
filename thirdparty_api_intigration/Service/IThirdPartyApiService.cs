namespace thirdparty_api_intigration.Service
{
    public interface IThirdPartyApiService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
