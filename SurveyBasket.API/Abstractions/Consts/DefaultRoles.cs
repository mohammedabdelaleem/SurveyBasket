namespace SurveyBasket.API.Abstractions.Consts;

public static class DefaultRoles
{
	public const string Admin = nameof(Admin);
	public const string AdminRoleId = "01979187-c392-77be-89d1-ffe5c2623fae";
	public const string AdminConcurrencyStamp = "01979187-c392-77be-89d1-ffe6910b4102";


	public const string Member = nameof(Member);
	public const string MemberRoleId = "01979187-c392-77be-89d1-ffe7d2df25d4";
	public const string MemberConcurrencyStamp = "01979187-c392-77be-89d1-ffe8850dfea6";
	
	////we can do :
	//public partial class Admin
	//{
	//	public const string Name = nameof(Admin);
	//	public const string Id = "01979187-c392-77be-89d1-ffe5c2623fae";
	//	public const string ConcurrencyStamp = "01979187-c392-77be-89d1-ffe6910b4102";
	//}

	//public partial class Member
	//{
	//	public const string Name = nameof(Member);
	//	public const string Id = "01979187-c392-77be-89d1-ffe5c2623fae";
	//	public const string ConcurrencyStamp = "01979187-c392-77be-89d1-ffe6910b4102";
	//}

	////then ===> DefaultRoles.Admin.Id , .....
}
