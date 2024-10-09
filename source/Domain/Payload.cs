using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CampaignWebhook.Domain
{
    /// <summary>
    /// The payload of the Webhook for conversion change event
    /// </summary>
    public class Payload
    {
        /// <summary>
        /// Id of the registered interaction (returned by the register interaction endpoint)
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Id of the campaign
        /// </summary>
        public string CampaignId { get; set; }
        /// <summary>
        /// Interaction change event code
        /// </summary>
        public string Event { get; set; }
        /// <summary>
        /// <para>The reward value given to the client</para>
        /// <para>Optional according to the type of the change event</para>
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Reward { get; set; }
        /// <summary>
        /// <para>Message to be sent to the client as notification</para>
        /// <para>Optional and varies according to the type of the change event</para>
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Notification { get; set; }
        /// <summary>
        /// Phone number to send the notification (in the EXACT format: '+55 (XX) XXXXX-XXXX’)
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Phone { get; set; }
        /// <summary>
        /// Indication whether the webhook call is a test one
        /// </summary>
        public bool Test { get; set; }
        /// <summary>
        /// <para>Unique string used as challenge of origin validation hashed in HMACSHA256 by client's WebHook Challenge Key</para>
        /// <para>The value is always null if no such key was defined</para>
        /// </summary>
        public string Challenge { get; set; }
        /// <summary>
        /// Hash calculated in HMACSHA256 based on the challenge value and the client's WebHook challenge key
        /// </summary>
        public string Hash { get; set; }

        #region Validation

        private string errors = "";

        internal bool Validate()
        {
            if (string.IsNullOrEmpty(Id))
                AddError("Id must not be empty.");

            if ((Event == Domain.Event.Select.Code || Event == Domain.Event.Grant.Code) && 
                (!Reward.HasValue || Reward.Value <= 0))
                AddError("Select and Grant events must have reward value greater than 0.");

            if (!Domain.Event.IsValid(Event))
                AddError("Event is invalid.");

            if (Event == Domain.Event.Select.Code || Event == Domain.Event.Notify.Code)
            {
                if (string.IsNullOrEmpty(Notification))
                    AddError("Notification must be informed in select or notify events.");
                if (string.IsNullOrEmpty(Phone))
                    AddError("Phone must be informed in select or notify events.");
            }

            return string.IsNullOrEmpty(GetErrors());
        }

        internal string GetErrors()
        {
            return errors;
        }

        private void AddError(string error)
        {
            if (string.IsNullOrEmpty(errors))
                errors = error;
            else
                errors += $"\n{error}";
        }

        #endregion

        #region Factories

        private static readonly JsonSerializerOptions jsonOptions = new() { PropertyNameCaseInsensitive = true };

        internal static Payload FactoryFrom(Stream body)
        {
            string json = new StreamReader(body).ReadToEnd();

            if (string.IsNullOrEmpty(json))
                return null;

            try
            {
                return JsonDocument.Parse(json).Deserialize<Payload>(jsonOptions);
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}