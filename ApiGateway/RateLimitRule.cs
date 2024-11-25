using System;

namespace ApiGateway
{
    public class RateLimitRule
    {
        public string ApiKey { get; set; }
        public string EndPoint { get; set; }
        public int Limit { get; set; } = 2;
        public TimeSpan Window { get; set; } = TimeSpan.FromSeconds(60);
    }
}
