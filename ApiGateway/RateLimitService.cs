namespace ApiGateway
{
    public class RateLimitService
    {
        private static readonly List<RateLimitRule> _rules = new List<RateLimitRule>()
        {
            new RateLimitRule()
            {
                ApiKey = "ABC-123",
                EndPoint = "/Weather",
                Limit = 2,
                Window = TimeSpan.FromSeconds(5),
                RequestHistory = new LinkedList()
            }
        };

        public bool IsLimitReached(string apiKey)
        {
            var rule = _rules.FirstOrDefault(x => x.ApiKey == apiKey);

            var head = rule?.RequestHistory.GetHead();
            var history = rule.RequestHistory;

            if (head == null)
            {
                history.AddLast(DateTime.Now);
                return false;
            }
            else
            {
                var totalRequest = history.Count();
                DateTime currentTime = DateTime.Now;

                double totalMinutesElapsed = (currentTime - head.TimeStamp).TotalMinutes;
                var isWithinWindow = totalMinutesElapsed <= rule.Window.TotalMinutes;

                if (totalRequest == rule.Limit && isWithinWindow)
                {
                    return true;
                }

                if (totalRequest == rule.Limit && !isWithinWindow)
                {
                    history.RemoveHead();
                    history.AddLast(DateTime.Now);
                    return false;
                }
                else
                {
                    history.AddLast(DateTime.Now);
                    return false;
                }
            }
        }
    }
}
