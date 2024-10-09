using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Webhook.WhatsApp;

namespace CampaignWebhook.WhatsApp
{
    /// <summary>
    /// Service for sending WhatsApp messages
    /// </summary>
    public class UseInsiderWAGateway
    {
        #region Public methods

        /// <summary>
        /// Sends a template (initiating) message to a given phone number
        /// </summary>
        /// <param name="phone">The phone number</param>
        /// <param name="message">The message to be sent</param>
        public static void Send(string phone, string message)
        {
            var (templateName, variables) = GetTemplateParms(message);

            SendTransactionalTemplateMessage(phone, templateName, variables);
        }

        #endregion

        #region Private methods

        private static (string templateName, List<string> variables) GetTemplateParms(string message)
        {
            var splits = message.Split("||");
            return (splits[0], splits[1..].ToList());
        }

        private static void SendTransactionalTemplateMessage(string phone, string templateName, List<string> variables)
        {
            if (string.IsNullOrWhiteSpace(templateName))
                return;

            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-ins-auth-key", Environment.GetEnvironmentVariable("UseInsiderApiKey"));

            using var request = new HttpRequestMessage(HttpMethod.Post, "https://whatsapp.useinsider.com/v1/send");

            var jsonPayload = JsonSerializer.Serialize(UseInsiderWATemplateMessagePayload.FactoryFrom(phone, templateName, variables),
                                                       new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var res = client.Send(request);

            if (res.StatusCode != HttpStatusCode.OK)
                Console.WriteLine($"Result > {res.StatusCode} - Content: {res.Content.ReadAsStringAsync().Result}\n");
        }

        #endregion
    }
}