using System;

namespace CampaignWebhook.Domain
{
    /// <summary>
    /// Service for handling webhook events
    /// </summary>
    public class Service
    {
        #region Public methods

        /// <summary>
        /// Receive webhook event
        /// </summary>
        /// <param name="payload">The event payload</param>
        /// <returns>value according to the type of the event</returns>
        public static string Receive(Payload payload)
        {
            if (payload.Test)
                return Test(payload);

            //TODO Adjust your business logic accordingly
            if (payload.Event == Event.Select.Code || payload.Event == Event.Notify.Code)
                return NotifyClient(payload);

            if (payload.Event == Event.Grant.Code)
                return GenerateVoucher(payload);

            return string.Empty;
        }

        #endregion

        #region Private methods

        private static string NotifyClient(Payload payload)
        {
            //TODO Call your Whatsapp service here

            //TODO Remove console log
            Console.WriteLine(" ");
            Console.WriteLine($"****  WHATSAPP: {payload.Phone}" +
                              "\n********************************************************************" +
                              "\n" + payload.Notification +
                              "\n********************************************************************");
            Console.WriteLine(" ");

            return string.Empty;
        }

        private static string GenerateVoucher(Payload payload)
        {
            //TODO Call your Voucher service here and return the voucher code

            //TODO Remove console log and voucher mock code
            string voucherCode = Guid.NewGuid().ToString().ToUpper();

            Console.WriteLine(" ");
            Console.WriteLine("****  VOUCHER:" +
                              "\n********************************************************************");
            Console.WriteLine($"\nvoucherCode '{voucherCode}' for interaction '{payload.Id}'" +
                               "\n********************************************************************");
            Console.WriteLine(" ");

            return voucherCode;
        }

        private static string Test(Payload payload)
        {
            Console.WriteLine();
            Console.WriteLine($"WEBHOOK TEST: {DateTime.UtcNow:u}");
            Console.WriteLine($"  campaignId: {payload.CampaignId}");
            Console.WriteLine($"  id: {payload.Id}");
            Console.WriteLine($"  event: {payload.Event}");

            if (payload.Reward.HasValue)
                Console.WriteLine($"  reward: {payload.Reward.Value:0.00}");

            if (!string.IsNullOrEmpty(payload.Notification))
            {
                Console.WriteLine($"  notification: {payload.Phone}" +
                                  "\n*********************************************************" +
                                  "\n" + payload.Notification +
                                  "\n*********************************************************");
            }

            Console.WriteLine($"  test: true");

            if (!string.IsNullOrEmpty(payload.Challenge))
            {
                Console.WriteLine($"  challenge: {payload.Challenge}");
                Console.WriteLine($"  hash: {payload.Hash}");
            }

            if (payload.Event == Event.Select.Code || payload.Event == Event.Grant.Code)
            {
                string voucherCode = Guid.NewGuid().ToString().ToUpper();

                Console.WriteLine($"*********************************************************");
                Console.WriteLine($"\nvoucherCode '{voucherCode}' for interaction '{payload.Id}'" +
                                   "\n*********************************************************");

                return voucherCode;
            }

            return string.Empty;
        }

        #endregion
    }
}