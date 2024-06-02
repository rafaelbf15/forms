using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forms.Core.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(List<string> emails, string message);
        
        Task SendEmailAsync(List<string> emails, string name, string message, string templateId);

        Task SendEmailAsync(List<string> emails, string name, string message, List<string> caixasPendentes, string templateId);

        Task SendEmailAsync(List<string> emails, string name, string message, string empreendimento, string dataRealizadoAte, string templateId);

        Task SendEmailFormsAsync(List<string> emails, List<string> nomes, string link, string message, string dataPreenhcimento, string templateId);
    }
}
