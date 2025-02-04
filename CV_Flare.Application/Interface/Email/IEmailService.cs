using CV_Flare.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV_Flare.Application.Interface.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
        public string GetCodeHtmlContent(string content, string code);
        public string GetNotificationHtmlContent(string content);
    }
}
