namespace ApiGateway
{
    public class RateLimitRuleRepository : IRateLimitRuleRepository
    {
        private static RateLimitRule rule1 = new RateLimitRule()
        {
            ApiKey = "XYZ-789",
            EndPoint = "/Weather",
            Limit = 2,
            Window = TimeSpan.FromMinutes(60),
            RequestHistory = new LinkedList()
        };

        private static RateLimitRule rule2 = new RateLimitRule()
        {
            ApiKey = "ABC-123",
            EndPoint = "/Weather",
            Limit = 5,
            Window = TimeSpan.FromMinutes(60),
            RequestHistory = new LinkedList()
        };

        private static RateLimitRule rule3 = new RateLimitRule()
        {
            ApiKey = "FFF-000",
            EndPoint = "/Weather",
            Limit = 5,
            Window = TimeSpan.FromMinutes(60),
            RequestHistory = new LinkedList()
        };

        private static RateLimitRule rule4 = new RateLimitRule()
        {
            ApiKey = "666-AAA",
            EndPoint = "/Weather",
            Limit = 5,
            Window = TimeSpan.FromMinutes(1),
            RequestHistory = new LinkedList()
        };

        private static RateLimitRule rule5 = new RateLimitRule()
        {
            ApiKey = "API-123",
            EndPoint = "/Weather",
            Limit = 5,
            Window = TimeSpan.FromMinutes(10),
            RequestHistory = new LinkedList()
        };

        public RateLimitRule Get(string apikey)
        {
            if(apikey == "XYZ-789")
            {
                return rule1;
            }

            if (apikey == "ABC-123")
            {
                return rule2;
            }

            if (apikey == "FFF-000")
            {
                return rule3;
            }

            if (apikey == "666-AAA")
            {
                return rule4;
            }

            if (apikey == "API-123")
            {
                return rule5;
            }

            return null;
        }
    }
}
