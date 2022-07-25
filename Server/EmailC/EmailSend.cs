using FluentEmail.Core;

namespace Strona_v2.Server.EmailC
{
    public interface IEmailSend
    {
        Task<bool> SendTemplateRegister(string to, string subject, string body);
        Task<bool> SendWithBody(string to, string subject, string body);
    }

    public class EmailSend : IEmailSend
    {
        private readonly IFluentEmail _email;
        private readonly ILogger<EmailSend> _logger;
        public EmailSend(IFluentEmail email, ILogger<EmailSend> logger)
        {
            _email = email;
            _logger = logger;
        }

        public async Task<bool> SendWithBody(string to, string subject, string body)
        {
            var result = await _email.To(to)
                .Subject(subject)
                .Body(body)
                .SendAsync();

            if (!result.Successful)
            {
                _logger.LogError("Failed to nest email\n", result.ErrorMessages);
            }
            return result.Successful;
        }
        public async Task<bool> SendTemplateRegister(string to, string subject, string body)
        {
            var result = await _email.To(to)
                .Subject(subject)
                .UsingTemplate(body, new { })
                .SendAsync();

            if (!result.Successful)
            {
                _logger.LogError("Failed to nest email\n", result.ErrorMessages);
            }
            return result.Successful;
        }


    }
}
