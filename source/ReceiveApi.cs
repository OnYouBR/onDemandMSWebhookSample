using CampaignWebhook.Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace CampaignWebhook
{
    /// <summary>
    /// Function to be call via webhook for handling campaign events
    /// </summary>
    public static class ReceiveApi
    {
        [Function("receive")]
        public static HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            var response = NewResponseFrom(req);
            var payload = Payload.FactoryFrom(req.Body);

            if (payload is null)
                return BadRequest(response, "Webhook payload (from body) is empty or could not be deserialized.");

            if (!CheckChallenge(payload))
                return Unauthorized(response, "Challange didn't pass.");

            if (!payload.Validate())
                return BadRequest(response, payload.GetErrors());

            var data = Service.Receive(payload);

            return OK(response, data);
        }

        #region Support (private) methods

        private static HttpResponseData NewResponseFrom(HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            return response;
        }

        private static bool CheckChallenge(Payload payload)
        {
            if (string.IsNullOrEmpty(payload.Challenge) || string.IsNullOrEmpty(payload.Hash))
                return false;

            byte[] key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("ChallengeKey"));

            bool valid = false;
            using (HMACSHA256 hmac = new(key))
            {
                byte[] hashToCheckBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload.Challenge));
                string hashToCheck = BitConverter.ToString(hashToCheckBytes).Replace("-", "");

                valid = payload.Hash == hashToCheck;
            }
            return valid;
        }

        private static HttpResponseData Unauthorized(HttpResponseData response, string message)
        {
            response.StatusCode = HttpStatusCode.Unauthorized;
            response.WriteString(message);
            return response;
        }

        private static HttpResponseData BadRequest(HttpResponseData response, string message)
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            response.WriteString(message);
            return response;
        }

        private static HttpResponseData OK(HttpResponseData response, string data)
        {
            response.StatusCode = HttpStatusCode.OK;
            response.WriteString(data);
            return response;
        }

        #endregion
    }
}