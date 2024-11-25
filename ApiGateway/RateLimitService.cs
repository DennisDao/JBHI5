namespace ApiGateway
{
    public class RateLimitService
    {
        private readonly IRateLimitRuleRepository _repository;
        private readonly IDateTimeProvider _date;
        public RateLimitService(IRateLimitRuleRepository repository, IDateTimeProvider dateTimeProvider)
        {
            _repository = repository;
            _date = dateTimeProvider;
         }

        public bool IsLimitReached(string apiKey)
        {
            var rule = _repository.Get(apiKey);
            var head = rule?.RequestHistory.GetHead();
            var history = rule.RequestHistory;

            if (head == null)
            {
                history.AddLast(_date.Now);
                return false;
            }
            else
            {
                var totalRequest = history.Count();
                DateTime currentTime = _date.Now;

                double totalMinutesElapsed = (currentTime - head.TimeStamp).TotalMinutes;
                var isWithinWindow = totalMinutesElapsed <= rule.Window.TotalMinutes;

                if (totalRequest == rule.Limit && isWithinWindow)
                {
                    return true;
                }

                if (totalRequest == rule.Limit && !isWithinWindow)
                {
                    history.RemoveHead();
                    history.AddLast(_date.Now);
                    return false;
                }
                else
                {
                    history.AddLast(_date.Now);
                    return false;
                }
            }
        }
    }
}
