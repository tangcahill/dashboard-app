using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

public class AlarmSender
{
    private readonly FirebaseMessaging _fm;

    public AlarmSender()
    {
        // AppContext.BaseDirectory is the bin folder at runtime
        var credPath = Path.Combine(AppContext.BaseDirectory, "service-account.json");
        FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromFile(credPath)
        });
        _fm = FirebaseMessaging.DefaultInstance;
    }


    public Task SendAlarmAsync(string fcmToken, DateTime due, string message)
    {
        var timestampMs = ((long)(due.ToUniversalTime() - DateTime.UnixEpoch).TotalMilliseconds).ToString();
        var msg = new Message
        {
            Token = fcmToken,
            Data = new Dictionary<string, string>
            {
                ["timestamp"] = timestampMs,
                ["message"] = message
            },
            Android = new AndroidConfig
            {
                Priority = Priority.High,
                TimeToLive = TimeSpan.FromHours(1)  // keep alive an hour in case of network hiccups
            }
        };
        return FirebaseMessaging.DefaultInstance.SendAsync(msg);
    }
}
