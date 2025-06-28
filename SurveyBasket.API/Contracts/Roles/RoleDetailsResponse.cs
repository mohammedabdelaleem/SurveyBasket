namespace SurveyBasket.API.Contracts.Roles;

public record RoleDetailsResponse(
	string Id,
	string Name,
	bool IsDeleted,
	IEnumerable<string> Permissions
	);