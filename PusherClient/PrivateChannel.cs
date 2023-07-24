namespace PusherClient
{
    /// <summary>
    /// Represents a Pusher Private Channel
    /// </summary>
    public class PrivateChannel : Channel
    {
        internal PrivateChannel(string channelName, ITriggerChannels pusher)
            : base(channelName, pusher)
        {
        }

        internal PrivateChannel(string channelName, string apiKey, string deviceId, ITriggerChannels pusher)
           : base(channelName, apiKey, deviceId,pusher)
        {
        }

        internal byte[] SharedSecret { get; set; }
    }
}