namespace PusherClient
{
    /// <summary>
    /// This class exists for backwards compatibility with code that isn't using GenericPresenceChannel.
    /// </summary>
    public class PresenceChannel : GenericPresenceChannel<dynamic>
    {
        internal PresenceChannel(string channelName, ITriggerChannels pusher)
            : base(channelName, pusher)
        {
        }

        internal PresenceChannel(string channelName, string apiKey, string deviceId, ITriggerChannels pusher)
            : base(channelName, apiKey, deviceId, pusher)
        {
        }
    }
}