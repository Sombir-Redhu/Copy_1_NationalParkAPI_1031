
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Copy_1_NationalParkWebAPI_1031
{
    public class MessageSender : ISmsSender
    {
        private readonly TwilioSetting _twilioSettings;
        public MessageSender(IOptions<TwilioSetting> twilioSettings)
        {
            _twilioSettings = twilioSettings.Value;
        }
        public async Task SendSmsAsync(string number, string message)
        {
            TwilioClient.Init(_twilioSettings.AccountSid, _twilioSettings.AuthToken);

            await MessageResource.CreateAsync(
              to: number,
              from: _twilioSettings.TwilioPhoneNumber,
              body: message);

        }
    }
}
