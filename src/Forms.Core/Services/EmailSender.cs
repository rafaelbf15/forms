using Forms.Core.DomainObjects;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forms.Core.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailAuthOptions _options;

        private readonly string apiKey;

        public EmailSender(IOptions<EmailAuthOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
            apiKey = _options.SendGridKey;
        }

        public Task SendEmailAsync(List<string> emails, string message)
        {
            return Execute(message, emails);
        }

        public Task SendEmailAsync(List<string> emails, string name, string message, string templateId)
        {
            return Execute(name, message, emails, templateId);
        }

        public Task SendEmailAsync(List<string> emails, string name, string message, List<string> caixasPendentes, string templateId)
        {
            return Execute(name, message, caixasPendentes, emails, templateId);
        }

        public Task SendEmailAsync(List<string> emails, string name, string message, string empreendimento, string dataRealizadoAte, string templateId)
        {
            return Execute(name, message, empreendimento, dataRealizadoAte, emails, templateId);
        }

        public Task SendEmailFormsAsync(List<string> emails, List<string> nomes, string link, string message, string dataPreenhcimento, string templateId)
        {
            return ExecuteEmailForms(emails, nomes, link, message, dataPreenhcimento, templateId);
        }

        public Task ExecuteEmailForms(List<string> emails, List<string> nomes, string link, string message, string dataPreenhcimento, string templateId)
        {
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress("sistemas@gmaia.com.br", "Sistemas GMaia");

            var templateData = new EmailFormularioPreenhcido
            {
                Mensagem = message,
                DataPreenchimento = dataPreenhcimento,
                Link = link
            };

            var tos = new List<EmailAddress>();


            for (int i = 0; i < emails.Count; i++)
            {
                tos.Add(new EmailAddress(emails[i], nomes[i]));
            }

            var msg = MailHelper.CreateSingleTemplateEmailToMultipleRecipients(from, tos, templateId, templateData);


            Task response = client.SendEmailAsync(msg);
            return response;
        }

        public Task Execute(string message, List<string> emails)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("sistemas@gmaia.com.br", "Sistemas GMaia"),
                PlainTextContent = message,
                HtmlContent = message
            };

            foreach (var email in emails)
            {
                msg.AddTo(new EmailAddress(email));
            }

            Task response = client.SendEmailAsync(msg);
            return response;
        }

        public Task Execute(string name, string message, List<string> emails, string templateId)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("sistemas@gmaia.com.br", "Sistemas GMaia"),
                TemplateId = templateId
            };
            msg.SetTemplateData(new EmailCaixa
            {
                Nome = name,
                Mensagem = message
            });

            foreach (var email in emails)
            {
                msg.AddTo(new EmailAddress(email));
            }

            Task response = client.SendEmailAsync(msg);
            return response;
        }

        public Task Execute(string name, string message, List<string>caixasPendentes, List<string> emails, string templateId)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("sistemas@gmaia.com.br", "Sistemas GMaia"),
                
                TemplateId = templateId
            };
            msg.SetTemplateData(new EmailCaixaAprovacao
            {
                Nome = name,
                Mensagem = message,
                CaixasPendentes = caixasPendentes
            });

            foreach (var email in emails)
            {
                msg.AddTo(new EmailAddress(email));
            }

            Task response = client.SendEmailAsync(msg);
            return response;
        }

        public Task Execute(string name, string message, string empreendimento, string dataRealizadoAte, List<string> emails, string templateId)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("sistemas@gmaia.com.br", "Sistemas GMaia"),
                TemplateId = templateId
            };
            msg.SetTemplateData(new EmailRealizadoPrevistoObras
            {
                Empreendimento = empreendimento,
                DataRealizadoAte = dataRealizadoAte,
                Mensagem = message
            }); ;

            foreach (var email in emails)
            {
                msg.AddTo(new EmailAddress(email));
            }

            Task response = client.SendEmailAsync(msg);
            return response;
        }

        private class EmailCaixa
        {
            [JsonProperty("name")]
            public string Nome { get; set; }

            [JsonProperty("message")]
            public string Mensagem { get; set; }
        }

        private class EmailCaixaAprovacao
        {
            [JsonProperty("name")]
            public string Nome { get; set; }

            [JsonProperty("message")]
            public string Mensagem { get; set; }

            [JsonProperty("caixasPendentes")]
            public List<string> CaixasPendentes = new List<string>();
        }

        private class EmailRealizadoPrevistoObras
        {
            [JsonProperty("empreendimento")]
            public string Empreendimento { get; set; }

            [JsonProperty("dataRealizadoAte")]
            public string DataRealizadoAte { get; set; }

            [JsonProperty("message")]
            public string Mensagem { get; set; }
        }

        private class EmailFormularioPreenhcido
        {

            [JsonProperty("message")]
            public string Mensagem { get; set; }

            [JsonProperty("dataPreenchimento")]
            public string DataPreenchimento { get; set; }

            [JsonProperty("link")]
            public string Link { get; set; }

        }
    }
}
