namespace SurveyBasket.API.Abstractions.Consts;

public static class Permissions
{
	public static string Type { get; } = "permissions";

	public const string GetPolls =	  "polls:read";
	public const string AddPoll =     "polls:add";
	public const string UpdatePoll = "polls:update";
	public const string DeletePoll = "polls:delete";

	public const string GetQuestion = "questions:read";
	public const string AddQuestion = "questions:add";
	public const string UpdateQuestion = "questions:update";

	public const string GetUsers = "users:read";
	public const string AddUser = "users:add";
	public const string UpdateUser = "users:update";
	public const string DeleteUser = "users:delete";


	public const string GetRoles = "roles:read";
	public const string AddRole = "roles:add";
	public const string UpdateRole = "roles:update";
	public const string DeleteRole = "roles:delete";

	public const string Results = "results:read";

	public static IList<string?> GetAllPermissions()=>
		typeof(Permissions).GetFields().Select(f => f.GetValue(f) as string).ToList();
}
