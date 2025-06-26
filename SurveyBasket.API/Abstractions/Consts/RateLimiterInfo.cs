namespace SurveyBasket.API.Abstractions.Consts;

public static class RateLimiterInfo
{
	public const string ConcurrenncyPolicy = "concurrency";
	public const string FixedWinsowPolicy = "fixed";
	public const string TokenBuckedPolicy = "token";
	public const string SlidingWinsowPolicy = "sliding";
	public const string IpAddressPolicy = "ipLimit";
	public const string UserAddressPolicy = "userLimit";
}