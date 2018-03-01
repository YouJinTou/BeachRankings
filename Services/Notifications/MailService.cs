using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace Services.Notifications
{
    public class MailService : IMailService
    {
        private const string RecipientVariables = "recipient-variables";

        public async Task SendMailAsync(
            IEnumerable<string> recipients, string from, string subject, string body)
        {
            try
            {
                var response = await this.SendMultipleAsync(recipients, from, subject, body);
            }
            finally
            {
            }
        }

        private async Task<IRestResponse> SendMultipleAsync(
            IEnumerable<string> recipients, string from, string subject, string body)
        {
            var client = new RestClient();
            client.BaseUrl = new Uri(ConfigurationManager.AppSettings["MailApiBaseUrl"]);
            client.Authenticator = new HttpBasicAuthenticator(
                "api", ConfigurationManager.AppSettings["MailApiKey"]);
            var request = new RestRequest { Resource = "{domain}/messages", Method = Method.POST };
            var recipientVariables = new JObject();

            request.AddParameter(
                "domain", 
                ConfigurationManager.AppSettings["MailDomainName"], 
                ParameterType.UrlSegment);
            request.AddParameter("from", from);
            request.AddParameter("subject", subject);
            request.AddParameter("html", body);

            foreach (var recipient in recipients)
            {
                request.AddParameter("to", recipient);

                recipientVariables.Add(
                    recipient, JsonConvert.SerializeObject(new RecipientVariable(recipient)));
            }

            request.AddParameter(RecipientVariables, recipientVariables);

            var taskCompletionSource = new TaskCompletionSource<IRestResponse>();

            client.ExecuteAsync(request, response =>
            {
                taskCompletionSource.SetResult(response);
            });

            return await taskCompletionSource.Task;
        }

        private struct RecipientVariable
        {
            private string recipient;

            public RecipientVariable(string recipient)
            {
                this.recipient = recipient;
            }

            public string Email => this.recipient;
        }
    }
}
