namespace ApiGateway
{
    public interface IRateLimitRuleRepository
    {
        RateLimitRule Get(string apikey);
    }
}
