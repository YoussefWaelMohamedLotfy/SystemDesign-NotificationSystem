namespace Worker.SMSService.Models;

internal class TwilioOptions
{
    public string AccountSid { get; set; } = default!;

    public string AuthToken { get; set; } = default!;

    public string FromPhoneNumber { get; set; } = default!;
}
