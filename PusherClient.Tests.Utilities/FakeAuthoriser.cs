using PusherServer;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PusherClient.Tests.Utilities
{
    public class FakeAuthoriser : IAuthorizer, IAuthorizerAsync
    {
        /// <summary>
        /// The minimum latency measured in milli-seconds.
        /// </summary>
        public const int MinLatency = 300;

        /// <summary>
        /// The maximum latency measured in milli-seconds.
        /// </summary>
        public const int MaxLatency = 1000;

        public const string TamperToken = "-tamper";

        private readonly string _userName;
        private readonly string _apiKey;
        private readonly string _deviceId;


        private readonly byte[] _encryptionKey;

        public FakeAuthoriser()
            : this("Unknown")
        {
        }

        public FakeAuthoriser(string userName)
            : this(userName, null)
        {
        }

        public FakeAuthoriser(string userName, string apiKey, string deviceId)
           : this(userName, apiKey, deviceId, null)
        {
        }

        public FakeAuthoriser(string userName, byte[] encryptionKey)
        {
            _userName = userName;
            _apiKey = string.Empty;
            _deviceId = string.Empty;
            _encryptionKey = encryptionKey;
        }

        public FakeAuthoriser(string userName, string apiKey, string deviceId, byte[] encryptionKey)
        {
            _userName = userName;
            _apiKey = apiKey;
            _deviceId = deviceId;
            _encryptionKey = encryptionKey;
        }

        public TimeSpan? Timeout { get; set; }

        public string Authorize(string channelName, string socketId)
        {
            return AuthorizeAsync(channelName, socketId).Result;
        }

        public string Authorize(string channelName, string apiKey, string deviceId, string socketId)
        {
            return AuthorizeAsync(channelName, apiKey, deviceId, socketId).Result;
        }

        public async Task<string> AuthorizeAsync(string channelName, string socketId)
        {
            string authData = null;
            double delay = (await LatencyInducer.InduceLatencyAsync(MinLatency, MaxLatency)) / 1000.0;
            Trace.TraceInformation($"{this.GetType().Name} paused for {Math.Round(delay, 3)} second(s)");
            await Task.Run(() =>
            {
                PusherServer.PusherOptions options = new PusherServer.PusherOptions
                {
                    EncryptionMasterKey = _encryptionKey,
                    Cluster = Config.Cluster,
                };
                var provider = new PusherServer.Pusher(Config.AppId, Config.AppKey, Config.AppSecret, options);
                if (channelName.StartsWith("presence-"))
                {
                    var channelData = new PresenceChannelData
                    {
                        user_id = socketId,
                        user_info = new FakeUserInfo { name = _userName }
                    };

                    authData = provider.Authenticate(channelName, socketId, channelData).ToJson();
                }
                else
                {
                    authData = provider.Authenticate(channelName, socketId).ToJson();
                }

                if (channelName.Contains(TamperToken))
                {
                    authData = authData.Replace("1", "2");
                }
            }).ConfigureAwait(false);
            return authData;
        }

        public async Task<string> AuthorizeAsync(string channelName, string apiKey, string deviceId, string socketId)
        {
            string authData = null;
            double delay = (await LatencyInducer.InduceLatencyAsync(MinLatency, MaxLatency)) / 1000.0;
            Trace.TraceInformation($"{this.GetType().Name} paused for {Math.Round(delay, 3)} second(s)");
            await Task.Run(() =>
            {
                PusherServer.PusherOptions options = new PusherServer.PusherOptions
                {
                    EncryptionMasterKey = _encryptionKey,
                    Cluster = Config.Cluster,
                };
                var provider = new PusherServer.Pusher(Config.AppId, Config.AppKey, Config.AppSecret, options);
                if (channelName.StartsWith("presence-"))
                {
                    var channelData = new PresenceChannelData
                    {
                        user_id = socketId,
                        user_info = new FakeUserInfo { name = _userName }
                    };

                    //TODO(custom): will need to change pusherserver project for this
                    //authData = provider.Authenticate(channelName, apiKey, deviceId, socketId, channelData).ToJson();
                    authData = provider.Authenticate(channelName, socketId, channelData).ToJson();
                }
                else
                {
                    //TODO(custom): will need to change pusherserver project for this
                    //authData = provider.Authenticate(channelName, apiKey, deviceId, socketId).ToJson();
                    authData = provider.Authenticate(channelName, socketId).ToJson();
                }

                if (channelName.Contains(TamperToken))
                {
                    authData = authData.Replace("1", "2");
                }
            }).ConfigureAwait(false);
            return authData;
        }

        private static ILatencyInducer LatencyInducer { get; } = new LatencyInducer();
    }
}
