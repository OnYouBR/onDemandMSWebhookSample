using System.Collections.Generic;

namespace CampaignWebhook.Domain
{
    /// <summary>
    /// Webhook event type
    /// </summary>
    public class Event
    {
        private static readonly List<string> codes = new();

        /// <summary>
        /// Type code
        /// </summary>
        public string Code { get; set; }

        public static readonly Event Skip = new(nameof(Skip));
        public static readonly Event Select = new(nameof(Select));
        public static readonly Event Grant = new(nameof(Grant));
        public static readonly Event Activate = new(nameof(Activate));
        public static readonly Event Notify = new(nameof(Notify));
        public static readonly Event Cancel = new(nameof(Cancel));
        public static readonly Event Abort = new(nameof(Abort));

        #region Construction

        private Event(string code)
        {
            Code = ToCamelCase(code);
            codes.Add(Code);
        }

        private static string ToCamelCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            char[] stringArray = input.ToCharArray();
            stringArray[0] = char.ToLower(stringArray[0]);
            return new string(stringArray);
        }

        #endregion

        #region Validation

        internal static bool IsValid(string code)
        {
            return codes.Contains(code);
        }

        #endregion
    }
}