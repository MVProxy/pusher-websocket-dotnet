﻿using System.Threading.Tasks;

namespace PusherClient
{
    internal interface IPusher
    {
        PusherOptions PusherOptions { get; set; }

        void ChangeConnectionState(ConnectionState state);
        void ErrorOccured(PusherException pusherException);
        void AddMember(string channelName, string member);
        void RemoveMember(string channelName, string member);
        void SubscriptionSuceeded(string channelName, string data);
        void SubscriberCount(string channelName, string data);
        void SubscriptionFailed(string channelName, string data);
        IEventBinder GetEventBinder(string eventBinderKey);
        IEventBinder GetChannelEventBinder(string eventBinderKey, string channelName);
        byte[] GetSharedSecret(string channelName);
        Task<Channel> SubscribeAsync(string channelName, SubscriptionEventHandler subscribedEventHandler = null);
        Task<Channel> SubscribeAsync(string channelName, string apiKey, string deviceId, SubscriptionEventHandler subscribedEventHandler = null);

    }
}
